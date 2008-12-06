using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Objects.Storage
{
	public abstract class PackObject
	{
	}

	public enum PackObjectType
	{
		OBJ_COMMIT = 1,
		OBJ_TREE = 2,
		OBJ_BLOB = 3,
		OBJ_TAG = 4,
		OBJ_OFS_DELTA = 6,
		OBJ_REF_DELTE = 7
	}
}
