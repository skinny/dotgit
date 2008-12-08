using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Objects.Storage.PackObjects
{
  internal abstract class Deltified : PackObject
  {
    internal Deltified(long size, ObjectType type, GitPackReader reader)
      : base(size, type)
    {
      Load(reader);
    }

    public abstract byte[] Delta
    {
      get;
      protected set;
    }

    public abstract void Load(GitPackReader reader);
  }
}
