using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SysInfo.Info
{
          
    class IME
    {
        [DllImport("Imm32.dll", CharSet = CharSet.Auto)]
        public extern static int ImmGetDescription(IntPtr Hkl, StringBuilder sbName, int nBuffer);


        /// <summary>  
        /// 得到指定输入法的说明
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        private static string GetImmName(InputLanguage input)
        {
            int nBuffer = 0;
            string sDesc = "";

            nBuffer = ImmGetDescription(input.Handle, null, nBuffer);
            if (nBuffer != 0)
            {
                StringBuilder sbName = new StringBuilder(nBuffer);
                ImmGetDescription(input.Handle, sbName, nBuffer);
                sDesc = sbName.ToString();
            }
            if (string.IsNullOrEmpty(sDesc))
            {
                sDesc = input.LayoutName;
            }

            return sDesc;
        }  

        public static void Show(TabPage tab)
        {
            string[] ItemName = { 
                                    "Name"
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

            string strFile = string.Format(@"{0}\{1}",
                            Environment.GetFolderPath(Environment.SpecialFolder.System),
                            "msctf.dll");
            //判斷TSF是否存在?
            if (System.IO.File.Exists(strFile) != true)
            {
                //收集系統已安裝的輸入法
                InputLanguageCollection myInput = InputLanguage.InstalledInputLanguages;

                //Get Data
                foreach (InputLanguage input in myInput)
                {
                    ListViewItem item = new ListViewItem();

                    //Name
                    item.Text = GetImmName(input);
                    listView1.Items.Add(item);
                }
            }
            else
            {
                //使用TSF
                foreach (short langID in TSFWapper.GetLangIDs())
                {
                    foreach (string name in TSFWapper.GetInputMethodList(langID))
                    {
                        ListViewItem item = new ListViewItem();

                        item.Text = name;

                        listView1.Items.Add(item);
                    }
                }
            }


            listView1.TodoAutoResizeColumns();
            listView1.EndUpdate();
        }
    }
}