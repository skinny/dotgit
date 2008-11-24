using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using dotGit.Generic;

namespace dotGit
{
	public class Contributer
	{
		private Contributer()
		{	}

		public Contributer(string name)
		{
			Name = name;
		}

		public Contributer(string name, string email)
			:this(name)
		{
			Email = email;
		}

		/// <summary>
		/// Load Contributer from git formatted string.
		/// </summary>
		/// <param name="input">string in format: 'John Doe &lt;john@doe.com&gt;'</param>
		/// <returns>parsed Contributer</returns>
		public static Contributer Parse(string input)
		{
			Match match = Utility.ContributorRegex.Match(input);
			string name, email = String.Empty;
			if (match.Success)
			{ // Found email address
				name = input.Substring(0, match.Index).Trim();
				email = match.Captures[0].Value.Trim(' ', '<', '>');
			}
			else
			{ // No email adress found
				name = input;
			}

			return new Contributer(name, email);
		}

		/// <summary>
		/// Returns the name and email in git format: 'John Doe &lt;john@doe.com&gt;'
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0} <{1}>", Name, Email);
		}

		/// <summary>
		/// The name of this contributer
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// The email address of this contribute
		/// </summary>
		public string Email
		{
			get;
			private set;
		}
	}
}
