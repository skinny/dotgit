using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit;
using System.Collections;

namespace dotGit.Objects
{
	public class NodeCollection : IEnumerable<Node>
	{
		private List<Node> _nodes = null;

		private NodeCollection() { }

		internal NodeCollection(IEnumerable<Node> nodes)
		{
			_nodes = nodes.ToList<Node>();
		}

		internal void Add(Node node)
		{
			_nodes.Add(node);
		}

		public IEnumerator<Node> GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}

	}
}
