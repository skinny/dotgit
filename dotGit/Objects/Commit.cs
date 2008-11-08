using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{
	public class Commit : GitObject
	{
		internal Commit(Repository repo, string sha)
			: base(repo, sha)
		{ 
			
		}

		public Tree Tree
		{
			get;
			private set;
		}
	}
}
