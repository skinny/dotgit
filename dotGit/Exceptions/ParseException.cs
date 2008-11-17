using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Exceptions
{
	public class ParseException : Exception
	{
		public ParseException(string message)
			:base(message)
		{  }
	}
}
