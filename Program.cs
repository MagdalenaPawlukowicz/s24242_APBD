using System;

namespace InitialProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj swoje imię:");
            
            // Pobranie imienia od użytkownika
            string name = Console.ReadLine();

            // Wyświetlenie powitania
            Console.WriteLine("Podaj swoje nazwisko:");
            
            // Pobranie imienia od użytkownika
            string surname = Console.ReadLine();

            // Wyświetlenie powitania
            Console.WriteLine("Witaj");
            Console.WriteLine($"{name} {surname}!");

            // Prosta operacja matematyczna
            int a = 5;
            int b = 3;
            int sum = a + b;
            Console.WriteLine($"Suma {a} i {b} wynosi: {sum}");

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
            Console.WriteLine("Zakończono działanie programu.");
        }
    }
}