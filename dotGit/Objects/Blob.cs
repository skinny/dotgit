using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using dotGit.Generic;
using dotGit.Exceptions;

namespace dotGit.Objects
{
	public class Blob : TreeNode
	{
		internal Blob(Repository repo)
			: base(repo)
		{ }

		internal Blob(Repository repo, string sha)
			: base(repo, sha)
		{ }

		public override void Deserialize(GitObjectReader stream)
		{
			Content = stream.ReadToEnd();
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This blob's real contents
		/// </summary>
		public byte[] Content
		{
			get;
			private set;
		}
	}
}
