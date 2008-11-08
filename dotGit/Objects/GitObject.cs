using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Objects
{
	public class GitObject
	{
		protected GitObject()
		{ }

		internal GitObject(Repository repo, string sha)
		{
			Repo = repo;
			SHA = sha;
		}

		/// <summary>
		/// The SHA this object is identified by
		/// </summary>
		public string SHA
		{
			get;
			private set;
		}

		public Repository Repo
		{
			get;
			private set;
		}

		public Node Parent
		{
			get;
			protected set;
		}

		public bool HasParent
		{
			get { return Parent != null; }
		}

		private byte[] FindFileContents()
		{
			throw new NotImplementedException();
		}
	}
}
