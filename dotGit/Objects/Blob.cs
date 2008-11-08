using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Objects
{
	public class Blob : Node
	{
		internal Blob(Repository repo, string sha)
			: base(repo, sha)
		{ }

		public byte[] Data
		{
			get
			{
				return null;
			}
			internal set
			{
				//TODO
				throw new NotImplementedException();
			}
		}
	}
}
