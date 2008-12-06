using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.IO;
using dotGit.Objects;
using System.Security.Cryptography;
using System.Globalization;
using dotGit.Objects.Storage;
using dotGit.Refs;
using dotGit.Index;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Repository repo = Repository.Open(@"C:\Projects\dotGit");
			Console.WriteLine(repo.Storage);
			
			//dotGit.Objects.Storage.Pack.LoadPack(System.IO.Path.Combine(repo.GitDir.FullName, "objects\\pack\\pack-209b24047b294e1d9a97680d62a5b9e1d4bef33c"));
			
			//Tag firstTag = repo.Tags["0.1-alpha"];
			//Branch master = repo.Branches["master"];
			Console.WriteLine(repo.HEAD.Commit);

			//Index idx = repo.Index;

			//TreeNodeCollection nodes = repo.HEAD.Commit.Tree.Children;

			//Commit obj = repo.Storage.GetObject<Commit>("5202e973f3d38c583b1a4645d23a638acc012c41");
			//Blob b = repo.Storage.GetObject<Blob>("58d11b659b2c71a0cbdc0d037e237243cfec8a88");

			//IStorableObject o = repo.Storage.GetObject("54040746289fd8beff929bcdcdd2b33be8a3700b");

			//foreach (Tag tag in repo.Tags)
			//{
			//				Console.WriteLine(String.Format("Tag: '{0}' points to {1}", tag.Name, tag.SHA));
			//}

			
			Console.ReadLine();




		}
	}


}
