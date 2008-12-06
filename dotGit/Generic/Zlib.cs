using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using zlib;

namespace dotGit.Objects
{
	public class Zlib
	{
		public static MemoryStream Decompress(string path)
		{
			return Decompress(File.OpenRead(path));
		}

		public static MemoryStream Decompress(Stream input)
		{
			var output = new MemoryStream();
			var zipStream = new zlib.ZOutputStream(output);

			using (input)
			{
				var buffer = new byte[2000];
				int len;

				while ((len = input.Read(buffer, 0, 2000)) > 0)
				{
					zipStream.Write(buffer, 0, len);
				}
			}

			output.Position = 0;

			byte[] content = new byte[output.Length];

			output.Read(content, 0, (int)output.Length);

			return output;
		}

		public static MemoryStream Decompress(BinaryReader input, long destLength)
		{
			int bufferLength = 4;

			MemoryStream output = new MemoryStream();
			ZOutputStream zipStream = new ZOutputStream(output);
			
			byte[] buffer = new byte[bufferLength];

			while (output.Length < destLength)
			{
				input.Read(buffer, 0, bufferLength);
				zipStream.Write(buffer, 0, bufferLength);
			}


			return output;
		}

	}
}