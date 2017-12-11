using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsPr5
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                Char[] delimiter = {'.', '!', '?', '\n'};
                Regex regex_filter = new Regex(@"[a-zA-Z]");
                Regex only_space = new Regex(@"\S");
                richTextBox1.Text += ' ';
                List<string> Sentence = richTextBox1.Text.Split(delimiter).ToList<string>();
                Threads threads = new Threads();

                richTextBox2.Text +=
                "---------------------------------------------------------------------\n" +
                "1.1)Фильтр отбрасывает:\n" +
                "-Сообщения состоящие из пробелов;\n" +
                "-Пустые сообщения;\n" +
                "-Сообщения содержащие английские символы.\n" +
                "---------------------------------------------------------------------\n" +
                "1.2)Фильтр пропустил следующие сообщения:\n";

                //Фильтр не пропускает пустые строки, строки состоящие только из пробелов и содержащие англ. символы сообщения.
                for (int j = 0; j < Sentence.Count; j++)
                {
                    try
                    {
                        if (Sentence[j][0] == ' ') Sentence[j] = Sentence[j].Substring(1);
                        if ((regex_filter.IsMatch(Sentence[j])) || (!only_space.IsMatch(Sentence[j])) || (Sentence[j] == ""))
                        {
                            Sentence.RemoveAt(j);
                            j--;
                        }
                    }
                    catch { }
                }

                foreach(string sentence in Sentence)
                {
                    richTextBox2.Text += sentence + "\n";
                }

                richTextBox2.Text += 
                "---------------------------------------------------------------------\n" +
                "2)Данные сообщения были разбиты на следующие слова:\n";

                string[][] Words = new string[Sentence.Count][];

                for (int i = 0; i < Words.Length; i++)
                {
                    Words[i] = Sentence[i].Split(' ');
                    foreach(string word in Words[i])
                    {
                        richTextBox2.Text += word + "\n";
                    }
                }

                richTextBox2.Text +=
                "---------------------------------------------------------------------\n" +
                "3.1)Блок заменяет:\n" +
                "-А, а на @;\n" +
                "-Е, е, Ё, ё на 3;\n" +
                "-\",\" на \";\".\n" +
                "- Т, т на 7\n" +
                "---------------------------------------------------------------------\n" +
                "3.2)Блок произвел следующие замены:\n";

                var Output = threads.StartThreads(Words);

                List<List<string>> Strings = new List<List<string>>();

                foreach (Stack parameters in Output)
                {
                    richTextBox2.Text += parameters.Text + "\n";
                    while (parameters.Sentence + 1 > Strings.Count)
                    {
                        Strings.Add(new List<string>());
                    }
                    while (parameters.Word + 1 > Strings[parameters.Sentence].Count)
                    {
                        Strings[parameters.Sentence].Add("");
                    }
                    Strings[parameters.Sentence][parameters.Word] = parameters.Text;
                }

                richTextBox2.Text +=
                "---------------------------------------------------------------------\n" +
                "4)Блок нумерации:\n";
                foreach (Stack parameters in Output)
                {
                    richTextBox2.Text+= parameters.Sentence + " / " + parameters.Word +
                                        " " + parameters.Text + "\n";
                }


                string Gluing = "";

                foreach (List<string> listString in Strings)
                {
                    foreach(string Str in listString)
                    {
                        Gluing += Str + ' ';
                    }
                    Gluing += "\n";
                }
                richTextBox2.Text +=
                "---------------------------------------------------------------------\n" +
                "5)Блок склейки:\n";
                richTextBox2.Text += Gluing;
            }
        }
    }
}
