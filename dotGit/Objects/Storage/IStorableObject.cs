using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotGit.Objects;

namespace dotGit.Objects.Storage
{
	public interface IStorableObject
	{
		/// <summary>
		/// Load object contents from byte array
		/// </summary>
		/// <param name="contents"></param>
		void Deserialize(GitObjectReader contents);

		/// <summary>
		/// Serializes the object to an array of bytes
		/// </summary>
		/// <returns></returns>
		byte[] Serialize();

		/// <summary>
		/// The SHA the object is identified by
		/// </summary>
		string SHA { get; }
	}
}
