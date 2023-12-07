using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class _2023Day7 : Solution
{
    private List<HandBidPair> _hands = new List<HandBidPair>();

    public _2023Day7(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        Console.WriteLine("Sorted:");
        List<HandBidPair> sortedCopy = SortHands(_hands);
        long total = 0;
        for (int i = 0; i < sortedCopy.Count; i++)
        {
            total += sortedCopy[i].Bid * (i + 1);
        }

        return total;
    }

    public override object DoPartTwo()
    {
        throw new NotImplementedException();
    }

    public override void Run()
    {
        Init();
        _hands = GetHands();
        Console.WriteLine($"Part one: {DoPartOne()}");
    }

    private List<HandBidPair> GetHands()
    {
        List<HandBidPair> hands = new List<HandBidPair>();
        foreach (string s in Input)
        {
            string[] splits = s.Split(" ");
            Hand h = new Hand(splits[0]);
            HandBidPair pair = new HandBidPair(h, int.Parse(splits[1]));
            hands.Add(pair);
        }
        return hands;
    }

    //Sort a list of HandBidPairs
    private List<HandBidPair> SortHands(List<HandBidPair> hands)
    {
        List<HandBidPair> sortedHands = [.. hands];
        sortedHands.Sort();
        return sortedHands;
    }


    private class HandBidPair : IComparable<HandBidPair>, IComparable, IEquatable<HandBidPair>
    {
        public Hand Hand { get; private set; }
        public int Bid { get; private set; }

        public HandBidPair(Hand hand, int bid)
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
            return HashCode.Combine(Hand,Bid);
        }
    }

    private class Hand : IComparable<Hand>, IComparable, IEquatable<Hand>
    {
        private readonly string _cards = "";
        private static string _cardValuesSortedAscending = "23456789TJQKA";
        private static string _cardValuesWithJokerRule = "J23456789TQKA";

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
