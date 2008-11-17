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
			:base(repo, sha)
		{ }

		public override void Deserialize(byte[] contents)
		{
			if (String.IsNullOrEmpty(SHA))
				SHA = Sha.Compute(contents);

			using (GitObjectStream stream = new GitObjectStream(contents))
			{
				//Skip header
				stream.ReadToNull();

				Content = stream.ReadToEnd();
			}
		}

		public override byte[] Serialize()
		{
			throw new NotImplementedException();
		}

		public byte[] Content
		{
			get;
			private set;
		}
	}
}
