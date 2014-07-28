using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImgConver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "200";
            textBox2.Text = "95";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Text = "請選擇資料夾";
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string strPath = folderBrowserDialog1.SelectedPath;
            if (Directory.Exists(strPath))
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
                button2.Enabled = true;
                label2.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            checkBox1.Enabled = false;

            List<String> files2 = new List<string>();

            if (checkBox1.Checked == false)
            {
                //單一資料夾
                var files = from file in
                                Directory.EnumerateFiles(label1.Text + @"\")
                            where file.ToLower().Contains(".jpg") 
                            select file;

                foreach (var file in files)
                {
                    files2.Add(file);
                }
            } else {
                //搜尋子目錄
                var files = from file in
                            Directory.EnumerateFiles(label1.Text + @"\", "*.*", SearchOption.AllDirectories)
                        where file.ToLower().Contains(".jpg")
                        select file;
                
                foreach (var file in files)
                {
                    files2.Add(file);
                }
            }

            int nWidth;
            try
            {
                nWidth = int.Parse(textBox1.Text);
            }
            catch {
                nWidth = 200;
                textBox1.Text = nWidth.ToString();
            }
            int nQuality;
            try
            {
                nQuality = int.Parse(textBox2.Text);
                if (nQuality > 100) 
                    nQuality = 100;
                else if (nQuality < 10) 
                    nQuality = 10;
                textBox2.Text = nQuality.ToString();
            }
            catch
            {
                nQuality = 95;
                textBox2.Text = nQuality.ToString();
            }


            //處理圖形品質
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;

            //找出Encoder
            int nLength = codecs.Length;
            for (int i = 0; i < nLength; i++)
            {
                ImageCodecInfo codec = codecs[i];
                if (codec.MimeType == "image/jpeg")
                {
                    ici = codec;
                }
            }

            //參數 - 高品質圖檔
            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, nQuality);


            foreach (string file in files2) {
                using (Bitmap bmpSrc = new Bitmap(file))
                {
                    int nWidthSrc = bmpSrc.Width;
                    int nHeightSrc = bmpSrc.Height;

                    if (nWidthSrc > nWidth)
                    {
                        int nHeight = (nHeightSrc * nWidth) / nWidthSrc;

                        using (Bitmap bmpDest = new Bitmap(nWidth, nHeight, PixelFormat.Format24bppRgb))
                        {
                            using (Graphics grap = Graphics.FromImage(bmpDest))
                            {
                                Rectangle rectSrc = new Rectangle(0, 0, nWidthSrc, nHeightSrc);
                                Rectangle rectDest = new Rectangle(0, 0, nWidth, nHeight);

                                grap.DrawImage(bmpSrc, rectDest, rectSrc, GraphicsUnit.Pixel);

                                //避免檔案鎖死, 無法取代
                                bmpSrc.Dispose();

                                if ((ici == null) || (ep == null))
                                {
                                    //儲存-低品質
                                    bmpDest.Save(file, ImageFormat.Jpeg);
                                }
                                else
                                {
                                    //儲存-高品質
                                    bmpDest.Save(file, ici, ep);
                                }
                            }
                        }
                    }
                }
            }
            

            label2.Text = "轉換完成";
            button1.Enabled = true;
            button2.Enabled = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            checkBox1.Enabled = true;
        }
    }
}
