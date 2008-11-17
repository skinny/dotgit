using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class Extensions
	{
		public static string FormatWith(this string input, params object[] parameters)
		{
			return String.Format(input, parameters);
		}
	}
}
