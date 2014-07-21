using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace SysInfo.Info
{
    class BIOS
    {
        public static void Show(TabPage tab)
        {
            string[] ColumnName = {
                                      "Name",
                                      "Version",
                                      "Manufacturer",
                                      "InstallableLanguages",
                                      "ListOfLanguages",
                                      "CurrentLanguage",
                                      "PrimaryBIOS"
                                  };
            string[] ItemName = { 
                                    "Version",
                                    "Manufacturer",
                                    "InstallableLanguages",
                                    "ListOfLanguages",
                                    "CurrentLanguage",
                                    "PrimaryBIOS"                                    
                                };

            ManagementClass c = new ManagementClass("Win32_BIOS");

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

            string strDot = "";
            StringBuilder sb = new StringBuilder();

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
                        if (o[itemName].GetType().IsArray)
                        {
                            strDot = "";
                            sb.Length = 0;
                            foreach(string x in (o[itemName] as string[])) {
                                sb.Append(strDot);
                                sb.Append(x);
                                strDot = ", ";
                            }
                            item.SubItems.Add(sb.ToString());
                        }
                        else
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
