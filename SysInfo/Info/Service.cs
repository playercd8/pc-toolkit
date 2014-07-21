using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace SysInfo.Info
{
    class Service
    {
        public static void Show(TabPage tab)
        {
            string[] ColumnName = {
                                      "Service Name",
                                      "DisplayName",
                                      "Description",
                                      "ServiceType",
                                      "Started",
                                      "StartMode",
                                      "StartName",
                                      "State",
                                      "PathName",
                                      "ProcessId",
                                      "Instance Path"
                                  };
            string[] ItemName = { 
                                    "DisplayName",
                                    "Description",
                                    "ServiceType",
                                    "Started",
                                    "StartMode",
                                    "StartName",
                                    "State",
                                    "PathName",
                                    "ProcessId"
                                };

            ManagementClass c = new ManagementClass("Win32_Service");

            ListView listView1;
            if (tab.Controls.Find("listView1", false).Length > 0)
            {
                listView1 = tab.Controls.Find("listView1", false)[0] as ListView;
                listView1.Items.Clear();
            }
            else 
            {
                listView1 = new ListView();
                listView1.Name = "listView1";
                listView1.View = View.Details;
                listView1.Scrollable = true;
                listView1.FullRowSelect = true;

                foreach (string columnName in ColumnName)
                {
                    listView1.Columns.Add(columnName);
                }

                tab.Controls.Add(listView1);
                listView1.Show();                
            }

            listView1.BeginUpdate();
            listView1.Width = tab.FindForm().Width - SystemInformation.VerticalScrollBarWidth;
            listView1.Height = tab.FindForm().Height - SystemInformation.HorizontalScrollBarHeight
                - SystemInformation.CaptionHeight
                - SystemInformation.ToolWindowCaptionButtonSize.Height;
            foreach (ManagementObject o in c.GetInstances())
            {
                ListViewItem item = new ListViewItem();
                item.Text =  o["Name"].ToString();
                foreach (string itemName in ItemName) {
                    if (o[itemName] != null)
                        item.SubItems.Add(o[itemName].ToString());
                    else
                        item.SubItems.Add("");
                }
                item.SubItems.Add(o.Path.ToString());

                listView1.Items.Add(item);
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.EndUpdate();
        }
    }
}
