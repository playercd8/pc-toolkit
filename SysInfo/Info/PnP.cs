using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace SysInfo.Info
{
    class PnP
    {
          public static void Show(TabPage tab)
        {
            string[] ItemName = { 
                                "Name",
                                "PNPDeviceID",
                                "ClassGuid",
                                "Manufacturer"
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

                foreach (string columnName in ItemName)
                    listView1.Columns.Add(columnName);

                tab.Controls.Add(listView1);
                listView1.Show();
            }

            listView1.BeginUpdate();
            listView1.Width = tab.FindForm().Width - SystemInformation.VerticalScrollBarWidth;
            listView1.Height = tab.FindForm().Height - SystemInformation.HorizontalScrollBarHeight
                - SystemInformation.CaptionHeight
                - SystemInformation.ToolWindowCaptionButtonSize.Height;

            ManagementClass c = new ManagementClass("Win32_PnPEntity");
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

                foreach (string itemName in ItemName)
                {
                    if (i == 0)
                    {
                        if (o[itemName] != null)
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

                listView1.Items.Add(item);
            }

            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}
