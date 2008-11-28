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
        string packHeader;
				int version, numberOfObjects;

				packHeader = reader.ReadBytes(4).GetString();
				version = reader.ReadBytes(4).Sum(b => b);
				numberOfObjects = reader.ReadBytes(4).Sum(b => b);


        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        List<byte> header = new List<byte>();

        while (true)
        {

          //header.Add((byte)System.Net.IPAddress.HostToNetworkOrder((int)reader.ReadByte()));
          header.Add(reader.ReadByte());
          //header.Add((byte)8);

          if (header.Last().GetBits(7, 1) == 0)
            break;
        }



				//byte[] contents = reader.ReadToNull();

				int type = header[0].GetBits(4, 3);
        //System.Collections.Specialized.BitVector32 v = new System.Collections.Specialized.BitVector32(header[0].GetBits(0, 4));

        //System.Collections.BitArray ar = new System.Collections.BitArray((byte)header[0].GetBits(0, 4));

        //(14 << 3) + 7

        int size = header[0].GetBits(0,4);
        for (int idx = header.Count-1; idx > 0 ;  idx--)
        {
          size = (header[idx].GetBits(0, 7) << Convert.ToString(size, 2).Length) + size;
        }
        

//        int size = (int)Convert.ToByte(s);
        byte[] contents = reader.ReadBytes(size);
        using (MemoryStream mStream = Zlib.Decompress(new MemoryStream(contents)))
        {
          string result = Encoding.ASCII.GetString(mStream.ToArray());
          return null;
        }
			}

		//	throw new NotImplementedException();
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
