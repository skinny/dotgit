using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Exceptions;
using System.IO;

namespace dotGit.Objects.Storage
{
	public class PackIndexV2
	{
		private static readonly string MAGIC_NUMBER = Encoding.ASCII.GetString(new byte[] { 255, 116, 79, 99 });

		private byte[] fanout;
		private byte[] shas;
		private byte[] crcs;
		private byte[] packFileOffsets;

		internal PackIndexV2(string path)
		{
			Path = path;

			Load();
		}

		public int Version
		{
			get { return 2; }
		}

		public string Path
		{
			get;
			private set;
		}

		public int NumberOfObjects
		{
			get;
			protected set;
		}

		private void Load()
		{
			string magicNumber;
			int version;

			using (GitPackReader reader = new GitPackReader(File.OpenRead(Path)))
			{
				magicNumber = reader.ReadBytes(4).GetString();
				version = reader.ReadBytes(4).Sum(b => b);

				if (Version != version)
					throw new PackFileException(String.Format("Bad version number {0}. Was expecting {1}", version, Version), Path);

				if (MAGIC_NUMBER != magicNumber)
					throw new PackFileException("Invalid header for pack-file. Needs to be: 'PACK'", Path);


				fanout = reader.ReadBytes(1024);

				NumberOfObjects = fanout[fanout.Length - 1];

				shas = reader.ReadBytes(NumberOfObjects * 20);
				crcs = reader.ReadBytes(NumberOfObjects * 4);
				packFileOffsets = reader.ReadBytes(NumberOfObjects * 4);
			}
		}

		public long GetPackFileOffset(string sha)
		{
			//TODO: use fanout table to get to the sha faster

			byte[] firstTwoChars = Encoding.ASCII.GetBytes(sha.Substring(0, 2));

			using (GitObjectReader reader = new GitObjectReader(shas))
			{
				for (long i = 0; i < NumberOfObjects; i++)
				{
					string currentSha = reader.ReadBytes(20).ToSHAString();

					if (currentSha == sha)
					{
						using (GitObjectReader offSetReader = new GitObjectReader(packFileOffsets))
						{
							offSetReader.BaseStream.Position = 4 * i;
							return offSetReader.ReadBytes(4).Sum(b => b);
						}
					}
				}
			}

			throw new ObjectNotFoundException(sha);
		}
	}
}
