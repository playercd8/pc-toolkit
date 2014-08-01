using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SysInfo.Info
{
    class FileExt
    {
        public static void Show(TabPage tab)
        {
            string[] ItemName = { 
                                    "FileExt",
                                    "PerceivedType",
                                    "Content Type",
                                    "Description",
                                    "DefaultIcon",
                                    "open",
                                    "edit",
                                    "play",
                                    "print"
                                };

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

            RegistryKey regKey = Registry.ClassesRoot;

            //Get Data
            string keyName;
            string SubKeyNameRedirect = "";
            object o;
            string strValue;
            String[] SubKeyNames = regKey.GetSubKeyNames();
            foreach (string SubKeyName in SubKeyNames) 
            {
                if (SubKeyName[0] =='.')
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = SubKeyName;

                    keyName = string.Format(@"{0}\{1}",
                        Registry.ClassesRoot.ToString(),
                        SubKeyName);

                    //PerceivedType
                    strValue = (string)Registry.GetValue(keyName, "PerceivedType", "");
                    item.SubItems.Add(strValue);

                    //Content Type
                    strValue = (string)Registry.GetValue(keyName, "Content Type", "");
                    item.SubItems.Add(strValue);
                  
                    o = Registry.GetValue(keyName, "", null);
                    if (o == null)
                        SubKeyNameRedirect = SubKeyName;
                    else
                        SubKeyNameRedirect = (string)o;

                    //Description
                    keyName = string.Format(@"{0}\{1}",
                       Registry.ClassesRoot.ToString(),
                       SubKeyNameRedirect);
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    //DefaultIcon
                    keyName = string.Format(@"{0}\{1}\DefaultIcon",
                        Registry.ClassesRoot.ToString(),
                        SubKeyNameRedirect);
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    //open
                    keyName = string.Format(@"{0}\{1}\shell\open\command",
                        Registry.ClassesRoot.ToString(),
                        SubKeyNameRedirect);
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    //edit
                    keyName = string.Format(@"{0}\{1}\shell\edit\command",
                        Registry.ClassesRoot.ToString(),
                        SubKeyNameRedirect);
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    //play
                    keyName = string.Format(@"{0}\{1}\shell\play\command",
                        Registry.ClassesRoot.ToString(),
                        SubKeyNameRedirect);
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    //print
                    keyName = string.Format(@"{0}\{1}\shell\print\command",
                        Registry.ClassesRoot.ToString(),
                        SubKeyNameRedirect);
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    listView1.Items.Add(item);
                }
            }
           
            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}
