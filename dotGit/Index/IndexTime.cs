using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;
using dotGit.Objects;

namespace dotGit.Index
{
	public class IndexTime
	{
		internal IndexTime(GitObjectReader input)
		{
			Seconds = input.ReadBytes(4).ToInt();
			NanoSeconds = input.ReadBytes(4).ToInt();
		}


		public int Seconds
		{
			get;
			private set;
		}

		public int NanoSeconds
		{
			get;
			private set;
		}
	}
}
