using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph.Compatibility {
	/// <summary>
	/// Determines the compatibility between two node item connectors based on the type of the Tag property.
	/// </summary>
	public class InOutTagTypeListCompatibility : ICompatibilityStrategy {
		/// <summary>
		/// Determine if two node item connectors could be connected to each other.
		/// </summary>
		/// <param name="from">From which node connector are we connecting.</param>
		/// <param name="to">To which node connector are we connecting?</param>
		/// <returns><see langword="true"/> if the connection is valid; <see langword="false"/> otherwise</returns>
		public bool CanConnect( NodeConnector from, NodeConnector to ) 
		{
			if (null == from.Item.outputTag || null == to.Item.inputTag)
				return false;
			if (0 == from.Item.outputTag.Length || 0 == to.Item.inputTag.Length)
				return false;

			foreach (var tag in from.Item.outputTag)
			{
				if (to.Item.inputTag.ToList<object> ().Contains (tag))
				{
					return true;
				}
			}
			return false;
		}
	}
}
