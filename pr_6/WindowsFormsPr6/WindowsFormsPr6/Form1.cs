using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsPr6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Prime prime = new Prime();
        Server server = new Server();
        Sec sec = new Sec();
        Adler32 adler = new Adler32();

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            if ((Login.Text != "") && (Password.Text != "") && (!server.user_exist(Login.Text)))
            {
                richTextBox1.Text += "Данный пользователь еще не разегестрирован.\n";

                Login.ReadOnly = true;
                Password.ReadOnly = true;

                string S = prime.random_string();
                string sp = S + Password.Text;
                uint x = adler.adler32(sp);//(uint)sp.GetHashCode();
                uint N = prime.get_safe_prime();
                uint g = prime.get_safe_prime(7);
                ulong v = prime.powm(g, x, N);      //a^b mod n
                server.registration(Login.Text, S, v, N, g, prime.k);
                sec.s(Login.Text, Password.Text);
                richTextBox1.Text += 
                    "Клиент вычисляет salt " + S + "\n" +
                    "Вычисляет хеш от salt + password" + Password.Text + "\n" +
                    "Вычисляет N" + N + "\n" +
                    "Вычисляет g" + g + "\n" +
                    "Вычисляет k" + prime.k + "\n" +
                    "Вычисляет v = g^x mod N" + g + "\n" +
                    "Передает серверу логин, salt, v, N, g, k";
                Login.ReadOnly = false;
                Password.ReadOnly = false;
            }
            else richTextBox2.Text = "Пользователь с таким логином уже существует.\nИли логин/пароль пустые!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";

            if ((Login.Text != "") && (Password.Text != "") && (server.user_exist(Login.Text)))
            {
                richTextBox1.Text += "Данный пользователь есть в базе.\n";

                Login.ReadOnly = true;
                Password.ReadOnly = true;

                var ngk = server.server_autentification1(Login.Text);

                richTextBox1.Text +=
                "Клиент получает от сервера:\n" +
                "N =" + ngk.N + "; " +
                "g =" + ngk.g + "; " +
                "k =" + ngk.k + "\n";

                uint a = prime.get_safe_prime();
                ulong A = prime.powm(ngk.g, a, ngk.N);

                richTextBox1.Text +=
                "Клиент вычисляет и передает серверу:\n" +
                "a =" + a + "\n" +
                "A = g^a mod N =" + A + "\n";

                var sb = server.server_autentification2(Login.Text, A);

                if (sb.B != 0)
                {
                    ulong AB = A + sb.B;
                    var u = adler.adler32(Convert.ToString(AB));//(uint)AB.GetHashCode();
                    richTextBox1.Text +=
                    "Клиент получает от сервера:\n" +
                    "Salt =" + sb.S + "\n" +
                    "B =" + sb.B + "\n";
                    if (u != 0)
                    {
                        string sp = sb.S + Password.Text;
                        uint x = adler.adler32(sp);//(uint)sp.GetHashCode();
                        ulong S = prime.powm((sb.B - prime.k * (prime.powm(ngk.g, x, ngk.N))),(a + u*x), ngk.N);
                        uint K = adler.adler32(Convert.ToString(S));//(uint)S.GetHashCode();
                        var K_ = server.server_autentification3();
                        richTextBox1.Text +=
                        "Клиент и сервер вычисляют К:\n" +
                        "К клиента =" + K + "\n";
                        if (sec.p(Login.Text, Password.Text))
                        {
                            richTextBox1.Text += "K сервера =" + K + "\n";
                        }
                        else richTextBox1.Text += "K сервера =" + K_ + "\n";
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                Login.ReadOnly = false;
                Password.ReadOnly = false;
            }
            else richTextBox2.Text = "Пользователя с таким логином не существует.\nИли логин/пароль пустые!";
        }

    }
}
