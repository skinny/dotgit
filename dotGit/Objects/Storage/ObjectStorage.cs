using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using dotGit.Generic;

namespace dotGit.Objects.Storage
{

	/// <summary>
	/// Gateway to all of Git's stored objects. Be it loose or packed
	/// </summary>
	public class ObjectStorage
	{
		private ObjectStorage()
		{ }

		public ObjectStorage(Repository repo)
		{
			Repo = repo;

			string[] indexFiles = Directory.GetFiles(Path.Combine(ObjectsDir, "pack"));

			// strip file extension and collect unique pack names
			indexFiles = indexFiles.Select((content) => Path.Combine(Path.GetDirectoryName(content), Path.GetFileNameWithoutExtension(content))).Distinct().ToArray();
			Packs = new InternalWritableList<Pack>(indexFiles.Length);
			
			foreach (string packFile in indexFiles)
			{
				Packs.Add(new Pack(packFile));
			}
		}

		internal InternalWritableList<Pack> Packs
		{
			get;
			private set;
		}

		public string ObjectsDir
		{
			get
			{
				return Path.Combine(Repo.GitDir.FullName, "objects");
			}
		}

		private Repository Repo { get; set; }

		public GitObject GetObject(string sha)
		{
			return GetObject(sha, Repo);
		}

		public T GetObject<T>(string sha) where T : GitObject
		{
			return (T)GetObject(sha);
		}

		/// <summary>
		/// Find object in database and return it as an IStorableObject. Use the generic overload if you know the type up front
		/// </summary>
		/// <param name="sha">SHA object identifier</param>
		/// <param name="repo">Reference to repository for easy path finding</param>
		/// <returns></returns>
		public static GitObject GetObject(string sha, Repository repo)
		{
			if (!Utility.SHAExpression.IsMatch(sha))
				throw new ArgumentException("Need a valid sha", "sha");

			string gitObjectsDirectory = Path.Combine(repo.GitDir.FullName, "objects");

			string looseObjectPath = Path.Combine(gitObjectsDirectory, Path.Combine(sha.Substring(0, 2), sha.Substring(2)));
			if (File.Exists(looseObjectPath))
			{
				byte[] contents = Zlib.Decompress(looseObjectPath);

				return GitObject.LoadFromContent(repo, contents, sha);
			}
			else
			{
				// TODO: Look for object in pack files
			}


			// Object was not found
			throw new ObjectNotFoundException(sha);
		}

		public static T GetObject<T>(string sha, Repository repo) where T : GitObject
		{
			return (T)GetObject(sha, repo);
		}
	}
}
