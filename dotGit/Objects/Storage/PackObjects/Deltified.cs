﻿using System;
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

    internal byte[] ApplyDelta(byte[] baseData)
    {
      int deltaPtr = 0;

      // Length of the base object (a variable length int).
      //
      int baseLen = 0;
      int c, shift = 0;
      do
      {
        c = Delta[deltaPtr++];
        baseLen |= (c & 0x7f) << shift;
        shift += 7;
      } while ((c & 0x80) != 0);
      if (baseData.Length != baseLen)
        throw new ArgumentException("baseData length incorrect");

      // Length of the resulting object (a variable length int).
      //
      int resLen = 0;
      shift = 0;
      do
      {
        c = Delta[deltaPtr++] & 0xff;
        resLen |= (c & 0x7f) << shift;
        shift += 7;
      } while ((c & 0x80) != 0);

      byte[] result = new byte[resLen];
      int resultPtr = 0;
      while (deltaPtr < Delta.Length)
      {
        int cmd = Delta[deltaPtr++] & 0xff;
        if ((cmd & 0x80) != 0)
        {
          // Determine the segment of the base which should
          // be copied into the output. The segment is given
          // as an offset and a length.
          //
          int copyOffset = 0;
          if ((cmd & 0x01) != 0)
            copyOffset = Delta[deltaPtr++] & 0xff;
          if ((cmd & 0x02) != 0)
            copyOffset |= (Delta[deltaPtr++] & 0xff) << 8;
          if ((cmd & 0x04) != 0)
            copyOffset |= (Delta[deltaPtr++] & 0xff) << 16;
          if ((cmd & 0x08) != 0)
            copyOffset |= (Delta[deltaPtr++] & 0xff) << 24;

          int copySize = 0;
          if ((cmd & 0x10) != 0)
            copySize = Delta[deltaPtr++] & 0xff;
          if ((cmd & 0x20) != 0)
            copySize |= (Delta[deltaPtr++] & 0xff) << 8;
          if ((cmd & 0x40) != 0)
            copySize |= (Delta[deltaPtr++] & 0xff) << 16;
          if (copySize == 0)
            copySize = 0x10000;

          Array.Copy(baseData, copyOffset, result, resultPtr, copySize);
          resultPtr += copySize;
        }
        else if (cmd != 0)
        {
          // Anything else the data is literal within the delta
          // itself.
          //
          Array.Copy(Delta, deltaPtr, result, resultPtr, cmd);
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

      return result;
    }

    internal void ApplyDelta(Deltified delta)
    {


      Delta = ApplyDelta(delta.Delta);
    }
  }
}
