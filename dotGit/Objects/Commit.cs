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

		private List<string> _parentShas = null;
		private CommitCollection _parents = null;

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
		public CommitCollection Parents
		{
			get
			{
				if (_parents == null && _parentShas.Count > 0)
					LoadParents();

				return _parents;
			}
		}

		private void LoadParents()
		{
			_parents = new CommitCollection();

			foreach(string parentSha in _parentShas)
				_parents.Add(Repo.Storage.GetObject<Commit>(parentSha));
		}

		public bool HasParents
		{
			get { return Parents != null && Parents.Count > 0; }
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
			_parentShas = new List<string>();
			string parentOrAuthor = input.ReadWord().GetString();

			// TODO: Make recursive
			while (parentOrAuthor == "parent")
			{
				_parentShas.Add(input.ReadLine().GetString());

				// Skip 'author'
				parentOrAuthor = input.ReadWord().GetString();
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
