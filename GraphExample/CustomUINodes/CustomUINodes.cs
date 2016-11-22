using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Items;

namespace GraphNodes.CustomUINodes
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
			this.AddItem (new NodeCheckListBoxItem (new string [] { "CustomUINode", "Value1", "Value2", "Value3" }, 0, false, true) { Tag = "tagCheckListBox", outputTag = new object [] { "tagOutCheckListBox" } });
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
			var colorItem = new NodeColorItem ("Color", Color.Black, false, true) { Tag = "tagCheckListBox" };

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
			this.AddItem (new NodeDropDownItem (new string [] { "CustomUINode", "Value1", "Value2", "Value3" }, 0, false, true) { Tag = "tagDropDown", outputTag = new object [] { "tagOutDropDownList" } });
		}
	}
	public sealed class CustomUINodeImageItem : CustomUINode
	{
		public CustomUINodeImageItem (string title) :
			base (title)
		{
		}
	}
	public sealed class CustomUINodeLabelItem : CustomUINode
	{
		public CustomUINodeLabelItem (string title) :
			base (title)
		{
			this.AddItem (new NodeLabelItem (title, false, true) { Tag = "CustomUINodeLabelItem", outputTag = new object [] { "tagTextLabel" }});
		}
	}
	public sealed class CustomUINodeNumericSliderItem : CustomUINode
	{
		public CustomUINodeNumericSliderItem (string title) :
			base (title)
		{
		}
	}
	public sealed class CustomUINodeSliderItem : CustomUINode
	{
		public CustomUINodeSliderItem (string title) :
			base (title)
		{
		}
	}
	public sealed class CustomUINodeMultilineTextBoxItem : CustomUINode
	{
		public CustomUINodeMultilineTextBoxItem (string title) :
			base (title)
		{
			this.AddItem (new NodeTextBoxItem ("CustomUINode", false, true) { Tag = "CustomUINodeMultilineTextBoxItem", outputTag = new object [] { "tagMultilineText" } });
		}
	}
	public sealed class CustomUINodeTextBoxItem : CustomUINode
	{
		public CustomUINodeTextBoxItem (string title) :
			base (title)
		{
			this.AddItem (new NodeTextBoxItem ("CustomUINode", false, true) { Tag = "CustomUINodeTextBoxItem", outputTag = new object [] { "tagTextBox" } });
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