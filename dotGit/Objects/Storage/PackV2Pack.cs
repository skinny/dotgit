using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using System.Collections.Specialized;
using dotGit.Refs;

namespace dotGit.Objects.Storage
{
	public class PackV2Pack : Pack
	{
		private static readonly string HEADER = "PACK";

		internal PackV2Pack(Repository repo, string path)
			:base(repo, path)
		{
			VerifyPack();
		}

		public override int Version
		{
			get { return 2; }
		}

		public override IStorableObject GetObject(string sha)
		{
			throw new NotImplementedException();
		}

		public IStorableObject GetObjectWithOffset(string sha, long offset)
		{
			if (!Utility.IsValidSHA(sha))
				throw new ArgumentException("Must have valid sha", "sha");

			using (GitPackReader reader = new GitPackReader(File.OpenRead(Path)))
			{
				// Set stream position to offset
				reader.Position = offset;
				
				// Read first byte, it contains the type and 4 bits of object length
				byte buffer = reader.ReadByte();
				ObjectType type = (ObjectType)((buffer >> 4) & 7);
				long size = buffer & 0xf;// >> 4;

				// Read byte while 8th bit is 1. 
				int bitCount = 4;
				do
				{
					buffer = reader.ReadByte();

					size |= (long)(buffer & 0x7f) << bitCount;
					bitCount += 7;

				} while (buffer >> 7 == 1);


				using (MemoryStream inflated = reader.UncompressToLength(size))
        {
					using(GitObjectReader objectReader = new GitObjectReader(inflated))
					{
						objectReader.Rewind();
						switch (type)
						{
							case ObjectType.Commit:
								return ObjectStorage.LoadObjectFromContent<Commit>(Repo, objectReader, sha, size);

							case ObjectType.Tree:
								return ObjectStorage.LoadObjectFromContent<Tree>(Repo, objectReader, sha, size);

							case ObjectType.Blob:
								return ObjectStorage.LoadObjectFromContent<Blob>(Repo, objectReader, sha, size);

							case ObjectType.Tag:
								return ObjectStorage.LoadObjectFromContent<Tag>(Repo, objectReader, sha, size);
							default:
								throw new NotImplementedException();
						}

						
					}
        }
			}
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
