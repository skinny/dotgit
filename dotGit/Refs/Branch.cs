using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using IO = System.IO;
using dotGit.Exceptions;
using dotGit.Objects;
using System.Runtime.Serialization;
using dotGit.Objects;

namespace dotGit.Refs
{
	/// <summary>
	/// The Branch class represents a branch in the git repository. It points to a certain commit
	/// </summary>
	public class Branch : Ref
	{
		internal Branch(Repository repo, string path)
			: base(repo, path)
		{
			string sha = IO.File.ReadAllBytes(File.FullName).GetString().Trim();
			if (!Utility.IsValidSHA(sha))
				throw new ArgumentException("Need valid sha", "contents");


			Commit = Repo.Storage.GetObject<Commit>(sha);
		}

		/// <summary>
		/// The commit this branch points to
		/// </summary>
		public Commit Commit
		{
			get;
			protected set;
		}
	}
}
