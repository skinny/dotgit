using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Objects;
using dotGit.Exceptions;
using dotGit.Generic;

namespace dotGit
{
	public static class Extensions
	{
		public static string FormatWith(this string input, params object[] parameters)
		{
			return String.Format(input, parameters);
		}

		public static string ToSHAString(this byte[] input)
		{
			string sha = Sha.Decode(input);
			if (!Utility.IsValidSHA(sha))
				throw new ParseException("Invalid sha: {0}".FormatWith(sha));

			return sha;
		}

		public static int ToInt(this byte[] input)
		{
			return BitConverter.ToInt32(input, 0);
		}

		public static long ToLong(this byte[] input)
		{
			return BitConverter.ToInt32(input, 0);
		}

		public static string GetString(this byte[] input)
		{
			return Encoding.ASCII.GetString(input);
		}

		public static int GetBits(this byte b, int offset, int count)
		{
			int result = 0;
			int pow = 1;

			b >>= offset;
			for (int i = 0; i < count; ++i)
			{
				if (((byte)1 & b) == 1)
				{
					result += pow;
				}

				b >>= 1;
				pow *= 2;
			}

			return result;
		}

		/*
		public static long decodeUInt32(this byte[] intbuf, int offset)
		{
			int low = (intbuf[offset + 1] & 0xff) << 8;
			low |= (intbuf[offset + 2] & 0xff);
			low <<= 8;

			low |= (intbuf[offset + 3] & 0xff);

			return ((long)(intbuf[offset] & 0xff)) << 24 | low;
		}
		*/
		//public static int ToInt32(this byte[] input, int offset)
		//{

		//  return BitConverter.ToInt32(input, offset);
		//}
	}
}
