using System;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace WinFormPr1_decrypt
{
    class Text_analize
    {
        public double[] Analize (Stream Input)
        {
            Acsii Converter = new Acsii();      //Создаем экземпляр класса для конвертации acsii символов в int
            double[] array = new double[33];    //Выходной массив
            long[] letters = new long[33];      //Количество встреченных символов
            double all_letters = 0;
            String Text;
            try
            {
                if (Input != null)
                {
                    using (StreamReader in_reader = new StreamReader(Input, Encoding.GetEncoding(1251)))
                    {
                        Text = in_reader.ReadToEnd();
                    }

                    for(int i=0; i < Text.Length; i++)
                    {
                        int symbol;
                        symbol = Converter.return_number(Text[i]); //Конвертируем символ в алфавит 0-33. где - 33 остальные символы
                        if (symbol != 33)
                        {
                            letters[symbol]++;       //Инкрементируем ячейку соответствующую символу
                            all_letters++;
                        }
                    }

                    for(int i=0; i < letters.Length; i++)
                    {
                        if (letters[i] > 0)
                            array[i] = (letters[i] / all_letters);
                        else array[i] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Невозможно прочитать файл с диска. Original error: " + ex.Message);
            }
            return array;
        }

    }
}
