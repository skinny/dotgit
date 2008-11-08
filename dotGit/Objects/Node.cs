using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{
	public class Node : GitObject
	{
		private Node() { }

		internal Node(Repository repo, string sha)
			:base(repo, sha)
		{
			
		}
	}
}
