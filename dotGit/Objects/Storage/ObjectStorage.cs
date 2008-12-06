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
					PackObjectType type;
					size = reader.ReadObjectHeader(out type);

					switch (type)
					{
						case PackObjectType.OBJ_COMMIT:
							return ObjectStorage.LoadObjectFromContent<Commit>(Repo, reader, sha, size);

						case PackObjectType.OBJ_TREE:
							return ObjectStorage.LoadObjectFromContent<Tree>(Repo, reader, sha, size);

						case PackObjectType.OBJ_BLOB:
							return ObjectStorage.LoadObjectFromContent<Blob>(Repo, reader, sha, size);

						case PackObjectType.OBJ_TAG:
							return ObjectStorage.LoadObjectFromContent<Tag>(Repo, reader, sha, size);

						case PackObjectType.OBJ_OFS_DELTA:
						case PackObjectType.OBJ_REF_DELTE:
						default:
							throw new NotImplementedException();
					}

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
					{  }
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

		internal static T LoadObjectFromContent<T>(Repository repo, GitObjectReader input, string sha, long length) where T : IStorableObject
		{
			ConstructorInfo constr = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,new Type[] { typeof(Repository), typeof(String) }, null);
			IStorableObject result = (IStorableObject)constr.Invoke(new object[] { repo, sha });			

			// Let the respective object type load itself from the object content
			result.Deserialize(input);

			return (T)result;
		}
	}
}
