using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WinFormPr1
{
    public partial class Form1 : Form
    {
        private String input_string;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream input_stream;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((input_stream = openFileDialog1.OpenFile()) != null)
                    {
                        using (StreamReader in_reader = new StreamReader(input_stream, Encoding.GetEncoding(1251))) //Прописываем энкодинг с русскими символами
                        {
                            input_string = in_reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно прочитать файл с диска. Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream output_stream;
            String Key;
            if ((Key = textBox1.Text) != null)
            {
                if (Key.Length <= 33)
                {
                    if (input_string != null)
                    {
                        Crypt To_Crypt = new Crypt();

                        saveFileDialog1.InitialDirectory = "c:\\";
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                if ((output_stream = saveFileDialog1.OpenFile()) != null)
                                {
                                    using (StreamWriter out_writer = new StreamWriter(output_stream, Encoding.GetEncoding(1251))) //Прописываем энкодинг с русскими символами
                                    {
                                        out_writer.Write(To_Crypt.input_for_crypt(Key, input_string));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: Невозможно записать в файл. Original error: " + ex.Message);
                            }
                        }
                    }
                    else MessageBox.Show("Error: Входной файл не выбран или пуст.");
                }
                else MessageBox.Show("Error: Ключ превышает длину алфавита.");
            }
        }

    }
}
