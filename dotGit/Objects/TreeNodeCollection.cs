using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.Collections;
using dotGit.Generic;

namespace dotGit.Objects
{
	public class TreeNodeCollection : InternalWritableList<TreeNode>
	{
		public TreeNodeCollection()
			: base()
		{ }

		public TreeNodeCollection(int capacity)
			:base(capacity)
		{

		}
	}
}
