using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{
	public abstract class TreeNode : GitObject
	{
		internal TreeNode(Repository repo)
			:base(repo)
		{	}

		internal TreeNode(Repository repo, string sha)
			: base(repo, sha)
		{ }

		public string Path
		{
			get;
			internal set;
		}

		public FileMode Mode
		{
			get;
			internal set;
		}

		public TreeNode Parent
		{
			get;
			internal set;
		}

		public bool HasParent
		{
			get { return Parent != null; }
		}


		public bool IsTree
		{
			get { return this is Tree; }
		}

		public bool IsBlob
		{
			get { return this is Blob; }
		}
	}
}
