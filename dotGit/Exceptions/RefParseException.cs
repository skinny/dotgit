using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Exceptions
{
	public class RefParseException : Exception
	{
		public RefParseException(string refLocation, string refContents)
			:base(String.Format("Could not parse ref '{0}' from ref file '{1}'", refContents, refLocation))
		{  }
	}
}
