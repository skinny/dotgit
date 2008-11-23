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
	}
}
