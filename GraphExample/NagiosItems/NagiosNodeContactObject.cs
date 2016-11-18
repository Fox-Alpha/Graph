using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Items;

namespace GraphNodes.NagiosItems.Contact
{
    public sealed class NagiosNodeContactTemplateObject : Node
    {
        public NagiosNodeContactTemplateObject (string title) :
            base (title)
        {
        }
    }

    public sealed class NagiosNodeContactGroupObject : Node
    {
        public NagiosNodeContactGroupObject (string title) :
            base (title)
        {
        }
    }

	public sealed class NagiosNodeContactObject : Node
	{
		public NagiosNodeContactObject (string title) :
			base (title)
		{
			this.AddItem (new NodeLabelItem ("use", true, false) { Tag = "NagiosContactTemplate" });
			this.AddItem (new NodeTextBoxItem ("contact_name", false, true) { Tag = "NagiosContact" });
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
