using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using static System.Console;

namespace Rsa_ibizi_lb4
{
    class Program
    {

        static public char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                        'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                        'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                        'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                        '8', '9', '0' };

        //зашифровать
        static List<string> Encrypt(string text, string p_string, string q_string, out string d_out, out string n_out)
        {
            d_out = "err";
            n_out = "err";
            long p = Convert.ToInt64(p_string);
            long q = Convert.ToInt64(q_string);

            if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
            {
                string s = text;
                s = s.ToUpper();

                long n = p * q;
                long m = (p - 1) * (q - 1);
                long d = Calculate_d(m);
                long e_ = Calculate_e(d, m);

                List<string> result = RSA_Endoce(s, e_, n);
                //foreach (string item in result)
                //    res += $"{item}\n";

                d_out = d.ToString();
                n_out = n.ToString();
                return result;
            }
            else
                return null;
        }

        //расшифровать
        static string Decrypt(List<string> input_list, string d_input, string n_input)
        {
            string result = "";
            long d = Convert.ToInt64(d_input);
            long n = Convert.ToInt64(n_input);

            result = RSA_Dedoce(input_list, d, n);
            return result;
        }

        //проверка: простое ли число?
        static bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        //зашифровать
        static List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }


        //расшифровать
        static string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";
            
            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }

        //вычисление параметра d. d должно быть взаимно простым с m
        static long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        //вычисление параметра e

        static long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        static string List_to_string(List<string> input)
        {
            string res = "";
            foreach (string item in input)
                res += $"{item}\n";
            return res;
        }

        public static void run_rsa_test()
        {
            string p_Openkey = "101";
            string q_Openkey = "103";
            string d_Protectedkey;
            string n_Protectedkey;
            string origin_text = "Привет мир";
            string encoded_text;
            string decoded_text;

            List<string> temp = Encrypt(origin_text, p_Openkey, q_Openkey, out d_Protectedkey, out n_Protectedkey);
            encoded_text = List_to_string(temp);
            decoded_text = Decrypt(temp, d_Protectedkey, n_Protectedkey);

            WriteLine("Открытый ключ;");
            WriteLine($"p = {p_Openkey}; q = {q_Openkey}");

            WriteLine("Исходное сообщение:");
            WriteLine($"{origin_text}");

            WriteLine("Закодированное сообщение:");
            WriteLine($"{encoded_text}");

            WriteLine("Открытый ключ;");
            WriteLine($"d = {d_Protectedkey}; n = {n_Protectedkey}");

            WriteLine("Раскодированное сообщение:");
            WriteLine($"{decoded_text}");
        }

        
        static void Main(string[] args)
        {
            run_rsa_test();
            ReadKey();
        }
    }
}
