using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Generic;
using dotGit.Objects;

namespace dotGit.Refs
{
	public class Head
	{
		private Commit _commit = null; // Will only be set if HEAD is detached

		internal Head(Repository repo)
		{
			Repo = repo;

			string headContents = File.ReadAllText(Path.Combine(Repo.GitDir.FullName, @"HEAD")).Trim();

			if (!Utility.IsValidSHA(headContents))
			{ // HEAD content does not contain valid SHA. Will assume format: 'refs: path/to/ref'
				Branch = new Branch(Repo, headContents.Split(' ').Last().Replace('/', Path.DirectorySeparatorChar).Trim());
			}
			else
			{
				_commit = Repo.Storage.GetObject<Commit>(headContents);
			}
		}

		/// <summary>
		/// The branch you're currently working on. Returns null if detached
		/// </summary>
		public Branch Branch
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns true if HEAD file directly references a commit. Return false if a branch is referenced
		/// </summary>
		public bool IsDetached
		{
			get { return Branch == null; }
		}

		/// <summary>
		/// If detached directly returns the commit otherwise returns the branches' commit
		/// </summary>
		public Commit Commit
		{
			get
			{
				if (IsDetached)
					return _commit;
				else
					return Branch.Commit;
			}
		}

		protected Repository Repo
		{
			get;
			private set;
		}

	}
}
