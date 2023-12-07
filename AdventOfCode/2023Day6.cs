using System.Text;

namespace AdventOfCode;
internal class _2023Day6 : Solution
{
    private List<Race> _races = new List<Race>();
    private BigRace _bigChungus = new BigRace();

    public _2023Day6(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        Boat b = new Boat(1);
        List<int> counts = new List<int>();
        foreach (Race r in _races)
        {
            List<int> distances = b.SimulateTravelledDistance(r.Time);
            List<int> larger = distances.Where(x => x > r.Distance).ToList();
            //needs work
            counts.Add(larger.Count);
        }
        return CalculateMarginOfError(counts);
    }

    public override object DoPartTwo()
    {
        Boat b = new Boat(1);
        List<long> distances = b.SimulateTravelledDistance(_bigChungus.Time);
        List<long> larger = distances.Where(x => x > _bigChungus.Distance).ToList();
        return larger.Count;
    }

    public override void Run()
    {
        Init();
        ParseInput();
        Console.WriteLine($"2023 Day 5");
        Console.WriteLine($"Part 1: {DoPartOne()}");
        Console.WriteLine($"Part 2: {DoPartTwo()}");
    }

    private void ParseInput()
    {
        int[] times = ExtractValues(Input[0]);
        int[] distances = ExtractValues(Input[1]);
        _races = ParseRaceList(times, distances);
        _bigChungus = ParseBigRace(times, distances);
    }

    private List<Race> ParseRaceList(int[] times, int[] distances)
    {
        if (times.Length != distances.Length)
        {
            throw new Exception("Times and distances must be of equal length");
        }
        List<Race> races = new List<Race>();
        for (int i = 0; i < times.Length; i++)
        {
            races.Add(new Race(times[i], distances[i], i));
        }
        return races;
    }

    private BigRace ParseBigRace(int[] times, int[] distances)
    {
        string[] timesString = times.Select(x => x.ToString()).ToArray();
        string[] distancesString = distances.Select(x => x.ToString()).ToArray();
        string time = string.Join("", timesString);
        string distance = string.Join("", distancesString);
        return new BigRace(long.Parse(time), long.Parse(distance));
    }

    private int[] ExtractValues(string line)
    {
        string[] splits = line.Split(':');
        string target = splits[1].Trim();
        string[] temp = target.Split(' ');
        temp = RemoveEmptyOrWhiteSpace(temp);
        int[] result = new int[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            result[i] = int.Parse(temp[i]);
        }
        return result;
    }

    private string[] RemoveEmptyOrWhiteSpace(string[] values)
    {
        List<string> result = new List<string>();
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                result.Add(value);
            }
        }
        return result.ToArray();
    }

    private int CalculateMarginOfError(int[] values)
    {
        int temp = 1;
        foreach (var value in values)
        {
            temp *= value;
        }
        return temp;
    }

    private int CalculateMarginOfError(List<int> values)
    {
        return CalculateMarginOfError(values.ToArray());
    }

    public override string? ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var race in _races)
        {
            sb.Append(race.ToString());
        }
        return sb.ToString();
    }

    private class Race
    {
        public Race(int time, int distance, int raceNumber)
        {
            Time = time;
            Distance = distance;
            RaceNumber = raceNumber;
        }

        public Race()
        {
            Time = 0;
            Distance = 0;
            RaceNumber = 0;
        }

        public int RaceNumber { get; }
        public int Time { get; }
        public int Distance { get; }

        public override string? ToString()
        {
            StringBuilder sb = new StringBuilder();
            return sb.AppendLine($"Race: {RaceNumber}")
                .AppendLine($"Time: {Time}")
                .AppendLine($"Distance: {Distance}")
                .AppendLine()
                .ToString();
        }
    }

    private class BigRace
    {
        public BigRace()
        {
        }

        public BigRace(long time, long distance)
        {
            Time = time;
            Distance = distance;
        }

        public long Time { get; }
        public long Distance { get; }


        public override string? ToString()
        {
            StringBuilder sb = new StringBuilder();
            return sb.AppendLine($"The Big Race")
                .AppendLine($"Time: {Time}")
                .AppendLine($"Distance: {Distance}")
                .AppendLine()
                .ToString();
        }
    }

    private class Boat
    {
        private int _baseSpeed = 0;
        private int _baseSpeedIncrease = 1;

        public Boat()
        {
            _baseSpeedIncrease = 1;
        }

        public Boat(int baseSpeedIncrease)
        {
            _baseSpeedIncrease = baseSpeedIncrease;
        }



        public int SimulateTravelledDistance(int timeHeld, int totalTime)
        {
            if (timeHeld > totalTime)
            {
                return int.MinValue;
            }

            int remainingTime = totalTime - timeHeld;
            int speed = _baseSpeed + _baseSpeedIncrease * timeHeld;
            int distance = speed * remainingTime;
            return distance;
        }

        public List<int> SimulateTravelledDistance(int totalTime)
        {
            List<int> result = new List<int>();
            for (int i = 0; i <= totalTime; i++)
            {
                result.Add(SimulateTravelledDistance(i, totalTime));
            }
            return result;
        }

        public long SimulateTravelledDistance(long timeHeld, long totalTime)
        {
            if (timeHeld > totalTime)
            {
                return long.MinValue;
            }

            long remainingTime = totalTime - timeHeld;
            long speed = _baseSpeed + _baseSpeedIncrease * timeHeld;
            long distance = speed * remainingTime;
            return distance;
        }

        public List<long> SimulateTravelledDistance(long totalTime)
        {
            List<long> result = new List<long>();
            for (long i = 0; i <= totalTime; i++)
            {
                result.Add(SimulateTravelledDistance(i, totalTime));
            }
            return result;
        }
    }
}
