using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using dotGit.Generic;

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
			if (String.IsNullOrEmpty(SHA))
				SHA = Sha.Compute(stream);


			//Skip header
			if(stream.IsStartOfStream)
				stream.ReadToNull();

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
