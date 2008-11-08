using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using System.Text.RegularExpressions;
using System.Security.AccessControl;

namespace dotGit
{
	/// <summary>
	/// 
	/// </summary>
	public class Repository
	{
		#region Fields
		private Ref _head = null;

		#endregion

		#region Constructors / Factory methods
		private Repository()
		{	}

		private Repository(string path, bool autoInit)
			:this()
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException("repository", "Need path to repository");

			DirectoryInfo gitDir = null;
			if (Utility.IsGitRepository(path, out gitDir))
			{
				GitDir = gitDir;
			}
			else if (autoInit)
			{
				Utility.CreateGitDirectoryStructure(path);
				Console.WriteLine(String.Format("Initialized empty git repository in {0}", path));
			}
			else
				throw new RepositoryNotFoundException("'{0}' could not be opened as a git repository");
		}

		private Repository(string path)
			: this(path, false)
		{ }

		/// <summary>
		/// Opens a repository from given path
		/// </summary>
		/// <param name="repositoryPath"></param>
		/// <returns></returns>
		public static Repository Open(string repositoryPath)
		{
			return new Repository(repositoryPath);
		}

		/// <summary>
		/// Creates a new repository at given path. If path is an existing repository it does the same as "Open".
		/// </summary>
		/// <param name="newRepositoryPath"></param>
		/// <returns></returns>
		public static Repository Init(string newRepositoryPath)
		{
			return new Repository(newRepositoryPath, true);
		}

		private void LoadHead()
		{
			string headContents = File.ReadAllText(Path.Combine(GitDir.FullName, @"HEAD"));

			//TODO: Check for type type in HEAD file and load either a Tag or Branch object
			HEAD = new Branch(this, headContents.Split(' ').Last().Replace('/', Path.DirectorySeparatorChar).Trim());
		}

		#endregion Constructors / Factory methods

		#region Properties

		public Ref HEAD
		{
			get
			{
				if (_head == null)
					LoadHead();

				return _head;
			}
			private set
			{
				_head = value;
			}
		}

		public List<Branch> Branches
		{
			get;
			private set;
		}

		public Index Index
		{
			get;
			private set;
		}

		public string RepositoryPath
		{
			get;
			private set;
		}

		public DirectoryInfo RepositoryDir
		{
			get
			{
				return GitDir.Parent;
			}
		}

		public DirectoryInfo GitDir
		{
			get;
			private set;
		}

		#endregion

	}
}
