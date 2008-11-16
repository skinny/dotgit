using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Generic;
using dotGit.Objects.Storage;

namespace dotGit.Objects
{
	public abstract class GitObject
	{
		private GitObject()
		{ }

		protected GitObject(Repository repo)
		{
			Repo = repo;
		}

		protected GitObject(Repository repo, string sha)
			:this(repo)
		{
			if (!Utility.SHAExpression.IsMatch(sha))
				throw new ArgumentException("Need a valid sha", "sha");

			SHA = sha;
		}

		public static GitObject LoadFromContent(Repository repo, byte[] uncomprContents, string sha)
		{
			long length;
			string type;
			using (GitObjectStream stream = new GitObjectStream(uncomprContents))
			{
				length = stream.ReadObjectHeader(out type);
			}

			bool haveSha = !String.IsNullOrEmpty(sha);

			if (haveSha && !Utility.IsValidSHA(sha))
				throw new ArgumentException("Must have valid sha", "sha");

			GitObject result;
			switch (type)
			{
				case "commit":
					if (haveSha)
						result = new Commit(repo, sha);
					else
						result = new Commit(repo);
					break;
				case "tree":
					if (haveSha)
						result = new Tree(repo, sha);
					else
						result = new Tree(repo);
					break;
				case "blob":
					if (haveSha)
						result = new Blob(repo, sha);
					else
						result = new Blob(repo);
					break;
				default:
					throw new TypeUnloadedException(String.Format("Could not open object of type: {0}", type));
			}

			result.Deserialize(uncomprContents);

			return result;
		}

		public static GitObject LoadFromContent(Repository repo, byte[] uncomprContents)
		{
			return LoadFromContent(repo, uncomprContents, null);
		}

		public static T LoadFromContent<T>(Repository repo, byte[] uncomprContents) where T : GitObject
		{
			return (T)LoadFromContent(repo, uncomprContents);
		}

		/// <summary>
		/// The SHA this object is identified by
		/// </summary>
		public string SHA
		{
			get;
			protected set;
		}

		protected Repository Repo
		{
			get;
			set;
		}

		public abstract void Deserialize(byte[] contents);
		public abstract byte[] Serialize();
	
	}
}
