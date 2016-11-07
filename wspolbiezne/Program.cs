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
            public bool sem = false;
            public HashSet<int> set = new HashSet<int>();
            public void Add(int x)
            {
                while (sem == true) { }
                sem = true;
                set.Add(x);
                sem = false;
            }
        }
        public static void single(HashSet<int> X, HashSet<int> Y, SemaphoredSet Z)
        {
            foreach (int i in X)
            {
                if (!Y.Contains(i))
                {
                    Z.Add(i);
                }
            }

            //Thread.Sleep(10);

            foreach (int i in Y)
            {
                if (!X.Contains(i))
                {
                    Z.Add(i);
                }
            }

            //Thread.Sleep(10);
        }

        public static class dual
        {
            static public HashSet<int> X;
            static public HashSet<int> Y;
            static public SemaphoredSet Z;

            static public Task[] tasks;

            static public HashSet<int> Z1, Z2;

            static public void start(HashSet<int> A, HashSet<int> B, SemaphoredSet C)
            {
                X = A;
                Y = B;
                Z = C;
                tasks = new Task[2];

                Z1 = new HashSet<int>();
                Z2 = new HashSet<int>();
            }

            static void check(HashSet<int> A, HashSet<int> B, SemaphoredSet Z)
            {
                foreach (int i in A)
                {
                    if (!B.Contains(i))
                    {
                        Z.Add(i);
                    }
                }
                //Thread.Sleep(10);
            }

            static void checkNoSemaphore(HashSet<int> A, HashSet<int> B, HashSet<int> Z)
            {
                foreach (int i in A)
                {
                    if (!B.Contains(i))
                    {
                        Z.Add(i);
                    }
                }
                //Thread.Sleep(10);
            }

            static public void run()
            {
                tasks[0] = Task.Factory.StartNew(() => check(X, Y, Z));
                tasks[1] = Task.Factory.StartNew(() => check(Y, X, Z));

                Task.WaitAll(tasks);
            }

            static public void runNoSemaphore()
            {
                tasks[0] = Task.Factory.StartNew(() => checkNoSemaphore(X, Y, Z1));
                tasks[1] = Task.Factory.StartNew(() => checkNoSemaphore(Y, X, Z2));

                Task.WaitAll(tasks);
            }

            static public void joinNoSemaphore()
            {
                foreach (int i in Z1)
                {
                    Z.Add(i);
                }
                foreach(int i in Z2)
                {
                    Z.Add(i);
                }
            }
        }

        public static class multi
        {
            static public HashSet<int> X;
            static public HashSet<int> Y;
            static public SemaphoredSet Z;

            static public Task[] tasks;
            static public bool[] results;

            static public void start(HashSet<int> A, HashSet<int> B, SemaphoredSet C)
            {
                X = A;
                Y = B;
                Z = C;

                tasks = new Task[X.Count + Y.Count];
                results = new bool[10000];
            }

            static void check(int i, HashSet<int> L)
            {
                if (!L.Contains(i))
                {
                    Z.Add(i);
                }
                //Thread.Sleep(10);
            }

            static void checkNoSemaphore(int i, HashSet<int> L)
            {
                if (!L.Contains(i))
                {
                    results[i] = true;
                }
                //Thread.Sleep(10);
            }
            static public void run()
            {
                int i = 0;
                foreach (int x in X)
                {
                    tasks[i] = Task.Factory.StartNew(() => check(x, Y));
                    i++;
                }
                foreach (int y in Y)
                {
                    tasks[i] = Task.Factory.StartNew(() => check(y, X));
                    i++;
                }

                Task.WaitAll(tasks);
            }

            static public void runNoSemaphore()
            {
                int i = 0;
                foreach (int x in X)
                {
                    tasks[i] = Task.Factory.StartNew(() => checkNoSemaphore(x, Y));
                    i++;
                }
                foreach (int y in Y)
                {
                    tasks[i] = Task.Factory.StartNew(() => checkNoSemaphore(y, X));
                    i++;
                }

                Task.WaitAll(tasks);
            }

            static public void joinNoSemaphore()
            {
                for(int i = 0; i < 10000; i++)
                {
                    if (results[i] == true)
                    {
                        Z.Add(i);
                    }
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
                int a = rnd.Next(50000, 100000);
                int b = rnd.Next(50000, 100000);
                for(int i=0; i<a; i++)
                {
                    X.Add(rnd.Next(0, 10000));
                }
                for (int i = 0; i < b; i++)
                {
                    Y.Add(rnd.Next(0, 10000));
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

            dual.start(X, Y, Z);
            multi.start(X, Y, Z);

            Console.ReadKey();

            startTime = DateTime.Now;
            single(X, Y, Z);
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nJEDEN WATEK - Zbior Z:");
                foreach (int i in Z.set)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nJEDEN WATEK - Czas pracy:\n" + roznica.TotalMilliseconds);


            Z.set.Clear();

            Console.ReadKey();

            startTime = DateTime.Now;
            dual.run();
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nDWA WATKI - Zbior Z:");
                while (Z.sem == true) { }
                foreach (int i in Z.set)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nDWA WATKI - Czas pracy:\n" + roznica.TotalMilliseconds);

            
            Z.set.Clear();

            Console.ReadKey();

            startTime = DateTime.Now;
            dual.runNoSemaphore();
            stopTime = DateTime.Now;
            dual.joinNoSemaphore();
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nDWA WATKI BEZ SEMAFORÓW - Zbior Z:");
                while (Z.sem == true) { }
                foreach (int i in Z.set)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nDWA WATKI BEZ SEMAFORÓW - Czas pracy:\n" + roznica.TotalMilliseconds);


            Z.set.Clear();

            Console.ReadKey();

            startTime = DateTime.Now;           
            multi.run();
            stopTime = DateTime.Now;
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nWIELE WATKOW - Zbior Z:");
                foreach (int i in Z.set)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nWIELE WATKOW - Czas pracy:\n" + roznica.TotalMilliseconds);


            Z.set.Clear();

            Console.ReadKey();

            startTime = DateTime.Now;
            multi.run();
            stopTime = DateTime.Now;
            multi.joinNoSemaphore();
            roznica = stopTime - startTime;
            if (wypisuj)
            {
                Console.WriteLine("\nWIELE WATKOW BEZ SEMAFORÓW - Zbior Z:");
                foreach (int i in Z.set)
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine("\nWIELE WATKOW BEZ SEMAFORÓW - Czas pracy:\n" + roznica.TotalMilliseconds);

            Console.ReadKey();
        }
    }
}
