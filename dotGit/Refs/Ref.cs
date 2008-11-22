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
	/// Represents a reference to a commit
	/// </summary>
	public abstract class Ref
	{
		private string _path = null;
		private FileInfo _file = null;

		protected Ref(Repository repo)
		{
			Repo = repo;
		}

		protected Ref(Repository repo, string path)
			: this(repo)
		{
			Path = path;
		}

		protected Repository Repo
		{
			get;
			private set;
		}

		/// <summary>
		/// Relative path to ref file in GITDIR
		/// </summary>
		public string Path
		{
			get { return _path; }
			set
			{
				if (_path != value)
				{
					_path = value;
					_file = null;
				}
			}
		}

		/// <summary>
		/// The name of this REF
		/// </summary>
		public string Name
		{
			get
			{
				return File.Name;
			}
		}

		/// <summary>
		/// The full path to ref's file
		/// </summary>
		public FileInfo File
		{
			get
			{
				if (_file != null) return _file;

				_file = GetRefFile(Repo, Path);
				
				return _file;
			}
		}

		protected static FileInfo GetRefFile(Repository repo, string path)
		{
			if (IO.File.Exists(IO.Path.Combine(repo.GitDir.FullName, path)))
				return new FileInfo(IO.Path.Combine(repo.GitDir.FullName, path));
			else
				throw new FileNotFoundException(String.Format("Could not find ref file '{0}'", path));
		}
	}
}
