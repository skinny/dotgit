using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Objects.Storage
{
	public class GitPackReader : BinaryReader
	{
		public GitPackReader(Stream stream)
			:base(stream, Encoding.ASCII)
		{	}

		public GitPackReader(byte[] contents)
			:this(new MemoryStream(contents))
		{		}
	}
}
