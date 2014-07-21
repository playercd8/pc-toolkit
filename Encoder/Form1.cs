using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Sys.Security;

namespace Encoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] comboItems = { 
                                    //可逆編碼(對稱金鑰)
                                    "AES",
                                    "DES",
                                    "RC2",
                                    "TripleDES",

                                    //可逆編碼(非對稱金鑰)
                                    "RSA",

                                    //不可逆編碼(雜湊值)
                                    "MD5",
                                    "SHA1",
                                    "SHA256",
                                    "SHA384",
                                    "SHA512"
                                  };

            this.comboBox1.Items.Clear();
            foreach (string item in comboItems)
            {
                this.comboBox1.Items.Add(item);
            }
            this.comboBox1.SelectedIndex = 0;
        }

        Sys.Security.Encoder encode = new Sys.Security.Encoder();

        EncoderType GetEncoderType()
        {
            string str = this.comboBox1.Items[this.comboBox1.SelectedIndex].ToString();
            switch (str)
            {
                //可逆編碼(對稱金鑰)
                case "AES":         return EncoderType.AES;
                case "DES":         return EncoderType.DES;
                case "RC2":         return EncoderType.RC2;
                case "TripleDES":   return EncoderType.TripleDES;

                //可逆編碼(非對稱金鑰)
                case "RSA":         return EncoderType.RSA;

                //不可逆編碼(雜湊值)
                case "MD5":         return EncoderType.MD5;
                case "SHA1":        return EncoderType.SHA1;
                case "SHA256":      return EncoderType.SHA256;
                case "SHA384":      return EncoderType.SHA384;
                case "SHA512":      return EncoderType.SHA512;
                default:
                    {
                        this.comboBox1.SelectedIndex = 0;
                        return EncoderType.AES;
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncoderType type = GetEncoderType();
            encode.Key = this.textBox3.Text;
            encode.IV = this.textBox4.Text;
            this.textBox2.Text = encode.Encrypt(type, this.textBox1.Text);            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EncoderType type = GetEncoderType();
            encode.Key = this.textBox3.Text;
            encode.IV = this.textBox4.Text;
            this.textBox1.Text = encode.Decrypt(type, this.textBox2.Text);     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EncoderType type = GetEncoderType();
            encode.GenerateKey(type);
            this.textBox3.Text= encode.Key;
            this.textBox4.Text = encode.IV;
        }
    }
}
