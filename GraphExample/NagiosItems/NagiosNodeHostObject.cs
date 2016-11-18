using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Items;

namespace GraphNodes.NagiosItems.Host
{

	//  Hosttemplate
	/*
		define host {
			host_name	host_name
			alias	alias
			display_name	display_name
			address	address
			parents	host_names
			hourly_value	#
			hostgroups	hostgroup_names
			check_command	command_name
			initial_state	[o,d,u]
			max_check_attempts	#
			check_interval	#
			retry_interval	#
			active_checks_enabled	[0/1]
			passive_checks_enabled	[0/1]
			check_period	timeperiod_name
			obsess_over_host|obsess	[0/1]
			check_freshness	[0/1]
			freshness_threshold	#
			event_handler	command_name
			event_handler_enabled	[0/1]
			low_flap_threshold	#
			high_flap_threshold	#
			flap_detection_enabled	[0/1]
			flap_detection_options	[o,d,u]
			process_perf_data	[0/1]
			retain_status_information	[0/1]
			retain_nonstatus_information	[0/1]
			contacts	contacts
			contact_groups	contact_groups
			notification_interval	#
			first_notification_delay	#
			notification_period	timeperiod_name
			notification_options	[d,u,r,f,s]
			notifications_enabled	[0/1]
			stalking_options	[o,d,u]
			notes	note_string
			notes_url	url
			action_url	url
			icon_image	image_file
			icon_image_alt	alt_string
			vrml_image	image_file
			statusmap_image	image_file
			2d_coords	x_coord,y_coord
			3d_coords	x_coord,y_coord,z_coord
		}    	
	 */
	//  Register 0
	//  Nagios define host
	public sealed class NagiosNodeHostTemplateObject : Node
    {
        public NagiosNodeHostTemplateObject (string title) :
            base (title)
        {
            this.AddItem (new NodeLabelItem ("use", true, false) { Tag = "NagiosHostTemplate" });
            this.AddItem (new NodeTextBoxItem ("host_name", false, false) { IsImportantNodeItem = true });
            this.AddItem (new NodeTextBoxItem ("alias", false, false));
            this.AddItem (new NodeTextBoxItem ("adress", false, false));
            this.AddItem (new NodeLabelItem ("contacts", true, false) { Tag = "NagiosContact" });
            this.AddItem (new NodeLabelItem ("check_command", true, false) { Tag = "NagiosCheckCommand", IsImportantNodeItem = true });
            this.AddItem (new NodeLabelItem ("contact_groups", true, false) { Tag = "NagiosContactGroup" });
            this.AddItem (new NodeLabelItem ("notification_period", true, false) { Tag = "NagiosTimePeriod" });
            this.AddItem (new NodeLabelItem ("check_period", true, false) { Tag = "NagiosTimePeriod" });
            this.AddItem (new NodeCheckboxItem ("active_checks_enables", false, false));
            this.AddItem (new NodeCheckboxItem ("passive_checks_enables", false, false));

            this.AddItem (new NodeNumericSliderItem ("max_check_attemps", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, false) { ShowNumeric = true });

            this.AddItem (new NodeTextBoxItem ("Host Options", true, false));
            this.AddItem (new NodeDropDownItem (new string [] { "OK", "WARNING", "CRITICAL", "UNKNOWN" }, 0, false, false));


			this.AddItem (new NodeLabelItem ("check_command", true, false) { Tag = "NagiosCheckCommand", IsImportantNodeItem = true });

			this.AddItem (new NodeLabelItem ("notification_period", true, false) { Tag = "NagiosTimePeriod" });
			this.AddItem (new NodeLabelItem ("check_period", true, false) { Tag = "NagiosTimePeriod" });
			this.AddItem (new NodeCheckboxItem ("active_checks_enables", false, false));
			this.AddItem (new NodeCheckboxItem ("passive_checks_enables", false, false));

			this.AddItem (new NodeNumericSliderItem ("max_check_attemps", 55.0f, 16.0f, 1, 10.0f, 1.0f, false, false) { ShowNumeric = true });

			this.AddItem (new NodeTextBoxItem ("Host Options", true, false));
			this.AddItem (new NodeDropDownItem (new string [] { "OK", "WARNING", "CRITICAL", "UNKNOWN" }, 0, false, false));
		}
	}

	//#####
	//  Nagios define hostgroup
	/*
    define hostgroup
    {
        i hostgroup_name 
        i alias 
        n members 
        n hostgroup_members 
        n notes note_string
        n notes_url url
        n action_url url
    }
    */
	//  #####
	public sealed class NagiosNodeHostGroupObject : Node
	{
		public NagiosNodeHostGroupObject (string title) :
			base (title)
		{
			this.AddItem (new NodeTextBoxItem ("hostgroup_name", false, true) { Tag = "NagiosHostGroup", IsImportantNodeItem = true });
			this.AddItem (new NodeTextBoxItem ("alias", false, false) { IsImportantNodeItem = true });
			this.AddItem (new NodeLabelItem ("members", true, true) { Tag = "NagiosHostObject" });
			this.AddItem (new NodeLabelItem ("hostgroup_members", true, false) { Tag = "NagiosHostGroup" });
			this.AddItem (new NodeTextBoxItem ("notes", false, false));
			this.AddItem (new NodeLabelItem ("notes_url", true, false) { Tag = "NagiosUrl" });
			this.AddItem (new NodeLabelItem ("action_url", true, false) { Tag = "NagiosUrl" });
		}
	}
    
	//  Nagios define host
	public sealed class NagiosNodeHostObject : Node
	{
		public NagiosNodeHostObject (string title) :
			base (title)
		{
			this.AddItem (new NodeLabelItem ("use", true, false) { Tag = "NagiosHostTemplate"});
			this.AddItem (new NodeTextBoxItem ("host_name", false, true, true) { Tag = "NagiosHostObject", IsImportantNodeItem = true});
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
			this.AddItem (new NodeCheckListBoxItem (new string [] { "OK", "WARNING", "CRITICAL", "UNKNOWN" }, 0, false, false));
		}
	}
}
