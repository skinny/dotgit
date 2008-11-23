using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Objects;
using dotGit.Objects;
using System.IO;

namespace dotGit.Index
{
	public class IndexEntry
	{
		internal IndexEntry(GitObjectReader source)
		{

			// TODO: really parse all the var stuff
			Created = new IndexTime(source);
			Modified = new IndexTime(source);

			//var time = source.ReadBytes(16);

			var dev = source.ReadBytes(4);
			var ino = source.ReadBytes(4);
			var mode = source.ReadBytes(4);
			var uid = source.ReadBytes(4);
			var gid = source.ReadBytes(4);
			Size = source.ReadBytes(4).ToLong();
			SHA = source.ReadBytes(20).ToSHAString();


			var flags = source.ReadBytes(2);
			var assumeValid = flags[0].GetBit(0, 1);
			var updateNeeded = flags[0].GetBit(1, 1);
			Stage = (IndexStage)flags[0].GetBit(2, 2);
			
			Path = source.ReadToNull().GetString();

			
			
			string rest = source.ReadToNextNonNull().GetString();
		}

		public string Path
		{
			get;
			private set;
		}

		public string SHA
		{
			get;
			private set;
		}

		public IndexStage Stage
		{
			get;
			private set;
		}

		public IndexTime Created
		{
			get;
			private set;
		}

		public IndexTime Modified
		{
			get;
			private set;
		}

		public long Size
		{
			get;
			private set;
		}
	}

	public enum IndexStage
	{
		Normal,
		Ancestor,
		Our,
		Their
	}

}
