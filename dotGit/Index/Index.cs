using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;
using System.IO;
using dotGit.Exceptions;

namespace dotGit.Index
{
	public class Index
	{
		private static readonly string HEADER = "DIRC";
		private static readonly int[] VERSIONS = new int[] { 2 };

		internal Index(Repository repo)
		{
			Repo = repo;

			using(GitObjectStream stream = new GitObjectStream(File.OpenRead(Path.Combine(Repo.GitDir.FullName, "index"))))
			{
				string header = Encoding.UTF8.GetString(stream.ReadBytes(4));

				if (header != HEADER)
					throw new ParseException("Could not parse Index file. Expected HEADER: '{0}', got: '{1}'".FormatWith(HEADER, header));

				Version = stream.ReadBytes(4).Sum(b => (int)b);

				if (!VERSIONS.Contains(Version))
					throw new ParseException("Unknown version number {0}. Needs to be one of: {1}".FormatWith(Version, String.Join(",", VERSIONS.Select(i => i.ToString()).ToArray())));

				NumberOfEntries = stream.ReadBytes(4).Sum(b => (int)b);


				Entries = new IndexEntryCollection(NumberOfEntries);


				for (int i = 0; i < NumberOfEntries; i++)
				{
					Entries.Add(new IndexEntry(stream));
				}
				
				string indexSHA = stream.ReadToEnd().ToSHAString();
			}
		}

		private Repository Repo
		{ get; set; }


		public int Version
		{
			get;
			private set;
		}

		public int NumberOfEntries
		{
			get;
			private set;
		}

		public IndexEntryCollection Entries
		{
			get;
			private set;
		}
	}
}
