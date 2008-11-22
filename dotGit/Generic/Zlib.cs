using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Generic
{
	public class Zlib
	{
		public static MemoryStream Decompress(string path)
		{
			return Decompress(File.Open(path, FileMode.Open, FileAccess.Read));
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

			// reset output stream to start so we can read it to a string
			output.Position = 0;

			byte[] content = new byte[output.Length];

			output.Read(content, 0, (int)output.Length);

			return output;
		}

	}
}