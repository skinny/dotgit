using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using dotGit;
using System.IO;
using dotGit.Objects;
using dotGit.Refs;


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

		[Test]
		public void RepositoryShouldNotThrowExceptionWhenWalkingTree()
		{
			Repository repo = Repository.Open(Global.RestRepositoryPath);
			try
			{
				WalkHistory(repo.HEAD.Commit);
				Assert.IsTrue(true, "Walking the history from HEAD does not throw exception");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(false, "Walking the history should not throw exception, caught: '{0}'".FormatWith(ex));
			}
		}

		[Test]
		public void RepositoryShouldContainTag()
		{
			Repository repo = Repository.Open(Global.RestRepositoryPath);
			try
			{
				Tag t = repo.Tags["0.1-alpha"];
				Assert.IsNotNull(t, "dotGit should not return null when fethcing tag '0.1-alpha'");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(false, "Tag '0.1-alpha' should be in test repository, caught: {0}".FormatWith(ex));
			}
		}

		private Commit WalkHistory(Commit commit)
		{
			if (commit.HasParent)
				return WalkHistory(commit.Parent);
			else
				return commit;
		}

	}
}
