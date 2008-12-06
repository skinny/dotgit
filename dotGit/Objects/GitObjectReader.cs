using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dotGit.Objects.Storage;
using dotGit.Exceptions;

namespace dotGit.Objects
{
	public class GitObjectReader : BinaryReader
	{
		public GitObjectReader(Stream stream)
			: base(stream, Encoding.ASCII)
		{ }

		public GitObjectReader(byte[] contents)
			: this(new MemoryStream(contents))
		{ }

		public byte[] ReadToNextNonNull()
		{
			return SkipChars('\0');
		}

		public byte[] SkipChars(char charToSkip)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				while (PeekChar() == charToSkip)
				{
					ms.WriteByte(ReadByte());
				}

				return ms.ToArray();
			}
		}

		public byte[] ReadToChar(char stop, bool consume)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				while (BaseStream.Position < BaseStream.Length && PeekChar() != stop)
				{
					ms.WriteByte(ReadByte());
				}

				if (consume)
					BaseStream.Position++;

				return ms.ToArray();
			}
		}

		public byte[] ReadWord()
		{
			return ReadToChar(' ', true);
		}

		public byte[] ReadWord(bool consumeSpace)
		{
			return ReadToChar(' ', consumeSpace);
		}

		public byte[] ReadLine()
		{
			return ReadToChar('\n', true);
		}

		public byte[] ReadToNull()
		{
			return ReadToChar('\0', true);
		}

		public void Rewind()
		{
			BaseStream.Position = 0;
		}

		public byte[] ReadToEnd()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				while (!IsEndOfStream)
				{
					ms.WriteByte(ReadByte());
				}

				return ms.ToArray();
			}
		}

		public long ReadObjectHeader(out ObjectType type)
		{
			if (!IsStartOfStream)
				Rewind();

			long length;

			string typeString = ReadWord().GetString();
			switch (typeString)
			{
				case "blob":
					type = ObjectType.Blob;
					break;
				case "commit":
					type = ObjectType.Commit;
					break;
				case "tag":
					type = ObjectType.Tag;
					break;
				case "tree":
					type = ObjectType.Tree;
					break;
				default:
					throw new ParseException("Unknown type: {0}".FormatWith(typeString));
			}
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

		public long Position
		{
			get { return BaseStream.Position; }
			set { BaseStream.Position = value; }
		}
	}
}
