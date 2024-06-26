﻿using System;

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
        
        public static double CalculateAverage(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty");
            }

            int sum = 0;
            foreach (int x in numbers)
            {
                sum += x;
            }

            return (double)sum / numbers.Length;
        }
        
        public static int FindMax(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty");
            }

            int max = numbers[0];
            foreach (int num in numbers)
            {
                if (num > max)
                {
                    max = num;
                }
            }

            return max;
        }
    }
}