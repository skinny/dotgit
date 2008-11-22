using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using dotGit.Generic;
using System.IO;
using dotGit.Objects.Storage;

namespace dotGit.Objects
{
	public class Tree : TreeNode
	{
		private byte[] _childrenRaw;
		private TreeNodeCollection _children;

		internal Tree(Repository repo)
			: base(repo)
		{ }

		internal Tree(Repository repo, string sha)
			: base(repo, sha)
		{ }

		public TreeNodeCollection Children
		{
			get
			{
				if (_children != null) return _children;

				// Load children from _childrenRaw
				LoadChildren();

				return _children;
			}
		}

		public bool HasChildren
		{
			get { return Children != null && Children.Count > 0; }
		}

		private void LoadChildren()
		{
			_children = new TreeNodeCollection();

			using (GitObjectReader stream = new GitObjectReader(_childrenRaw))
			{
				while (!stream.IsEndOfStream)
				{
					string mode, path, sha;

					mode = stream.ReadWord().GetString();
					path = stream.ReadToNull().GetString();
					sha = Sha.Decode(stream.ReadBytes(20));

					TreeNode child = Repo.Storage.GetObject<TreeNode>(sha);
					child.Path = path;
					child.Mode = FileMode.FromBits(int.Parse(mode));
					child.Parent = this;

					_children.Add(child);
				}
			}
		}

		public override void Deserialize(GitObjectReader input)
		{
			//Skip header
			if(input.IsStartOfStream)
				input.ReadToNull();

			_childrenRaw = input.ReadToEnd();
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}
	}
}
