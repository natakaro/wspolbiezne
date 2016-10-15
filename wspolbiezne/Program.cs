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
        public class semeforZ
        {
            public bool sem = false;
            public List<int> Z = new List<int>();
            public void Add(int x)
            {
                while (sem == true) { }
                sem = true;
                Z.Add(x);
                sem = false;
            }
        }
        public static void jedno(List<int> X, List<int> Y, semeforZ Z)
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

        static void sprawdzDwu(List<int> X, List<int> Y, semeforZ Z)
        {
            foreach (int i in X)
            {
                if (!Y.Contains(i))
                {
                    Z.Add(i);
                }
            }
        }

        public static void dwu(List<int> X, List<int> Y, semeforZ Z)
        {
            //Thread trd = new Thread(new ThreadStart(sprawdz(X, Y, Z)));
            Thread trd = new Thread(() => sprawdzDwu(X, Y, Z));
            Thread trd2 = new Thread(() => sprawdzDwu(Y, X, Z));
            //trd.IsBackground = true;
            trd.Start();
            trd2.Start();
        }


        static void sprawdzWielo(int i, List<int> Y, semeforZ Z)
        {
            if (!Y.Contains(i))
            {
                Z.Add(i);
            }
        }
        public static void wielo(List<int> X, List<int> Y, semeforZ Z)
        {
            foreach (int i in X)
            {
                Thread trd = new Thread(() => sprawdzWielo(i, Y, Z));
                trd.Start();
            }
            foreach (int i in Y)
            {
                Thread trd = new Thread(() => sprawdzWielo(i, X, Z));
                trd.Start();
            }

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
            semeforZ Z = new semeforZ();
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
            foreach (int i in Z.Z)
            {
                Console.Write(i + " ");
            }
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            Console.WriteLine("\nCzas pracy:\n" + roznica.TotalMilliseconds);


            startTime = DateTime.Now;
            dwu(X, Y, Z);
            Console.WriteLine("\nZbior Z:");
            while (Z.sem == true);
            foreach (int i in Z.Z)
            {
                Console.Write(i + " ");
            }
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            Console.WriteLine("\nCzas pracy:\n" + roznica.TotalMilliseconds);

            startTime = DateTime.Now;
            wielo(X, Y, Z);
            Console.WriteLine("\nZbior Z:");
            while (Z.sem == true) ;
            foreach (int i in Z.Z)
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
