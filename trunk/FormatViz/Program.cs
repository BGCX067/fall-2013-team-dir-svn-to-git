using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatViz
{
    public class Program
    {
        private static string computeNumber(string str)
        {
            if (str.Length == 0)
                return "0.0";
            double num = 0;
            bool decimalPlace = false;
            double numDec = 1;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    if (decimalPlace)
                    {
                        num += (0.1*numDec) * double.Parse(str[i].ToString());
                        numDec *= 0.1;
                    }
                    else
                    {
                        num *= 10;
                        num += double.Parse(str[i].ToString());
                    }
                }
                else if (str[i] == '.')
                {
                    decimalPlace = true;
                }
                else if (str[i] == 'K')
                {
                    num *= 1000;
                }
                else if (str[i] == 'M')
                {
                    num *= 1000000;
                }
                else if (str[i] == 'B')
                {
                    num *= 1000000000;
                }
            }
            return num.ToString();
        }

        static void Main(string[] args)
        {
            try
            {
                for (int year = 1996; year < 2013; year++)
                {
                    String filename = "Stormdata_" + year.ToString() + ".csv";
                    String file = filename.Remove(filename.IndexOf(".csv"));
                    using (StreamWriter outfile = new StreamWriter(file + "Edit.csv"))
                    {
                        try
                        {
                            using (StreamReader sr = new StreamReader(filename))
                            {
                                while (!sr.EndOfStream)
                                {
                                    string line = sr.ReadLine();
                                    string newLine = "";
                                    for (int i = 0; i < 14; i++)
                                    {
                                        newLine += line.Substring(0, line.IndexOf(',') + 1);
                                        line = line.Remove(0, line.IndexOf(',') + 1);
                                    }
                                    newLine += computeNumber(line.Substring(0, line.IndexOf(','))) + ',';
                                    line = line.Remove(0, line.IndexOf(',') + 1);
                                    newLine += computeNumber(line.Substring(0, line.IndexOf(',')));
                                    line = line.Remove(0, line.IndexOf(','));
                                    newLine += line;
                                    outfile.WriteLine(newLine);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Unable to read file:");
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to write to file:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
