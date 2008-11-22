using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotGit.Generic
{
	public class InternalWritableList<T> : IEnumerable<T>
	{
		internal InternalWritableList()
		{
			List = new List<T>();
		}

		internal InternalWritableList(int capacity)
		{
			List = new List<T>(capacity);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return List.GetEnumerator();
		}

		internal void Add(T item)
		{
			List.Add(item);
		}

		protected List<T> List
		{
			get;
			private set;

		}

		public int Count
		{
			get { return List.Count; }
		}
	}
}
