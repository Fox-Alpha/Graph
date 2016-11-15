using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Items;

namespace GraphNodes.NagiosItems
{
	public sealed class NagiosNodeHostObject : Node
	{
		public NagiosNodeHostObject(string title) :
			base (title)
		{
			this.AddItem (new NodeTextBoxItem ("Hostname", false, false));
			this.AddItem (new NodeLabelItem ("Hostgroups", false, false));
			this.AddItem (new NodeLabelItem ("HostCheckCommand", false, true));
			this.AddItem (new NodeTextBoxItem ("Host Options", false, true));
			this.AddItem (new NodeDropDownItem (new string [] { "OK", "WARNING", "CRITICAL", "UNKNOWN" }, 0, false, false));
		}

		public Node GetHostObjectNode ()
		{
			return this;
		}
	}
}
