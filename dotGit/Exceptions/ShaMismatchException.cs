using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Exceptions
{
	public class ShaMismatchException : Exception
	{
		public ShaMismatchException(string expectedSha, string receivedSha)
			:base("Expected sha did not match calculated sha. Expected: '{0}', Calculated: '{1}'".FormatWith(expectedSha, receivedSha))
		{

		}
	}
}
