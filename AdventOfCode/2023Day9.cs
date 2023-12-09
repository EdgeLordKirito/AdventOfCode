using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class _2023Day9 : Solution
{
    private List<int[]> lines = new List<int[]>();

    public _2023Day9(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        List<long> values = new List<long>();
        foreach (int[] line in lines)
        {
            SequenceCalculator calculator = new SequenceCalculator(line);
            values.Add(calculator.Last);
        }
        return CalculateTotal(values);
    }

    public override object DoPartTwo()
    {
        List<long> values = new List<long>();
        foreach (int[] line in lines)
        {
            SequenceCalculator calculator = new SequenceCalculator(line);
            values.Add(calculator.Beggining);
        }
        return CalculateTotal(values);
    }

    public override void Run()
    {
        Init();
        ParseInput();
        Console.WriteLine($"2023 Day 9");
        Console.WriteLine($"Part one: {DoPartOne()}");
        Console.WriteLine($"Part two: {DoPartTwo()}");
    }

    private void ParseInput()
    {
        List<int[]> ints = new List<int[]>();
        foreach (string line in Input)
        {
            List<int> values = new List<int>();
            string[] split = line.Split(' ');
            foreach (string s in split)
            {
                if (int.TryParse(s, out int i))
                {
                    values.Add(i);
                }
            }
            ints.Add(values.ToArray());
        }
        lines = ints;
    }


    private long CalculateTotal(List<long> values)
    {
        long total = 0;
        foreach (long value in values)
        {
            total += value;
        }
        return total;
    }

    private class SequenceCalculator
    {
        private readonly int[] _values;

        public SequenceCalculator(int[] values)
        {
            _values = values;
        }

        public long Last => CalculateLast();
        public long Beggining => CalculateBeginning();

        private long CalculateLast()
        {
            List<List<int>> layers = CalculateLayers(_values);
            List<long> nexts = new List<long>();
            nexts.Add(0);
            IEnumerable<List<int>> reverseLayers = layers.Reverse<List<int>>();
            foreach(List<int> layer in reverseLayers)
            {
                int layerLast = layer.Last();
                long nextLast = nexts.Last();
                long next = layerLast + nextLast;
                nexts.Add(next);
            }
            return nexts.Last();
        }

        private long CalculateBeginning()
        {
            List<List<int>> layers = CalculateLayers(_values);
            List<long> nexts = new List<long>();
            nexts.Add(0);
            IEnumerable<List<int>> reverseLayers = layers.Reverse<List<int>>();
            foreach (List<int> layer in reverseLayers)
            {
                int layerFirst = layer.First();
                long nextLast = nexts.Last();
                long next = layerFirst - nextLast;
                nexts.Add(next);
            }
            return nexts.Last();
        }

        private List<List<int>> CalculateLayers(int[] startLayer)
        {
            List<List<int>> layers = new List<List<int>>();
            List<int> current = startLayer.ToList();
            while (!IsLayerEnd(current))
            {
                layers.Add(current);
                current = CalculateLayer(current);
            }

            layers.Add(current);
            if (layers.Last().Count == 1)
            {
                List<int> last = new List<int>();
                last.Add(0);
                layers.Add(last);
            }
            return layers;
        }

        private List<int> CalculateLayer(List<int> values)
        {
            List<int> layer = new List<int>();
            for (int i = 0; i < values.Count - 1; i++)
            {
                int abs = Difference(values[i + 1], values[i]);
                layer.Add(abs);
            }
            return layer;
        }

        private bool IsLayerEnd(List<int> layer)
        {
            bool zeros = layer.All(i => i == 0);
            bool count = layer.Count == 1;
            return zeros || count;
        }

        private int Difference (int a, int b)
        {
            return a - b;
        }
    }
}
