using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2_alg
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Оберіть метод розв'язання:");
            Console.WriteLine("1 - Пошук з емуляцією відпалу");
            Console.WriteLine("2 - Пошук із поверненнями ");
            int methodChoice;
            while (!int.TryParse(Console.ReadLine(), out methodChoice) || (methodChoice != 1 && methodChoice != 2))
            {
                Console.WriteLine("Невірний вибір. Спробуйте ще раз:");
            }

            var sa = new SimulatedAnnealing(RegionsAdjacency.regions);
            Random random = new Random();
            string selectedRegion = RegionsAdjacency.regions.Keys.ElementAt(random.Next(RegionsAdjacency.regions.Count));
            

            if (methodChoice == 1) 
            {
                Console.WriteLine($"Випадково обрана початкова область: {selectedRegion}");
                double initialTemp = 1000; 
                double k = 0.1;           

                Dictionary<string, string> bestSolution = null;
                int bestConflicts = int.MaxValue;
                

                  

                var solution = sa.Solve(selectedRegion, initialTemp, k);
                int conflicts = sa.Value(solution);

                    if (conflicts < bestConflicts)
                    {
                        bestConflicts = conflicts;
                        bestSolution = solution;
                    }

                Console.WriteLine("\nФінальне розфарбування:");
                foreach (var kvp in bestSolution)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
                Console.WriteLine($"Фінальна кількість конфліктів: {bestConflicts}");
            }
            else if (methodChoice == 2) // Бектрекінг
            {
                var colors = new string[] { "Red", "Green", "Blue", "Yellow" };
                var backtracking = new BacktrackingMRV(RegionsAdjacency.regions, colors);

                if (backtracking.Solve())
                {
                    Console.WriteLine("Знайдено рішення:");
                    backtracking.PrintSolution();
                    Console.WriteLine($"Кількість конфліктів: {backtracking.CountConflicts()}");
                    //Console.WriteLine($"Кількість ітерацій: {backtracking.iterations}");
                    //Console.WriteLine($"Кількість усього перекрашувань: {backtracking.all_states}");
                }
                else
                {
                    Console.WriteLine("Рішення не знайдено");
                }
            }
        }
    }
}
