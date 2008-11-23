using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Objects
{
	public class GitObjectReader : BinaryReader
	{
		public GitObjectReader(Stream stream)
			:base(stream, Encoding.ASCII)
		{	}

		public GitObjectReader(byte[] contents)
			:this(new MemoryStream(contents))
		{		}

		public byte[] ReadToNextNonNull()
		{
			return SkipChars('\0');
		}

		public byte[] SkipChars(char charToSkip)
		{
			var bytes = new List<byte>();

			while(PeekChar() == charToSkip)
			{
				bytes.Add(ReadByte());
			}

			return bytes.ToArray();
		}

		public byte[] ReadToChar(char stop)
		{
			var bytes = new List<byte>();

			int current;
			while (true)
			{
				current = ReadChar();
				
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
			BaseStream.Position = 0;
		}

		public byte[] ReadToEnd()
		{
			var bytes = new List<byte>();


			while (!IsEndOfStream)
			{
				bytes.Add((byte)ReadByte());
			}

			return bytes.ToArray();
		}

		public long ReadObjectHeader(out string type)
		{
			if (!IsStartOfStream)
				Rewind();

			long length;

			type = ReadWord().GetString();
			length = ReadToNull().Sum(b => b);
			return length;
		}

		public string GetString(int numberOfBytes)
		{
			return ReadBytes(numberOfBytes).GetString();
		}

		public bool IsEndOfStream
		{
			get { return BaseStream.Position >= BaseStream.Length; }
		}

		public bool IsStartOfStream
		{
			get { return BaseStream.Position == 0; }
		}
	}
}
