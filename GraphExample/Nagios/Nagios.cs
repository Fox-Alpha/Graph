using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphnodes.Nagios.Node;
using Graphnodes.Nagios.NodeItem;
using Graph;
using Graph.Items;
using Newtonsoft.Json;


namespace GraphNodes.Nagios
{
	[JsonObject (MemberSerialization.OptIn, Title = "nagios")]
	public class Nagios
	{
		[JsonProperty ("")]
		List<string> listNagiosObjects;
		[JsonProperty ("")]
		List<NagiosNode> listNagiosNodes;
		[JsonProperty ("")]
		List<NagiosNodeItem> listNagiosNodeItems;

		public Nagios ()
		{
		}
	}
}

namespace Graphnodes.Nagios.Node
{
	[JsonObject (MemberSerialization.OptIn, Title = "nagiosnodes")]
	public class NagiosNode
	{
		[JsonProperty ("")]
		List<string> listNagiosNodeItems;

		public NagiosNode()
		{

		}
	}
}

namespace Graphnodes.Nagios.NodeItem
{
	[JsonObject (MemberSerialization.OptIn, Title = "nagiosnodeitems")]
	public class NagiosNodeItem
	{
		[JsonProperty ("")]
		List<NagiosNodeItemObject> nagiosImportantNodeItemObjects;
		[JsonProperty ("")]
		List<NagiosNodeItemObject> nagiosAdditionalNodeItemObjects;

		public NagiosNodeItem ()
		{

		}
	}

	[JsonObject (MemberSerialization.OptIn, Title = "nagiosnodeitemobjects")]
	public class NagiosNodeItemObject
	{
		[JsonProperty ("")]
		public string itemName { get; set; } = "";
		[JsonProperty ("")]
		public string itemtype { get; set; } = "";  //	TODO: String2Emum
		[JsonProperty ("")]
		public bool hasInputConnector { get; set; } = false;
		[JsonProperty ("")]
		public bool hasOutputConnector { get; set; } = false;
		[JsonProperty ("")]
		public string itemTag { get; set; } = "";
		[JsonProperty ("")]
		public bool isTitel { get; set; } = false;
		[JsonProperty ("")]
		public bool isMandatory { get; set; } = false;
		[JsonProperty ("")]
		public List<string> itemsNames;
		[JsonProperty ("")]
		public List<string> itemsValues;

		public NagiosNodeItemObject ()
		{

		}
	}

}
