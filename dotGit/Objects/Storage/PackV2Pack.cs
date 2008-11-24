using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;

namespace dotGit.Objects.Storage
{
	public class PackV2Pack : Pack
	{
		private static readonly string HEADER = "PACK";

		internal PackV2Pack(string path)
			:base(path)
		{
			VerifyPack();
		}

		public override int Version
		{
			get { return 2; }
		}

		public override PackObject GetObject(string sha)
		{
			throw new NotImplementedException();
		}

		public PackObject GetObjectWithOffset(long offset)
		{
			using (GitPackReader reader = new GitPackReader(File.OpenRead(Path)))
			{
				reader.BaseStream.Position = offset;
				byte[] header = reader.ReadBytes(3);
				byte[] contents = reader.ReadToNull();

				int type = header[0].GetBits(1, 3);

				using (MemoryStream mStream = Zlib.Decompress(new MemoryStream(contents)))
				{
					string result = Encoding.ASCII.GetString(mStream.ToArray());
				}
			}

			throw new NotImplementedException();
		}

		private void VerifyPack()
		{
			using (GitPackReader reader = new GitPackReader(File.OpenRead(Path)))
			{
				string header;
				int version, numberOfObjects;

				header = reader.ReadBytes(4).GetString();
				version = reader.ReadBytes(4).Sum(b => b);
				numberOfObjects = reader.ReadBytes(4).Sum(b => b);

				if (Version != version)
					throw new PackFileException(String.Format("Bad version number {0}. Was expecting {1}", version, Version), Path);

				if (HEADER != header)
					throw new PackFileException("Invalid header for pack-file. Needs to be: 'PACK'", Path);
			}
		}
	}
}
