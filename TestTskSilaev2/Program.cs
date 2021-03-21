using System;
using System.Collections.Generic;
using System.IO;

namespace TestTskSilaev2
{

    class Program
    {
        public class Cash
        {
            public uint nominal;
            public uint nominalsCount;
            public Cash(uint Nominal, uint NominalsCount)
            {
                this.nominal = Nominal;
                this.nominalsCount = NominalsCount;
            }
        }
        static List<Cash> Money = new List<Cash>();

        static bool ReadData(string path)
        {
            bool result = true;
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int indexOfChar = line.IndexOf(":");
                        string nominalStr = line.Substring(0, indexOfChar);
                        string nominalCountStr = line.Substring(indexOfChar + 1);

                        Cash Bancnotes = new Cash(Convert.ToUInt32(nominalStr), Convert.ToUInt32(nominalCountStr));
                        Money.Add(Bancnotes);
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Файл не найден!");
                result = false;
            }
            return result;
        }

        static uint GetTotalSum()
        {
            uint result = 0;
            for (int i= 0; i < Money.Count; i++ )
            {
                result = result + Money[i].nominal * Money[i].nominalsCount;
            }
            return result;
        }

        static uint GetUserSum()
        {
            Console.WriteLine("Введите сумму: ");
            uint result = Convert.ToUInt32(Console.ReadLine());
            return result;
        }

        static uint withdrawal(uint sum)
        {
            uint result = 0;
            int var = Money.Count - 1;
            for (int i = var; i >= 0; i--)
            {
                if (Money[i].nominalsCount > 0)
                {
                    if (sum >= Money[i].nominal)
                    {
                        result = Money[i].nominal;
                        Money[i].nominalsCount -= 1;
                        break;
                    }
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            string path = @"c:\temp\ATM.txt";
            if (!Program.ReadData(path))
                {
                 Console.WriteLine("Не могу прочитать исходные данные!");
                 return;
                }
            uint userSum = GetUserSum();
            uint startSum = userSum;

            uint total = GetTotalSum();
            if (total < userSum)
            {
                Console.WriteLine("В банкомате недостаточно средств");
                return;
            }

            uint accumulation = 0;
            uint current_withdrawal = 0;

            while (true)
            {
                current_withdrawal = withdrawal(userSum);

                if (current_withdrawal == 0)
                {
                    Console.WriteLine("Операцию выполнить невозможно");
                    break;
                }
                else
                {
                    accumulation += current_withdrawal;
                    if (accumulation == startSum)
                    {
                        Console.WriteLine("Операция выполнена успешно!");
                    }
                    else
                    {
                        userSum -= current_withdrawal;
                    }
                }
            }
        }
    }
}
