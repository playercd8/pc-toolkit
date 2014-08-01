using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;


namespace SysInfo.Info
{
    class COM
    {
        // 參考資料
        // http://msdn.microsoft.com/en-us/library/windows/desktop/ms691424%28v=vs.85%29.aspx
        public static void Show(TabPage tab)
        {
            string[] ItemName = { 
                                    "CLSID",
                                    "Name",
                                    "VersionIndependentProgID",
                                    "ProgId",
                                    "Version",
                                    "Path",
                                    "ThreadingModel",
                                    "ActiveX Control",
                                    "InprocHandler32",
                                    "TypeLib"
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

            //HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\CLSID");

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
                    //CLSID
                    item.Text = SubKeyName;

                    keyName = string.Format(@"{0}\{1}",
                        regKey.Name,
                        SubKeyName);

                    //Name
                    strValue = (string)Registry.GetValue(keyName, "", "");
                    item.SubItems.Add(strValue);

                    //VersionIndependentProgID
                    keyName2 = string.Format(@"{0}\{1}\VersionIndependentProgID",
                        regKey.Name,
                        SubKeyName);
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);

                    //ProgId
                    keyName2 = string.Format(@"{0}\{1}\ProgId",
                        regKey.Name,
                        SubKeyName);
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);

                    //Version
                    keyName2 = string.Format(@"{0}\{1}\Version",
                        regKey.Name,
                        SubKeyName);
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);

                    keyName2 = string.Format(@"{0}\{1}\InprocServer32",
                        regKey.Name,
                        SubKeyName);

                    //Path
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);

                    //ThreadingModel
                    strValue = (string)Registry.GetValue(keyName2, "ThreadingModel", "");
                    item.SubItems.Add(strValue);

                    //ActiveX Control
                    keyName2 = string.Format(@"SOFTWARE\Classes\CLSID\{0}\Control",
                      SubKeyName);
                    RegistryKey regKey2 = Registry.LocalMachine.OpenSubKey(keyName2);
                    strValue = (regKey2 != null).ToString();
                    item.SubItems.Add(strValue);
                   

                    //InprocHandler32
                    keyName2 = string.Format(@"{0}\{1}\InprocHandler32",
                        regKey.Name,
                        SubKeyName);
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);


                    //TypeLib
                    keyName2 = string.Format(@"{0}\{1}\TypeLib",
                        regKey.Name,
                        SubKeyName);
                    strValue = (string)Registry.GetValue(keyName2, "", "");
                    item.SubItems.Add(strValue);

                    //HKEY_LOCAL_MACHINE\SOFTWARE\Classes\TypeLib

                    listView1.Items.Add(item);
                }
            }
            
            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}
