using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using dotGit.Generic;
using System.IO;
using dotGit.Objects.Storage;
using dotGit.Exceptions;

namespace dotGit.Objects
{
	/// <summary>
	/// Represents a tree in a git repository. Has Children of type TreeNode which can either be a blog or a tree.
	/// </summary>
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

		/// <summary>
		/// All of the children of this tree. A child can either be a blob or a tree
		/// </summary>
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

		/// <summary>
		/// Returns true if this tree has at least 1 child
		/// </summary>
		public bool HasChildren
		{
			get { return Children != null && Children.Count > 0; }
		}


		/// <summary>
		/// Gets called be the Children getter for lazy loading
		/// </summary>
		private void LoadChildren()
		{
			_children = new TreeNodeCollection();

			try
			{
				using (GitObjectReader stream = new GitObjectReader(_childrenRaw))
				{
					while (!stream.IsEndOfStream)
					{
						string path, sha;

            // TODO: Make this a little bit less sucky
            string m = stream.ReadWord().GetString();

						FileMode mode = FileMode.FromBits(int.Parse(m));

						path = stream.ReadToNull().GetString();
						sha = Sha.Decode(stream.ReadBytes(20));

            // TODO: Add support for submodules
            if (mode.ObjectType != ObjectType.Tree && mode.ObjectType != ObjectType.Blob)
              continue;

						TreeNode child = Repo.Storage.GetObject<TreeNode>(sha);
						child.Path = path;
            child.Mode = mode;
						child.Parent = this;

						_children.Add(child);
					}
				}
			}
			catch (Exception)
			{
				// Reset _children field, otherwise the object would be in an invalid state
				_children = null;

				throw;
			}
		}


		/// <summary>
		/// Loads the tree from the GitObjectReader. The child objects themselves will be lazy loaded
		/// </summary>
		/// <param name="input">A reader with inflated tree contents</param>
		public override void Deserialize(GitObjectReader input)
		{
			//string sha = Sha.Compute(input);
			//if (SHA != sha)
			//  throw new ShaMismatchException(SHA, sha);

			_childrenRaw = input.ReadToEnd();
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}
	}
}
