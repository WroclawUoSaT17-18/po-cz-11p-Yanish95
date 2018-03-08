using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadanie_domowe_1
{

    public class student
    {
            int numer;
            double mech;
            double aisuk;
            double prog;
            double srednia;

        public void Dodaj()
        {
            Console.WriteLine("Podaj numer indeksu ");
            Console.ReadLine(numer);



            /*
            Console.WriteLine("Podaj numer indeksu ");
            var numer=Console.ReadLine();
            Console.WriteLine("Podaj ocenę z  mechaniki ");
            var mech=Console.ReadLine();
            Console.WriteLine("Podaj ocenę z Analizy i Syntezy Układów Kinematycznych ");
            var asiuk=Console.ReadLine();
            Console.WriteLine("Podaj ocenę z programowania obiektowego ");
            var prog=Console.ReadLine();
            Console.WriteLine($"Student o numerze: {numer}, Posiada oceny:\n Mechanika: {mech}\n Analiza i synteza ukł. kinematycznych: {aisuk}\n Programowanie obiektowe: {prog}\n\n\n");
            */

        }

    }
    class Program
    {
        
        static void Main(string[] args)
        {









           /*
            student s1 = new student();
            s1.Dodaj();

            student s2 = new student();
            s2.Dodaj();
            */

            Console.ReadKey(true);


        }
    }
}

