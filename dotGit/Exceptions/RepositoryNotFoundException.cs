using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Exceptions
{
	public class RepositoryNotFoundException : DirectoryNotFoundException
	{
		public RepositoryNotFoundException(string message)
			:base(message)
		{	}

		public RepositoryNotFoundException(string message, Exception innerEx)
			: base(message, innerEx)
		{ }

	}
}
