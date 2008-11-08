using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using IO = System.IO;
using System.Text.RegularExpressions;
using dotGit.Objects;


namespace dotGit
{
	/// <summary>
	/// Represents a reference to a commit
	/// </summary>
	public abstract class Ref
	{
		private string _path = null;
		private FileInfo _file = null;

		internal Ref(Repository repo, string path)
		{
			Repo = repo;
			_path = path;

			ParseRef();
		}

		public Commit Commit
		{
			get;
			protected set;
		}

		public Repository Repo
		{
			get;
			private set;
		}

		protected string Path
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

		public override string ToString()
		{
			return Commit.SHA;
		}

		protected FileInfo File
		{
			get
			{
				if (_file != null) return _file;

				if (IO.File.Exists(IO.Path.Combine(Repo.GitDir.FullName, Path)))
					_file = new FileInfo(IO.Path.Combine(Repo.GitDir.FullName, Path));
				else
					throw new FileNotFoundException(String.Format("Could not find ref file '{0}'", Path));

				return _file;
			}
		}

		private void ParseRef()
		{
			string fileContens = IO.File.ReadAllText(File.FullName).Trim();		
			
			if(!Utility.IsValidSHA(fileContens))
				throw new RefParseException(File.FullName, fileContens);

			Commit = new Commit(Repo, fileContens);
		}
	}
}
