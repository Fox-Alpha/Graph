﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graph.Forms
{
	sealed partial class DropdownSelectionForm : Form
	{
		public DropdownSelectionForm()
		{
			InitializeComponent();
		}


		public int		SelectedIndex { get { return TextComboBox.SelectedIndex; } set { TextComboBox.SelectedIndex = value <= TextComboBox.Items.Count?value:-1; } }
		public string[] Items 
		{ 
			get 
			{ 
				return (from item in TextComboBox.Items.OfType<string>() select item).ToArray();
			} 
			set 
			{
				TextComboBox.Items.Clear();
				if (value == null || 
					value.Length == 0)
					return;
				foreach (var item in value)
					TextComboBox.Items.Add(item);
			} 
		}

		private void OnSelectionFormLoad(object sender, EventArgs e)
		{
			TextComboBox.Focus();
		}
	}
}
