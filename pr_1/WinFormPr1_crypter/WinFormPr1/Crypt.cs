using System;

namespace WinFormPr1
{
    class Crypt
    {
        private String Output;
        private int alphabet_number;
        private bool letter_is_big;
        private bool not_a_letter = true;

        public String input_for_crypt(String Key, String Input)
        {
            int[] Letters = new int[33];
            int After_Key = 0;

            for (int i = 0; i < Key.Length; i++)    //Записываем ключ в начало алфавитной таблицы без повторов одинаковых символов ключа
            {
                return_number(Key[i], false);
                if (i == 0)
                    {
                        Letters[After_Key] = alphabet_number;
                        After_Key++;
                    }
                else for (int n = 0; n < i; n++)
                    {
                        if (Letters[n] == alphabet_number) break;
                        else if (n == i - 1)
                        {
                            Letters[After_Key] = alphabet_number;
                            After_Key++;
                        }
                    }
            }

            for (int i = After_Key, k = 0; i < Letters.Length;) //Дозаписываем шифрованную таблицу алфавита
            {
                for (int n = 0; n < After_Key; n++)      //Сравниваем текущую букву с ключом
                {
                    if (Letters[n] == k) break;
                    else if (n == After_Key-1)
                    {
                        Letters[i] = k;
                        i++;
                    }
                }
                k++;
            }

            for (int i=0; i < Input.Length; i++)
            {
                return_number(Input[i], true);
                if (not_a_letter) Output += Input[i];
                else Output += return_acsii(Letters[alphabet_number], letter_is_big);
            }
            return (Output);
        }

        private void return_number(int acsii_number, bool reg_is_need)  //Переводим ACSII код в код алфавита 0-32
        {
            if ((acsii_number <= 1103) & (acsii_number >= 1040))
            {
                if ((acsii_number <= 1071))
                {
                    alphabet_number = acsii_number - 1040;
                    if (reg_is_need) letter_is_big = true;
                }
                else
                {
                    alphabet_number = acsii_number - 1072;
                    if (reg_is_need) letter_is_big = false;
                }
                not_a_letter = false;
                if (alphabet_number > 5) alphabet_number++;       //В ACSII символы Ё и ё идут не по порядку алфавита.
            }
            else if ((acsii_number == 1025) || (acsii_number == 1105))
            {
                alphabet_number = 6;
                not_a_letter = false;
            }
            else not_a_letter = true;
        }

        private Char return_acsii(int alphabet_number, bool is_big) //Переводим код алфавита 0-32 в ACSII
        {
            int acsii_number;
            if (is_big) if (alphabet_number == 6) acsii_number = 1025;
                        else if (alphabet_number < 6) acsii_number = alphabet_number + 1040;
                             else acsii_number = alphabet_number + 1039;
            else if (alphabet_number == 6) acsii_number = 1105;
                 else if (alphabet_number < 6) acsii_number = alphabet_number + 1072;
                      else acsii_number = alphabet_number + 1071;
            return ((Char)acsii_number);
        }
    }
}
