using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace dotGit.Objects.Pack
{
	public class PackObjectCollection : IEnumerable<PackObject>
	{
		private List<PackObject> Objects
		{
			get;
			set;
		}

		public IEnumerator<PackObject> GetEnumerator()
		{
			return Objects.GetEnumerator();
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return Objects.GetEnumerator();
		}

	}
}
