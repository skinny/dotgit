using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using IO = System.IO;
using dotGit.Exceptions;
using dotGit.Objects;

namespace dotGit
{
	public class Branch : Ref
	{
		internal Branch(Repository repo, string path)
			: base(repo, path)
		{ }
	}
}
