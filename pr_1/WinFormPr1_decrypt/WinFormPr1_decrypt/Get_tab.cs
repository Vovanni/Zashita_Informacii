using System;
using System.IO;
using System.Windows.Forms;

namespace WinFormPr1_decrypt
{
    class Get_tab
    {
        public double[] Tab(Stream Input)
        {
            double[] array = new double[33];
            try
            {
                if (Input != null)
                {

                    using (BinaryReader in_reader = new BinaryReader(Input))
                    {
                        for (int i = 0; i < 33; i++)
                        {

                            array[i] = in_reader.ReadDouble();
                                //Convert.ToDouble(in_reader.ReadLine());
                        }
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
