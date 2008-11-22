using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO = System.IO;
using dotGit.Exceptions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;


namespace dotGit.Objects.Storage
{
	public class Pack
	{
		private static readonly string HEADER = "PACK";
		private static readonly int[] VERSIONS = new int[] { 2 };
		private static readonly string MAGIC_NUMBER = Encoding.BigEndianUnicode.GetString(new byte[] { 255, 116, 79, 99 });

		private Pack() { }

		internal Pack(string path)
		{
			Path = path.TrimEnd(IO.Path.DirectorySeparatorChar);

			CheckIndexFile();

			if(Version == 2)
				CheckPackFile();
		}

		public static Pack LoadPack(string path)
		{
			return new Pack(path);
		}

		public int NumberOfObjects
		{
			get;
			private set;
		}

		public int Version
		{
			get;
			private set;
		}

		public string Path
		{
			get;
			private set;
		}

		private string PackFilePath
		{
			get { return Path + ".pack"; }
		}

		private string IndexFilePath
		{
			get { return Path + ".idx"; }
		}

		private void CheckPackFile()
		{
			byte[] header = new byte[4];
			byte[] version = new byte[4];
			byte[] numberOfObjects = new byte[4];

			using (IO.FileStream fs = new IO.FileStream(PackFilePath, IO.FileMode.Open, IO.FileAccess.Read))
			{
				fs.Read(header, 0, 4);
				fs.Read(version, 0, 4);
				fs.Read(numberOfObjects, 0, 4);
			}
			
			NumberOfObjects = numberOfObjects.Sum(b => b);

			if (!VERSIONS.Contains(Version))
				throw new PackFileException(String.Format("Unknown version number {0}. Needs to be one of: {1}", Version, String.Join(",", VERSIONS.Select(i => i.ToString()).ToArray())), PackFilePath);

			if (HEADER != Encoding.BigEndianUnicode.GetString(header))
				throw new PackFileException("Invalid header for pack-file. Needs to be: 'PACK'", PackFilePath);
		}

		private void CheckIndexFile()
		{
			byte[] magicNumber = new byte[4];
			byte[] version = new byte[4];

			using (IO.FileStream fs = new IO.FileStream(IndexFilePath, IO.FileMode.Open, IO.FileAccess.Read))
			{
				fs.Read(magicNumber, 0, 4);
				fs.Read(version, 0, 4);
			}

			Version = version.Sum(b => b);

			if (!VERSIONS.Contains(Version))
				throw new PackFileException(String.Format("Unknown version number {0}. Needs to be one of: {1}", Version, String.Join(",", VERSIONS.Select(i => i.ToString()).ToArray())), IndexFilePath);

			if (MAGIC_NUMBER != Encoding.BigEndianUnicode.GetString(magicNumber))
				throw new PackFileException("Invalid header for pack-file. Needs to be: 'PACK'", IndexFilePath);
		}

	}
}
