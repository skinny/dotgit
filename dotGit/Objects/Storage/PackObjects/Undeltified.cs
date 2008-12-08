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
        switch (Type)
        {
          case ObjectType.Commit:
            return ObjectStorage.LoadObjectFromContent<Commit>(repo, objectReader, sha, Size);

          case ObjectType.Tree:
            return ObjectStorage.LoadObjectFromContent<Tree>(repo, objectReader, sha, Size);

          case ObjectType.Blob:
            return ObjectStorage.LoadObjectFromContent<Blob>(repo, objectReader, sha, Size);

          case ObjectType.Tag:
            return ObjectStorage.LoadObjectFromContent<Tag>(repo, objectReader, sha, Size);
          default:
            throw new NotImplementedException();
        }
      }
    }

    internal void ApplyDelta(Deltified delta)
    {
      int deltaPtr = 0;

      // Length of the base object (a variable length int).
      //
      int baseLen = 0;
      int c, shift = 0;
      do
      {
        c = delta.Delta[deltaPtr++];
        baseLen |= (c & 0x7f) << shift;
        shift += 7;
      } while ((c & 0x80) != 0);
      if (Content.Length != baseLen)
        throw new ArgumentException("baseData length incorrect");

      // Length of the resulting object (a variable length int).
      //
      int resLen = 0;
      shift = 0;
      do
      {
        c = delta.Delta[deltaPtr++] & 0xff;
        resLen |= (c & 0x7f) << shift;
        shift += 7;
      } while ((c & 0x80) != 0);

      byte[] result = new byte[resLen];
      int resultPtr = 0;
      while (deltaPtr < delta.Delta.Length)
      {
        int cmd = delta.Delta[deltaPtr++] & 0xff;
        if ((cmd & 0x80) != 0)
        {
          // Determine the segment of the base which should
          // be copied into the output. The segment is given
          // as an offset and a length.
          //
          int copyOffset = 0;
          if ((cmd & 0x01) != 0)
            copyOffset = delta.Delta[deltaPtr++] & 0xff;
          if ((cmd & 0x02) != 0)
            copyOffset |= (delta.Delta[deltaPtr++] & 0xff) << 8;
          if ((cmd & 0x04) != 0)
            copyOffset |= (delta.Delta[deltaPtr++] & 0xff) << 16;
          if ((cmd & 0x08) != 0)
            copyOffset |= (delta.Delta[deltaPtr++] & 0xff) << 24;

          int copySize = 0;
          if ((cmd & 0x10) != 0)
            copySize = delta.Delta[deltaPtr++] & 0xff;
          if ((cmd & 0x20) != 0)
            copySize |= (delta.Delta[deltaPtr++] & 0xff) << 8;
          if ((cmd & 0x40) != 0)
            copySize |= (delta.Delta[deltaPtr++] & 0xff) << 16;
          if (copySize == 0)
            copySize = 0x10000;

          Array.Copy(Content, copyOffset, result, resultPtr, copySize);
          resultPtr += copySize;
        }
        else if (cmd != 0)
        {
          // Anything else the data is literal within the delta
          // itself.
          //
          Array.Copy(delta.Delta, deltaPtr, result, resultPtr, cmd);
          deltaPtr += cmd;
          resultPtr += cmd;
        }
        else
        {
          // cmd == 0 has been reserved for future encoding but
          // for now its not acceptable.
          //
          throw new ArgumentException("unsupported command 0");
        }
      }

      Content = result;

    }

	}
}
