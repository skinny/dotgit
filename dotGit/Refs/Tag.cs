using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using dotGit.Objects.Storage;
using dotGit.Generic;
using IO = System.IO;
using dotGit.Objects;
using dotGit.Exceptions;

namespace dotGit.Refs
{
	public class Tag : Ref, IStorableObject
	{
		internal Tag(Repository repo)
			: base(repo)
		{ }

		internal Tag(Repository repo, string sha)
			: this(repo)
		{
			SHA = sha;
		}

		public string SHA
		{
			get;
			private set;
		}

		internal static Tag GetTag(Repository repo, string path)
		{
			IO.FileInfo refFile = Ref.GetRefFile(repo, path);

			string sha = IO.File.ReadAllText(refFile.FullName).Trim();
			if(!Utility.IsValidSHA(sha))
				throw new ParseException("Tag does not contain valid sha");


			IStorableObject obj = repo.Storage.GetObject(sha);


			// This is kind of flaky but for now the only way we can distinguish a regular and a lightweight tag
			if (obj is Tag)
			{
				((Tag)obj).Path = path;
				return (Tag)obj;
			}
			else
			{
				Tag t = new Tag(repo, sha);
				t.Object = obj;
				t.Path = path;
				return t;
			}		
		}

		public IStorableObject Object
		{
			get;
			private set;
		}

		public Contributer Tagger
		{
			get;
			private set;
		}

		public DateTime TagDate
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public bool IsAnnotated
		{
			get { return !String.IsNullOrEmpty(Message) && Tagger != null && TagDate != null; }
		}

		public void Deserialize(byte[] contents)
		{
			if (String.IsNullOrEmpty(SHA))
				SHA = Sha.Compute(contents);


			using (GitObjectStream stream = new GitObjectStream(contents))
			{
				string sha;
				if (Utility.IsValidSHA(Encoding.UTF8.GetString(stream.ReadBytes(20)), out sha))
				{ // Tag contains a regular SHA so we can assume it's a commit
					Object = Repo.Storage.GetObject(sha);
					return;
				}
				else
				{
					stream.Rewind();

					// Skip header
					stream.ReadToNull();

					// Skip object keyword
					stream.ReadWord();

					string objectSha;
					objectSha = Encoding.UTF8.GetString(stream.ReadLine()).Trim();
					if (!Utility.IsValidSHA(objectSha))
						throw new ParseException("Invalid sha from tag content");

					// Load object; a ParseException will be thrown for unknown types
					Object = Repo.Storage.GetObject(objectSha);

					// Skip type and tag
					stream.ReadLine(); stream.ReadLine();

					// Tagger
					stream.ReadWord();
					string taggerLine = Encoding.UTF8.GetString(stream.ReadLine());
					TagDate = Utility.StripDate(taggerLine, out taggerLine);
					Tagger = Contributer.Parse(taggerLine);
				
					//Skip extra '\n' and read message
					stream.ReadBytes(1);
					Message = Encoding.UTF8.GetString(stream.ReadToEnd()).TrimEnd();


				}
			}
		}

		public byte[] Serialize()
		{
			throw new NotImplementedException();
		}
	}
}
