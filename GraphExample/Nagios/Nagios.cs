using System;
using System.IO;
using System.Diagnostics;
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
	public class NagiosJSON
	{
		[JsonProperty ("nagiosobjects")]
		List<string> listNagiosObjects;
		[JsonProperty ("nagiosnodes")]
		Dictionary<string, NagiosNode> listNagiosNodes;
		[JsonProperty ("nagiosnodeitems")]
		List<NagiosNodeItem> listNagiosNodeItems;

		public string ObjectDirectory
		{
			get
			{
				//return Path.Combine (System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Name);
				//return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly ().GetName ().FullName);
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}
		public string ObjectPath { get { return Path.Combine (new string [] { ObjectDirectory, "Nagios", "Nagios_Nodes_ItemNodes.json" } );  } }

		public NagiosJSON ()
		{
			if (LoadSettings ())
			{
				Debug.WriteLine ("JSON wurde geladen", "NAGIOS");
			}
			else
				Debug.WriteLine ("Laden dere JSON Settings fehlgeschlagen", "NAGIOS");
		}
		public bool LoadSettings ()
		{
			if (File.Exists (ObjectPath))
			{
				JsonConvert.PopulateObject (File.ReadAllText (ObjectPath, Encoding.UTF8), this);
				return true;
			}
			else
				return false;

		}
	}
}

namespace Graphnodes.Nagios.Node
{
	[JsonObject (MemberSerialization.OptIn, Title = "nagiosnodes")]
	public class NagiosNode
	{
		[JsonProperty ("nodeitems")]
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
		[JsonProperty ("important")]
		List<NagiosNodeItemObject> nagiosImportantNodeItemObjects;
		[JsonProperty ("additional")]
		List<NagiosNodeItemObject> nagiosAdditionalNodeItemObjects;

		public NagiosNodeItem ()
		{

		}
	}

	[JsonObject (MemberSerialization.OptIn, Title = "nagiosnodeitemobjects")]
	public class NagiosNodeItemObject
	{
		[JsonProperty ("itemname")]
		public string itemName { get; set; } = "";
		[JsonProperty ("itemtype")]
		public string itemtype { get; set; } = "";  //	TODO: String2Emum
		[JsonProperty ("input")]
		public bool hasInputConnector { get; set; } = false;
		[JsonProperty ("output")]
		public bool hasOutputConnector { get; set; } = false;
		[JsonProperty ("tag")]
		public string itemTag { get; set; } = "";
		[JsonProperty ("isTitel")]
		public bool isTitel { get; set; } = false;
		[JsonProperty ("important")]
		public bool isMandatory { get; set; } = false;
		[JsonProperty ("names")]
		public List<string> itemsNames;
		[JsonProperty ("values")]
		public List<string> itemsValues;

		public NagiosNodeItemObject ()
		{

		}
	}

}
