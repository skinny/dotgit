using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;

namespace dotGit.Objects.Storage.PackObjects
{
  public abstract class PackObject
  {
    private PackObject()
    { }

    internal PackObject(long size, ObjectType type)
    {
      Size = size;
      Type = type;
    }

    public long Size
    {
      get;
      private set;
    }

    public ObjectType Type
    {
      get;
      private set;
    }
  }
}
