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
		public NagiosNodeHostObject (string title) :
			base (title)
		{
			this.AddItem (new NodeLabelItem ("use", false, true) { Tag = "NagiosHostTemplate"});
			this.AddItem (new NodeTextBoxItem ("host_name", false, false));
			this.AddItem (new NodeTextBoxItem ("alias", false, false));
			this.AddItem (new NodeTextBoxItem ("adress", false, false));
			this.AddItem (new NodeLabelItem ("contacts", false, true) { Tag = "NagiosContact"});
			this.AddItem (new NodeLabelItem ("check_command", false, true) { Tag = "NagiosCheckCommand"});
			this.AddItem (new NodeLabelItem ("contact_groups", false, true) { Tag = "NagiosContactGroup" });
			this.AddItem (new NodeLabelItem ("notification_period", false, true) { Tag = "NagiosTimePeriod" });
			this.AddItem (new NodeLabelItem ("check_period", false, true) { Tag = "NagiosTimePeriod" });
			this.AddItem (new NodeCheckboxItem ("active_checks_enables", false, false));
			this.AddItem (new NodeCheckboxItem ("passive_checks_enables", false, false));

			this.AddItem (new NodeNumericSliderItem ("max_check_attemps", 64.0f, 16.0f, 1, 10.0f, 1.0f, false, false));

			this.AddItem (new NodeTextBoxItem ("Host Options", false, true));
			this.AddItem (new NodeDropDownItem (new string [] { "OK", "WARNING", "CRITICAL", "UNKNOWN" }, 0, false, false));
		}
	}
}
