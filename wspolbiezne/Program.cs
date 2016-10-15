using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wspolbiezne
{
    class Program
    {
        public static void jedno(List<int> X, List<int> Y, List<int> Z)
        {
            foreach (int i in X)
            {
                if (!Y.Contains(i))
                {
                    Z.Add(i);
                }
            }

            foreach (int i in Y)
            {
                if (!X.Contains(i))
                {
                    Z.Add(i);
                }
            }
        }

        static void sprawdz(List<int> X, List<int> Y, List<int> Z)
        {
            foreach (int i in X)
            {
                if (!Y.Contains(i))
                {
                    Z.Add(i);
                }
            }
        }
        public static void dwu(List<int> X, List<int> Y, List<int> Z)
        {
            //Thread trd = new Thread(new ThreadStart(sprawdz(X, Y, Z)));
            Thread trd = new Thread(() => sprawdz(X, Y, Z));
            Thread trd2 = new Thread(() => sprawdz(Y, X, Z));
            //trd.IsBackground = true;
            trd.Start();
            trd2.Start();
        }

        private static void sprawdz()
        {
            throw new NotImplementedException();
        }

        static void Main(string[] args)
        {
            //tu bo Program jest static
            List<int> X = new List<int>();
            List<int> Y = new List<int>();
            List<int> Z = new List<int>();
            string filename;
            Console.WriteLine("Podaj nazwe pliku:");
            filename = Console.ReadLine();
            string[] lines = System.IO.File.ReadAllLines(filename+".txt");

            bool temp=false;
            foreach (string line in lines)
            {
                if(line == "#A") {}
                else if(line == "#B") {temp = true;}
                else
                {
                    if (temp == false) { X.Add(Int32.Parse(line)); }
                    else { Y.Add(Int32.Parse(line)); }
                }
            }

            Console.WriteLine("\nZbior X:");
            foreach (int i in X)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine("\nZbior Y:");
            foreach (int i in Y)
            {
                Console.Write(i + " ");
            }


            DateTime startTime;
            DateTime stopTime;
            TimeSpan roznica;


            startTime = DateTime.Now;
            jedno(X, Y, Z);
            Console.WriteLine("\nZbior Z:");
            foreach (int i in Z)
            {
                Console.Write(i + " ");
            }
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            Console.WriteLine("\nCzas pracy:\n" + roznica.TotalMilliseconds);


            startTime = DateTime.Now;
            dwu(X, Y, Z);
            Console.WriteLine("\nZbior Z:");
            foreach (int i in Z)
            {
                Console.Write(i + " ");
            }
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            Console.WriteLine("\nCzas pracy:\n" + roznica.TotalMilliseconds);



            Console.ReadKey();
        }
    }
}
