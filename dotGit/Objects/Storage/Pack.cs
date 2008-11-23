using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO = System.IO;
using dotGit.Exceptions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;


namespace dotGit.Objects.Storage
{
	public abstract class Pack
	{
		private Pack() { }

		internal Pack(string path)
		{
			Path = path.TrimEnd(IO.Path.DirectorySeparatorChar);			
		}

		
		public static Pack LoadPack(string path)
		{
			// TODO: Add version parsing to instantiate the right Pack version. Much like GetObject in ObjectStorage
			return new PackV2(path);
		}
		

		public int NumberOfObjects
		{
			get;
			protected set;
		}

		public abstract int Version
		{
			get;
		}

		public string Path
		{
			get;
			private set;
		}

		public abstract PackObject GetObject(string sha);
		
	}
}
