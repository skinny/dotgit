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
			private set
			{
				_children = value;
			}
		}

		private void LoadChildren()
		{
			Children = new TreeNodeCollection();

			using (GitObjectStream stream = new GitObjectStream(_childrenRaw))
			{
				while (!stream.IsEndOfFile)
				{
					string mode, path, sha;

					mode = Encoding.UTF8.GetString(stream.ReadWord());
					path = Encoding.UTF8.GetString(stream.ReadToNull());
					sha = Sha.Decode(stream.ReadBytes(20));

					TreeNode child = Repo.Storage.GetObject<TreeNode>(sha);
					child.Path = path;
					child.Mode = mode;
					child.Parent = this;

					_children.Add(child);
				}
			}
		}

		public override void Deserialize(byte[] contents)
		{
			if (String.IsNullOrEmpty(SHA))
				SHA = Sha.Compute(contents);

			using (GitObjectStream stream = new GitObjectStream(contents))
			{
				//string content = Encoding.UTF8.GetString(stream.ReadToEnd());
				
				//Skip header
				stream.ReadToNull();

				_childrenRaw = stream.ReadToEnd();
			}
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}
	}
}
