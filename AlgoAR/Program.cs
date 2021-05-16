using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AlgoAR
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            int[,] tab;
            
             // @@ Zmienne @@
            int pop = 40;    //populacja
            int k = 10;    //wielkość turnieju
            int ilosc = 1000000;    //ilość powtórzeń
            int procent = 77; // 1/procent to szansa na mutacje
            string nazwa = "berlin52.txt";  //nazwa pliku
            // @@ Zmienne @@



            //wczytanie pliku

            using (StreamReader sr = new StreamReader(nazwa))
            {
                n = Int32.Parse(sr.ReadLine());
                tab = new int[n, n];
                Console.WriteLine(n);
                for (int i = 0; i < n; i++)
                {
                    string[] temp = sr.ReadLine().Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j)
                        {
                            tab[i, j] = 0;
                            j = n;
                        }
                        else
                        {
                            tab[i, j] = Int32.Parse(temp[j]);
                            tab[j, i] = tab[i, j];
                        }
                    }
                }
            }


            Random rand = new Random();

            //Populacja
            int[,] populacja = new int[pop, n];
            int[] wynik = new int[pop];

            // Wynik końcowy
            int[] naj = new int[n];
            int najWynik = 9999999;

            for (int i = 0; i < pop; i++)
            {
                List<int> temp = new List<int>();
                int tempL;
                for (int j = 0; j < n; j++)
                {
                    do
                    {
                        tempL = rand.Next(0, n);
                    } while (temp.Contains(tempL));
                    populacja[i, j] = tempL;
                    temp.Add(tempL);

                }
            }
            for (int i = 0; i < pop; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == 0)
                    {
                        wynik[i] = 0;
                    }
                    else
                    {
                        wynik[i] = wynik[i] + tab[populacja[i, j], populacja[i, j - 1]];
                    }
                }
                wynik[i] = wynik[i] + tab[populacja[i, n - 1], populacja[i, 0]];
                if (wynik[i] < najWynik)
                {
                    najWynik = wynik[i];
                    for (int a = 0; a < n; a++)
                    {
                        naj[a] = populacja[i, a];
                    }
                }
            }
            int[,] populacjaN;
            
            //początek pętli

            for (int il = 0; il < ilosc; il++)
                {

                populacjaN = Turniej(rand, populacja, pop, wynik, n, k);
                populacjaN = Krzyz(rand, populacjaN, pop, n, procent);
                populacjaN = Mutacja(rand, populacjaN, pop, n);

                for (int i = 0; i < pop; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        populacja[i, j] = populacjaN[i, j];
                    }
                }


                for (int i = 0; i < pop; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (j == 0)
                        {
                            wynik[i] = 0;
                        }
                        else
                        {
                            wynik[i] = wynik[i] + tab[populacja[i, j], populacja[i, j - 1]];
                        }
                    }
                    wynik[i] = wynik[i] + tab[populacja[i, n - 1], populacja[i, 0]];
                    if (wynik[i] < najWynik)
                    {
                        najWynik = wynik[i];
                        for (int a = 0; a < n; a++)
                        {
                            naj[a] = populacja[i, a];
                        }
                    }
                }

                }

            //koniec wykonania


            //wypisz wynik

            for (int a = 0; a < n-1; a++)
            {
                Console.Write(naj[a] + "-");
            }
            Console.Write(naj[n-1]);
            Console.Write(" " + najWynik);

            Console.WriteLine();

            //Podaj a, aby skończyć
            char key;
            do
            {
                key= Console.ReadKey().KeyChar;
            } while (key != 'a');
            
        }

        //selekcje

        static int[,] Ruletka(Random rand, int[,] populacja, int pop, int[] wynik, int n)
        {
            int[] kolo = new int[pop]; 
            int[,] populacjaKolo = new int[pop, n]; 
            int max = 0; 
            int suma = 0; 
            for (int i = 0; i < pop; i++)
            {
                if (max < wynik[i])
                {
                    max = wynik[i];
                }
            }
            for (int i = 0; i < pop; i++)
            {

                kolo[i] = (max - wynik[i] + 1); 
                suma += kolo[i];
                if (i > 0)
                {
                    kolo[i] = kolo[i] + kolo[i - 1]; 
                }
            }

            for (int i = 0; i < pop; i++)
            {
                int temp = rand.Next(0, suma); 
                for (int j = 0; j < pop; j++)
                {

                    if (temp <= kolo[j]) 
                    {
                        for (int a = 0; a < n; a++)
                        {
                            populacjaKolo[i, a] = populacja[j, a]; 
                        }
                        break;
                    }
                }
            }
            return populacjaKolo;
        }

        static int[,] Turniej(Random rand, int[,] populacja, int pop, int[] wynik, int n, int wielkosc)
        {
            int[,] turniejWynik = new int[wielkosc,wielkosc];
            int[,] populacjaTurniej = new int[pop, n];
            for (int i = 0; i < pop; i++)
            {
                for (int j = 0; j < wielkosc; j++)
                {
                    int num = rand.Next(0, pop);

                    turniejWynik[j,0] = wynik[num];
                    turniejWynik[j, 1] = num;

                }
                int min = 99999;
                int index = 0;
                for (int j = 0; j < wielkosc; j++)
                {
                    if (min > turniejWynik[j,0])
                    {
                        min = turniejWynik[j,0];
                        index = j;
                    }
                }

                for (int m = 0; m < n; m++)
                {
                    populacjaTurniej[i, m] = populacja[turniejWynik[index, 1], m];
                }
            }


            return populacjaTurniej;
        }

        //krzyżowanie

        static int[,] Krzyz(Random rand, int[,] populacjaN, int pop, int n, int procent)
        {
            int[,] populacjaKrzyz = new int[pop, n];
            for (int a = 0; a < pop; a += 2)
            {
                if (rand.Next(0, 101) <= procent)
                {
                    int z = 0;
                    int p1 = rand.Next(0, n - 1);
                    int p2 = rand.Next(p1 + 1, n);
                    int[] temp1 = new int[n];
                    int[] temp2 = new int[n];
                    for (int b = p2; b < n; b++)
                    {
                        temp1[z] = populacjaN[a, b];
                        temp2[z] = populacjaN[a + 1, b];
                        z++;
                    }
                    for (int b = 0; b < p1; b++)
                    {
                        temp1[z] = populacjaN[a, b];
                        temp2[z] = populacjaN[a + 1, b];
                        z++;
                    }
                    for (int b = p1; b < p2; b++)
                    {
                        temp1[z] = populacjaN[a, b];
                        temp2[z] = populacjaN[a + 1, b];

                        populacjaKrzyz[a, b] = populacjaN[a, b];
                        populacjaKrzyz[a + 1, b] = populacjaN[a + 1, b];

                        z++;

                    }
                    for (int c = 0; c < n; c++)
                    {
                        for (int b = p1; b < p2; b++)
                        {
                            if (temp2[c] == populacjaKrzyz[a, b])
                            {
                                temp2[c] = -7;
                            }
                            if (temp1[c] == populacjaKrzyz[a + 1, b])
                            {
                                temp1[c] = -7;
                            }
                        }
                    }
                    int t1 = 0;
                    int t2 = 0;
                    for (int b = 0; b < p1; b++)
                    {
                        while (temp2[t2] == -7)
                        {
                            t2++;
                        }
                        while (temp1[t1] == -7)
                        {
                            t1++;
                        }
                        populacjaKrzyz[a, b] = temp2[t2];
                        populacjaKrzyz[a + 1, b] = temp1[t1];
                        t1++;
                        t2++;

                    }
                    for (int b = p2; b < n; b++)
                    {
                        while (temp2[t2] == -7)
                        {
                            t2++;
                        }
                        while (temp1[t1] == -7)
                        {
                            t1++;
                        }
                        populacjaKrzyz[a, b] = temp2[t2];
                        populacjaKrzyz[a + 1, b] = temp1[t1];
                        t1++;
                        t2++;
                    }
                }
                else
                {
                    for (int b = 0; b < n; b++)
                    {
                        populacjaKrzyz[a, b] = populacjaN[a, b];
                        populacjaKrzyz[a + 1, b] = populacjaN[a + 1, b];
                    }
                }
            }

            return populacjaKrzyz;
        }

        //Mutacja

        static int[,] Mutacja(Random rand, int[,] populacjaN, int pop, int n)
        {
            int[,] populacjaMutacja = new int[pop, n];

            for (int a = 0; a < pop; a++)
            {
                if (rand.Next(0, 101) <= 5)
                {
                    int p1 = rand.Next(0, n);
                    int p2;
                    do
                    {
                        p2 = rand.Next(0, n);
                    } while (p2 == p1);
                    for (int b = 0; b < n; b++)
                    {
                        if (b == p1)
                        {
                            populacjaMutacja[a, b] = populacjaN[a, p2];
                        }
                        else if (b == p2)
                        {
                            populacjaMutacja[a, b] = populacjaN[a, p1];
                        }
                        else
                        {
                            populacjaMutacja[a, b] = populacjaN[a, b];
                        }

                    }
                }
                else
                {
                    for (int b = 0; b < n; b++)
                    {
                        populacjaMutacja[a, b] = populacjaN[a, b];

                    }
                }
            }
            return populacjaMutacja;
        }
        
       
        
    }
}