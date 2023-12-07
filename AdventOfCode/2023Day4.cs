using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class _2023Day4 : Solution
{
    public _2023Day4(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        List<ScratchCard> cards = ExtractScratchCards(Input);
        List<int> scores = new List<int>();
        foreach (ScratchCard card in cards)
        {
            scores.Add(card.CalculateScore());
        }
        return scores.Sum();
    }

    public override object DoPartTwo()
    {
        List<ScratchCard> originalStack = ExtractScratchCards(Input);
        CountingDictionary dict = new CountingDictionary(originalStack);
        foreach (ScratchCard card in dict)
        {
            Console.WriteLine($"Card {card.Id} has {dict.GetCount(card)}");
            int count = dict.GetCount(card);
            for (int i = 0; i < count; i++)
            {
                int[] range = card.CalculateIdRange();
                List<ScratchCard> cards = GetAllCardsWithIdInRange(originalStack, range);
                foreach (ScratchCard c in cards)
                {
                    dict.Add(c);
                }
            }
        }

        return dict.TotalCount();
    }

    public override void Run()
    {
        Init();
        Console.WriteLine($"2023 Day 4");
        Console.WriteLine($"Part one: {DoPartOne()}");
        Console.WriteLine("Starting Part 2");
        Console.WriteLine($"Part two: {DoPartTwo()}");
    }

    public void Test()
    {
        Init();
        Console.WriteLine($"2023 Day 4 - Test");
        List<ScratchCard> cards = ExtractScratchCards(Input);
        int score = cards[0].CalculateScore();
        Console.WriteLine($"Part one: {score}");
    }

    private List<ScratchCard> ExtractScratchCards(string[] input)
    {
        List<ScratchCard> result = new List<ScratchCard>();
        foreach (string line in input)
        {
            result.Add(new ScratchCard(line));
        }
        return result;
    }

    private List<ScratchCard> GetAllCardsWithIdInRange(List<ScratchCard> input, int[] range)
    {
        List<ScratchCard> result = new List<ScratchCard>();
        foreach (ScratchCard card in input)
        {
            if (range.Contains(card.Id))
            {
                result.Add(card);
            }
        }
        return result;
    }
















    //Parses Correctly
    private class ScratchCard
    {
        public int Id { get; set; }
        public List<int> WinningNumbers { get; set; }
        public List<int> Numbers { get; set; }

        public ScratchCard(string line)
        {
            string[] first = line.Split(":");
            Id = ExtractId(first[0]);
            string[] second = first[1].Split("|");
            WinningNumbers = ExtractWinningNumbers(second[0]);
            Numbers = ExtractNumbers(second[1]);
        }

        private int ExtractId(string line)
        {
            string temp = line.Replace("Card ", "");
            temp = temp.Trim();

            return int.Parse(temp);
        }

        private List<int> ExtractWinningNumbers(string line)
        {
            List<int> result = new List<int>();
            string[] winningNumbers = line.Split(" ");
            foreach (string number in winningNumbers)
            {
                if (number.Equals(" ") || number.Equals(""))
                {
                    continue;
                }
                result.Add(int.Parse(number)); 
            }
            return result;
        }

        private List<int> ExtractNumbers(string line)
        {
            List<int> result = new List<int>();
            string[] numbers = line.Split(" ");
            foreach (string number in numbers)
            {
                if (number.Equals(" ") || number.Equals(""))
                {
                    continue;
                }
                result.Add(int.Parse(number));
            }
            return result;
        }

        public int CalculateScore()
        {
            int result = 0;
            foreach (int number in Numbers)
            {
                if (WinningNumbers.Contains(number))
                {
                    if (result == 0)
                    {
                        result = 1;
                        continue;
                    }
                    result *= 2;
                }
            }
            return result;
        }

        private int CalculateMatching()
        {
            int result = 0;
            foreach (int number in Numbers)
            {
                if (WinningNumbers.Contains(number))
                {
                    result++;
                }
            }
            return result;
        }

        public int[] CalculateIdRange()
        {
            int matching = CalculateMatching();
            int id = Id;
            int[] range = new int[matching];
            for (int i = 0; i < range.Length; i++)
            {
                range[i] = ++id;
            }
            return range;
        }
    }

    private class CountingDictionary
    {
        private Dictionary<ScratchCard, int> _dict = new Dictionary<ScratchCard, int>();

        private List<ScratchCard> _cards = new List<ScratchCard>();

        public CountingDictionary(List<ScratchCard> cards)
        {
            _cards = cards;
            foreach (ScratchCard card in cards)
            {
                Add(card);
            }
        }

        public void Add(int id, int amount)
        {
            ScratchCard? card = _cards.Find(x => x.Id == id);
            if (card == null)
            {
                return;
            }
            Add(card,amount);
        }

        public void Add(ScratchCard card)
        {
            if (_dict.ContainsKey(card))
            {
                _dict[card]++;
            }
            else
            {
                _dict.Add(card, 1);
            }
        }

        public void Add(ScratchCard card, int amount)
        {
            if (_dict.ContainsKey(card))
            {
                _dict[card] += amount;
            }
            else
            {
                _dict.Add(card, amount);
            }
        }

        public int GetCount(ScratchCard card)
        {
            if (_dict.ContainsKey(card))
            {
                return _dict[card];
            }
            return 0;
        }

        public IEnumerator<ScratchCard> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        public int TotalCount()
        {
            int result = 0;
            foreach (int count in _dict.Values)
            {
                result += count;
            }
            return result;
        }
    }
}
