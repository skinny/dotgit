using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using dotGit.Objects.Storage;
using dotGit.Objects;
using dotGit.Refs;
using IDX = dotGit.Index;

namespace dotGit
{
	/// <summary>
	/// 
	/// </summary>
	public class Repository
	{
		#region Fields

		private Head _head = null;
		private ObjectStorage _storage = null;
		private RefCollection<Branch> _branches = null;
		private RefCollection<Tag> _tags = null;
		private IDX.Index _index = null;

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
				Console.WriteLine(String.Format("Opened git repository @ '{0}'".FormatWith(path)));
			}
			else if (autoInit)
			{
				Utility.CreateGitDirectoryStructure(path);
				Console.WriteLine(String.Format("Initialized empty git repository in {0}", path));
			}
			else
				throw new RepositoryNotFoundException("'{0}' could not be opened as a git repository");
		}

		/// <summary>
		/// Defaults the autoInit parameter of the other overload to false
		/// </summary>
		/// <param name="path"></param>
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

		/// <summary>
		/// Gets called from the Branches property getter for lazy loading
		/// </summary>
		private void LoadBranches()
		{
			string[] branches = Directory.GetFiles(Path.Combine(GitDir.FullName, @"refs\heads"));

			_branches = new RefCollection<Branch>(branches.Length);

			foreach (string file in branches)
			{
				_branches.Add(new Branch(this, Path.Combine(@"refs\heads", Path.GetFileName(file))));
			}
		}

		/// <summary>
		/// Gets called from the Tags property getter for lazy loading
		/// </summary>
		private void LoadTags()
		{
			string[] tags = Directory.GetFiles(Path.Combine(GitDir.FullName, @"refs\tags"));

			_tags = new RefCollection<Tag>(tags.Length);

			foreach (string file in tags)
			{
				_tags.Add(Tag.GetTag(this, Path.Combine(@"refs\tags", Path.GetFileName(file))));
			}
		}


		/// <summary>
		/// Gets called from the Storage property getter for lazy loading
		/// </summary>
		private void LoadStorage()
		{
			_storage = new ObjectStorage(this);
		}


		#endregion Constructors / Factory methods

		#region Properties


		/// <summary>
		/// The HEAD of the repository. If it's detached, the Branch property returns NULL
		/// </summary>
		public Head HEAD
		{
			get
			{
				if (_head == null)
					_head = new Head(this);

				return _head;
			}
		}

		/// <summary>
		/// All branches in this repository
		/// </summary>
		public RefCollection<Branch> Branches
		{
			get
			{
				if (_branches == null)
					LoadBranches();

				return _branches;
			}
		}


		/// <summary>
		/// All tags in this repository
		/// </summary>
		public RefCollection<Tag> Tags
		{
			get
			{
				if (_tags == null)
					LoadTags();

				return _tags;
			}
		}

		/// <summary>
		/// The Index of the repository. It's the staging area for changes to be committed and contains a cached tree of the current HEAD
		/// </summary>
		public IDX.Index Index
		{
			get
			{
				if (_index == null)
					_index = new IDX.Index(this);

				return _index;
			}
		}

		/// <summary>
		/// The working directory of this repository. If it's a 'bare' repository this property is of no use
		/// </summary>
		public DirectoryInfo RepositoryDir
		{
			get
			{
				return GitDir.Parent;
			}
		}

		/// <summary>
		/// The git directory containing all the git stuff like the objects, HEAD, config and index
		/// </summary>
		public DirectoryInfo GitDir
		{
			get;
			private set;
		}

		/// <summary>
		/// All the objects(blobs, trees, commits and tags) are stored and retrieved from the ObjectStorage instance
		/// </summary>
		public ObjectStorage Storage
		{
			get
			{
				if (_storage == null)
					LoadStorage();

				return _storage;
			}
		}

		#endregion

	}
}
