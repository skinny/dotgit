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
			:base(repo)
		{ }

		internal Commit(Repository repo, string sha)
			:base(repo, sha)
		{	}

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

				if(!String.IsNullOrEmpty(_treeSha))
					_tree = Repo.Storage.GetObject<Tree>(_treeSha);

				return _tree;
			}
			private set
			{
				_tree = value;
			}
		}

		public Commit Parent
		{
			get
			{
				return _parent;
			}
			private set
			{
				_parent = value;
			}
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

		public override void Deserialize(byte[] contents)
		{
			if(String.IsNullOrEmpty(SHA))
				SHA = Sha.Compute(contents);

			using (GitObjectStream stream = new GitObjectStream(contents))
			{
				//Skip header
				stream.ReadToNull();


				//Skip 'tree' at beginning of line and read tree sha
				stream.ReadWord();
				_treeSha = Encoding.UTF8.GetString(stream.ReadLine());

				// Check for 'parent' at beginning of line
				string parentOrAuthor = Encoding.UTF8.GetString(stream.ReadWord());
				if (parentOrAuthor == "parent")
				{
					_parentSha = Encoding.UTF8.GetString(stream.ReadLine());

					// Skip 'author'
					stream.ReadWord();
				}

				// Author
				string authorLine = Encoding.UTF8.GetString(stream.ReadLine());
				AuthoredDate = Utility.StripDate(authorLine, out authorLine);
				Author = Contributer.Parse(authorLine);

				// Committer
				string committerLine = Encoding.UTF8.GetString(stream.ReadLine());
				CommittedDate = Utility.StripDate(committerLine, out committerLine);
				Committer = Contributer.Parse(committerLine);

				//Skip extra '\n'
				stream.ReadBytes(1);
				Message = Encoding.UTF8.GetString(stream.ReadToEnd()).TrimEnd();
			}
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}


	}
}
