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

    internal Undeltified(string sha, long size, ObjectType type, byte[] content)
      :base(sha, size, type, content)
    {

    }

    public IStorableObject ToGitObject(Repository repo)
    {
      using (GitObjectReader objectReader = new GitObjectReader(Content))
      {
        switch (Type)
        {
          case ObjectType.Commit:
            return ObjectStorage.LoadObjectFromContent<Commit>(repo, objectReader, SHA, Size);

          case ObjectType.Tree:
            return ObjectStorage.LoadObjectFromContent<Tree>(repo, objectReader, SHA, Size);

          case ObjectType.Blob:
            return ObjectStorage.LoadObjectFromContent<Blob>(repo, objectReader, SHA, Size);

          case ObjectType.Tag:
            return ObjectStorage.LoadObjectFromContent<Tag>(repo, objectReader, SHA, Size);
          default:
            throw new NotImplementedException();
        }
      }
    }

    public static IStorableObject operator +(Undeltified baseObject, Deltified delta)
    {
      throw new NotImplementedException();
    }

	}
}
