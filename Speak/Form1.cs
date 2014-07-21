using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
//using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace Speak
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            reader = new SpeechSynthesizer();
            foreach (InstalledVoice voice in reader.GetInstalledVoices())
            {
                comboBox1.Items.Add(voice.VoiceInfo.Name);
            }
            comboBox1.SelectedIndex = 0;

            this.button1.Enabled = true;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
            this.button6.Enabled = true;
        }

        SpeechSynthesizer reader = null;
        //SpeechRecognitionEngine _recognizer = null;

        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text.Trim();
            if (text.Length > 0)
            {
                if (reader != null)
                    reader.Dispose();

                reader = new SpeechSynthesizer();
                reader.SelectVoice(this.comboBox1.Items[this.comboBox1.SelectedIndex].ToString());
                reader.SpeakAsync(text);

                comboBox1.Enabled = false;
                this.button1.Enabled = false;
                this.button3.Enabled = true;
                this.button4.Enabled = true;
                this.button5.Enabled = false;
                this.button6.Enabled = false;

                reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            }
        }

        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            this.comboBox1.Enabled = true;
            this.button1.Enabled = true;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
            this.button6.Enabled = true;
        }
 

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.textBox1.Text = File.ReadAllText(this.openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            this.comboBox1.Enabled = true;
            this.button1.Enabled = true;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                if (reader.State == SynthesizerState.Speaking)
                {
                    reader.Pause();
                    this.button1.Enabled = false;
                    this.button3.Enabled = true;
                    this.button4.Enabled = false;
                    this.button5.Enabled = true;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                if (reader.State == SynthesizerState.Paused)
                {
                    reader.Resume();
                }
                this.button1.Enabled = false;
                this.button3.Enabled = true;
                this.button4.Enabled = true;
                this.button5.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text.Trim();
            if (text.Length > 0)
            {
                saveFileDialog1.Filter = "Wave files (*.wav)|*.wav";
                saveFileDialog1.FilterIndex = 1;
                DialogResult result = this.saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    if (reader != null)
                        reader.Dispose();

                    reader = new SpeechSynthesizer();
                    reader.SelectVoice(this.comboBox1.Items[this.comboBox1.SelectedIndex].ToString());
                    reader.SetOutputToWaveFile(this.saveFileDialog1.FileName);
                    reader.SpeakAsync(text);

                    comboBox1.Enabled = false;
                    this.button1.Enabled = false;
                    this.button3.Enabled = false;
                    this.button4.Enabled = false;
                    this.button5.Enabled = false;
                    this.button6.Enabled = false;

                    reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted2);
                }
            }
        }

        void reader_SpeakCompleted2(object sender, SpeakCompletedEventArgs e)
        {
            MessageBox.Show("錄音完成");

            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            this.comboBox1.Enabled = true;
            this.button1.Enabled = true;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
            this.button6.Enabled = true;
        }
    }
}
