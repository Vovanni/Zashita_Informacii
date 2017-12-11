using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace WindowsFormsPr6
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class Sec
    {
        public void s(string l, string p)
        {
            Stream dbstream = new FileStream("C://pass.txt", FileMode.Append, FileAccess.Write);
            StreamWriter dbwriter = new StreamWriter(dbstream);
            dbwriter.WriteLine(l + " " + p);
            dbwriter.Close();
        }

        public bool p(string l, string p)
        {
            return parser(l, p, read_database());
        }

        private string read_database()
        {
            Stream dbstream = new FileStream("C://pass.txt", FileMode.Open, FileAccess.Read);
            StreamReader dbreader = new StreamReader(dbstream);
            string database = dbreader.ReadToEnd();
            dbreader.Close();
            return database;
        }

        private bool parser(string Login, string pass, string database)
        {
            Regex regex = new Regex(Login);
            Regex regexp = new Regex(pass);
            var a = regex.Split(database);
            var b = a[1].Split('\n');
            foreach (string c in b)
            {
                if (regexp.IsMatch(c)) return true;
            }
            return false;
        }
    }
}
