using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;

namespace dotGit.Objects.Storage.PackObjects
{
  internal class REFDelta : Deltified
	{
    internal REFDelta(long size, ObjectType type, GitPackReader reader)
      : base(size, type, reader)
    {  }

    public override byte[] Delta
    {
      get;
      protected set;
    }

    public string BaseSHA
    {
      get;
      private set;
    }

    public override void Load(GitPackReader reader)
    {
      byte[] shaContents = reader.ReadBytes(20);
      
      BaseSHA = Sha.Decode(shaContents);
      Delta = reader.UncompressToLength(Size).ToArray();
    }
	}
}
