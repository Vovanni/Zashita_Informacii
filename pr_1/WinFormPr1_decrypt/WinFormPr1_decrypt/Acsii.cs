using System;

namespace WinFormPr1_decrypt
{
    class Acsii
    {
        public int return_number(int acsii_number)  //Переводим ACSII код в код алфавита 0-32, 33 - другой символ
        {
            int alphabet_number;
            if ((acsii_number <= 1103) & (acsii_number >= 1040))
            {
                if ((acsii_number <= 1071)) alphabet_number = acsii_number - 1040; //Большие
                else alphabet_number = acsii_number - 1072;                        //Малые
                if (alphabet_number > 5) return(alphabet_number+1);       //В ACSII символы Ё и ё идут не по порядку алфавита.
                return (alphabet_number);
            }
            else if ((acsii_number == 1025) || (acsii_number == 1105)) return 6; // Возвращаем ё
            else return 33;
        }

        public bool is_big (int acsii_number)
        {
            if ((acsii_number <= 1103) & (acsii_number >= 1040))
            {
                if ((acsii_number <= 1071)) return true; //Большие
                else return false;                       //Малые
            }
            else if (acsii_number == 1025) return true;
                 else if (acsii_number == 1105) return false;
            return false;
        }

        public Char return_acsii(int alphabet_number, bool is_big) //Переводим код алфавита 0-32 в ACSII
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
