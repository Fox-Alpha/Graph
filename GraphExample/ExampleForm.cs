using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Graph;
using System.Drawing.Drawing2D;
using Graph.Compatibility;
using Graph.Items;
using GraphNodes.NagiosItems.Contact;
using GraphNodes.NagiosItems.Host;
using Newtonsoft.Json;
using System.IO;

using GraphNodes.Nagios;
using Graphnodes.Nagios.Node;
using Graphnodes.Nagios.NodeItem;


namespace GraphNodes
{
	public partial class ExampleForm : Form
	{
		NagiosJSON nagiosNodes;

		public ExampleForm ()
		{
			InitializeComponent ();

			graphControl.CompatibilityStrategy = new AlwaysCompatible ();

			graphControl.ConnectionAdded += new EventHandler<AcceptNodeConnectionEventArgs> (OnConnectionAdded);
			graphControl.ConnectionAdding += new EventHandler<AcceptNodeConnectionEventArgs> (OnConnectionAdding);
			graphControl.ConnectionRemoving += new EventHandler<AcceptNodeConnectionEventArgs> (OnConnectionRemoved);
			graphControl.ShowElementMenu += new EventHandler<AcceptElementLocationEventArgs> (OnShowElementMenu);

			//#####
			//	Laden der Definitionen aus der JSON Datei
			//#####

			nagiosNodes = new NagiosJSON ();

			if (nagiosNodes.LoadSettings ())
			{
				Debug.WriteLine ("JSON wurde geladen", "NAGIOS");

				if (nagiosNodes.listNagiosObjects.Count > 0)
				{
					foreach (var nagObject in nagiosNodes.listNagiosObjects)
					{
						int index = nodeMenu.Items.Add (
						new ToolStripMenuItem
						{
							Name = "Object_" + nagiosNodes.listNagiosObjects.IndexOf (nagObject),
							Text = nagiosNodes.listNagiosObjects [nagiosNodes.listNagiosObjects.IndexOf (nagObject)],
							Tag = nagiosNodes.listNagiosObjects.IndexOf (nagObject)
						});
						nodeMenu.Items [index].Click += windowNewMenu_Click;
					}
				}
			}
			else
				Debug.WriteLine ("Laden dere JSON Settings fehlgeschlagen", "NAGIOS");
			//#####

		}

		private void windowNewMenu_Click (object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem) != null)
			{
				int index = (int) (sender as ToolStripMenuItem).Tag;
				string nagObj = nagiosNodes.listNagiosObjects [index];
				NagiosNodeItemObject nnio = new NagiosNodeItemObject ();
				NagiosNode nagNode = new NagiosNode ();
				List<string> nodeItemsList = new List<string> ();

				if(nagiosNodes.listNagiosNodes.TryGetValue(nagObj, out nodeItemsList))
				{
					var node = new Node (nagObj);

					foreach (var str in nodeItemsList)
					{
						if (nagiosNodes.listNagiosNodeItems.nagiosImportantNodeItemObjects.TryGetValue (str, out nnio))
						{
							//	Ist in den Important NodeItems vorhanden
							node.AddItem (new NodeLabelItem (nnio.itemName, nnio.hasInputConnector, nnio.hasOutputConnector, nnio.isTitel) { Tag = nnio.itemTag, IsImportantNodeItem = nnio.isMandatory });

							node.Location = new Point (300, 150);
						}
						else if (nagiosNodes.listNagiosNodeItems.nagiosAdditionalNodeItemObjects.TryGetValue (str, out nnio))
						{
							//	Ist in den Additional NodeItems vorhanden
							node.AddItem (new NodeLabelItem (nnio.itemName, nnio.hasInputConnector, nnio.hasOutputConnector, nnio.isTitel) { Tag = nnio.itemTag, IsImportantNodeItem = nnio.isMandatory});

							
						}
					}
					node.Location = new Point (300, 150);
					graphControl.AddNode (node);
				}

			}
		}

		public void _ExampleForm()
		{
			InitializeComponent();

			graphControl.CompatibilityStrategy = new AlwaysCompatible();

			var someNode = new Node("My Title");
			someNode.Location = new Point(500, 100);
			var check1Item = new NodeCheckboxItem("Check 1", true, false) { Tag = 31337 };
			someNode.AddItem(check1Item);
			someNode.AddItem(new NodeCheckboxItem("Check 2", true, false) { Tag = 42f });
			
			graphControl.AddNode(someNode);

			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel		= new NodeSliderItem("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var greenChannel	= new NodeSliderItem("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var blueChannel		= new NodeSliderItem("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var colorItem		= new NodeColorItem("Color", Color.Black, false, true) { Tag = 1337 };

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object sender, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged		+= channelChangedDelegate;
			greenChannel.ValueChanged	+= channelChangedDelegate;
			blueChannel.ValueChanged	+= channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);
			graphControl.AddNode(colorNode);

			var textureNode = new Node("Texture");
			textureNode.Location = new Point(300, 150);
			var imageItem = new NodeImageItem(Properties.Resources.example, 64, 64, false, true) { Tag = 1000f };
			imageItem.Clicked += new EventHandler<NodeItemEventArgs>(OnImgClicked);
			textureNode.AddItem(imageItem);
			graphControl.AddNode(textureNode);

			graphControl.ConnectionAdded	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdded);
			graphControl.ConnectionAdding	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdding);
			graphControl.ConnectionRemoving += new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionRemoved);
			graphControl.ShowElementMenu	+= new EventHandler<AcceptElementLocationEventArgs>(OnShowElementMenu);

			graphControl.Connect(colorItem, check1Item);
		}

		void OnImgClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("IMAGE");
		}

		void OnColClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("Color");
		}

		void OnConnectionRemoved(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
			e.Connection.DoubleClick -= new EventHandler<NodeConnectionEventArgs> (OnConnectionDoubleClick);
			int fromHash = e.Connection.From.Node.GetHashCode ();
			int toHash = e.Connection.To.Node.GetHashCode();
		}

		void OnShowElementMenu(object sender, AcceptElementLocationEventArgs e)
		{
			if (e.Element == null)
			{
				// Show a test menu for when you click on nothing
				//testMenuItem.Text = "(clicked on nothing)";
				//nodeMenu.Show(e.Position);
				nodeMenu.Show (e.Position);
				e.Cancel = false;
			} else
			if (e.Element is Node)
			{
				// Show a test menu for a node
				testMenuItem.Text = ((Node)e.Element).Title;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is NodeItem)
			{
				// Show a test menu for a nodeItem
				testMenuItem.Text = e.Element.GetType().Name;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			{
				// if you don't want to show a menu for this item (but perhaps show a menu for something more higher up) 
				// then you can cancel the event
				e.Cancel = true;
			}
		}

		void OnConnectionAdding(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		static int counter = 1;
		void OnConnectionAdded(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
			e.Connection.Name = "Connection " + counter ++;
			e.Connection.DoubleClick += new EventHandler<NodeConnectionEventArgs>(OnConnectionDoubleClick);
		}

		void OnConnectionDoubleClick(object sender, NodeConnectionEventArgs e)
		{
			e.Connection.Name = "Connection " + counter++;
		}

		private void SomeNode_MouseDown(object sender, MouseEventArgs e)
		{
			var node = new NagiosNodeHostObject ("Nagios HOST"); //new Node("Some node");
			
			
			this.DoDragDrop(node, DragDropEffects.Copy);
		}

		private void TextureNode_MouseDown(object sender, MouseEventArgs e)
		{
            var node = new NagiosNodeContactObject ("Nagios CONTACT");

			this.DoDragDrop(node, DragDropEffects.Copy);
		}

		private void ColorNode_MouseDown(object sender, MouseEventArgs e)
		{
			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel = new NodeSliderItem("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var greenChannel = new NodeSliderItem("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var blueChannel = new NodeSliderItem("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var colorItem = new NodeColorItem("Color", Color.Black, false, true);

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object s, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged += channelChangedDelegate;
			greenChannel.ValueChanged += channelChangedDelegate;
			blueChannel.ValueChanged += channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);

			this.DoDragDrop(colorNode, DragDropEffects.Copy);
		}

		private void OnShowLabelsChanged(object sender, EventArgs e)
		{
			graphControl.ShowLabels = showLabelsCheckBox.Checked;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			NagiosJSON myNagios = new NagiosJSON ();

			JsonSerializerSettings jsonSerializerSettings;

			jsonSerializerSettings = new JsonSerializerSettings ();
			jsonSerializerSettings.Formatting = Formatting.Indented;
			jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
			jsonSerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
			jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;

			//JsonSerializer serializer = JsonSerializer.CreateDefault (jsonSerializerSettings);
			//using (StreamReader sr = new StreamReader (myNagios.ObjectPath))
			//{
			//	using (JsonReader reader = new JsonTextReader (sr))
			//	{
			//		myNagios = serializer.Deserialize<NagiosJSON> (reader);
			//	}
			//}


			if (myNagios.LoadSettings ())
			//if (myNagios.SaveSettings ())
			{
				Debug.WriteLine ("JSON wurde geladen", "NAGIOS");
			}
			else
				Debug.WriteLine ("Laden dere JSON Settings fehlgeschlagen", "NAGIOS");

			//myNagios.LoadSettings ();
		}
	}
}
