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
	/// <summary>
	/// Is the base class for TreeNode and Commit
	/// </summary>
	public abstract class GitObject : IStorableObject
	{
		private GitObject()
		{ }

		protected GitObject(Repository repo)
		{
			if (repo == null)
				throw new ArgumentNullException("repo", "repo cannot be null");

			Repo = repo;
		}

		protected GitObject(Repository repo, string sha)
			:this(repo)
		{
			if (!Utility.SHAExpression.IsMatch(sha))
				throw new ArgumentException("Need a valid sha", "sha");

			SHA = sha;
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

		public abstract void Deserialize(GitObjectReader input);
		public abstract byte[] Serialize();
	
	}
}
