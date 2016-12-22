using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Items;

namespace GraphNodes.CustomUI.Nodes
{
	public class CustomUINode : Node
	{
		public CustomUINode (string title) :
			base (title){ }
	}
	public sealed class CustomUINodeCheckBox : CustomUINode
	{
		public string CheckBoxText { get; set; } = string.Empty;

		public CustomUINodeCheckBox (string title) :
			base (title)
		{
			this.AddItem (new NodeCheckboxItem (string.IsNullOrWhiteSpace (CheckBoxText) ? title : CheckBoxText, false, true) { Tag = "tagCheckBox", outputTag = new object [] { "tagCheckBox" } });
		}
	}
	public sealed class CustomUINodeCheckListBoxItem : CustomUINode
	{
		public CustomUINodeCheckListBoxItem (string title) :
			base (title)
		{
			this.AddItem (new NodeCheckListBoxItem (new string [] { "CustomUINode", "Value1", "Value2", "Value3" }, 0, false, true) { Tag = "tagCheckListBox", outputTag = new object [] { "tagCheckListBox" } });
		}
	}
	public sealed class CustomUINodeColorSliderItem : CustomUINode
	{
		public CustomUINodeColorSliderItem (string title) :
			base (title)
		{
			//this.AddItem (new NodeCheckListBoxItem (new string [] { "CustomUINode", "Value1", "Value2", "Value3" }, 0, false, true) { Tag = "CustomUINodeCheckListBoxItem", outputTag = new object [] { "tagOutCheckListBox" } });

			var redChannel = new NodeSliderItem ("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var greenChannel = new NodeSliderItem ("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var blueChannel = new NodeSliderItem ("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
			var colorItem = new NodeColorItem ("Color", Color.Black, false, true) { Tag = "tagColorSlider", outputTag = new object [] { "tagCheckListBox" } };

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate (object s, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = blueChannel.Value;
				var blue = greenChannel.Value;
				colorItem.Color = Color.FromArgb ((int) Math.Round (red * 255), (int) Math.Round (green * 255), (int) Math.Round (blue * 255));
			};
			redChannel.ValueChanged += channelChangedDelegate;
			greenChannel.ValueChanged += channelChangedDelegate;
			blueChannel.ValueChanged += channelChangedDelegate;


			this.AddItem (redChannel);
			this.AddItem (greenChannel);
			this.AddItem (blueChannel);

			this.AddItem (colorItem);

		}
	}
	public sealed class CustomUINodeDropDownItem : CustomUINode
	{
		public CustomUINodeDropDownItem (string title) :
			base (title)
		{
			this.AddItem (new NodeDropDownItem (new string [] { "CustomUINode", "Value1", "Value2", "Value3" }, 0, false, true) { Tag = "tagDropDown", outputTag = new object [] { "tagDropDown" } });
		}
	}
	public sealed class CustomUINodeImageItem : CustomUINode
	{
		public CustomUINodeImageItem (string title) :
			base (title)
		{
            this.AddItem (new NodeImageItem (Properties.Resources.example, 64, 64, false, true) { Tag = "tagImage", outputTag = new string [] { "tagImage" } });
		}
	}
	public sealed class CustomUINodeLabelItem : CustomUINode
	{
		public CustomUINodeLabelItem (string title) :
			base (title)
		{
			this.AddItem (new NodeLabelItem (title, true, true) { Tag = "tagLabelItem", inputTag = new object [] { "tagLabel" }, outputTag = new object [] { "tagLabel" }});
		}
	}
	public sealed class CustomUINodeNumericSliderItem : CustomUINode
	{
		public CustomUINodeNumericSliderItem (string title) :
			base (title)
		{
            this.AddItem (new NodeNumericSliderItem ("NodeNumericSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagNumericSlider", outputTag = new string [] { "tagNumericSlider", "tagSlider" } });
        }
	}
	public sealed class CustomUINodeSliderItem : CustomUINode
	{
		public CustomUINodeSliderItem (string title) :
			base (title)
		{
            this.AddItem (new NodeNumericSliderItem ("NodeSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagSlider", outputTag = new string [] { "tagNumericSlider", "tagSlider" } });
        }
	}
	public sealed class CustomUINodeMultilineTextBoxItem : CustomUINode
	{
		public CustomUINodeMultilineTextBoxItem (string title) :
			base (title)
		{
            //  TODO: gegen MultilineTextBox ersetzen
			this.AddItem (new NodeTextBoxItem ("CustomUINode", false, true) { Tag = "tegMultilineTextBoxItem", outputTag = new object [] { "tagMultilineText" } });
		}
	}
	public sealed class CustomUINodeTextBoxItem : CustomUINode
	{
		public CustomUINodeTextBoxItem (string title) :
			base (title)
		{
			this.AddItem (new NodeTextBoxItem ("CustomUINode", false, true) { Tag = "tagTextBoxItem", outputTag = new object [] { "tagTextBox" } });
		}
	}
}
namespace GraphNodes.CustomUI.Items
{
    public class CustomUIItem
    {
        public string CheckBoxText { get; set; } = string.Empty;
        public string [] CheckListBoxListItems = new string [] { "CustomUINode", "Value1", "Value2", "Value3" };
        public string [] DropDownListItems = new string [] { "CustomUINode", "Value1", "Value2", "Value3" };

        public  CustomUIItem ()
        {
        }

        public NodeLabelItem CustomUIItemLabel(string title)
        {
            return new NodeLabelItem (title, true, true) { Tag = "tagLabelItem", inputTag = new object [] { "tagLabel" }, outputTag = new object [] { "tagLabel" } };
        }

        public NodeTextBoxItem CustomUIItemTextBox (string title)
        {
            return new NodeTextBoxItem ("CustomUINode", false, true) { Tag = "tagTextBoxItem", outputTag = new object [] { "tagTextBox" } };
        }

        public NodeTextBoxItem CustomUIItemMultilineTextBox (string title)
        {
            //  TODO: gegen MultilineTextBox ersetzen
            return new NodeTextBoxItem ("CustomUINode", false, true) { Tag = "tegMultilineTextBoxItem", outputTag = new object [] { "tagMultilineText" } };
        }

        public NodeCheckboxItem CustomUIItemCheckBox (string title)
        {
            return new NodeCheckboxItem (string.IsNullOrWhiteSpace (CheckBoxText) ? title : CheckBoxText, false, true) { Tag = "tagCheckBox", outputTag = new object [] { "tagCheckBox" } };
        }

        public NodeCheckListBoxItem CustomUIItemCheckListBox (string title, string [] ListItems)
        {
            if (ListItems.Length > 0)
            {
                CheckListBoxListItems = ListItems;
            }
            return new NodeCheckListBoxItem (CheckListBoxListItems, 0, false, true) { Tag = "tagCheckListBox", inputTag = new object [] { "tagCheckListBox" }, outputTag = new object [] { "tagCheckListBox" } };
        }

        public NodeDropDownItem CustomUIItemDropDown (string title, string [] ListItems)
        {
            if (ListItems.Length > 0)
            {
                CheckListBoxListItems = ListItems;
            }
            return new NodeDropDownItem (DropDownListItems, 0, false, true) { Tag = "tagDropDown", outputTag = new object [] { "tagDropDown" } };
        }

        public NodeNumericSliderItem CustomUIItemSlider (string title)
        {
            return new NodeNumericSliderItem ("NodeSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagSlider", outputTag = new string [] { "tagNumericSlider", "tagSlider" } };
        }

        public NodeNumericSliderItem CustomUIItemNumericSlider (string title)
        {
            return new NodeNumericSliderItem ("NodeNumericSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagNumericSlider", outputTag = new string [] { "tagNumericSlider", "tagSlider" } };
        }

        public NodeItem[] CustomUIItemColorSlider (string title)
        {
            NodeItem [] CustomUIColorSlider; 

            var redChannel = new NodeSliderItem ("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
            var greenChannel = new NodeSliderItem ("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
            var blueChannel = new NodeSliderItem ("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
            var colorItem = new NodeColorItem ("Color", Color.Black, false, true) { Tag = "tagColorSlider", outputTag = new object [] { "tagCheckListBox" } };

            EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate (object s, NodeItemEventArgs args)
            {
                var red = redChannel.Value;
                var blue = blueChannel.Value;
                var green = greenChannel.Value;
                colorItem.Color = Color.FromArgb ((int) Math.Round (red * 255), (int) Math.Round (green * 255), (int) Math.Round (blue * 255));
            };
            redChannel.ValueChanged += channelChangedDelegate;
            greenChannel.ValueChanged += channelChangedDelegate;
            blueChannel.ValueChanged += channelChangedDelegate;


            CustomUIColorSlider = new NodeItem[] { (NodeItem)redChannel, (NodeItem) greenChannel, (NodeItem) blueChannel, (NodeItem) colorItem };

            return CustomUIColorSlider;
        }

        public NodeImageItem CustomUIItemImage (string title)
        {
            return new NodeImageItem (Properties.Resources.example, 64, 64, false, true) { Tag = "tagImage", outputTag = new string [] { "tagImage" } };
        }
    }

    class dummyNodeItem
    {
        string strDummy = string.Empty;
        NodeItem nodeItem;
        Node clickedNode;
        dummyNodeItem ()
        {
            switch (strDummy)
            {
            
                case "CheckBox":
                        nodeItem = new NodeCheckboxItem ("NodeCheckboxItem", false, true) { Tag = "tagCheckBox", outputTag = new string [] { "tagCheckBox" } };
                break;
                    case "CheckListBox":
                        nodeItem = new NodeCheckListBoxItem (new string [] { "NodeCheckListBoxItem" }, 0, false, true) { Tag = "tagCheckListBox", outputTag = new string [] { "tagCheckListBox" } };
                break;
                    case "ColorSlider":
                        nodeItem = new NodeSliderItem ("NodeSliderItem", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, true) { Tag = "tagColorSlider", outputTag = new string [] { "tagColorSlider" } };
                break;
                    case "DropDown":
                        nodeItem = new NodeDropDownItem (new string [] { "NodeDropDownItem" }, 1, false, true) { Tag = "tagDropDown", outputTag = new string [] { "tagDropDown" } };
                break;
                    case "Image":
                        nodeItem = new NodeImageItem (Properties.Resources.example, 64, 64, false, true) { Tag = "tagImage", outputTag = new string [] { "tagImage" } };
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
    }
}
/*
 * CustomUINodeCheckBox
 * CustomUINodeCheckListBoxItem
 * CustomUINodeDropDownItem
 * CustomUINodeImageItem
 * CustomUINodeLabelItem
 * CustomUINodeNumericSliderItem
 * CustomUINodeSliderItem
 * CustomUINodeMultilineTextBoxItem
 * CustomUINodeTextBoxItem
 */

//	"tagCheckBox", "tagCheckListBox", "tagColorSlider", "tagDropDown", "tagImage", "tagLabel", "tagNumericSlider", "tagSlider", "tagMultilineText", "tagTextBox"