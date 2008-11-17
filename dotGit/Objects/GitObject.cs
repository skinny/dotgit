using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Generic;
using dotGit.Objects.Storage;
using dotGit.Refs;
using dotGit.Exceptions;

namespace dotGit.Objects
{
	public abstract class GitObject : IStorableObject
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

		public static IStorableObject LoadFromContent(Repository repo, byte[] uncomprContents, string sha)
		{
			long length;
			string type;
			using (GitObjectStream stream = new GitObjectStream(uncomprContents))
			{
				length = stream.ReadObjectHeader(out type);
			}

			bool haveSha = !String.IsNullOrEmpty(sha);

			// If sha is passed we can forward it to the object so the SHA does not have to be calculated from the objects contents
			if (haveSha && !Utility.IsValidSHA(sha))
				throw new ArgumentException("Must have valid sha", "sha");

			IStorableObject result;
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
				case "tag":
					if (haveSha)
						result = new Tag(repo, sha);
					else
						result = new Tag(repo);
					break;
				default:
					throw new ParseException(String.Format("Could not open object of type: {0}", type));
			}

			// Let the respective object type load itself from the object content
			result.Deserialize(uncomprContents);

			return result;
		}

		public static IStorableObject LoadFromContent(Repository repo, byte[] uncomprContents)
		{
			return LoadFromContent(repo, uncomprContents, null);
		}

		public static T LoadFromContent<T>(Repository repo, byte[] uncomprContents) where T : IStorableObject
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
			private set;
		}

		public abstract void Deserialize(byte[] contents);
		public abstract byte[] Serialize();
	
	}
}
