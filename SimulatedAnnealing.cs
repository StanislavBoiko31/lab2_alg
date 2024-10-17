using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_alg
{
    public class SimulatedAnnealing
    {
        public readonly Dictionary<string, List<string>> regions;
        public readonly string[] colors = { "Red", "Green", "Blue", "Yellow" };
        public readonly Random random = new Random();
        public Dictionary<string, string>  state = new Dictionary<string, string>();
        public SimulatedAnnealing(Dictionary<string, List<string>> regions)
        {
            this.regions = regions;
        }

        
        public void PrintRegions()
        {
            var regionsList = regions.Keys.ToList();
            for (int i = 0; i < regionsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {regionsList[i]}");
            }
        }

        public int Value(Dictionary<string, string> state)
        {
            int conflicts = 0;
            foreach (var region in regions)
            {
                string currentColor = state[region.Key];
                foreach (var neighbor in region.Value)
                {
                    if (state[neighbor] == currentColor)
                        conflicts++;
                }
            }
            return conflicts / 2; 
        }

        
        public void RandomColouring()
        {
            

            /*
            state[startRegion] = colors[random.Next(colors.Length)];

            
            foreach (var region in regions.Keys)
            {
                if (region != startRegion)
                {
                    state[region] = colors[random.Next(colors.Length)];
                }
            } */
            foreach (var region in regions.Keys)
            {
                state[region] = colors[random.Next(colors.Length)];
            }
            
        }

        public string ChooseWorst()
        {
            int maxConflicts = 0;
            int currentConflicts = 0;
            string worstRegion = null;
            foreach (var region in regions.Keys)
            {
                currentConflicts = 0;
                
                foreach (var neighbor in regions[region])
                {
                    if (state[neighbor] == state[region])
                    {
                        currentConflicts++;
                    }
                }
                if (currentConflicts > maxConflicts)
                {
                    maxConflicts = currentConflicts;
                    worstRegion = region;
                }
            }
            return worstRegion;
        }
        
        private Dictionary<string, string> GetRandomNextState()
        {
            var next = new Dictionary<string, string>(state);
            string regionToChange = ChooseWorst();
            if (regionToChange == null) 
            {
                return null;
            }
            string currentColor = next[regionToChange];

            
            string newColor;
            do
            {
                newColor = colors[random.Next(colors.Length)];
            } while (newColor == currentColor);

            next[regionToChange] = newColor;
            return next;
        }

        public Dictionary<string, string> Solve(string startRegion, double initialTemp, double k)
        {

            RandomColouring();
            
            //var current = ChooseWorst(state);
            double currentConflicts = Value(state);
            
            int t = 1;
            int CountColour = 0;
            while (true)
            {
                
                double T = initialTemp - k * t;
                
                if (T <= 0) break;

                var next = GetRandomNextState();
                if (next == null)
                {
                    break;
                }
                double nextConflicts = Value(next);

                double delta = nextConflicts - currentConflicts;

                
                double randomValue = random.Next(0, 10001) / 10000.0;
                double our_value = Math.Exp(-delta / T);

                if (delta < 0 || randomValue < our_value)
                {
                    state = next;
                    currentConflicts = nextConflicts;
                    CountColour++;
                }
                
                t++;
            }
            Console.WriteLine($"Кількість ітерацій: {t}");
            //Console.WriteLine($"Кількість вузлів: {CountColour}");
            return state; 
        }
    }
}
