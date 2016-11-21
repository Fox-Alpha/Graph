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
		#region JSON
		[JsonProperty ("nagiosobjects")]
		public List<string> listNagiosObjects;

		[JsonProperty ("nagiosnodes")]
		public Dictionary<string, List<string>> listNagiosNodes = new Dictionary<string, List<string>>();

		[JsonProperty ("nagiosnodeitems")]
		public NagiosNodeItem listNagiosNodeItems;
		#endregion

		#region Properties

		static string JsonFile { get; set; } = ObjectDirectory + @"\Nagios\Nagios_Nodes_ItemNodes.json";

		static string ObjectDirectory
		{
			get
			{
				//return Path.Combine (System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Name);
				//return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly ().GetName ().FullName);
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}
		static string ObjectPath { get { return Path.Combine (new string [] { JsonFile } );  } }
		#endregion

		#region Funcs
		public NagiosJSON ()
		{
			//listNagiosObjects = new List<string> ();
			//listNagiosNodeItems = new List<NagiosNodeItem> ();
		}

		public bool LoadSettings ()
		{
			JsonSerializerSettings jsonSerializerSettings;

			jsonSerializerSettings = new JsonSerializerSettings ();
			jsonSerializerSettings.Formatting = Formatting.Indented;
			jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
			jsonSerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
			jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;

			if (File.Exists (ObjectPath))
			{
				JsonConvert.PopulateObject (File.ReadAllText (ObjectPath, Encoding.UTF8), this, jsonSerializerSettings);
				return true;
			}
			else
				return false;
		}

		public bool LoadSettings (string _jsonFile)
		{
			if (File.Exists (_jsonFile))
			{
				JsonFile = _jsonFile;
				return LoadSettings ();
			}
			else
				return false;
		}

		public bool SaveSettings()
		{
			//string JSON = "";
			//JsonSerializerSettings JsonSettings = new JsonSerializerSettings ();
			//JsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
			//JsonSettings.Formatting = Formatting.Indented;

			//JSON = JsonConvert.SerializeObject (this, JsonSettings);

			//Debug.WriteLine (JSON, "SaveSettings");

			return true;
		}
		#endregion
	}
}

namespace Graphnodes.Nagios.Node
{
	[JsonObject (MemberSerialization.OptIn, Title = "nagiosnodes")]
	public class NagiosNode
	{
		[JsonProperty ("nodeitems")]
		public List<string> listNagiosNodeItems;

		public bool AddNagiosNode (string strNode)
		{
			if (!string.IsNullOrWhiteSpace (strNode))
			{
				listNagiosNodeItems.Add (strNode);
				return true;
			}

			return false;
		}

		public bool AddNagiosNode (string [] strNode)
		{
			if (strNode.Length > 0)
			{
				listNagiosNodeItems.AddRange (strNode);
				return true;
			}

			return false;
		}
		public NagiosNode () { }
	}
}

namespace Graphnodes.Nagios.NodeItem
{
	[JsonObject (MemberSerialization.OptIn)]
	public class NagiosNodeItem
	{
		[JsonProperty ("important")]
		public Dictionary<string, NagiosNodeItemObject> nagiosImportantNodeItemObjects;
		[JsonProperty ("additional")]
		public Dictionary<string, NagiosNodeItemObject> nagiosAdditionalNodeItemObjects;

		public NagiosNodeItem () { }
	}

	[JsonObject (MemberSerialization.OptIn)]
	public class NagiosNodeItemObject
	{
		[JsonProperty ("itemname")]
		public string itemName { get; set; } = "";
		[JsonProperty ("itemtype")]
		public string itemtype { get; set; } = "";  //	TODO: String2Emum
		[JsonProperty ("connectors")]
		public NagiosNodeItemObjectConector itemConnectors;
		[JsonProperty ("tags")]
		public NagiosNodeItemObjectTags itemConnectorsTags;
		//[JsonProperty ("tag")]
		//public string itemTag { get; set; } = "";
		[JsonProperty ("isTitel")]
		public bool isTitel { get; set; } = false;
		[JsonProperty ("isimportant")]
		public bool isMandatory { get; set; } = false;
		[JsonProperty ("names")]
		public List<string> itemsNames;
		[JsonProperty ("values")]
		public List<string> itemsValues;

		public NagiosNodeItemObject (){}
	}

	[JsonObject (MemberSerialization.OptIn)]
	public class NagiosNodeItemObjectConector
	{
		[JsonProperty ("input")]
		public List<string> inputObjList;
		[JsonProperty ("output")]
		public List<string> outputObjList;

		public bool hasOutput
		{
			get
			{
				return outputObjList.Count > 0;
			}
		}
		public bool hasInput
		{
			get
			{
				return inputObjList.Count > 0;
			}
		}

		NagiosNodeItemObjectConector () { }
	}

	[JsonObject (MemberSerialization.OptIn)]
	public class NagiosNodeItemObjectTags
	{
		[JsonProperty ("tag")]
		public string Tag;
		[JsonProperty ("inputtag")]
		public List<string> inputObjList;
		[JsonProperty ("outputtag")]
		public List<string> outputObjList;

		public bool hasOutput
		{
			get
			{
				return outputObjList.Count > 0;
			}
		}
		public bool hasInput
		{
			get
			{
				return inputObjList.Count > 0;
			}
		}

		NagiosNodeItemObjectTags () { }
	}
}
