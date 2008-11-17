using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using IO = System.IO;
using dotGit.Exceptions;
using dotGit.Objects;
using System.Runtime.Serialization;
using dotGit.Generic;

namespace dotGit.Refs
{
	public class Branch : Ref
	{
		internal Branch(Repository repo, string path)
			: base(repo, path)
		{
			string sha = Encoding.UTF8.GetString(IO.File.ReadAllBytes(File.FullName)).Trim();
			if (!Utility.IsValidSHA(sha))
				throw new ArgumentException("Need valid sha", "contents");


			Commit = Repo.Storage.GetObject<Commit>(sha);
		}

		/// <summary>
		/// The commit this reference points to
		/// </summary>
		public Commit Commit
		{
			get;
			protected set;
		}
	}
}
