using System;
using System.Collections.Generic;
using System.Text;
using dotGit.Objects.Storage;
using System.Security.Cryptography;
using dotGit.Generic;

namespace dotGit.Objects
{
	public class Commit : GitObject
	{
		private string _treeSha = null;
		private Tree _tree = null;

		private string _parentSha = null;
		private Commit _parent = null;

		internal Commit(Repository repo)
			: base(repo)
		{ }

		internal Commit(Repository repo, string sha)
			: base(repo, sha)
		{ }

		public string Message
		{
			get;
			private set;
		}

		public Tree Tree
		{
			get
			{
				if (_tree != null) return _tree;

				if (!String.IsNullOrEmpty(_treeSha))
					_tree = Repo.Storage.GetObject<Tree>(_treeSha);

				return _tree;
			}
			private set
			{
				_tree = value;
			}
		}

		// TODO: This should be a collection of parents
		public Commit Parent
		{
			get
			{
				if (_parent == null && !String.IsNullOrEmpty(_parentSha))
					_parent = Repo.Storage.GetObject<Commit>(_parentSha);

				return _parent;
			}
			private set
			{
				_parent = value;
			}
		}

		public bool HasParent
		{
			get { return Parent != null; }
		}

		public Contributer Committer
		{
			get;
			private set;
		}

		public DateTime CommittedDate
		{
			get;
			private set;
		}

		public Contributer Author
		{
			get;
			private set;
		}

		public DateTime AuthoredDate
		{
			get;
			private set;
		}

		public override void Deserialize(GitObjectReader input)
		{
			if (String.IsNullOrEmpty(SHA))
				SHA = Sha.Compute(input);


			input.Rewind();

			//Skip header
			if(input.IsStartOfStream)
				input.ReadToNull();


			//Skip 'tree' at beginning of line and read tree sha
			input.ReadWord();
			_treeSha = input.ReadLine().GetString();

			// Check for 'parent' at beginning of line
			string parentOrAuthor = input.ReadWord().GetString();

			// TODO: Make recursive
			if (parentOrAuthor == "parent")
			{
				_parentSha = input.ReadLine().GetString();

				// Skip 'author'
				input.ReadWord();
			}

			// Author
			string authorLine = input.ReadLine().GetString();
			AuthoredDate = Utility.StripDate(authorLine, out authorLine);
			Author = Contributer.Parse(authorLine);

			// Committer
			input.ReadWord();
			string committerLine = input.ReadLine().GetString();
			CommittedDate = Utility.StripDate(committerLine, out committerLine);
			Committer = Contributer.Parse(committerLine);

			//Skip extra '\n'
			input.ReadBytes(1);
			Message = input.ReadToEnd().GetString().TrimEnd();
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}


	}
}
