using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using dotGit.Generic;


namespace dotGit.Objects.Storage
{
	public class PackV2 : Pack
	{
		internal PackV2(Repository repo, string path)
			:base(repo, path)
		{
			Index = new PackIndexV2(IndexFilePath);
			Pack = new PackV2Pack(Repo, PackFilePath);
		}

		public override int Version
		{
			get { return 2; }
		}

		private string PackFilePath
		{
			get { return Path + ".pack"; }
		}

		private string IndexFilePath
		{
			get { return Path + ".idx"; }
		}

		private PackIndexV2 Index
		{
			get;
			set;
		}

		private PackV2Pack Pack
		{
			get;
			set;
		}


		public override IStorableObject GetObject(string sha)
		{
			if (Index != null)
			{
				long packFileOffset = Index.GetPackFileOffset(new Sha(sha));
				return Pack.GetObjectWithOffset(sha, packFileOffset);
			}
			else
			{
				return Pack.GetObject(sha);
			}
		}
	}
}
