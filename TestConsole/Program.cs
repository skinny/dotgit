using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.IO;
using dotGit.Objects;
using System.Security.Cryptography;
using System.Globalization;

namespace Git.NET
{
	class Program
	{
		static void Main(string[] args)
		{
			Repository repo = Repository.Open(@"C:\Projects\dotGit\Test\Resources\TestRepo");

			//dotGit.Objects.Storage.Pack.LoadPack(System.IO.Path.Combine(repo.GitDir.FullName, "objects\\pack\\pack-209b24047b294e1d9a97680d62a5b9e1d4bef33c"));

			Console.WriteLine(repo.HEAD.Commit.Tree.Children);
			Commit obj = repo.Storage.GetObject<Commit>("5202e973f3d38c583b1a4645d23a638acc012c41");

		
			
			Console.ReadLine();

		}

		public string getSHA1(string userPassword) { return BitConverter.ToString(SHA1Managed.Create().ComputeHash(Encoding.Default.GetBytes(userPassword))).Replace("-", ""); }
	}


}
