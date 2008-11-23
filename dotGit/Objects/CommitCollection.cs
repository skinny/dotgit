using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Objects;

namespace dotGit.Objects
{
	public class CommitCollection : InternalWritableList<Commit>
	{
		internal CommitCollection()
			: base()
		{ }

		internal CommitCollection(int capacity)
			: base(capacity)
		{ }

	}
}
