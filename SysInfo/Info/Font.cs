using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using Microsoft.Win32;
using System.IO;

namespace SysInfo.Info
{
    class Font
    {
        public static void Show(TabPage tab)
        {
            //string[] ItemName = { 
            //                        "Name",
            //                        "Regular",
            //                        "Bold",
            //                        "Italic",
            //                        "Underline",
            //                        "Strikeout"
            //                    };


            string[] ItemName = { 
                                    "Font",
                                    "File",
                                    "FileSize"
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

           /*
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            // Get the array of FontFamily objects.
            FontFamily[] fontFamilies = installedFontCollection.Families;

            //Get Data
            foreach (FontFamily fm in fontFamilies)
            {
                ListViewItem item = new ListViewItem();

                item.Text = fm.Name;

                FontFamily ff = new FontFamily(fm.Name);

                item.SubItems.Add(ff.IsStyleAvailable(FontStyle.Regular).ToString());
                item.SubItems.Add(ff.IsStyleAvailable(FontStyle.Bold).ToString());
                item.SubItems.Add(ff.IsStyleAvailable(FontStyle.Italic).ToString());
                item.SubItems.Add(ff.IsStyleAvailable(FontStyle.Underline).ToString());
                item.SubItems.Add(ff.IsStyleAvailable(FontStyle.Strikeout).ToString());
                listView1.Items.Add(item);
            }
            */

            string InstalledWindowsFontsRegistryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts";

            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(InstalledWindowsFontsRegistryKey))
            {
                String[] ValueNames = regKey.GetValueNames();
                string strValue;
                string fileName;

                foreach (string ValueName in ValueNames) 
                {
                    ListViewItem item = new ListViewItem();

                    item.Text = ValueName;

                    //File
                    strValue = (string)regKey.GetValue(ValueName, "");
                    item.SubItems.Add(strValue);

                    if (strValue.Substring(1,2) != ":\\")
                    {
                        fileName = string.Format(@"{0}\{1}",
                            Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                            strValue);
                    } else {
                        fileName = strValue;
                    }
                    FileInfo f = new FileInfo(fileName);
                    item.SubItems.Add(f.Length.ToString());

                    listView1.Items.Add(item);
                }
            }
            

            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}
