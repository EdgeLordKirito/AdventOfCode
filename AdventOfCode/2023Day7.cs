namespace AdventOfCode;
internal class _2023Day7 : Solution
{
    public _2023Day7(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        List<Hand> hands = GetHandTemps(false);
        hands.Sort();
        return CalculateTotal(hands);
    }

    public override object DoPartTwo()
    {
        List<Hand> hands = GetHandTemps(true);
        hands.Sort();
        return CalculateTotal(hands);
    }

    public override void Run()
    {
        Init();
        Console.WriteLine($"2023 Day 7");
        Console.WriteLine($"Part one: {DoPartOne()}");
        Console.WriteLine($"Part two: {DoPartTwo()}");
    }

    private long CalculateTotal(List<Hand> list)
    {
        long total = 0;
        for (int i = 0; i < list.Count; i++)
        {
            total += list[i].Bid * (i + 1);
        }

        return total;
    }

    private List<Hand> GetHandTemps(bool special)
    {
        List<Hand> hands = new List<Hand>();
        foreach (string s in Input)
        {
            string[] splits = s.Split(" ");
            Hand hand = new Hand(splits[0], int.Parse(splits[1]),special);
            hands.Add(hand);
        }
        return hands;
    }

    private class HandComparer : IComparer<Hand>
    {
        private readonly string _cardValuesSortedAscending = "";

        public string Order => new string(_cardValuesSortedAscending);

        public HandComparer(string ordering)
        {
            _cardValuesSortedAscending = ordering;
        }

        public HandComparer(bool mode)
        {
            if (mode)
            {
                _cardValuesSortedAscending = "J23456789TQKA";
            }
            else
            {
                _cardValuesSortedAscending = "23456789TJQKA";
            }
        }

        public int Compare(Hand? x, Hand? y)
        {
            if (x is null && y is null)
            {
                return 0;
            }
            else if (x is null)
            {
                return -1;
            }
            else if (y is null)
            {
                return 1;
            }
            Type l = x.GetHandType();
            Type r = y.GetHandType();
            if (l.CompareTo(r) > 0)
            {
                return 1;
            }
            else if (l.CompareTo(r) < 0)
            {
                return -1;
            }
            else
            {
                //Equality case
                return CompareHands(x.Cards, y.Cards);
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
    }

    private class Hand : IEquatable<Hand>, IComparable<Hand>, IComparable
    {
        private bool _isSpecial = false;
        private string _cards = "";
        private int _bid = 0;

        public string Cards => new string(_cards);
        public int Bid => _bid;

        public Hand(string cards, int bid, bool special)
        {
            _cards = cards;
            _bid = bid;
            _isSpecial = special;
        }

        public Hand(string cards, int bid) : this(cards, bid, false)
        {
        }

        public Type GetHandType()
        {
            Type type = GetHandType(_cards);
            string target = new string(_cards);
            if (!_isSpecial)
            {
                return type;
            }

            if (!target.Contains('J') || type.Equals(Type.FiveOfAKind))
            {
                return type;
            }
            int jokerCount = target.Count(c => c == 'J');
            Type t = GetHandType(target);
            switch (t, jokerCount)
            {
                case (Type.FourOfAKind, _):
                    return Type.FiveOfAKind;

                case (Type.FullHouse, _):
                    return Type.FiveOfAKind;

                case (Type.ThreeOfAKind, 1):
                    return Type.FourOfAKind;

                case (Type.ThreeOfAKind,2):
                    return Type.FiveOfAKind;

                case (Type.TwoPair, 1):
                    return Type.FullHouse;

                case (Type.TwoPair, 2):
                    return Type.FourOfAKind;

                case (Type.Pair, 1):
                    return Type.ThreeOfAKind;

                case (Type.Pair, 2):
                    return Type.FourOfAKind;

                case (Type.HighCard, 1):
                    return Type.Pair;

                default:
                    return t;
            }
        }

        private Type GetHandType(string hand)
        {
            if (IsFiveOfAKind(hand))
            {
                return Type.FiveOfAKind;
            }
            else if (IsFourOfAKind(hand))
            {
                return Type.FourOfAKind;
            }
            else if (IsFullHouse(hand))
            {
                return Type.FullHouse;
            }
            else if (IsThreeOfAKind(hand))
            {
                return Type.ThreeOfAKind;
            }
            else if (IsTwoPair(hand))
            {
                return Type.TwoPair;
            }
            else if (IsPair(hand))
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
            return obj is not null && Equals(obj as Hand);
        }

        public int CompareTo(Hand? other)
        {
            HandComparer comparer = new HandComparer(_isSpecial);
            return comparer.Compare(this, other);
        }

        public int CompareTo(object? obj)
        {
            if(obj is not Hand)
            {
                throw new ArgumentException("Object is not valid");
            }
            return CompareTo((Hand)obj);
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
