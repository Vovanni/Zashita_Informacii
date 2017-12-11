using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WindowsFormsPr6
{
    
    class Ngk
    {
        public uint N;
        public uint g;
        public uint k;
    }

    class Svngk
    {
        public string S;
        public ulong v;
        public uint N;
        public uint g;
        public uint k;
    }

    class SB
    {
        public string S;
        public ulong B;
    }

    class Server
    {
        Prime prime = new Prime();
        Adler32 adler = new Adler32();

        public void registration(string Login, string Solt, ulong verifier, uint N, uint g, uint k)
        {
            Stream dbstream = new FileStream("database.txt", FileMode.Append, FileAccess.Write);
            StreamWriter dbwriter = new StreamWriter(dbstream);
            dbwriter.WriteLine(Login + " " + Solt + " " + verifier + " " + N + " " + g + " " + k);
            dbwriter.Close();
        }

        public bool user_exist(string Login)
        {
            string database = read_database();
            Regex regex = new Regex(Login);
            return regex.IsMatch(database);
        }

        public Ngk server_autentification1(string Login)
        {
            string database = read_database();
            return svngk_to_ngk(parser(Login, database));
        }

        private uint u;
        private Svngk svngk_;
        private SB sb;
        private ulong A_;
        public SB server_autentification2(string Login, ulong A)
        {
            sb = new SB();
            if (A != 0)
            {
                svngk_ = parser(Login, read_database());
                var b = prime.get_safe_prime();
                var B = svngk_.k * svngk_.v + prime.powm(svngk_.g, b, svngk_.N);
                ulong AB = A + B;
                sb.S = svngk_.S;
                sb.B = B;
                u = adler.adler32(Convert.ToString(AB));//(uint)AB.GetHashCode();
                A_ = A;
            }
            return sb;
        }

        public uint server_autentification3()
        {
            ulong S = prime.powm((A_ * prime.powm(svngk_.v, u, svngk_.N)), sb.B, svngk_.N);
            uint K = adler.adler32(Convert.ToString(S));//(uint)S.GetHashCode();
            return K;
        }

        private string read_database()
        {
            Stream dbstream = new FileStream("database.txt", FileMode.Open, FileAccess.Read);
            StreamReader dbreader = new StreamReader(dbstream);
            string database = dbreader.ReadToEnd();
            dbreader.Close();
            return database;
        }

        private Svngk parser(string Login, string database)
        {
            Svngk svngk = new Svngk();
            Regex regex = new Regex(Login);
            var a = regex.Split(database);
            var b = a[1].Split(' ', '\n');

            svngk.S = b[1];
            svngk.v = Convert.ToUInt64(b[2]);
            svngk.N = Convert.ToUInt32(b[3]);
            svngk.g = Convert.ToUInt32(b[4]);
            svngk.k = Convert.ToUInt32(b[5]);
            Debug.WriteLine(svngk.S);
            Debug.WriteLine(svngk.v);
            Debug.WriteLine(svngk.N);
            Debug.WriteLine(svngk.g);
            Debug.WriteLine(svngk.k);
            return svngk;
        }

        private Ngk svngk_to_ngk(Svngk svngk)
        {
            Ngk ngk = new Ngk();
            ngk.N = svngk.N;
            ngk.g = svngk.g;
            ngk.k = svngk.k;
            return ngk;
        }
    }
}
