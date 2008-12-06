using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using IO = System.IO;
using dotGit.Exceptions;
using dotGit.Objects;
using System.Runtime.Serialization;


namespace dotGit.Refs
{
	/// <summary>
	/// The Branch class represents a branch in the git repository. It points to a certain commit
	/// </summary>
	public class Branch : Ref
	{
		Commit _commit = null;

		internal Branch(Repository repo, string path, string sha)
			: base(repo, path, sha)
		{
			
		}

		/// <summary>
		/// The commit this branch points to
		/// </summary>
		public Commit Commit
		{
			get
			{
				if (_commit == null)
					Deserialize();

				return _commit;
			}
		}

		internal override void Deserialize()
		{
			if (!Utility.IsValidSHA(SHA))
				throw new ArgumentException("Need valid sha", "contents");

			_commit = Repo.Storage.GetObject<Commit>(SHA);

		}
	}
}
