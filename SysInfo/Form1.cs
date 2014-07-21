using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Input;

namespace SysInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.tabControl1.TabPages.Clear();
                        
            int i = 0;
            Assembly a = this.GetType().Assembly;
            foreach (Type type in a.GetTypes())
            {
                string tabName = type.Name;
                if ((tabName != "Form1") && (tabName != "Program") && (tabName != "Resources") && (tabName != "Settings"))
                {
                    this.tabControl1.TabPages.Add(tabName);
                    this.tabControl1.TabPages[i].Show();
                    i++;
                }
            }          
           
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor oldCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            
            TabPage tab = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];
            
            Assembly a = this.GetType().Assembly;
            object o = a.CreateInstance("SysInfo.Info."+tab.Text);  
            Type t = o.GetType();  
            MethodInfo mi = t.GetMethod("Show", new Type[] { typeof(TabPage) });
            if (mi != null) {
                mi.Invoke(o, new object [] {tab});  
            }

            Cursor.Current = oldCursor;
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
