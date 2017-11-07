using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormPr2_Diffie
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrimeWorker Forecer = new PrimeWorker();
            Forecer.get_two_primes();
            uint g = Forecer.min_prime;
            ulong p = Forecer.max_prime;
            p = 2*p+1;                                //Умножаем p на 2 и прибавляем 1 для безопасности

            uint a = 0, b = 0;
            ulong A = 0, B = 0;
            ulong KA = 0, KB = 0;

            ThreadStart core_1_start = new ThreadStart(get_A);
            ThreadStart core_2_start = new ThreadStart(get_B);
            Thread th_1 = new Thread(core_1_start);
            Thread th_2 = new Thread(core_2_start);
            th_1.IsBackground = true;
            th_2.IsBackground = true;
            th_1.Start();
            th_2.Start();
            while (th_1.IsAlive || th_2.IsAlive);

            richTextBox1.Text += "Алиса генерирует a,g,p:\na=" + a + ";\ng=" + g + ";\np=" + p + ";\n";
            richTextBox3.Text += "Боб генерирует b:\nb=" + b + ";\n";

            richTextBox1.Text += "Алиса вычисляет A = (g^a) mod p:\nA=" + A +";\n" + "Алиса отправлет Бобу A, g, p;\n";
            richTextBox2.Text += "Ева перехватывает от Алисы A,g,p:\nA=" + A + ";\ng=" + g + ";\np=" + p + ";\n";
            richTextBox3.Text += "Боб получает от Алисы A,g,p:\nA=" + A + ";\ng=" + g + ";\np=" + p + ";\n";

            richTextBox3.Text += "Боб вычисляет B = (g^b) mod p:\nB=" + B +";\n" + "Боб отправлет Алисе B;\n";
            richTextBox2.Text += "Ева перехватывает от Боба B:\nB=" + B + ";\n";
            richTextBox1.Text += "Алиса получает от Боба B:\nB=" + B + ";\n";

            ThreadStart core_3_start = new ThreadStart(get_KA);
            ThreadStart core_4_start = new ThreadStart(get_KB);
            Thread th_3 = new Thread(core_3_start);
            Thread th_4 = new Thread(core_4_start);
            th_3.IsBackground = true;
            th_4.IsBackground = true;
            th_3.Start();
            th_4.Start();
            while (th_3.IsAlive || th_4.IsAlive);

            richTextBox3.Text += "Боб вычисляет закрытый ключ K = (A^b) mod p:\nK=" + KB + ";\n";
            richTextBox1.Text += "Алиса вычисляет закрытый ключ K = (B^a) mod p:\nK=" + KA + ";\n";

            void get_A()
            {
                a = Forecer.UintLessRand(999999);
                A = Forecer.powm(g, a, p);
            }

            void get_B()
            {
                b = Forecer.UintLessRand(999999);
                B = Forecer.powm(g, b, p);
            }

            void get_KA()
            {
                KA = Forecer.powm(B, a, p);
            }

            void get_KB()
            {
                KB = Forecer.powm(A, b, p);
            }
        }
    }

    public class PrimeWorker
    {
        private Random randomizer = new Random();
        private int max;
        public uint min_prime;
        public uint max_prime;

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

        public void get_two_primes()
        {
            get_min_prime();
            get_max_prime();
            if (min_prime>max_prime)
            {
                var buf = min_prime;
                min_prime = max_prime;
                max_prime = buf;
            }
        }

        private void get_max_prime()
        {
            set_max(1999999999);
            max_prime = prime_gen(UlongRand());
        }

        private void get_min_prime()
        {
            set_max(7);
            min_prime = prime_gen(UlongRand());
        }

        public void set_max(int n)
        {
            max = n;
        }

        private uint prime_gen(uint number)                              //Генератор простого числа
        {
            var local_num = number;
            if (divide(local_num, 2)) local_num++;
            while (!is_prime(local_num)) local_num += 2;
            return local_num;
        }

        private bool is_prime(uint number)
        {
            return is_prime_core(number, 3, number / 2 + 1);
        }

        private bool is_prime_core(uint number, uint k, uint n)            //Проверка на простоту
        {
            for (uint i = k; i <= n; i += 2)
            {
                if (divide(number, i)) return false;
            }
            return true;
        }

        private bool divide(uint num, uint denom)                         //Проверка на делимость без остатка
        {
            if ((num % denom) == 0) return true;
            else return false;
        }

        private uint UlongRand()                                           //Сгенерировать число
        {
            return UintLessRand(max);                                     //Можно до 2147483647
        }

        public uint UintLessRand(int A)                                   //Получить число меньшее А
        {
            return (uint)randomizer.Next(3, A);
        }
    }
}
    
