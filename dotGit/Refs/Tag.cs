using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dotGit.Refs
{
	public class Tag : Ref
	{
		internal Tag(Repository repo, string path)
			: base(repo, path)
		{ }
	}
}
