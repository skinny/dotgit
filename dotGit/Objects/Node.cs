using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{
	public abstract class Node : GitObject
	{
		internal Node(Repository repo)
			:base(repo)
		{	}

		internal Node(Repository repo, string sha)
			: base(repo, sha)
		{ }

		public string Path
		{
			get;
			internal set;
		}

		public string Mode
		{
			get;
			internal set;
		}

		public Node Parent
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
