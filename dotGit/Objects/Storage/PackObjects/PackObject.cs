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

    internal PackObject(string sha, long size, ObjectType type, byte[] content)
    {
      SHA = sha;
      Size = size;
      Type = type;
      Content = content;
    }

    public string SHA
    {
      get;
      private set;
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

    public byte[] Content
    {
      get;
      private set;
    }

	}
}
