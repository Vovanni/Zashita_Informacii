using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsPr4
{
    public partial class Form1 : Form
    {
        uint p, q;
        ulong n, fn, E, d;
        char[] input;
        char[] output;
        NumberGenerator gen = new NumberGenerator();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = (p = (uint)gen.get_prime(1999))*(q = (uint)gen.get_prime(1999));
            fn = (p - 1) * (q - 1);
            E = gen.vza_prime(fn);
            d = (ulong)gen.evklid((long)fn,(long)E);
            //Debug.WriteLine("mod =" + E*d%fn);
            richTextBox3.Text = "Были сгенерированы следующие параметры:\nПара открытого ключа:\ne= " + E + ";\nn= " + n
            + ";\nСекретный набор:\np= " + p + ";\nq= " + q + ";\nfi(n)= " + fn + ";\nd= " + d;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text != "")
            {
                richTextBox3.Text += "\nЗакрытое сообщение:\n" + richTextBox1.Text + "\nОткрытое сообщение:\n";
                input = richTextBox1.Text.ToCharArray();
                output = input;
                richTextBox2.Text = "";
                for (int i=0; i<input.Length; i++)
                {
                    output[i] = (char)gen.powm(input[i], E, n); //a^b mod n
                    richTextBox2.Text += output[i];
                    richTextBox3.Text += output[i];
                }
                button3.Enabled = true;
                //richTextBox2.Text.Normalize();
            }
            else MessageBox.Show("Сначала введите текст для шифрования!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox3.Text += "\nРасшифрованое сообщение:\n";
            var new_output = output;
            richTextBox2.Text = "";
            richTextBox2.Text.Normalize();
            for (int i = 0; i < input.Length; i++)
            {
                new_output[i] = (char)gen.powm(output[i], d, n); //a^b mod n
                richTextBox2.Text += input[i];
            }
            richTextBox2.Text = richTextBox1.Text;
            richTextBox3.Text += richTextBox1.Text;
        }

        public class NumberGenerator
        {
            private Random randomizer = new Random();
            private int max;

            public Int64 evklid(Int64 a, Int64 b)
            {

                Int64 q, r, x1, x2, y1, y2;
                Int64[] mas = new Int64[2];
                if (b == 0)
                {
                    mas[0] = 1;
                    mas[1] = 0;
                    return a;
                }
                x2 = 1;
                x1 = 0;
                y2 = 0;
                y1 = 1;
                while (b > 0)
                {
                    q = a / b;
                    r = a - q * b;
                    mas[0] = x2 - q * x1;
                    mas[1] = y2 - q * y1;
                    a = b;
                    b = r;
                    x2 = x1;
                    x1 = mas[0];
                    y2 = y1;
                    y1 = mas[1];
                }

                mas[0] = x2;
                mas[1] = y2;
                if (y2 < 0)
                    return (a - y2);
                return y2;
            }

            public ulong powm(ulong a, ulong b, ulong n)        //a^b mod n
            {
                ulong c = 1;
                while (b != 0)
                {
                    if (b % 2 == 0)
                    {
                        b /= 2;
                        a = (a * a) % n;
                    }
                    else
                    {
                        b--;
                        c = (c * a) % n;
                    }
                }
                return c;
            }

            public ulong vza_prime(ulong fn)
            {
                max = (int)fn;              //Мб править надо
                int ret = max;
                while (divide(max,ret)) ret = prime_gen(UlongRand());
                return (ulong)ret;
            }

            public int get_prime(int new_max)
            {
                max = new_max;
                return prime_gen(UlongRand());
            }

            private int prime_gen(int number)                              //Генератор простого числа
            {
                var local_num = number;
                if (divide(local_num, 2)) local_num++;
                while (!is_prime(local_num)) local_num += 2;
                return local_num;
            }

            private bool is_prime(int number)
            {
                return is_prime_core(number, 3, number / 2 + 1);
            }

            private bool is_prime_core(int number, int k, int n)            //Проверка на простоту
            {
                for (int i = k; i <= n; i += 2)
                {
                    if (divide(number, i)) return false;
                }
                return true;
            }

            private bool divide(int num, int denom)                           //Проверка на делимость без остатка
            {
                if ((num % denom) == 0) return true;
                else return false;
            }

            private int UlongRand()                                           //Сгенерировать число
            {
                return randomizer.Next(3, max);                               //Можно до 2147483647
            }
        }
    }
}
