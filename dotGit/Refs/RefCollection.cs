using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;

namespace dotGit.Refs
{
	public class RefCollection<T> : InternalWritableList<T>
		where T : Ref
	{
		internal RefCollection()
			:base()
		{	}

		internal RefCollection(int capacity)
			:base(capacity)
		{	}

		public T this[string name]
		{
			get
			{
				return List.First(obj => obj.Name == name);
			}
		}
	}
}
