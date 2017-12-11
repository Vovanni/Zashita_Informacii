using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;

namespace WindowsFormsPr5
{
    public class Stack
    {
        public int Sentence;
        public int Word;
        public string Text;
    }

    class Threads
    {
        //Заменяются только следующие символы.
        Regex regex_filter1 = new Regex(@"[аА]");
        Regex regex_filter2 = new Regex(@"[,]");
        Regex regex_filter3 = new Regex(@"[еЕёЁ]");
        Regex regex_filter4 = new Regex(@"[тТ]");
        List<Thread> threadList = new List<Thread>();
        string[][] Words;
        List<Stack> Output = new List<Stack>();

        public List<Stack> StartThreads(string[][] Set)
        {
            Words = Set;
            for (int i = 0; i < Words.Length; i++)
            {
                for (int j = 0; j < Words[i].Length; j++)
                {
                    Stack parmeters = new Stack();
                    parmeters.Sentence = i;
                    parmeters.Word = j;
                    parmeters.Text = Words[i][j];
                    Thread thread = new Thread(new ParameterizedThreadStart(filter));
                    thread.Start(parmeters);
                    threadList.Add(thread);
                }
            }
            while(true)
            {
                bool threadsIsAlive = false;
                foreach (Thread thread in threadList)
                {
                    threadsIsAlive |= thread.IsAlive;
                    if (threadsIsAlive) break;
                }
                if (!threadsIsAlive) break;
            }
            return Output;
        }

        //Функция замены для потоков
        void filter(object parameters)
        {
            Stack param = (Stack)parameters;
            param.Text = regex_filter1.Replace(param.Text, "@");
            param.Text = regex_filter2.Replace(param.Text, ";");
            param.Text = regex_filter3.Replace(param.Text, "3");
            param.Text = regex_filter4.Replace(param.Text, "7");
            Output.Add(param);
        }
    }
}
