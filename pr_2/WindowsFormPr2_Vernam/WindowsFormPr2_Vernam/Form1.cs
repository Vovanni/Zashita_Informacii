using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormPr2_Vernam
{
    public partial class Form1 : Form
    {
        Crypt Crypter = new Crypt();
        Crypt.TwoBytesString Text_Key = new Crypt.TwoBytesString();
        Encoding Rus;

        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = richTextBox1.Text != "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Changer(checkBox1.Checked, checkBox2, checkBox3, button3);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Changer(checkBox2.Checked, checkBox1, checkBox3, button3);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Changer(checkBox3.Checked, checkBox2, checkBox1, button3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Text_Key = Crypter.KeyAndText(richTextBox1.Text);

            saveFileDialog1.InitialDirectory = "c:\\";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream save_key = saveFileDialog1.OpenFile();
                try
                {
                    if (save_key != null)
                    {
                        using (BinaryWriter key_writer = new BinaryWriter(save_key))
                        {
                            for (int i = 0; i < Text_Key.Key.Length; i++) key_writer.Write(Text_Key.Key[i]); //Запись побитно в файл
                        }
                        button2.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно записать в файл. Original error: " + ex.Message);
                }
            }

            for (int i = 0; i < Text_Key.Text.Length; i++)
            { 
                Debug.Write("Key= " + Text_Key.Key[i] + " Ans= " + Text_Key.Text[i] + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog2.InitialDirectory = "c:\\";
            saveFileDialog2.RestoreDirectory = true;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Stream save_text = saveFileDialog2.OpenFile();
                try
                {
                    if (save_text != null)
                    {
                        using (BinaryWriter key_writer = new BinaryWriter(save_text))
                        {
                            for (int i = 0; i < Text_Key.Text.Length; i++) key_writer.Write(Text_Key.Text[i]); //Запись побитно в файл
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно записать в файл. Original error: " + ex.Message);
                }
            }

            for (int i = 0; i < Text_Key.Text.Length; i++)
            {
                Debug.Write("Key= " + Text_Key.Key[i] + " Ans= " + Text_Key.Text[i] + "\n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stream input_text;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((input_text = openFileDialog1.OpenFile()) != null)
                    {
                        Array.Resize(ref Text_Key.Text, (int)input_text.Length);
                        Rus = Encoding.GetEncoding(1251);
                        using (BinaryReader text_reader = new BinaryReader(input_text, Rus)) //Прописываем энкодинг с русскими символами
                        {
                            for (int i = 0; i < Text_Key.Text.Length; i++)
                            {
                                Text_Key.Text[i] = text_reader.ReadByte();
                                //Debug.Write("Text " + i + " " + Text_Key.Text[i] + "\n");
                            }
                        }
                        button4.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно прочитать файл с диска. Original error: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Stream input_key;
            openFileDialog2.InitialDirectory = "c:\\";
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((input_key = openFileDialog2.OpenFile()) != null)
                    {
                        Array.Resize(ref Text_Key.Key, (int)input_key.Length);
                        using (BinaryReader key_reader = new BinaryReader(input_key, Rus)) //Прописываем энкодинг с русскими символами
                        {
                            for (int i = 0; i < Text_Key.Text.Length; i++)
                            {
                                Text_Key.Key[i] = key_reader.ReadByte();
                               //Debug.Write("Key " + i + " " + Text_Key.Key[i] + "\n");
                            }
                        }
                        button5.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно прочитать файл с диска. Original error: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                richTextBox2.Enabled = true;
                if (Text_Key.Key.Length == Text_Key.Text.Length)
                {
                    byte[] Output_text = new byte[Text_Key.Text.Length];
                    for (int i=0; i < Text_Key.Key.Length; i++)
                    {
                        Output_text[i] = Crypter.Mod(Text_Key.Key[i], Text_Key.Text[i]);
                        Debug.Write("Output " + i + " " + Output_text[i] + "\n");
                    }
                    char[] Output_char = Rus.GetChars(Output_text,0,Output_text.Length);
                    //Debug.Write(Output_char[0]);
                    String Output_String = "";
                    for (int i = 0; i < Output_char.Length; i++) Output_String += Output_char[i];
                    //Debug.Write(Output_String);
                    richTextBox2.Text = Output_String;
                }
            }
        }

        private void Changer(bool Event, CheckBox a, CheckBox b, Button c)
        {
                a.Enabled = !Event;
                b.Enabled = !Event;
                c.Enabled = Event;
        }
    }
}
