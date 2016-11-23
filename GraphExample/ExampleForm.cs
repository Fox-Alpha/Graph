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
using GraphNodes.CustomUINodes;


namespace GraphNodes
{
	public partial class ExampleForm : Form
	{
		NagiosJSON nagiosNodes;
		Point mouseClickForMenu;
        Node clickedNode;
        NodeItem clickedNodeItem;
        NodeConnector clickedNodeItemConnector;


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
        public ExampleForm ()
		{
			InitializeComponent ();

			graphControl.CompatibilityStrategy = new InOutTagTypeListCompatibility ();

			graphControl.ConnectionAdded += new EventHandler<AcceptNodeConnectionEventArgs> (OnConnectionAdded);
			graphControl.ConnectionAdding += new EventHandler<AcceptNodeConnectionEventArgs> (OnConnectionAdding);
			graphControl.ConnectionRemoving += new EventHandler<AcceptNodeConnectionEventArgs> (OnConnectionRemoved);
			graphControl.ShowElementMenu += new EventHandler<AcceptElementLocationEventArgs> (OnShowElementMenu);

			//#####
			//	Laden der Definitionen aus der JSON Datei
			//#####

			nagiosNodes = new NagiosJSON ();
			string [] connectorMenuItems = new string [] { "CheckBox", "CheckListBox", "ColorSlider", "DropDown", "Image", "Label", "NumericSlider", "Slider", "MultilineText", "TextBox" };

			if (nagiosNodes.LoadSettings ())
			{
				Debug.WriteLine ("JSON wurde geladen", "NAGIOS");

				if (nagiosNodes.listNagiosObjects.Count > 0)
				{
					foreach (var nagObject in nagiosNodes.listNagiosObjects)
					{
						int index = objectMenu.Items.Add (
						new ToolStripMenuItem
						{
							Name = "Object_" + nagiosNodes.listNagiosObjects.IndexOf (nagObject),
							Text = nagiosNodes.listNagiosObjects [nagiosNodes.listNagiosObjects.IndexOf (nagObject)],
							Tag = nagiosNodes.listNagiosObjects.IndexOf (nagObject)
						});
						objectMenu.Items [index].Click += ContextMenu_Click;
					}
				}

				if (connectorMenuItems.Length > 0)
				{
					foreach (var nagConnector in connectorMenuItems)
					{
						int index = connectorMenu.Items.Add (
						new ToolStripMenuItem
						{
							Name = "Connect_" + connectorMenuItems.ToList<string>().IndexOf (nagConnector),
							Text = connectorMenuItems [connectorMenuItems.ToList<string> ().IndexOf (nagConnector)],
							Tag = connectorMenuItems[connectorMenuItems.ToList<string> ().IndexOf (nagConnector)]
						});
						connectorMenu.Items [index].Click += ContextMenu_Click;
					}
				}
			}
			else
				Debug.WriteLine ("Laden dere JSON Settings fehlgeschlagen", "NAGIOS");
			//#####

		}

		private void ContextMenu_Click (object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem) != null)
			{
				ToolStrip mi = (sender as ToolStripMenuItem).GetCurrentParent ();
				int menuTag = 0;
				if (!int.TryParse ((string)mi.Tag, out menuTag))
					return;
				switch (menuTag)
				{
					case 1:
                        //Add NodeItemInput to cliecked Node
                        AddNodeItem2Node (sender as ToolStripMenuItem);
						break;
					case 2:
						AddNagiosNode (sender as ToolStripMenuItem);
						break;
					case 3:
						AddNagiosCustomUIItem (sender as ToolStripMenuItem);
						break;
				}
			}
		}

        private void AddNodeItem2Node (ToolStripMenuItem toolStripMenuItem)
        {
            NodeItem nodeItem = null;
            switch ((string) toolStripMenuItem.Tag)
            {
                //	"CheckBox", "CheckListBox", "ColorSlider", "DropDown", "Image", "Label", "NumericSlider", "Slider", "MultilineText", "TextBox" 
                //	"tagCheckBox", "tagCheckListBox", "tagColorSlider", "tagDropDown", "tagImage", "tagLabel", "tagNumericSlider", "tagSlider", "tagMultilineText", "tagTextBox"

                case "CheckBox":
                    nodeItem = new NodeCheckboxItem ("NodeCheckboxItem", false, true) { Tag = "tagCheckBox", outputTag = new string [] { "tagCheckBox" } };
                    break;
                case "CheckListBox":
                    nodeItem = new NodeCheckListBoxItem (new string [] { "NodeCheckListBoxItem" }, 0, false, true) { Tag = "tagCheckListBox", outputTag = new string [] { "tagCheckListBox" } };
                    break;
                case "ColorSlider":
                    nodeItem = new NodeSliderItem ("NodeSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagColorSlider", outputTag = new string [] { "tagColorSlider" }};
                    break;
                case "DropDown":
                    nodeItem = new NodeDropDownItem (new string [] { "NodeDropDownItem" }, 1, false, true) { Tag = "tagDropDown", outputTag = new string [] { "tagDropDown" }};
                    break;
                case "Image":
                    nodeItem = new NodeImageItem (Properties.Resources.example, 64, 64, false, true) { Tag = "tagImage" , outputTag = new string [] { "tagImage" }};
                    break;
                case "Label":
                    nodeItem = new NodeLabelItem ("NodeLabelItem", true, true) { Tag = "tagLabel", inputTag = new string [] { "tagLabel" }, outputTag = new string [] { "tagLabel" } };
                    break;
                case "NumericSlider":
                    nodeItem = new NodeNumericSliderItem ("NodeNumericSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagNumericSlider", outputTag = new string [] { "tagNumericSlider" } };
                    break;
                case "Slider":
                    nodeItem = new NodeSliderItem ("NodeSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagSlider", outputTag = new string [] { "tagSlider" } };
                    break;
                case "MultilineText":
                    //nodeItem = new NodeMultilineTextBoxItem ("NodeMultilineTextBoxItem", false, true) { Tag = "tagMultilineText", outputTag = new string [] { "tagMultilineText" } };
                    break;
                case "TextBox":
                    nodeItem = new NodeTextBoxItem ("NodeTextBoxItem", false, true) { Tag = "tagTextBox", outputTag = new string [] { "tagTextBox" } };
                    break;
                default:
                    break;
            }
            Node node = clickedNode;
            if (node != null)
            {
                node.AddItem (nodeItem);
            }
        }

        private void AddNagiosCustomUIItem (ToolStripMenuItem toolStripMenuItem)
		{
			CustomUINode node = null;
			switch((string)toolStripMenuItem.Tag)
			{
                //	"CheckBox", "CheckListBox", "ColorSlider", "DropDown", "Image", "Label", "NumericSlider", "Slider", "MultilineText", "TextBox" 
                case "CheckBox":
					node = new CustomUINodeCheckBox ("CustomUINodeCheckBox");
					break;
				case "CheckListBox":
					node = new CustomUINodeCheckListBoxItem ("CustomUINodeCheckListBoxItem");
					break;
				case "ColorSlider":
					node = new CustomUINodeColorSliderItem ("CustomUINodeColorSliderItem");
					break;
				case "DropDown":
					node = new CustomUINodeDropDownItem ("CustomUINodeDropDownItem");
					break;
				case "Image":
					node = new CustomUINodeImageItem ("CustomUINodeImageItem");
					break;
				case "Label":
					node = new CustomUINodeLabelItem ("CustomUINodeLabelItem");
					break;
				case "NumericSlider":
					node = new CustomUINodeNumericSliderItem ("CustomUINodeNumericSliderItem");
					break;
				case "Slider":
					node = new CustomUINodeSliderItem ("CustomUINodeSliderItem");
					break;
				case "MultilineText":
					node = new CustomUINodeMultilineTextBoxItem ("CustomUINodeMultilineTextBoxItem");
					break;
				case "TextBox":
					node = new CustomUINodeTextBoxItem ("CustomUINodeTextBoxItem");
					break;
			}

			if (node != null)
			{
                if (node.Items.Count<NodeItem>() > 0 && clickedNodeItem != null)
                {
                    if(graphControl.Connect (node.Items.First<NodeItem> (), clickedNodeItem) != null)
                    {
                        node.Title = (string)clickedNodeItem.Tag;
                    }

                }
                    
                node.Location = (PointF) graphControl.PointToClient (new Point (mouseClickForMenu.X - 100 - node.boundSite.ToSize().Width, mouseClickForMenu.Y) );
				graphControl.AddNode (node);
			}
			else
			{
				Node newNode = new Node ("UNDEFINED");
				NodeItem ni = new NodeLabelItem ("UNDEFINED") { Tag = "UNDEFINED", };
				newNode.AddItem (ni);
				graphControl.AddNode (newNode);
			}
		}

        private void AddNagiosNode (ToolStripMenuItem sender)
		{
			int index = (int) (sender as ToolStripMenuItem).Tag;
			string nagObj = nagiosNodes.listNagiosObjects [index];
			NagiosNodeItemObject nnio = new NagiosNodeItemObject ();
			NagiosNode nagNode = new NagiosNode ();
			List<string> nodeItemsList = new List<string> ();

			if (nagiosNodes.listNagiosNodes.TryGetValue (nagObj, out nodeItemsList))
			{

                nodeItemsList.Sort ();

                var node = new Node (nagObj) { Tag = nagObj };
				Matrix inverse_transformation = new Matrix ();

				foreach (var str in nodeItemsList)
				{
					if (nagiosNodes.listNagiosNodeItems.nagiosImportantNodeItemObjects.TryGetValue (str, out nnio))
					{
						//	Ist in den Important NodeItems vorhanden
						node.AddItem (new NodeLabelItem (
								nnio.itemName,
								nnio.itemConnectors.hasInput,
								nnio.itemConnectors.hasOutput,
								nnio.isTitel)
						{
							Tag = nnio.itemConnectorsTags.Tag,
							IsImportantNodeItem = nnio.isMandatory,
							inputTag = nnio.itemConnectorsTags.inputObjList.ToArray<object> (),
							outputTag = nnio.itemConnectorsTags.outputObjList.ToArray<object> ()
						});

						//node.Location = new Point (300, 150);
					}
					else if (nagiosNodes.listNagiosNodeItems.nagiosAdditionalNodeItemObjects.TryGetValue (str, out nnio))
					{
						//	Ist in den Additional NodeItems vorhanden
						node.AddItem (new NodeLabelItem (nnio.itemName, nnio.itemConnectors.hasInput, nnio.itemConnectors.hasOutput, nnio.isTitel) { Tag = nnio.itemConnectorsTags.Tag, IsImportantNodeItem = nnio.isMandatory, inputTag = nnio.itemConnectorsTags.inputObjList.ToArray<object> (), outputTag = nnio.itemConnectorsTags.outputObjList.ToArray<object> () });
					}
				}
				node.Location = (PointF) graphControl.PointToClient (mouseClickForMenu);
				graphControl.AddNode (node);
			}
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

		void OnShowElementMenu (object sender, AcceptElementLocationEventArgs e)
		{
            clickedNode = null;
            clickedNodeItem = null;
            clickedNodeItemConnector = null;

            if (e.Element == null)
			{
				// Show a test menu for when you click on nothing
				//testMenuItem.Text = "(clicked on nothing)";
				//nodeMenu.Show(e.Position);
				objectMenu.Show (e.Position);
				mouseClickForMenu = e.Position;
				e.Cancel = false;
			}
            else if ((e.Element is Node) && !(e.Element is CustomUINode))
			{
				// Show a test menu for a node
				//testMenuItem.Text = ((Node) e.Element).Title;
                clickedNode = (Node) e.Element;
				nodeMenu.Show (e.Position);
				e.Cancel = false;
			} 
			//if (e.Element is NodeItem)
			//{
			//	// Show a test menu for a nodeItem
			//	testMenuItem.Text = e.Element.GetType ().Name;
			//	nodeMenu.Show (e.Position);
			//	e.Cancel = false;
			//}
            else if (e.Element is NodeConnector)
			{
                clickedNodeItemConnector = (NodeConnector) e.Element;
                clickedNodeItem = (NodeItem) clickedNodeItemConnector.Item;
                mouseClickForMenu = e.Position;
                connectorMenu.Tag = "3";
                connectorMenu.Show (e.Position);

				e.Cancel = false;
			}
            else if (e.Element is NodeItem)
            {
                clickedNodeItem = (NodeItem) e.Element;
                mouseClickForMenu = e.Position;
                connectorMenu.Tag = "3";
                connectorMenu.Show (e.Position);

                e.Cancel = false;
            }
            else if (e.Element is CustomUINode)
            {
                clickedNode = (CustomUINode) e.Element;
                mouseClickForMenu = e.Position;
                connectorMenu.Tag = "1";
                connectorMenu.Show (e.Position);

                e.Cancel = false;
            }
            else
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
            //#####
            //	Laden der Definitionen aus der JSON Datei
            //#####

            nagiosNodes = new NagiosJSON ();
            string [] connectorMenuItems = new string [] { "CheckBox", "CheckListBox", "ColorSlider", "DropDown", "Image", "Label", "NumericSlider", "Slider", "MultilineText", "TextBox" };

            if (nagiosNodes.LoadSettings ())
            {
                Debug.WriteLine ("JSON wurde geladen", "NAGIOS");

                if (nagiosNodes.listNagiosObjects.Count > 0)
                {
                    foreach (var nagObject in nagiosNodes.listNagiosObjects)
                    {
                        int index = objectMenu.Items.Add (
                        new ToolStripMenuItem
                        {
                            Name = "Object_" + nagiosNodes.listNagiosObjects.IndexOf (nagObject),
                            Text = nagiosNodes.listNagiosObjects [nagiosNodes.listNagiosObjects.IndexOf (nagObject)],
                            Tag = nagiosNodes.listNagiosObjects.IndexOf (nagObject)
                        });
                        objectMenu.Items [index].Click += ContextMenu_Click;
                    }
                }

                if (connectorMenuItems.Length > 0)
                {
                    foreach (var nagConnector in connectorMenuItems)
                    {
                        int index = connectorMenu.Items.Add (
                        new ToolStripMenuItem
                        {
                            Name = "Connect_" + connectorMenuItems.ToList<string> ().IndexOf (nagConnector),
                            Text = connectorMenuItems [connectorMenuItems.ToList<string> ().IndexOf (nagConnector)],
                            Tag = connectorMenuItems [connectorMenuItems.ToList<string> ().IndexOf (nagConnector)]
                        });
                        connectorMenu.Items [index].Click += ContextMenu_Click;
                    }
                }
            }
            else
                Debug.WriteLine ("Laden dere JSON Settings fehlgeschlagen", "NAGIOS");
            //#####
        }
    }
}
