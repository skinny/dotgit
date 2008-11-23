using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.Collections;
using dotGit.Objects;

namespace dotGit.Objects
{
	public class TreeNodeCollection : InternalWritableList<TreeNode>
	{
		internal TreeNodeCollection()
			: base()
		{ }

		internal TreeNodeCollection(int capacity)
			:base(capacity)
		{

		}
	}
}
