using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2_alg
{
    public class BacktrackingMRV
    {
        private Dictionary<string, List<string>> regions;
        private string[] colors;
        private Dictionary<string, string> solution;
        public int iterations;
        public int all_states;
        private Random random;
        private bool isFirstIteration;

        public BacktrackingMRV(Dictionary<string, List<string>> regions, string[] colors)
        {
            this.regions = regions;
            this.colors = colors;
            this.solution = new Dictionary<string, string>();
            iterations = 0;
            all_states = 0;
            random = new Random();
            isFirstIteration = true;
        }

        public bool Solve()
        {
            return Backtrack();
        }

        private bool Backtrack()
        {
            //iterations++;
            if (solution.Count == regions.Count)
            {
                return true; 
            }
            string regionToColor;
            if (isFirstIteration)
            {
                regionToColor = SelectRandomRegion();
                Console.WriteLine($"Випадково обрана початкова область: {regionToColor}");
            }
            else
            {
                regionToColor = SelectRegionMRV();
            }
            isFirstIteration = false;
            foreach (var color in colors)
            {
                //all_states++;
                if (IsColorValid(regionToColor, color))
                {
                    solution[regionToColor] = color;
                    
                    if (Backtrack())
                    {
                        return true;
                    }
                    solution.Remove(regionToColor);
                }
            }
            return false;
        }

        private string SelectRandomRegion()
        {
            var uncoloredRegions = regions.Keys.Where(r => !solution.ContainsKey(r)).ToList();
            return uncoloredRegions[random.Next(uncoloredRegions.Count)];
        }

        private string SelectRegionMRV()
        {
            return regions.Keys
                .Where(r => !solution.ContainsKey(r))
                .OrderBy(r => colors.Count(c => IsColorValid(r, c)))
                .First();
        }

        private bool IsColorValid(string region, string color)
        {
            return !regions[region].Any(neighbor =>
                solution.ContainsKey(neighbor) && solution[neighbor] == color);
        }

        public void PrintSolution()
        {
            foreach (var kvp in solution)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        public int CountConflicts()
        {
            int conflicts = 0;
            foreach (var region in regions)
            {
                foreach (var neighbor in region.Value)
                {
                    if (solution[region.Key] == solution[neighbor])
                    {
                        conflicts++;
                    }
                }
            }
            return conflicts / 2; 
        }
    }
}