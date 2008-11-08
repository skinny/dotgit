using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using dotGit;
using System.IO;


namespace Test
{
	[TestFixture]	
	public class RepositoryTests
	{

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RepositoryThrowsArgumentExceptionForNullPath()
		{
			Repository.Open(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RepositoryThrowsArgumentExceptionForEmptyPath()
		{
			Repository.Open(String.Empty);
		}

	}
}
