namespace AdventOfCode;
internal class _2023Day7 : Solution
{
    private List<HandBidPair> _One = new List<HandBidPair>();
    private List<HandBidPair> _Two = new List<HandBidPair>();

    public _2023Day7(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        List<HandBidPair> sortedCopy = SortHands(_One);
        return CalculateTotal(sortedCopy);
    }

    public override object DoPartTwo() // On Example it is correct but puzzle input is incorrect
    {
        List<HandBidPair> sortedCopy = SortHands(_Two);
        foreach (HandBidPair pair in sortedCopy)
        {
            Console.WriteLine(pair);
        }
        return CalculateTotal(sortedCopy);
    }

    public override void Run()
    {
        Init();
        _One = GetHands(false);
        _Two = GetHands(true);
        Console.WriteLine($"2023 Day 7");
        Console.WriteLine($"Part one: {DoPartOne()}");
        Console.WriteLine($"Part two: {DoPartTwo()}");
    }

    private long CalculateTotal(List<HandBidPair> list)
    {
        long total = 0;
        for (int i = 0; i < list.Count; i++)
        {
            total += list[i].Bid * (i + 1);
        }

        return total;
    }

    private List<HandBidPair> GetHands(bool special)
    {
        List<HandBidPair> hands = new List<HandBidPair>();
        if (special)
        {
            foreach (string s in Input)
            {
                string[] splits = s.Split(" ");
                Hand2 h = new Hand2(splits[0]);
                HandBidPair pair = new HandBidPair(h, int.Parse(splits[1]));
                hands.Add(pair);
            }
            return hands;
        }
        else
        {
            foreach (string s in Input)
            {
                string[] splits = s.Split(" ");
                Hand h = new Hand(splits[0]);
                HandBidPair pair = new HandBidPair(h, int.Parse(splits[1]));
                hands.Add(pair);
            }
            return hands;
        }
    }

    //Sort a list of HandBidPairs
    private List<HandBidPair> SortHands(List<HandBidPair> hands)
    {
        List<HandBidPair> sortedHands = [.. hands];
        sortedHands.Sort();
        return sortedHands;
    }

    private interface IHand : IComparable<IHand>, IComparable, IEquatable<IHand>
    {
        public Type GetHandType();
        public string Cards { get; }
    }

    private class HandBidPair : IComparable<HandBidPair>, IComparable, IEquatable<HandBidPair>
    {
        public IHand Hand { get; private set; }
        public int Bid { get; private set; }

        public HandBidPair(IHand hand, int bid)
        {
            Hand = hand;
            Bid = bid;
        }

        public override string ToString()
        {
            return $"{Hand} | {Bid}";
        }

        public int CompareTo(HandBidPair? other)
        {
            if (other is null)
            {
                return 1;
            }
            return Hand.CompareTo(other.Hand);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is HandBidPair h)
            {
                return CompareTo(h);
            }
            else
            {
                throw new ArgumentException("Object is not a HandBidPair");
            }
        }

        public bool Equals(HandBidPair? other)
        {
            if (other is null)
            {
                return false;
            }
            return Hand.Equals(other.Hand);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is HandBidPair h ? Equals(h) : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hand, Bid);
        }
    }

    private class Hand : IHand, IComparable<Hand>, IComparable, IEquatable<Hand>
    {
        private readonly string _cards = "";
        private static string _cardValuesSortedAscending = "23456789TJQKA";

        public string Cards => new string(_cards);

        public Hand(string cards)
        {
            _cards = cards;
        }

        public Type GetHandType()
        {
            if (IsFiveOfAKind())
            {
                return Type.FiveOfAKind;
            }
            else if (IsFourOfAKind())
            {
                return Type.FourOfAKind;
            }
            else if (IsFullHouse())
            {
                return Type.FullHouse;
            }
            else if (IsThreeOfAKind())
            {
                return Type.ThreeOfAKind;
            }
            else if (IsTwoPair())
            {
                return Type.TwoPair;
            }
            else if (IsPair())
            {
                return Type.Pair;
            }
            else
            {
                return Type.HighCard;
            }
        }

        private bool IsFiveOfAKind()
        {
            return _cards.Distinct().Count() == 1;
        }

        private bool IsFourOfAKind()
        {
            return _cards.GroupBy(x => x).Any(g => g.Count() == 4);
        }

        private bool IsFullHouse()
        {
            var groups = _cards.GroupBy(x => x).Select(g => g.Count()).OrderBy(c => c).ToList();
            return groups.Count == 2 && groups[0] == 2 && groups[1] == 3;
        }

        private bool IsThreeOfAKind()
        {
            return _cards.GroupBy(x => x).Any(g => g.Count() == 3);
        }

        private bool IsTwoPair()
        {
            return _cards.GroupBy(x => x).Where(g => g.Count() == 2).Count() == 2;
        }

        private bool IsPair()
        {
            return _cards.GroupBy(x => x).Any(g => g.Count() == 2);
        }

        public override string ToString()
        {
            return $"{_cards}";
        }

        public int CompareTo(Hand? other)
        {
            if (other is null)
            {
                return 1;
            }
            Type self = GetHandType();
            Type o = other.GetHandType();
            if (self.CompareTo(o) > 0)
            {
                return 1;
            }
            else if (self.CompareTo(o) < 0)
            {
                return -1;
            }
            else
            {
                //Equality case
                return CompareHands(_cards, other.Cards);
            }
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is Hand h)
            {
                return CompareTo(h);
            }
            else
            {
                throw new ArgumentException("Object is not a Hand");
            }
        }

        private int CompareHands(string left, string right)
        {
            if (left.Equals(right))
            {
                return 0;
            }
            else
            {
                for (int i = 0; i < left.Length; i++)
                {
                    int comparison = CompareCharacters(left[i], right[i]);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                return 0;
            }
        }

        private int CompareCharacters(char left, char right)
        {
            if (left.Equals(right))
            {
                return 0;
            }
            else
            {
                return _cardValuesSortedAscending.IndexOf(left).CompareTo(_cardValuesSortedAscending.IndexOf(right));
            }
        }

        public bool Equals(Hand? other)
        {
            if (other is null)
            {
                return false;
            }
            return _cards.Equals(other.Cards);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is Hand h ? Equals(h) : false;
        }

        public override int GetHashCode()
        {
            return _cards.GetHashCode();
        }

        public int CompareTo(IHand? other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other is Hand h)
            {
                return CompareTo(h);
            }
            else
            {
                throw new ArgumentException("Object is not a Hand");
            }
        }

        public bool Equals(IHand? other)
        {
            if (other is null)
            {
                return false;
            }
            if (other is Hand h)
            {
                return Equals(h);
            }
            else
            {
                return false;
            }
        }
    }

    private class Hand2 : IHand, IComparable<Hand2>, IComparable, IEquatable<Hand2>
    {
        private readonly string _cards = "";
        private static string _cardValuesSortedAscending = "J23456789TQKA";

        public string Cards => new string(_cards);

        public Hand2(string cards)
        {
            _cards = cards;
        }

        public Type GetHandType()
        {
            string target = MaximizeHand(_cards);

            if (IsFiveOfAKind(target))
            {
                return Type.FiveOfAKind;
            }
            else if (IsFourOfAKind(target))
            {
                return Type.FourOfAKind;
            }
            else if (IsFullHouse(target))
            {
                return Type.FullHouse;
            }
            else if (IsThreeOfAKind(target))
            {
                return Type.ThreeOfAKind;
            }
            else if (IsTwoPair(target))
            {
                return Type.TwoPair;
            }
            else if (IsPair(target))
            {
                return Type.Pair;
            }
            else
            {
                return Type.HighCard;
            }
        }

        private bool IsFiveOfAKind(string target)
        {
            return target.Distinct().Count() == 1;
        }

        private bool IsFourOfAKind(string target)
        {
            return target.GroupBy(x => x).Any(g => g.Count() == 4);
        }

        private bool IsFullHouse(string target)
        {
            var groups = target.GroupBy(x => x).Select(g => g.Count()).OrderBy(c => c).ToList();
            return groups.Count == 2 && groups[0] == 2 && groups[1] == 3;
        }

        private bool IsThreeOfAKind(string target)
        {
            return target.GroupBy(x => x).Any(g => g.Count() == 3);
        }

        private bool IsTwoPair(string target)
        {
            return target.GroupBy(x => x).Where(g => g.Count() == 2).Count() == 2;
        }

        private bool IsPair(string target)
        {
            return target.GroupBy(x => x).Any(g => g.Count() == 2);
        }

        private Dictionary<char, int> GetCardCounts()
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            foreach (char c in _cards)
            {
                if (counts.TryGetValue(c, out int value))
                {
                    counts[c] = ++value;
                }
                else
                {
                    counts.Add(c, 1);
                }
            }
            return counts;
        }

        private (char card, int count) GetMaxCard(Dictionary<char, int> counts)
        {
            char maxCard = '0';
            int maxCount = 0;
            foreach (KeyValuePair<char, int> kvp in counts)
            {
                if (kvp.Value > maxCount && kvp.Key != 'J')
                {
                    maxCard = kvp.Key;
                    maxCount = kvp.Value;
                }
            }
            return (maxCard, maxCount);
        }

        private string MaximizeHand(string target)
        {
            if (!target.Contains('J'))
            {
                return target;
            }
            Dictionary<char, int> counts = GetCardCounts();
            if (IsTwoPair(counts))
            {
                return HandleTwoPair(target, counts);
            }
            var tuple = GetMaxCard(counts);
            char Id = tuple.card;
            int count = tuple.count;
            if (count >= 3)
            {
                return HandleThreeAndUp(Id, target);
            }else if (count == 2)
            {
                return HandleOnePair(Id, target);
            }
            else
            {
                return HandleHighCard(target);
            }
        }

        private string HandleThreeAndUp(char id, string target)
        {
            string s = target.Replace('J', id);
            return s;
        }

        private IEnumerable<char> GetTwoPair(Dictionary<char, int> counts)
        {
            IEnumerable<char> chars = counts.Where(kvp => kvp.Value == 2).Select(kvp => kvp.Key);
            return chars;
        }

        private bool IsTwoPair(Dictionary<char,int> counts)
        {
            return counts.Where(kvp => kvp.Value == 2).Count() == 2;
        }

        private string HandleTwoPair(string target, Dictionary<char,int>counts)
        {
            IEnumerable<char> chars = GetTwoPair(counts);
            char stronger = GetStrongerOne(chars);
            string s = target.Replace('J', stronger);
            return s;
        }

        private char GetStrongerOne(IEnumerable<char> chars)
        {
            char stronger = 'J';
            foreach (char c in chars)
            {
                if (_cardValuesSortedAscending.IndexOf(c) > _cardValuesSortedAscending.IndexOf(stronger))
                {
                    stronger = c;
                }
            }
            return stronger;
        }

        private string HandleOnePair(char id, string target)
        {
            string s = target.Replace('J', id);
            return s;
        }

        private string HandleHighCard(string target)
        {
            if (target.Contains('J'))
            {
                return target.Replace('J', 'A');
            }
            else
            {
                return target;
            }
        }

        public override string ToString()
        {
            return $"{_cards}";
        }

        public int CompareTo(IHand? other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (other is Hand2 h)
            {
                return CompareTo(h);
            }
            else
            {
                throw new ArgumentException("Object is not a Hand2");
            }

        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }
            if (obj is Hand2 h)
            {
                return CompareTo(h);
            }
            else
            {
                throw new ArgumentException("Object is not a Hand2");
            }
        }

        public int CompareTo(Hand2? other)
        {
            if (other is null)
            {
                return 1;
            }
            Type self = GetHandType();
            Type o = other.GetHandType();
            if (self.CompareTo(o) > 0)
            {
                return 1;
            }
            else if (self.CompareTo(o) < 0)
            {
                return -1;
            }
            else
            {
                //Equality case
                return CompareHands(_cards, other.Cards);
            }
        }

        private int CompareHands(string left, string right)
        {
            if (left.Equals(right))
            {
                return 0;
            }
            else
            {
                for (int i = 0; i < left.Length; i++)
                {
                    int comparison = CompareCharacters(left[i], right[i]);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                return 0;
            }
        }

        private int CompareCharacters(char left, char right)
        {
            if (left.Equals(right))
            {
                return 0;
            }
            else
            {
                return _cardValuesSortedAscending.IndexOf(left).CompareTo(_cardValuesSortedAscending.IndexOf(right));
            }
        }

        public bool Equals(IHand? other)
        {
            if (other is null)
            {
                return false;
            }
            if (other is Hand2 h)
            {
                return Equals(h);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Hand2? other)
        {
            if (other is null)
            {
                return false;
            }
            return _cards.Equals(other.Cards);
        }
    }

    private enum Type
    {
        HighCard = 0,
        Pair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6,
    }
}
