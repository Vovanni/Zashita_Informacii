using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WinFormPr1_decrypt
{
    public partial class Form1 : Form
    {
        private Acsii Converter = new Acsii();          //Экземпляр класса конвертации acsii
        private String input_to_decr;                   //Текст для дешифрации
        private String output = "";                     //Текст после дешифрации
        private double[] freq_tab = new double[33];     //Частотная таблица внешняя
        private double[] input_tab = new double[33];    //Частотная таблица текста
        private int[] conf_tab = new int[33];           //Таблица соответствия букв

        public Form1()
        {
            InitializeComponent();
        }

        private double module(double m)
        {
            if (m < 0) return (m * (-1));
            else return m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream tab_stream = openFileDialog1.OpenFile();
                if(checkBox1.Checked)
                {
                    Get_tab Input_tab = new Get_tab();      //Создаем экземпляр класса который принимает готовую таблицу
                    freq_tab = Input_tab.Tab(tab_stream);
                }
                else
                {
                    Text_analize Tab_analize = new Text_analize();
                    freq_tab = Tab_analize.Analize(tab_stream);
                    if(checkBox2.Checked)                       //Сохранение таблицы в файл
                    {
                        saveFileDialog1.InitialDirectory = "c:\\";
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            Stream save_tab_stream = saveFileDialog1.OpenFile();
                            try
                            {
                                if (save_tab_stream != null)
                                    using (BinaryWriter tab_writer = new BinaryWriter(save_tab_stream))
                                    {
                                        for (int i = 0; i <= 32; i++) tab_writer.Write(freq_tab[i]); //Запись побитно в файл
                                    }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: Невозможно записать в файл на диске. Original error: " + ex.Message);
                            }
                        }

                    }
                }

                richTextBox1.Text = "1.Входная частотная таблица:\n";
                for (int i = 0; i <= 32; i++)
                {
                    richTextBox1.Text += Converter.return_acsii(i,true) + ". " + freq_tab[i] + "\n";
                }
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            Stream input_stream;
            openFileDialog2.InitialDirectory = "c:\\";
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((input_stream = openFileDialog2.OpenFile()) != null)
                    {
                        using (StreamReader in_reader = new StreamReader(input_stream, Encoding.GetEncoding(1251))) //Прописываем энкодинг с русскими символами
                        {
                            input_to_decr = in_reader.ReadToEnd();
                        }
                        Text_analize Input_analize = new Text_analize();
                        input_tab = Input_analize.Analize(openFileDialog2.OpenFile());

                        double[][] difference = new double[33][];

                        for (int i = 0; i <= 32; i++)
                        {
                            difference[i] = new double[33];
                        }

                        for (int i=0; i<=32; i++)
                        {
                            for (int k=0; k<=32; k++)
                            {
                                difference[i][k] = module(freq_tab[k] - input_tab[i]);
                           // Debug.Write(Converter.return_acsii(i, true) + ", " + Converter.return_acsii(k, true) + " diff = " + difference[i][k]);
                            }
                            //Debug.Write('\n');
                        }

                        bool[] already_used_i = new bool[33];
                        bool[] already_used_k = new bool[33];

                        for (int n = 0; n <= 32; n++)
                        {
                            double min_el = 1;
                            int min_i = 0;
                            int min_k = 0;
                            for (int i = 0; i <= 32; i++)
                            {
                                if (!already_used_i[i])
                                    for (int k = 0; k <= 32; k++)
                                    {
                                        if (!already_used_k[k])
                                            if (difference[i][k] < min_el)
                                            {
                                                min_el = difference[i][k];
                                                min_i = i;
                                                min_k = k;
                                            }
                                    }
                            }
                            conf_tab[min_i] = min_k;
                            already_used_i[min_i] = true;
                            already_used_k[min_k] = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно прочитать файл с диска. Original error: " + ex.Message);
                }
            }
            richTextBox1.Text += "\n2.Частотная таблица по тексту:\n";
            for (int i = 0; i <= 32; i++)
            {
                richTextBox1.Text += Converter.return_acsii(i, true) + ". " + input_tab[i];
                if (i != 32) richTextBox1.Text += "\n";
            }

            for (int i = 0; i <= 32; i++)
            {
                textBox1.Text += Converter.return_acsii(conf_tab[i], true);
            }
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 32; i++)
            {
                conf_tab[i] = Converter.return_number(textBox1.Text[i]);
            }

            for (int i = 0; i < input_to_decr.Length; i++)
            {
                if (Converter.return_number(input_to_decr[i]) != 33)
                {
                    output += Converter.return_acsii(conf_tab[Converter.return_number(input_to_decr[i])], Converter.is_big(input_to_decr[i]));
                }
                else output += input_to_decr[i];
            }

            saveFileDialog2.InitialDirectory = "c:\\";
            saveFileDialog2.RestoreDirectory = true;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Stream save_file_stream = saveFileDialog2.OpenFile();
                try
                {
                    if (save_file_stream != null)
                        using (StreamWriter tab_writer = new StreamWriter(save_file_stream))
                        {
                            tab_writer.Write(output);
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Невозможно записать в файл на диск. Original error: " + ex.Message);
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Enabled = !checkBox2.Enabled;
            if (checkBox2.Enabled) button1.Text = "Выбрать текст для таблицы";
            else button1.Text = "Выбрать готовую таблицу";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = !checkBox1.Enabled;
            if (checkBox1.Enabled) button1.Text = "Выбрать текст для таблицы";
            else button1.Text = "Выбрать текст и сохранить таблицу";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
