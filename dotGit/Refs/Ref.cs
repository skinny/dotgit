using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using IO = System.IO;
using System.Text.RegularExpressions;
using dotGit.Objects;
using dotGit.Generic;


namespace dotGit.Refs
{
	/// <summary>
	/// Represents a reference to an object
	/// </summary>
	public abstract class Ref
	{
		private Ref(Repository repo)
		{
			Repo = repo;
		}

		protected Ref(Repository repo, string sha)
			:this(repo)
		{
			SHA = sha;
		}

		protected Ref(Repository repo, string path, string sha)
			: this(repo, sha)
		{
			Path = path;
		}

		protected Repository Repo
		{
			get;
			private set;
		}

		public string Path
		{
			get;
			protected set;
		}

		/// <summary>
		/// The SHA this tag is referenced by
		/// </summary>
		public string SHA
		{
			get;
			private set;
		}

		/// <summary>
		/// The name of this REF
		/// </summary>
		public string Name
		{
			get { return IO.Path.GetFileName(Path); }
		}

		internal abstract void Deserialize();
		
	}
}
