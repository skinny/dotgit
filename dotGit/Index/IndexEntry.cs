using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;

namespace dotGit.Index
{
	public class IndexEntry
	{


		internal IndexEntry(GitObjectStream source)
		{

			// TODO: really parse all the var stuff
			var time = source.ReadBytes(16);


			var dev = source.ReadBytes(4);
			var ino = source.ReadBytes(4);
			var mode = source.ReadBytes(4);
			var uid = source.ReadBytes(4);
			var gid = source.ReadBytes(4);
			var size = source.ReadBytes(4);
			SHA = source.ReadBytes(20).ToSHAString();
			var flags = source.ReadBytes(2);

			Path = Encoding.UTF8.GetString(source.ReadToNull());
						

			
			string rest = Encoding.UTF8.GetString(source.ReadToNextNonNull());
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
	}

}
