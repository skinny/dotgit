using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using dotGit.Generic;
using dotGit.Refs;
using System.Reflection;

namespace dotGit.Objects.Storage
{

  /// <summary>
  /// Gateway to all of Git's stored objects. Either loose or packed
  /// </summary>
  public class ObjectStorage
  {
    private ObjectStorage()
    { }

    /// <summary>
    /// Instantiates a new ObjectStorage object. It needs a reference to a repository object to operate
    /// </summary>
    /// <param name="repo">The repository to work with</param>
    public ObjectStorage(Repository repo)
    {
      Repo = repo;

      string[] packFiles = Directory.GetFiles(Path.Combine(ObjectsDir, "pack"));

      // strip file extension and collect unique pack names
      packFiles = packFiles.Select((content) => Path.Combine(Path.GetDirectoryName(content), Path.GetFileNameWithoutExtension(content))).Distinct().ToArray();
      Packs = new InternalWritableList<Pack>(packFiles.Length);

      foreach (string packFile in packFiles)
      {
        Packs.Add(Pack.LoadPack(Repo, packFile));
      }
    }

    internal InternalWritableList<Pack> Packs
    {
      get;
      private set;
    }

    /// <summary>
    /// The full path to repositories' objects directory
    /// </summary>
    public string ObjectsDir
    {
      get
      {
        return Path.Combine(Repo.GitDir.FullName, "objects");
      }
    }

    private Repository Repo { get; set; }

    /// <summary>
    /// Find object in database and return it as an IStorableObject. Use the generic overload if you know the type up front. Throws ObjectNotFoundException if object was not found in database.
    /// </summary>
    /// <param name="sha">SHA object identifier. Throws ArgumentException if it is null or not a valid sha</param>
    /// <returns>GitObject in database</returns>
    public IStorableObject GetObject(string sha)
    {
      if (!Utility.SHAExpression.IsMatch(sha))
        throw new ArgumentException("Need a valid sha", "sha");


      string looseObjectPath = Path.Combine(ObjectsDir, Path.Combine(sha.Substring(0, 2), sha.Substring(2)));
      // First check if object is stored in loose format
      if (File.Exists(looseObjectPath))
      { // Object is stored loose. Inflate and load it from content
        using (GitObjectReader reader = new GitObjectReader(Zlib.Decompress(looseObjectPath)))
        {
          long size;
          ObjectType type;
          size = reader.ReadObjectHeader(out type);
          IStorableObject obj;
          switch (type)
          {
            case ObjectType.Commit:
              obj = new Commit(Repo, sha);
              break;
            case ObjectType.Tree:
              obj = new Tree(Repo, sha);
              break;
            case ObjectType.Blob:
              obj = new Blob(Repo, sha);
              break;
            case ObjectType.Tag:
              obj = new Tag(Repo, sha);
              break;
            default:
              throw new NotImplementedException();
          }
          obj.Deserialize(reader);
          return obj;
        }
      }
      else
      {
        IStorableObject result = null;
        foreach (Pack pack in Packs)
        {
          try
          {
            result = pack.GetObject(sha);
          }
          catch (ObjectNotFoundException)
          { }
        }

        if (result != null)
          return result;
      }


      // Object was not found
      throw new ObjectNotFoundException(sha);
    }

    /// <summary>
    /// Use this if you already know the objects type
    /// </summary>
    /// <typeparam name="T">The type of object to fetch from the db. IStorableObject must be implemented</typeparam>
    /// <param name="sha"></param>
    /// <returns></returns>
    public T GetObject<T>(string sha) where T : IStorableObject
    {
      // For now we're just casting it to the given type
      return (T)GetObject(sha);
    }
  }
}
