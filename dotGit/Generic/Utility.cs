using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace dotGit.Generic
{
	public class Utility
	{
		public static readonly Regex TrailingDotGitExpression = new Regex(@"\.git\/?\Z");
		public static readonly Regex SHAExpression = new Regex(@"([a-f]|\d){40}");
		public static readonly Regex DateTimeRegex = new Regex(@"\s(\d)+(\s(\+|-)(\d){4})?\Z");
		public static readonly DateTime UnixEPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static readonly Regex ContributorRegex = new Regex(@"\s\<[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})\>");


		/// <summary>
		/// Verifies dir is, or contains a .git directory
		/// </summary>
		/// <param name="dir"></param>
		/// <param name="gitDir">This contains a reference to the .git directory</param>
		/// <returns>true when path exists and is or contains a .git directory, otherwise false</returns>
		public static bool IsGitRepository(string path, out DirectoryInfo gitDir)
		{
			DirectoryInfo dir;
			try
			{
				dir = new DirectoryInfo(path);

				// Directory is passed and exist on system?
				if (!dir.Exists)
				{
					gitDir = null;
					return false;
				}
			}
			catch (ArgumentException)
			{ // Gets thrown when null is passed to DirectoryInfo constructor
				gitDir = null;
				return false;
			}

			// Check if trailing ".git" was passed
			if (TrailingDotGitExpression.IsMatch(dir.FullName))
			{ // .git directory found, no need to do anything
				gitDir = dir;
				return true;
			}
			else
			{ // Return .git directory if found in direct descendants of dir
				foreach (DirectoryInfo dirInfo in dir.GetDirectories(".git", SearchOption.TopDirectoryOnly))
				{
					if (dirInfo.Name == ".git")
					{
						gitDir = dirInfo;
						return true;
					}
				}

				// No .git directory found in direct descendants
				gitDir = null;
				return false;
			}
		}

		public static bool IsValidSHA(string input)
		{
			return SHAExpression.IsMatch(input);
		}

		/// <summary>
		/// Creates the structure for a blank git repository
		/// </summary>
		/// <param name="path"></param>
		public static void CreateGitDirectoryStructure(string path)
		{
			bool hideGitDir = false;

			if (!TrailingDotGitExpression.IsMatch(path))
			{
				path = Path.Combine(path, ".git");
				hideGitDir = true;
			}

			DirectoryInfo dotGitDir = null;

			if (!Directory.Exists(path))
				dotGitDir = Directory.CreateDirectory(path);

			// Hide .git dir
			if (hideGitDir)
				dotGitDir.Attributes |= FileAttributes.Hidden;

			string[] directories = new string[] { "hooks", "info", "objects", @"objects\info", @"objects\pack", "refs", @"refs\heads", @"refs\tags" };

			// Create top level directory structure
			foreach (string dirName in directories)
			{
				Directory.CreateDirectory(Path.Combine(path, dirName));
			}

			// Create the info/exclude file
			using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(path, @"info\exclude"), FileMode.Create)))
			{
				sw.WriteLine("# git-ls-files --others --exclude-from=.git/info/exclude");
				sw.WriteLine("# Lines that start with '#' are comments.");
				sw.WriteLine("# For a project mostly in C, the following would be a good set of");
				sw.WriteLine("# exclude patterns (uncomment them if you want to use them):");
				sw.WriteLine("# *.[oa]");
				sw.WriteLine("# *~");
			}

			// Create the config file
			using (StreamWriter sw = File.CreateText(Path.Combine(path, "config")))
			{
				// Leave it empty for now
			}

			// Create the description file
			using (StreamWriter sw = File.CreateText(Path.Combine(path, "description")))
			{
				sw.Write("Unnamed repository; edit this file to name it for gitweb.");
			}

			// Create the HEAD file
			using (StreamWriter sw = File.CreateText(Path.Combine(path, "HEAD")))
			{
				sw.Write("ref: refs/heads/master");
			}
		}

		public static void FileCopy(string srcdir, string destdir, bool recursive)
		{
			DirectoryInfo dir;
			FileInfo[] files;
			DirectoryInfo[] dirs;
			string tmppath;

			//determine if the destination directory exists, if not create it
			if (!Directory.Exists(destdir))
				Directory.CreateDirectory(destdir);

			dir = new DirectoryInfo(srcdir);

			//if the source dir doesn't exist, throw
			if (!dir.Exists)
				throw new ArgumentException("source dir doesn't exist -> " + srcdir);

			//get all files in the current dir
			files = dir.GetFiles();

			//loop through each file
			foreach (FileInfo file in files)
			{
				//create the path to where this file should be in destdir
				tmppath = Path.Combine(destdir, file.Name);

				//copy file to dest dir
				file.CopyTo(tmppath, false);
			}

			//cleanup
			files = null;

			//if not recursive, all work is done
			if (!recursive) return;

			//otherwise, get dirs
			dirs = dir.GetDirectories();

			//loop through each sub directory in the current dir
			foreach (DirectoryInfo subdir in dirs)
			{
				//create the path to the directory in destdir
				tmppath = Path.Combine(destdir, subdir.Name);

				//recursively call this function over and over again
				//with each new dir.
				FileCopy(subdir.FullName, tmppath, recursive);
			}

			//cleanup
			dirs = null;

			dir = null;
		}


		/// <summary>
		/// Check for trailing date like (1225481010 +0100) and returns it + the remainder of the string. If no trailing date is found DateTime.MinValue is returned
		/// </summary>
		/// <param name="input">The string to strip the trailing date from</param>
		/// <param name="remainder">The remainder of input without the date</param>
		/// <returns>The stripped date</returns>
		public static DateTime StripDate(string input, out string remainder)
		{
			Match match = DateTimeRegex.Match(input);
			if (match.Success)
			{
				string capture = match.Captures[0].Value;

				int timeZoneIndex = capture.IndexOfAny(new char[] { '+', '-' });
				if (timeZoneIndex >= 0)
				{
					remainder = input.Substring(0, match.Index);

					//Kind of nasty, but for now the only way I know of to parse the seconds offset Unix EPOCH with timezone offset
					return DateTime.Parse(UnixEPOCH.AddSeconds(long.Parse(capture.Substring(0, timeZoneIndex))).ToString() + " " + capture.Substring(timeZoneIndex));			
				}
			}
			remainder = input;
			return DateTime.MinValue;

		}
	}
}
