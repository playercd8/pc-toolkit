using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace SysInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.tabControl1.TabPages.Clear();

            string [] TabName = {
                                    "OperatingSystem",
                                    "Processor",
                                    "LogicalDisk",
                                    "Service",
                                    "Process"
                                };
            int i = 0;
            foreach (string tabName in TabName) 
            {
                this.tabControl1.TabPages.Add(tabName);
                this.tabControl1.TabPages[i].Show();
                i++;
            }

            tabControl1_SelectedIndexChanged(null, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tab = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];
            switch (tab.Text)
            {
                case "OperatingSystem":
                    Info.OperatingSystem.Show(tab); break;
                case "Processor":
                    Info.Processor.Show(tab);
                    break;
                case "LogicalDisk":
                    Info.LogicalDisk.Show(tab);
                    break;
                case "Service": 
                    Info.Service.Show(tab); 
                    break;
                case "Process":
                    Info.Process.Show(tab);
                    break;
                default:
                    break;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            tabControl1.Width = this.FindForm().Width;
            tabControl1.Height = this.FindForm().Height - SystemInformation.CaptionHeight;

            TabPage tab = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];
            if (tab.Controls.Count > 0)
            {
                ListView listView1;
                if (tab.Controls.Find("listView1", false).Length > 0)
                {
                    listView1 = tab.Controls.Find("listView1", false)[0] as ListView;
                    listView1.BeginUpdate();
                    listView1.Width = tab.FindForm().Width - SystemInformation.VerticalScrollBarWidth;
                    listView1.Height = tab.FindForm().Height - SystemInformation.HorizontalScrollBarHeight
                        - SystemInformation.CaptionHeight
                        - SystemInformation.ToolWindowCaptionButtonSize.Height;
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.EndUpdate();
                }
            }
        }
    }
}
