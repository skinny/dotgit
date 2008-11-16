using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Generic
{
	public class GitObjectStream : IDisposable
	{
		private readonly MemoryStream stream;

		public GitObjectStream(byte[] bytes)
		{
			stream = new MemoryStream(bytes);
		}

		public byte[] ReadToNextNonNull()
		{
			byte[] result = ReadToChar('\0');

			// knock it back a byte because we'll have grabbed one extra
			stream.Position--;

			return result;
		}

		public byte[] ReadToChar(char stop)
		{
			var bytes = new List<byte>();

			int current;
			while (true)
			{
				current = stream.ReadByte();

				if (current == -1 || current == stop)
					break;

				bytes.Add((byte)current);
			}

			return bytes.ToArray();
		}

		public byte[] ReadWord()
		{
			return ReadToChar(' ');
		}

		public byte[] ReadLine()
		{
			return ReadToChar('\n');
		}

		public byte[] ReadToNull()
		{
			return ReadToChar('\0');
		}

		public void Rewind()
		{
			stream.Position = 0;
		}

		public byte[] ReadToEnd()
		{
			var bytes = new List<byte>();

			while (!IsEndOfFile)
			{
				bytes.Add((byte)stream.ReadByte());
			}

			return bytes.ToArray();
		}

		public byte[] ReadBytes(int numberOfBytes)
		{
			var bytes = new List<byte>();

			for (int i = 0; i < numberOfBytes; i++)
			{
				bytes.Add((byte)stream.ReadByte());
			}

			return bytes.ToArray();
		}

		public long ReadObjectHeader(out string type)
		{
			if (!IsStartOfFile)
				Rewind();

			long length;

			type = Encoding.UTF8.GetString(ReadWord());
			length = ReadToNull().Sum(b => b);
			return length;
		}

		public void Dispose()
		{
			stream.Dispose();
		}

		public bool IsEndOfFile
		{
			get { return stream.Position >= stream.Length; }
		}

		public bool IsStartOfFile
		{
			get { return stream.Position == 0; }
		}

		public long Position
		{
			get { return stream.Position; }
			set { stream.Position = value; }
		}

		public long Length
		{
			get { return stream.Length; }
		}
	}
}
