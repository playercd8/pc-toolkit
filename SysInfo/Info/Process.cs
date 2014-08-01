using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace SysInfo.Info
{
    class Process
    {
        public static void Show(TabPage tab)
        {
            string[] ColumnName = {
                                    "Process Name",
                                    "CommandLine",
                                    "ProcessId",
                                    "Priority",
                                    "ThreadCount",
                                    "KernelModeTime",
                                    "UserModeTime",
                                    "VirtualSize",
                                    "CreationDate",
                                    "Owner"
                                };
            string[] ItemName = { 
                                "Name",
                                "CommandLine",
                                "ProcessId",
                                "Priority",
                                "ThreadCount",
                                "KernelModeTime",
                                "UserModeTime",
                                "VirtualSize",
                                "CreationDate"
                            };

            string[] ToolTip = new string[ItemName.Length];

            ListView2 listView1;
            if (tab.Controls.Find("listView1", false).Length > 0)
            {
                listView1 = tab.Controls.Find("listView1", false)[0] as ListView2;
                listView1.Items.Clear();
            }
            else
            {
                listView1 = new ListView2();
                listView1.Name = "listView1";
                listView1.View = View.Details;
                listView1.Scrollable = true;
                listView1.FullRowSelect = true;

                foreach (string columnName in ColumnName)
                    listView1.Columns.Add(columnName);

                tab.Controls.Add(listView1);
                listView1.Show();
            }

            listView1.BeginUpdate();
            listView1.Width = tab.FindForm().Width - SystemInformation.VerticalScrollBarWidth;
            listView1.Height = tab.FindForm().Height - SystemInformation.HorizontalScrollBarHeight
                - SystemInformation.CaptionHeight
                - SystemInformation.ToolWindowCaptionButtonSize.Height;

            ManagementClass c = new ManagementClass("Win32_Process");
            c.Options.UseAmendedQualifiers = true;

            //Get ToolTip
            int i = 0;
            foreach (string itemName in ItemName)
            {
                ToolTip[i] = "";
                foreach (PropertyData p in c.Properties)
                {
                    if (p.Name.Equals(itemName))
                    {
                        foreach (QualifierData q in p.Qualifiers)
                        {
                            if (q.Name.Equals("Description"))
                            {
                                ToolTip[i] = q.Value.ToString();
                                break;
                            }
                        }
                        continue;
                    }
                }
                i++;
            }

            //Get Data
            foreach (ManagementObject o in c.GetInstances())
            {
                ListViewItem item = new ListViewItem();
                i = 0;

                item.Text = o["Name"].ToString();
                foreach (string itemName in ItemName)
                {
                    if (i == 0)
                    {
                        item.Text = o[itemName].ToString();
                        item.Tag = ToolTip[i];
                    }
                    else
                    {
                        if (o[itemName] != null)
                            item.SubItems.Add(o[itemName].ToString());
                        else
                            item.SubItems.Add("");
                        item.SubItems[i].Tag = ToolTip[i];
                    }
                    i++;
                }

                ManagementBaseObject outParams = o.InvokeMethod("GetOwner", null, null);
                if ((outParams["Domain"] != null) && (outParams["User"] != null))
                    item.SubItems.Add(outParams["Domain"].ToString() + "/" + outParams["User"].ToString());
                else
                    item.SubItems.Add("");
                item.SubItems[i].Tag = "";


                listView1.Items.Add(item);
            }
            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}
