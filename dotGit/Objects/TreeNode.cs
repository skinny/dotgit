using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{

	/// <summary>
	/// Base class for Tree and Blob. Represents a child of Tree
	/// </summary>
	public abstract class TreeNode : GitObject
	{
		internal TreeNode(Repository repo)
			:base(repo)
		{	}

		internal TreeNode(Repository repo, string sha)
			: base(repo, sha)
		{ }

		/// <summary>
		/// The path of this TreeNode
		/// </summary>
		public string Path
		{
			get;
			internal set;
		}

		/// <summary>
		/// The FileMode of this TreeNode
		/// </summary>
		public FileMode Mode
		{
			get;
			internal set;
		}

		/// <summary>
		/// The parent TreeNode
		/// </summary>
		public TreeNode Parent
		{
			get;
			internal set;
		}

		/// <summary>
		/// Returns true if this TreeNode has a parent TreeNode
		/// </summary>
		public bool HasParent
		{
			get { return Parent != null; }
		}

		/// <summary>
		/// Returns true if this TreeNode is of type Tree
		/// </summary>
		public bool IsTree
		{
			get { return this is Tree; }
		}

		/// <summary>
		/// Returns true if this TreeNode is of type Blob
		/// </summary>
		public bool IsBlob
		{
			get { return this is Blob; }
		}
	}
}
