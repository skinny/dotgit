using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using dotGit;
using System.IO;
using System.Reflection;
using dotGit.Generic;

namespace Test
{
	[TestFixture]
	public class UtilityTest
	{
		private string _testRepositoryPath = null;
		private string _fullTempTestRepoName = null;
		private string _tempTestRepoName = @"Resources\UtilityTestTempRepoPath";

		[SetUp]
		public void Setup()
		{
			_testRepositoryPath = Path.Combine(Global.AssemblyDir, @"Resources\TestRepo");
			_fullTempTestRepoName = Path.Combine(Global.AssemblyDir, _tempTestRepoName);

			if (Directory.Exists(_fullTempTestRepoName))
				Directory.Delete(_fullTempTestRepoName, true);

			Directory.CreateDirectory(_fullTempTestRepoName);
		}

		[TearDown]
		public void TearDown()
		{
			if (Directory.Exists(_fullTempTestRepoName))
				Directory.Delete(_fullTempTestRepoName, true);
		}

		[Test]
		public void IsGitRepositoryShouldReturnFalseOnInvalidPath()
		{
			DirectoryInfo gitDir;
			Assert.IsFalse(Utility.IsGitRepository(null, out gitDir), "IsGitRepository must return false when null is passed as path");
			Assert.IsTrue(gitDir == null, "IsGitRepository must return set dir to null if path is invalid");
		}

		[Test]
		public void IsGitRepositoryShouldReturnTrueOnValidPath()
		{
			DirectoryInfo gitDir;
			Assert.IsTrue(Utility.IsGitRepository(_testRepositoryPath, out gitDir), "IsGitRepository must return true for a valid path to a Git repository");
			Assert.IsTrue(gitDir != null && gitDir.Exists, "IsGitRepository cannot return dir as null when true was returned");
		}

		[Test]
		public void CreateGitDirectoryStructureShouldCreateTheRightStructure()
		{
			// All the directories that should be created when calling Utility.CreateGitDirectoryStructure
			string[] directories = new string[] { "hooks", "info", "objects", @"objects\info", @"objects\pack", "refs", @"refs\heads", @"refs\tags" };
			string dotGitDirectory = Path.Combine(_fullTempTestRepoName, ".git");

			// Create .git directory structure
			Utility.CreateGitDirectoryStructure(dotGitDirectory);

			foreach (string dirName in directories)
				Assert.IsTrue(Directory.Exists(Path.Combine(dotGitDirectory, dirName)), String.Format("CreateGitDirectoryStructure must create '{0}' directory in the new .git dir", dirName));

			// Check for info\exclude
			Assert.IsTrue(
					File.Exists(Path.Combine(dotGitDirectory, @"info\exclude")),
					@"CreateGitDirectoryStructure must create a default info\exclude file"
				);

			// Check for config file
			Assert.IsTrue(
					File.Exists(Path.Combine(dotGitDirectory, "config")),
					"CreateGitDirectoryStructure must create a default config file"
				);

			// Check for description file
			Assert.IsTrue(
					File.Exists(Path.Combine(dotGitDirectory, "description")),
					"CreateGitDirectoryStructure must create a default description file"
				);

			// Check for HEAD file
			Assert.IsTrue(
				File.Exists(Path.Combine(dotGitDirectory, "HEAD")), 
				"CreateGitDirectoryStructure must create a default HEAD file"
				);

			// Check contents of HEAD file
			Assert.IsTrue(
				File.ReadAllText(Path.Combine(dotGitDirectory, "HEAD")) == "ref: refs/heads/master", 
				"CreateGitDirectoryStructure should create HEAD file with 'ref: refs/heads/master' as contents"
				);
		}

	}
}
