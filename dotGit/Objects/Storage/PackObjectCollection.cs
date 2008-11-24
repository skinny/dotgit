using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using dotGit.Objects;
using dotGit.Generic;

namespace dotGit.Objects.Storage
{
	public class PackObjectCollection : InternalWritableList<PackObject>
	{
		internal PackObjectCollection()
			: base()
		{ }

		internal PackObjectCollection(int capacity)
			:base(capacity)
		{

		}
	}
}
