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
        public class SemaphoredSet
        {
            public Semaphore sem = new Semaphore(1, 1);
            public HashSet<int> Z = new HashSet<int>();
            public void Add(int x)
            {
                sem.WaitOne();
                Z.Add(x);
                sem.Release();
            }
        }
        public static void jedno(HashSet<int> X, HashSet<int> Y, SemaphoredSet Z)
        {
            foreach (int i in X)
            {
                if (!Y.Contains(i))
                {
                    Z.Z.Add(i);
                }
            }

            foreach (int i in Y)
            {
                if (!X.Contains(i))
                {
                    Z.Z.Add(i);
                }
            }
        }

        static void sprawdzDwu(HashSet<int> A, HashSet<int> B, SemaphoredSet Z)
        {
            foreach (int i in A)
            {
                if (!B.Contains(i))
                {
                    Z.Add(i);
                    Thread.Yield();
                }
            }
        }

        public static void dwu(HashSet<int> X, HashSet<int> Y, SemaphoredSet Z)
        {
            //Thread trd = new Thread(new ThreadStart(sprawdz(X, Y, Z)));
            Thread trd = new Thread(() => sprawdzDwu(X, Y, Z));
            Thread trd2 = new Thread(() => sprawdzDwu(Y, X, Z));
            //trd.IsBackground = true;
            //trd2.IsBackground = true;
            trd.Start();
            trd2.Start();

            trd.Join();
            trd2.Join();
        }

        public static class wielo
        {
            static public HashSet<int> X;
            static public HashSet<int> Y;
            static public SemaphoredSet Z;
            static public void start(HashSet<int> A, HashSet<int> B, SemaphoredSet C)
            {
                X = A;
                Y = B;
                Z = C;
            }

            static void sprawdzWielo(int i, HashSet<int> L)
            {
                if (!L.Contains(i))
                {
                    Z.Add(i);
                }
                Thread.Yield();
            }
            static public void doIt()
            {
                foreach (int i in X)
                {
                    Thread trd = new Thread(() => sprawdzWielo(i, Y));
                    //trd.IsBackground = true;
                    trd.Start();
                }
                foreach (int i in Y)
                {
                    Thread trd = new Thread(() => sprawdzWielo(i, X));
                    //trd.IsBackground = true;
                    trd.Start();
                }
            }
        }

        static void Main(string[] args)
        {
            //tu bo Program jest static
            HashSet<int> X = new HashSet<int>();
            HashSet<int> Y = new HashSet<int>();
            SemaphoredSet Z = new SemaphoredSet();
            Console.WriteLine("Wybor 1 - z pliku, 2 - random bez wypisywania zawartosci, else random");
            bool wypisuj = true;
            string wybor = Console.ReadLine();
            if(wybor == "1")
            {
                string filename;
                Console.WriteLine("Podaj nazwe pliku:");
                filename = Console.ReadLine();
                string[] lines = System.IO.File.ReadAllLines(filename + ".txt");

                bool temp = false;
                foreach (string line in lines)
                {
                    if (line == "#A") { }
                    else if (line == "#B") { temp = true; }
                    else
                    {
                        if (temp == false) { X.Add(Int32.Parse(line)); }
                        else { Y.Add(Int32.Parse(line)); }
                    }
                }
            }
            else
            {
                if (wybor == "2")
                    wypisuj = false;
                Random rnd = new Random();
                int a = rnd.Next(500, 1000);
                int b = rnd.Next(500, 1000);
                for(int i=0; i<a; i++)
                {
                    X.Add(rnd.Next(0, 1000));
                }
                for (int i = 0; i < b; i++)
                {
                    Y.Add(rnd.Next(0, 1000));
                }
            }

            if (wypisuj)
            {
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
            }

            DateTime startTime;
            DateTime stopTime;
            TimeSpan roznica;

            Console.WriteLine("\n============\n");

            startTime = DateTime.Now;
            jedno(X, Y, Z);
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nJEDEN WATEK - Zbior Z:");
                foreach (int i in Z.Z)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nJEDEN WATEK - Czas pracy:\n" + roznica.TotalMilliseconds);


            Z.Z.Clear();

            startTime = DateTime.Now;
            dwu(X, Y, Z);
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nDWA WATKI - Zbior Z:");
                foreach (int i in Z.Z)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nDWA WATKI - Czas pracy:\n" + roznica.TotalMilliseconds);
            

            Z.Z.Clear();

            wielo.start(X, Y, Z);
            startTime = DateTime.Now;           
            wielo.doIt();
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nWIELE WATKOW - Zbior Z:");
                foreach (int i in Z.Z)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nWIELE WATKOW - Czas pracy:\n" + roznica.TotalMilliseconds);

            Console.ReadKey();
        }
    }
}
