using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SysInfo.Info
{
    class ActiveMovieFilter
    {  
        public static void Show(TabPage tab)
        {
            string[] ItemName = { 
                                    "CLSID",
                                    "Name",
                                    "Path"
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

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(@"CLSID\{083863F1-70DE-11D0-BD40-00A0C911CE86}\Instance");

            //Get Data
            string keyName;
            string keyName2;
            string strValue;
            String[] SubKeyNames = regKey.GetSubKeyNames();
            foreach (string SubKeyName in SubKeyNames)
            {
                if (SubKeyName[0] == '{')
                {
                    ListViewItem item = new ListViewItem();

                    //CSLID
                    item.Text = SubKeyName;

                    keyName = string.Format(@"{0}\{1}",
                        regKey.Name,
                        SubKeyName);

                    //Name
                    strValue = (string)Registry.GetValue(keyName, "FriendlyName", "");
                    item.SubItems.Add(strValue);

                    //Path
                    keyName2 = string.Format(@"{0}\SOFTWARE\Classes\CLSID\{1}\InprocServer32",
                       Registry.LocalMachine.Name,
                       SubKeyName);
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);

                    listView1.Items.Add(item);
                }
            }
            
            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}