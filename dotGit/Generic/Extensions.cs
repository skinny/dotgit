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
			return BitConverter.ToInt64(input, 0);
		}

		public static string GetString(this byte[] input)
		{
			return Encoding.ASCII.GetString(input);
		}

		public static int GetBits(this byte b, int offset, int count)
		{
			byte buffer = b;

			int result = 0;
			int pow = 1;

			buffer >>= offset;
			for (int i = 0; i < count; ++i)
			{
				if (((byte)1 & buffer) == 1)
				{
					result += pow;
				}

				buffer >>= 1;
				pow *= 2;
			}

			return result;
		}


	}
}
