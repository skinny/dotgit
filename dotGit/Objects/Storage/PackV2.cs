using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Exceptions;
using dotGit.Generic;
using dotGit.Objects.Storage.PackObjects;


namespace dotGit.Objects.Storage
{
  public class PackV2 : Pack
  {
    internal PackV2(Repository repo, string path)
      : base(repo, path)
    {
      Index = new PackIndexV2(IndexFilePath);
      Pack = new PackV2Pack(Repo, PackFilePath);
    }

    public override int Version
    {
      get { return 2; }
    }

    private string PackFilePath
    {
      get { return Path + ".pack"; }
    }

    private string IndexFilePath
    {
      get { return Path + ".idx"; }
    }

    private PackIndexV2 Index
    {
      get;
      set;
    }

    private PackV2Pack Pack
    {
      get;
      set;
    }

    public override IStorableObject GetObject(string sha)
    {
      if (Index != null)
      {
        long packFileOffset = Index.GetPackFileOffset(new Sha(sha));
        PackObject obj = Pack.GetObjectWithOffset(sha, packFileOffset);

        if (obj is Undeltified)
        {
          return ((Undeltified)obj).ToGitObject(Repo);
        }
        else if (obj is Deltified)
        {
          long basePackFileOffset;
          Undeltified baseObject;
          if (obj is REFDelta)
          {
            string baseSha = ((REFDelta)obj).BaseSHA;
            basePackFileOffset = Index.GetPackFileOffset(new Sha(baseSha));
            baseObject = (Undeltified)Pack.GetObjectWithOffset(baseSha, basePackFileOffset);
          }
          else
          {
            basePackFileOffset = packFileOffset - ((OFSDelta)obj).BackwardsBaseOffset;
            throw new NotImplementedException();    
          }

          return baseObject + (Deltified)obj;
        }
        else
        {
          throw new ApplicationException("Don't know what to do with: {0}".FormatWith(obj.GetType().FullName));
        }
      }
      else
      {
        return Pack.GetObject(sha);
      }
    }
  }
}
