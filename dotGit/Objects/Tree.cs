using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{
	public class Tree : Node
	{
		internal Tree(Repository repo, string sha)
			: base(repo, sha)
		{ }

		internal Tree(Repository repo, string sha, NodeCollection children) :
			base(repo, sha)
		{
			Children = children;
		}

		public NodeCollection Children
		{
			get;
			private set;
		}
	}
}
