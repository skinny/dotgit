using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;


namespace dotGit.Objects.Storage
{
	public class PackV2 : Pack
	{
		internal PackV2(string path)
			:base(path)
		{
			Index = new PackIndexV2(IndexFilePath);
			Pack = new PackV2Pack(PackFilePath);
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


		public override PackObject GetObject(string sha)
		{
			if (Index != null)
			{
				long packFileOffset =	Index.GetPackFileOffset(sha);

				
			}


			throw new NotImplementedException();
		}
	}
}
