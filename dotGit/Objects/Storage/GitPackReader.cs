using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Objects.Storage
{
	public class GitPackReader : GitObjectReader
	{
		public GitPackReader(Stream stream)
			:base(stream)
		{	}

		public GitPackReader(byte[] contents)
			:base(contents)
		{		}

		public MemoryStream UncompressToLength(long destLength)
		{
			return Zlib.Decompress(this, destLength);
		}
	}
}
