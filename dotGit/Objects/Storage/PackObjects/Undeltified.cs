using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;
using dotGit.Refs;

namespace dotGit.Objects.Storage.PackObjects
{
	internal class Undeltified : PackObject
	{

    internal Undeltified(long size, ObjectType type, byte[] content)
      :base(size, type)
    {
      Content = content;
    }

    public byte[] Content
    {
      get;
      private set;
    }

    public IStorableObject ToGitObject(Repository repo, string sha)
    {
      using (GitObjectReader objectReader = new GitObjectReader(Content))
      {
        IStorableObject obj;
        switch (Type)
        {
          case ObjectType.Commit:
              obj = new Commit(repo, sha);
              break;
            case ObjectType.Tree:
              obj = new Tree(repo, sha);
              break;
            case ObjectType.Blob:
              obj = new Blob(repo, sha);
              break;
            case ObjectType.Tag:
              obj = new Tag(repo, sha);
              break;
            default:
              throw new NotImplementedException();
        }

        obj.Deserialize(objectReader);
        return obj;
      }
    }

    internal void ApplyDelta(Deltified delta)
    {
      Content = delta.ApplyDelta(Content);
    }
  }
}
