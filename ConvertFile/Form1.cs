using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using DirectShowLib;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using Microsoft.Win32;

namespace ConvertFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.label1.Text = "";
            this.label2.Text = "";
            this.label3.Text = "";

            DefFilter = "All files (*.*)|*.*";

            if (IsFilterExist("{1E1299A2-9D42-4F12-8791-D79E376F4143}")) //Matroska Mux Clsid
                DefFilter += "|Matroska Multimedia files (*.mkv)|*.mkv";
            if (IsFilterExist("{7C23220E-55BB-11D3-8B16-00C04FB6BD3D}")) //WM ASF Writer
                DefFilter += "|Windows Media Vidio files (*.wmv)|*.wmv";
        }

        bool IsFilterExist(string clsid) 
        { 
            string strKey = string.Format(@"CLSID\{0}", clsid);
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(strKey);
            if (regKey == null) return false;

            string strValue = (string)regKey.GetValue("", "");
            if (strValue == "") return false;

            return true;
        }

        const int WM_GRAPHNOTIFY = 0x00008001;
        //const string DefFilter = "All files (*.*)|*.*|Matroska Multimedia files (*.mkv)|*.mkv|Windows Media Vidio files (*.wmv)|*.wmv|MP4 Video (*.mp4)|*.mp4"; 
        //const string DefFilter = "All files (*.*)|*.*|Matroska Multimedia files (*.mkv)|*.mkv|Windows Media Vidio files (*.wmv)|*.wmv"; 
        string DefFilter;

        // The main com object
        FilterGraph fg = null;
        // The graphbuilder interface ref
        IGraphBuilder gb = null;
        // The mediacontrol interface ref
        IMediaControl mc = null;
        // The mediaevent interface ref
        IMediaEventEx me = null;

        private void button1_Click(object sender, EventArgs e)
        {
            string file = this.label1.Text;
            if (File.Exists(file))
            {
                this.openFileDialog1.FileName = file;
                this.openFileDialog1.InitialDirectory = Path.GetFullPath(file);
            }
            else
            {
                this.openFileDialog1.FileName = "";
                this.openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos) + @"\"; 
            }

            openFileDialog1.Filter = DefFilter;
            openFileDialog1.FilterIndex = 1;
            DialogResult result = this.openFileDialog1.ShowDialog();
	        if (result == DialogResult.OK) // Test result.
	        {
                this.label1.Text = this.openFileDialog1.FileName;
	        }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file = this.label2.Text;
            if (File.Exists(file))
            {
                this.saveFileDialog1.FileName = file;
                this.saveFileDialog1.InitialDirectory = Path.GetFullPath(file);
            }
            else
            {
                this.saveFileDialog1.FileName = "";
                this.saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos) + @"\";
            }

            saveFileDialog1.Filter = DefFilter; 
            saveFileDialog1.FilterIndex = 1;
            DialogResult result = this.saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.label2.Text = this.saveFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string src = this.label1.Text;
            string dest = this.label2.Text;
            if (File.Exists(src) == false)
            {
                MessageBox.Show("來源的檔案不存在, 無須轉換");
                return;
            }
            if (Path.GetExtension(src).ToLower() == Path.GetExtension(dest).ToLower())
            {
                MessageBox.Show("來源與目的的副檔名一樣, 無須轉換");
                return;
            }
            if (src.ToLower() == dest.ToLower())
            {
                MessageBox.Show("來源與目的的檔名一樣, 無須轉換");
                return;
            }

            try
            {
                button3.Enabled = false;

                // The main com object
                fg = new FilterGraph();
                // The graphbuilder interface ref
                gb = (IGraphBuilder)fg;
                // The mediacontrol interface ref
                mc = (IMediaControl)fg;
                // The mediaevent interface ref
                me = (IMediaEventEx)fg;

                int hr = 0;
                
                hr = me.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                DsError.ThrowExceptionForHR(hr);

                switch (Path.GetExtension(dest).ToLower())
                {
                    case ".mkv":
                        {
                            // we want to add a filewriter filter to the filter graph
                            FileWriter file_writer = new FileWriter();

                            // make sure we access the IFileSinkFilter interface to
                            // set the file name
                            IFileSinkFilter fs = (IFileSinkFilter)file_writer;

                            fs.SetFileName(dest, null);
                            DsError.ThrowExceptionForHR(hr);

                            // add the filter to the graph
                            hr = gb.AddFilter((IBaseFilter)file_writer, "File Writer");
                            DsError.ThrowExceptionForHR(hr);

                            // create an instance of the matroska multiplex filter and add it
                            // Matroska Mux Clsid = {1E1299A2-9D42-4F12-8791-D79E376F4143}	
                            Guid guid = new Guid("1E1299A2-9D42-4F12-8791-D79E376F4143");
                            Type comtype = Type.GetTypeFromCLSID(guid);
                            IBaseFilter matroska_mux = (IBaseFilter)Activator.CreateInstance(comtype);

                            hr = gb.AddFilter((IBaseFilter)matroska_mux, "Matroska Muxer");
                            DsError.ThrowExceptionForHR(hr);

                            // use Intelligent connect to build the rest of the graph
                            hr = gb.RenderFile(src, null);
                            DsError.ThrowExceptionForHR(hr);

                            // we are ready to convert
                            hr = mc.Run();
                            DsError.ThrowExceptionForHR(hr);
                        }
                        break;
                    case ".wmv":
                        {
                            // here we use the asf writer to create wmv files
                            WMAsfWriter asf_filter = new WMAsfWriter();
                            IFileSinkFilter fs = (IFileSinkFilter)asf_filter;

                            hr = fs.SetFileName(dest, null);
                            DsError.ThrowExceptionForHR(hr);

                            hr = gb.AddFilter((IBaseFilter)asf_filter, "WM Asf Writer");
                            DsError.ThrowExceptionForHR(hr);

                            hr = gb.RenderFile(src, null);
                            DsError.ThrowExceptionForHR(hr);

                            hr = mc.Run();
                            DsError.ThrowExceptionForHR(hr);
                        }
                        break;
                    default:
                        MessageBox.Show("未能處理的檔案格式, 轉換工具待修正");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                IntPtr p1, p2;
                EventCode code;

                if (me == null)
                    return;
                while (me.GetEvent(out code, out p1, out p2, 0) == 0)
                {
                    if (code == EventCode.Complete)
                    {
                        //label1.Text = "done";
                        button3.Enabled = true;
                        mc.Stop();
                    }

                    me.FreeEventParams(code, p1, p2);
                }
                return;
            }
            base.WndProc(ref m);
        }

    }
}
