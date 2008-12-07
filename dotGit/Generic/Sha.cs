using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using dotGit.Objects;

namespace dotGit.Generic
{
  public class Sha
  {
    private static readonly SHA1 _sha = SHA1.Create();

    public static string Compute(byte[] contents)
    {
      byte[] computedHash = _sha.ComputeHash(contents);
      return Decode(computedHash);
    }

    public static string Decode(byte[] sha)
    {
      return BitConverter.ToString(sha).Replace("-", "").ToLower();
    }


    internal static string Compute(GitObjectReader input)
    {
      long oldPosition = input.BaseStream.Position;

      input.Rewind();

      string hash = Compute(input.ReadToEnd());

      input.BaseStream.Position = oldPosition;

      return hash;
    }


    private int[] words = new int[5];
    

    public Sha(string sha)
    {
			SHAString = sha;

      int discarded = 0;
      byte[] b = HexEncoding.GetBytes(sha, out discarded);
     
      words = new int[5];

      FirstByte = (int)b[0];

      words[0] = System.Net.IPAddress.HostToNetworkOrder(System.BitConverter.ToInt32(b, 0));
      words[1] = System.Net.IPAddress.HostToNetworkOrder(System.BitConverter.ToInt32(b, 4));
      words[2] = System.Net.IPAddress.HostToNetworkOrder(System.BitConverter.ToInt32(b, 8));
      words[3] = System.Net.IPAddress.HostToNetworkOrder(System.BitConverter.ToInt32(b, 12));
      words[4] = System.Net.IPAddress.HostToNetworkOrder(System.BitConverter.ToInt32(b, 16));
      
    }

		public int FirstByte
		{
			get;
			private set;
		}

		public string SHAString
		{
			get;
			private set;
		}

		public override string ToString()
		{
			return SHAString;
		}

    internal int CompareTo(int[] data, int idx)
    {

      int result;

      result = words[0].CompareTo(data[idx]);
      if (result != 0)
        return result;

      result = words[1].CompareTo(data[idx + 1]);
      if (result != 0)
        return result;

      result = words[2].CompareTo(data[idx + 2]);
      if (result != 0)
        return result;

      result = words[3].CompareTo(data[idx + 3]);
      if (result != 0)
        return result;

      return words[4].CompareTo(data[idx + 4]);
    }

  }
}