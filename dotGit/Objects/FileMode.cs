using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Objects
{
	public class FileMode
  {
    private readonly static List<FileMode> _list = new List<FileMode>(6);

    #region FileModes
    public static readonly FileMode Tree = new FileMode("Tree", 0040000, ObjectType.Tree);
		
		public static readonly FileMode Symlink = new FileMode("Symbolic link", 0120000, ObjectType.Blob);

		public static readonly FileMode RegularFile = new FileMode("Regular file", 0100644, ObjectType.Blob);

		public static readonly FileMode ExecutableFile = new FileMode("Executable", 0100755, ObjectType.Blob);

		public static readonly FileMode Submodule = new FileMode("Submodule", 0160000, ObjectType.Commit);

		public static readonly FileMode Missing = new FileMode("Missing", 0000000, ObjectType.Bad);
    #endregion FileModes


		private FileMode(string name, int mode, ObjectType type)
		{
      Name = name;
			Bits = mode;
			ObjectType = type;
      

      _list.Add(this);
		}

		public int Bits 
    { 
      get; 
      private set; 
    }
		public ObjectType ObjectType 
    { 
      get; 
      private set; 
    }
    public string Name 
    { 
      get; 
      private set; 
    }

		public static FileMode FromBits(int bits)
		{
      foreach (FileMode mode in _list)
      {
        if (mode.Bits == bits)
          return mode;
      }

      throw new ApplicationException("Unkown FileMode: {0}".FormatWith(bits));

		}
	}
}
