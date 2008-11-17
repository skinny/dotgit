using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.Collections;
using dotGit.Generic;

namespace dotGit.Objects
{
	public class NodeCollection : InternalWritableList<Node>
	{
		public NodeCollection()
			: base()
		{ }

		public NodeCollection(int capacity)
			:base(capacity)
		{

		}
	}
}
