using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPr6
{
    public class Prime
    {
        private Random randomizer = new Random();

        public uint k = 3;

        public string random_string(int min = 10, int max = 20)
        {
            var lenght = randomizer.Next(min, max);
            string output = "";
            for (int i = 0; i <= lenght; i++)
            {
                output += random_char();
            }
            return output;
        }

        char random_char()
        {
            return "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789" [randomizer.Next(0, 61)];
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

        private int max;

        public uint get_safe_prime(int n = 1999999)
        {
            max = n;
            uint prime = 2 * prime_gen(UlongRand()) + 1;
            return prime;
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

        private uint UlongRand()                                          //Сгенерировать число
        {
            return UintLessRand(max);                                     //Можно до 2147483647
        }

        private uint UintLessRand(int A)                                  //Получить число меньшее А
        {
            return (uint)randomizer.Next(3, A);
        }
    }
}
