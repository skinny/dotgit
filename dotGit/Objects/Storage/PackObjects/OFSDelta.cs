using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Objects.Storage.PackObjects
{
  internal class OFSDelta : Deltified
  {
    internal OFSDelta(string sha, long size, ObjectType type, byte[] content)
      : base(sha, size, type, content)
    { }

    public override byte[] Delta
    {
      get { throw new NotImplementedException(); }
    }

    public long BackwardsBaseOffset
    {
      get
      {
        using (GitObjectReader reader = new GitObjectReader(Content))
        {
          byte buffer = reader.ReadByte();
          ObjectType type = (ObjectType)((buffer >> 4) & 7);
          long baseOffset = buffer & 0xf;

          // Read byte while 8th bit is 1. 
          do
          {
            buffer = reader.ReadByte();
            baseOffset += 1;
            baseOffset <<= 7;

            baseOffset |= ((long)buffer & 0x7f);

          } while (buffer >> 7 == 1);

          return baseOffset;
        }
      }
    }
  }
}
