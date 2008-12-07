using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Objects.Storage.PackObjects
{
  internal abstract class Deltified : PackObject
  {
    internal Deltified(string sha, long size, ObjectType type, byte[] content)
      : base(sha, size, type, content)
    { }

    public abstract byte[] Delta
    {
      get;
    }
  }
}
