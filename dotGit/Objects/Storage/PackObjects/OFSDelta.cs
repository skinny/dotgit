using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Objects.Storage.PackObjects
{
  internal class OFSDelta : Deltified
  {
    internal OFSDelta(long size, ObjectType type, GitPackReader reader)
      : base(size, type, reader)
    { }

    public override byte[] Delta
    {
      get;
      protected set;
    }

    public long BackwardsBaseOffset
    {
      get;
      private set;
    }

    public override void Load(GitPackReader reader)
    {
      byte buffer = reader.ReadByte();
      //ObjectType type = (ObjectType)((buffer >> 4) & 7);
      long baseOffset = buffer & 0xf;

      // Read byte while 8th bit is 1. 
      while ((buffer & 0x80) != 0)
      {
        buffer = reader.ReadByte();
        baseOffset += 1;
        baseOffset <<= 7;

        baseOffset |= ((long)buffer & 0x7f);

      }

      Delta = reader.UncompressToLength(Size).ToArray();

      BackwardsBaseOffset = baseOffset;
    }
  }
}
