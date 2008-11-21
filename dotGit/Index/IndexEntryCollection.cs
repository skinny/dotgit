using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Generic;

namespace dotGit.Index
{
	public class IndexEntryCollection : InternalWritableList<IndexEntry>
	{
		internal IndexEntryCollection()
			: base()
		{ }

		internal IndexEntryCollection(int capacity)
			: base(capacity)
		{ }
	}
}
