using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class _2023Day5 : Solution
{

    public long[] _seeds = Array.Empty<long>();
    public long[][] _seedToSoilMap = [];
    public long[][] _soilToFertilizerMap = [];
    public long[][] _fertilizerToWaterMap = [];
    public long[][] _waterToLightMap = [];
    public long[][] _lightToTemperaturMap = [];
    public long[][] _temperatureToHumidityMap = [];
    public long[][] _humidityToLocationMap = [];
    public _2023Day5(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        List<long> soils = RunMap(_seeds.ToList(), _seedToSoilMap);
        List<long> fertilizers = RunMap(soils, _soilToFertilizerMap);
        List<long> waters = RunMap(fertilizers, _fertilizerToWaterMap);
        List<long> lights = RunMap(waters, _waterToLightMap);
        List<long> temperatures = RunMap(lights, _lightToTemperaturMap);
        List<long> humidities = RunMap(temperatures, _temperatureToHumidityMap);
        List<long> locations = RunMap(humidities, _humidityToLocationMap);
        return locations.Min();
    }

    public override object DoPartTwo()
    {
        long minimum = long.MaxValue;
        List<Range> range = Seeds();
        long total = range.Sum(x => x.Count);
        long count = 0;
        foreach (Range r in range)
        {
            for (long i = r.Start; i < r.End; i++)
            {
                long curr = r.Current;
                List<long> soils = RunMap(new List<long> { curr }, _seedToSoilMap);
                List<long> fertilizers = RunMap(soils, _soilToFertilizerMap);
                List<long> waters = RunMap(fertilizers, _fertilizerToWaterMap);
                List<long> lights = RunMap(waters, _waterToLightMap);
                List<long> temperatures = RunMap(lights, _lightToTemperaturMap);
                List<long> humidities = RunMap(temperatures, _temperatureToHumidityMap);
                List<long> locations = RunMap(humidities, _humidityToLocationMap);
                long min = locations.Min();
                if (min < minimum)
                {
                    minimum = min;
                }
                count++;
                PrintMileStone(count, total, minimum);
                SafetyPrint(CalculatePercentage(count, total));

            }
        }
        return minimum;
    }

    private void SafetyPrint(double percentage)
    {
        DateTime now = DateTime.Now;
        if (now.Minute % 1 == 0 && now.Second == 0 && now.Millisecond % 999 == 0)
        {
            Console.WriteLine($"Current Percentage: {percentage}");
        }
    }

    private void PrintMileStone(long count, long total, long minimum)
    {
        double percent = CalculatePercentage(count, total);
        PrintIfFullPercent(percent, minimum);
    }

    //Write a method that takes in a current value and a total and return the percentage
    private double CalculatePercentage(long current, long total)
    {
        return current / (double)total;
    }

    private void PrintIfFullPercent(double d, long minimum)
    {
        if (d % 1 == 0)
        {
            // Set the console text colour to green.
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Current Percentage: {d}, Minimum: {minimum}");
            //Reset the console text colour.
            Console.ResetColor();
        }
    }

    public override void Init()
    {
        FileInfo fileInfo = new FileInfo(InputPath);
        if (!fileInfo.Exists)
        {
            return;
        }
        Input = File.ReadAllLines(InputPath);
    }

    public override void Run()
    {
        Init();
        ParseInput();
        Console.WriteLine($"2023 Day 5");
        Console.WriteLine($"Part one: {DoPartOne()}");
        object part2 = DoPartTwo();
        Console.WriteLine($"Part two: {part2}");
        File.WriteAllText(Path.Combine(Globals.Input2023, "Day5Solution.txt"), part2.ToString());
    }

    private List<long> FindValueInMap(long input, long[][] map)
    {
        List<long> result = new List<long>();
        foreach (long[] longs in map)
        {
            long upperBorder = longs[2] + longs[1];

            long value = int.MinValue;
            if (IsValueInRange(input, longs[1], upperBorder))
            {
                long offset = CalculateOffset(input, longs[1]);
                value = longs[0] + offset;
            }
            if (value == int.MinValue)
            {
                continue;
            }
            result.Add(value);
        }
        return result;
    }

    private List<long> RunMap(List<long> input, long[][] map)
    {
        List<long> result = new List<long>();
        foreach (long item in input)
        {
            List<long> temp = FilterOutNonFound(FindValueInMap(item, map));
            result.AddRange(temp);
        }
        return result;
    }

    private List<long> FilterOutNonFound(List<long> input)
    {
        List<long> result = new List<long>();
        foreach (long l in input)
        {
            if (l != int.MinValue)
            {
                result.Add(l);
            }
        }
        return result;
    }

    private Range SeedRange(long start, long length)
    {
        return new Range(start, length);
    }

    private List<Range> SeedRanges(long[] seed)
    {
        List<Range> result = new List<Range>();
        for (int i = 0; i < seed.Length; i = i + 2)
        {
            long start = seed[i];
            long length = seed[i + 1];
            result.Add(SeedRange(start, length));
        }
        return result;
    }

    private List<Range> Seeds()
    {
        List<Range> seedRanges = SeedRanges(_seeds);
        return seedRanges;
    }

    //private long Rec(long border, long Seed)
    //{
    //    long tempBorder = border;
    //    if (Seed < border)
    //    {
    //        tempBorder = border / 2;
    //        return Rec(tempBorder, Seed);
    //    }
    //    else
    //    {
    //        if (Seed > border)
    //        {
    //            tempBorder = border + border / 2;
    //            return Rec(tempBorder, Seed);
    //        }
    //        else
    //        {
    //            return border;
    //        }
    //    }
    //}

    private long CalculateOffset(long Seed, long start)
    {
        return Seed - start;
    }

    private bool IsValueInRange(long value, long start , long end)
    {
        if (value >= start && value <= end)
        {
            return true;
        }
        return false;
    }

    private long[] ParseSeeds(string line)
    {
        string s = line.Replace("seeds:", "").Trim();
        return ParseLine(s);
    }

    private void ParseInput()
    {
        List<List<long>> longs = new List<List<long>>();
        string[] cleanInput = Input.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        int[] indeces = new int[8];
        for (int i = 0; i < cleanInput.Length; i++)
        {
            string line = cleanInput[i];
            if (line.StartsWith("seeds:"))
            {
                indeces[0] = i;
            }

            if (line.StartsWith("seed-to-soil map:"))
            {
                indeces[1] = i;
            }

            if (line.StartsWith("soil-to-fertilizer map:"))
            {
                indeces[2] = i;
            }

            if (line.StartsWith("fertilizer-to-water map:"))
            {
                indeces[3] = i;
            }

            if (line.StartsWith("water-to-light map:"))
            {
                indeces[4] = i;
            }

            if (line.StartsWith("light-to-temperature map:"))
            {
                indeces[5] = i;
            }

            if (line.StartsWith("temperature-to-humidity map:"))
            {
                indeces[6] = i;
            }

            if (line.StartsWith("humidity-to-location map:"))
            {
                indeces[7] = i;
            }
        }
        _seeds = ParseSeeds(cleanInput[indeces[0]]);
        _seedToSoilMap = ParseArea(cleanInput, indeces[1], indeces[2]);
        _soilToFertilizerMap = ParseArea(cleanInput, indeces[2], indeces[3]);
        _fertilizerToWaterMap = ParseArea(cleanInput, indeces[3], indeces[4]);
        _waterToLightMap = ParseArea(cleanInput, indeces[4], indeces[5]);
        _lightToTemperaturMap = ParseArea(cleanInput, indeces[5], indeces[6]);
        _temperatureToHumidityMap = ParseArea(cleanInput, indeces[6], indeces[7]);
        _humidityToLocationMap = ParseArea(cleanInput, indeces[7], cleanInput.Length);
    }

    private long[] ParseLine(string line)
    {
        string[] splits = line.Split(' ');
        long[] result = new long[splits.Length];
        for (int i = 0; i < splits.Length; i++)
        {
            result[i] = long.Parse(splits[i]);
        }
        return result;
    }
    /// <summary>
    /// startIndex and stopIndex are not inclusive
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="startIndex"></param>
    /// <param name="stopIndex"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private long[][] ParseArea(string[] lines, int start, int end)
    {
        if (start < 0 || end < 0)
        {
            return Enumerable.Empty<long[]>().ToArray();
        }

        if (start > lines.Length || end > lines.Length)
        {
            return Enumerable.Empty<long[]>().ToArray();
        }

        int length = (end - start) - 1;
        long[][] result = new long[length][];
        for (int i = start + 1; i < end; i++)
        {
            result[i - start - 1] = ParseLine(lines[i]);
        }
        return result;
    }

    public override string? ToString()
    {
        StringBuilder sb = new StringBuilder();
        return sb.AppendLine($"")
        .AppendLine($"Seeds:")
        .AppendLine($"{string.Join(", ", _seeds)}")
        .AppendLine($"SeedToSoilMap:")
        .AppendLine($"{ArrayToString(_seedToSoilMap)}")
        .AppendLine($"SoilToFertilizerMap:")
        .AppendLine($"{ArrayToString(_soilToFertilizerMap)}")
        .AppendLine($"FertilizerToWaterMap:")
        .AppendLine($"{ArrayToString(_fertilizerToWaterMap)}")
        .AppendLine($"WaterToLightMap:")
        .AppendLine($"{ArrayToString(_waterToLightMap)}")
        .AppendLine($"LightToTemperaturMap:")
        .AppendLine($"{ArrayToString(_lightToTemperaturMap)}")
        .AppendLine($"TemperatureToHumidityMap:")
        .AppendLine($"{ArrayToString(_temperatureToHumidityMap)}")
        .AppendLine($"HumidityToLocationMap:")
        .AppendLine($"{ArrayToString(_humidityToLocationMap)}").ToString();
    }

    private string ArrayToString(long[][] array)
    {
        StringBuilder sb = new StringBuilder();
        foreach (long[] longs in array)
        {
            sb.AppendLine(string.Join(", ", longs));
        }
        return sb.ToString();
    }  
    
    private string ArrayToString(long[] array)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Join(", ", array));
        return sb.ToString();
    }

    private class Range
    {
        public Range(long start, long length)
        {
            Start = start;
            Length = length;
            End = Start + length;
        }

        public long Start { get; private set; }
        public long Length { get; private set; }
        public long End { get; private set; }
        private long Offset { get; set; } = 0;
        public long Count 
        { 
            get 
            { 
                return End - Start;
            } 
        }

        public long Current
        {
            get
            {
                return CalculateCurrent();
            }
        }

        private long CalculateCurrent()
        {
            long curr = Start + Offset;
            Offset++;
            if (curr > End)
            {
                throw new IndexOutOfRangeException();
            }
            return curr;
        }


        
    }
}
