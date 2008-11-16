using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Exceptions
{
	public class ObjectNotFoundException : Exception
	{
		public ObjectNotFoundException(string sha)
			: base(String.Format("Could not find object with id: {0}", sha))
		{
			SHA = sha;
		}

		public string SHA
		{
			get;
			private set;
		}
	}
}
