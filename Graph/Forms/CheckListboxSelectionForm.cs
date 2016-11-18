using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graph.Forms
{
	sealed partial class CheckboxSelectionForm : Form
	{
		public CheckboxSelectionForm()
		{
			InitializeComponent();
		}


		public int		SelectedIndex { get { return checkedListBox.SelectedIndex; } set { checkedListBox.SelectedIndex = value; } }
		public string[] Items 
		{ 
			get 
			{ 
				return (from item in checkedListBox.Items.OfType<string>() select item ).ToArray();
				//return (from item in checkedListBox.CheckedItems.OfType<string> () select item).ToArray ();
			} 
			set 
			{
				checkedListBox.Items.Clear();
				if (value == null || 
					value.Length == 0)
					return;
				foreach (var item in value)
					checkedListBox.Items.Add(item);
			} 
		}

		public string [] CheckedItems
		{
			get
			{
				//return (from item in checkedListBox.Items.OfType<string>() select item ).ToArray();
				return (from item in checkedListBox.CheckedItems.OfType<string> () select item).ToArray ();
			}
			set
			{
				//checkedListBox.Items.Clear ();
				if (value == null ||
					value.Length == 0)
					return;
				foreach (var item in value)
					checkedListBox.SetItemChecked (checkedListBox.Items.IndexOf (item), true);
			}
		}

		private void OnSelectionFormLoad(object sender, EventArgs e)
		{
			checkedListBox.Focus();
		}
	}
}
