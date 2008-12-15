using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using System.Collections.Specialized;
using dotGit.Refs;
using dotGit.Objects.Storage.PackObjects;
using System.Diagnostics;

namespace dotGit.Objects.Storage
{
  public class PackV2Pack : Pack
  {
    private static readonly string HEADER = "PACK";

    internal PackV2Pack(Repository repo, string path)
      : base(repo, path)
    {
      VerifyPack();
    }

    public override int Version
    {
      get { return 2; }
    }

    public override IStorableObject GetObject(string sha)
    {
      // TODO: Walk the entire pack file to find the object. This is used when this pack has no corresponding index file
      throw new NotImplementedException();
    }

    public PackObject GetObjectWithOffset(long offset)
    {
      Debug.WriteLine("Fetching object with offset: {0}".FormatWith(offset));

      using (GitPackReader reader = new GitPackReader(File.OpenRead(Path)))
      {
        // Set stream position to offset
        reader.Position = offset;

        // Read first byte, it contains the type and 4 bits of object length
        byte buffer = reader.ReadByte();
        ObjectType type = (ObjectType)((buffer >> 4) & 7);
        long size = buffer & 0xf;

        // Read byte while 8th bit is 1. 
        int bitCount = 4;
        while ((buffer & 0x80) != 0) // >> 7 == 1);
        {
          buffer = reader.ReadByte();

          size |= ((long)buffer & 0x7f) << bitCount;
          bitCount += 7;

        } 

        if (type == ObjectType.RefDelta)
        {
          return new REFDelta(size, type, reader);
        }
        else if (type == ObjectType.OFSDelta)
        {
          return new OFSDelta(size, type, reader);
        }
        else
        {
          using (MemoryStream inflated = reader.UncompressToLength(size))
          {
            return new Undeltified(size, type, inflated.ToArray());
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
