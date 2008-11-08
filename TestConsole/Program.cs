using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.IO;

namespace Git.NET
{
	class Program
	{
		static void Main(string[] args)
		{
			Repository repo = Repository.Open(@"C:\Projects\dotGit\Test\Resources\TestRepo\");
			//dotGit.Objects.Pack.Pack.LoadPack(System.IO.Path.Combine(repo.GitDir.FullName, "objects\\pack\\pack-209b24047b294e1d9a97680d62a5b9e1d4bef33c"));

			Utility.CreateGitDirectoryStructure(@"c:\test.git");

			//Directory.CreateDirectory(@"c:\test\test\test");

			Console.ReadLine();

		}
	}
}
