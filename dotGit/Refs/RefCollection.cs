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
				try
				{
					return List.First(obj => obj.Name == name);
				}
				catch (InvalidOperationException)
				{ // No object was found by that name
					throw new IndexOutOfRangeException("No {0} found with name: {1}".FormatWith(typeof(T).Name, name));
				}
			}
		}
	}
}
