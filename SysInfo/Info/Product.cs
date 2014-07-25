using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace SysInfo.Info
{
    class Product
    {
        public static void Show(TabPage tab)
        {
            string[] ColumnName = {
                                      "ProductName",
                                      "Version",
                                      "Description",
                                      "InstallLocation",
                                      "InstallDate",
                                      "InstallSource",
                                      "PackageName",
                                      "Language",
                                      "LocalPackage",
                                      "HelpLink",
                                      "HelpTelephone",
                                      "Vendor",
                                      "IdentifyingNumber"
                                  };
            string[] ItemName = { 
                                    "Version",
                                    "Description",
                                    "InstallLocation",
                                    "InstallDate2",
                                    "InstallSource",
                                    "PackageName",
                                    "Language",
                                    "LocalPackage",
                                    "HelpLink",
                                    "HelpTelephone",
                                    "Vendor",
                                    "IdentifyingNumber"
                                };

            ManagementClass c = new ManagementClass("Win32_Product");

            ListView listView1;
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
                item.Text = o["Name"].ToString();
                foreach (string itemName in ItemName)
                {
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
