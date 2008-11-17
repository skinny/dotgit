using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Test
{
	public class Global
	{
		public static string AssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);

		private static string _testRepositoryPath;
		static Global()
		{
			 _testRepositoryPath = Path.Combine(Global.AssemblyDir, @"Resources\TestRepo");
		}

		public static string RestRepositoryPath
		{
			get { return _testRepositoryPath; }
		}
	}
}
