using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;

namespace dotGit.Objects.Storage.PackObjects
{
  internal class REFDelta : Deltified
	{
    internal REFDelta(string sha, long size, ObjectType type, byte[] content)
      : base(sha, size, type, content)
    { }

    public override byte[] Delta
    {
      get { throw new NotImplementedException(); }
    }

    public string BaseSHA
    {
      get
      {
        return Content.Take(20).ToArray().GetString();
      }
    }
	}
}
