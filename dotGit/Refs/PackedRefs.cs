using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dotGit.Refs
{
	internal class PackedRefs
	{
		private Dictionary<string, string> _packedRefs;

		private PackedRefs()
		{ }

		internal PackedRefs(Repository repo)
		{
			Repo = repo;

				Load();
		}

		internal void Load()
		{
			string[] refs = File.ReadAllLines(Path.Combine(Repo.GitDir.FullName, "packed-refs"));
			refs = refs.Where(r => !r.StartsWith("#") && !r.StartsWith("^")).ToArray();

			_packedRefs = new Dictionary<string, string>(refs.Length);

			foreach (string refContent in refs)
			{
				string[] parts = refContent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				string sha = parts[0];
				string path = parts[1];

				_packedRefs.Add(path, sha);
			}
		}

		protected Repository Repo
		{
			get;
			private set;
		}

		public Dictionary<string, string>.KeyCollection Keys
		{
			get { return _packedRefs.Keys; }
		}

		public string this[string path]
		{
			get { return _packedRefs[path]; }
		}
		
	}


}
