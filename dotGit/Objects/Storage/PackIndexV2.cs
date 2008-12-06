using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Exceptions;
using System.IO;
using dotGit.Generic;

namespace dotGit.Objects.Storage
{
  public class PackIndexV2
  {
    private static readonly string MAGIC_NUMBER = Encoding.ASCII.GetString(new byte[] { 255, 116, 79, 99 });

    private static readonly int FANOUT = 256;

    private int[] fanout;
    private int[][] shas;
    private byte[][] crcs;
    private byte[][] offsets;


    internal PackIndexV2(string path)
    {
      Path = path;

      Load();
    }

    #region Properties

    public int Version
    {
      get { return 2; }
    }

    public string Path
    {
      get;
      private set;
    }

    public int NumberOfObjects
    {
      get;
      protected set;
    }

    #endregion


    public static int decodeInt32(byte[] intbuf, int offset)
    {
      int r = intbuf[offset] << 8;

      r |= intbuf[offset + 1] & 0xff;
      r <<= 8;

      r |= intbuf[offset + 2] & 0xff;
      return (r << 8) | (intbuf[offset + 3] & 0xff);
    }


    private void Load()
    {
      string magicNumber;
      int version;

      using (GitPackReader reader = new GitPackReader(File.OpenRead(Path)))
      {
        magicNumber = reader.ReadBytes(4).GetString();
        version = reader.ReadBytes(4).Sum(b => b);

        if (Version != version)
          throw new PackFileException(String.Format("Bad version number {0}. Was expecting {1}", version, Version), Path);

        if (MAGIC_NUMBER != magicNumber)
          throw new PackFileException("Invalid header for pack-file. Needs to be: 'PACK'", Path);

        #region Fanout Table

        fanout = new int[FANOUT];
        shas = new int[FANOUT][];
        offsets = new byte[FANOUT][];
        crcs = new byte[FANOUT][];

        // one big read, faster as 256 small 4 byte read statements ?
        byte[] fanoutRaw = new byte[FANOUT * 4];
        fanoutRaw = reader.ReadBytes(FANOUT * 4);

        for (int idx = 0; idx < FANOUT; idx++)
          fanout[idx] = System.Net.IPAddress.HostToNetworkOrder(BitConverter.ToInt32(fanoutRaw, idx * 4));


        #endregion

        NumberOfObjects = fanout[FANOUT - 1];


        #region SHA's

        for (int idx = 0; idx < FANOUT; idx++)
        {
          int bucketCount;

          if (idx == 0)
            bucketCount = fanout[idx];
          else
            bucketCount = fanout[idx] - fanout[idx - 1];

          if (bucketCount == 0)
          {
            shas[idx] = new int[] { };
            crcs[idx] = new byte[] { };
            offsets[idx] = new byte[] { };
            continue;
          }


          int recordLength = bucketCount * 20;

          int[] bin = new int[recordLength >> 2];
          byte[] rawRecord = reader.ReadBytes(recordLength);

          for (int i = 0; i < bin.Length; i++)
          {
            bin[i] = System.Net.IPAddress.HostToNetworkOrder(BitConverter.ToInt32(rawRecord, i << 2));
          }

          shas[idx] = bin;

          offsets[idx] = new byte[bucketCount * 4];
          crcs[idx] = new byte[bucketCount * 4];

        }


        #endregion

        #region CRC32

        for (int idx = 0; idx < FANOUT; idx++)
          crcs[idx] = reader.ReadBytes(crcs[idx].Length);

        #endregion

        #region 32 bit offset table

        for (int idx = 0; idx < FANOUT; idx++)
          offsets[idx] = reader.ReadBytes(offsets[idx].Length);

        #endregion

        // TODO: Support 64 bit tables



        string packChecksum = reader.ReadBytes(20).GetString();
        string idxChecksum = reader.ReadBytes(20).GetString();

      }
    }

    private int SearchLevelTwo(Sha sha, int index)
    {
      int[] data = shas[index];

      int high = offsets[index].Length >> 2;
      if (high == 0)
        return -1;

      int low = 0;
      while (low < high)
      {
        int mid = (low + high) >> 1;
        int mid4 = mid << 2;
        int cmp;

        cmp = sha.CompareTo(data, mid4 + mid);
        if (cmp < 0)
          high = mid;
        else if (cmp == 0)
          return mid;
        else
          low = mid + 1;
      }


      return -1;
    }
   


    public int GetPackFileOffset(Sha sha)
    {
      int levelTwo = SearchLevelTwo(sha, sha.FirstByte);

      return System.Net.IPAddress.HostToNetworkOrder(BitConverter.ToInt32(offsets[sha.FirstByte], levelTwo << 2));

    }

  }
}
