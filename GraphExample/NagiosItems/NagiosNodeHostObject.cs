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
			this.AddItem (new NodeLabelItem ("use", true, false) { Tag = "NagiosHostTemplate"});
			this.AddItem (new NodeTextBoxItem ("host_name", false, false) { IsImportantNodeItem = true});
			this.AddItem (new NodeTextBoxItem ("alias", false, false));
			this.AddItem (new NodeTextBoxItem ("adress", false, false));
			this.AddItem (new NodeLabelItem ("contacts", true, false) { Tag = "NagiosContact"});
			this.AddItem (new NodeLabelItem ("check_command", true, false) { Tag = "NagiosCheckCommand", IsImportantNodeItem = true});
			this.AddItem (new NodeLabelItem ("contact_groups", true, false) { Tag = "NagiosContactGroup" });
			this.AddItem (new NodeLabelItem ("notification_period", true, false) { Tag = "NagiosTimePeriod" });
			this.AddItem (new NodeLabelItem ("check_period", true, false) { Tag = "NagiosTimePeriod" });
			this.AddItem (new NodeCheckboxItem ("active_checks_enables", false, false));
			this.AddItem (new NodeCheckboxItem ("passive_checks_enables", false, false));

			this.AddItem (new NodeNumericSliderItem ("max_check_attemps", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, false) { ShowNumeric = true});

			this.AddItem (new NodeTextBoxItem ("Host Options", true, false));
			this.AddItem (new NodeDropDownItem (new string [] { "OK", "WARNING", "CRITICAL", "UNKNOWN" }, 0, false, false));
		}
	}

    //  Hosttemplaze
    //  Hostgroup

    public sealed class NagiosNodeContactObject : Node
    {
        public NagiosNodeContactObject (string title) :
            base (title)
        {
            this.AddItem (new NodeLabelItem ("use", true, false) { Tag = "NagiosContactTemplate" });
            this.AddItem (new NodeTextBoxItem ("contact_name", false, true) { Tag = "NagiosContact"});
            this.AddItem (new NodeTextBoxItem ("alias", false, false));
            this.AddItem (new NodeLabelItem ("service_notification_period", true, false) { Tag = "NagiosTimePeriod" });
            this.AddItem (new NodeLabelItem ("host_notification_period", true, false) { Tag = "NagiosTimePeriod" });
            this.AddItem (new NodeTextBoxItem ("email", false, false));

            this.AddItem (new NodeLabelItem ("service_notification_commands", true, false) { Tag = "NagiosCheckCommand" });
            this.AddItem (new NodeLabelItem ("host_notification_commands", true, false) { Tag = "NagiosCheckCommand" });

            this.AddItem (new NodeCheckboxItem ("host_notifications_enabled", false, false));
            this.AddItem (new NodeCheckboxItem ("service_notifications_enabled", false, false));

            this.AddItem (new NodeLabelItem ("service_notification_options", true, false) { Tag = "NagiosNotificationOption" });
            this.AddItem (new NodeLabelItem ("host_notification_options", true, false) { Tag = "NagiosNotificationOption" });
        }
    }

}
