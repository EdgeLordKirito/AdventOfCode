namespace AdventOfCode;
internal class _2023Day3 : Solution
{
    public _2023Day3(string input) : base(input)
    {
    }

    private char[][] _input = Array.Empty<char[]>();

    private bool[][] bools = Array.Empty<bool[]>();

    private List<(int i, int j)> neighbours = new List<(int i, int j)>
    {
        (0,-2), (0,-1), (0,0), (0,1), (0,2)
    };



    public override object DoPartOne()
    {
        List<DigitIndexPair> input = WalkInput();
        List<DigitIndexPair> surrounded = SurroundedIndexes(input);
        List<int> numbers = GetAllPartNumbers(surrounded);
        int result = 0;
        foreach (int number in numbers)
        {
            result += number;
        }
        return result;
    }

    public override object DoPartTwo()
    {
        List<DigitIndexPair> input = WalkInput();
        return 0;
    }

    //public override void Init()
    //{
    //    FileInfo info = new FileInfo(InputPath);
    //    if (!info.Exists)
    //    {
    //        return;
    //    }
    //    Input = File.ReadAllLines(InputPath);
    //    _input = InputAsArray();
    //    bools = InitHeatMap();
    //}

    public override void Run()
    {
        //Init();
        //Console.WriteLine($"2023 Day 2");
        //Console.WriteLine($"Part one: {DoPartOne()}");
        //Console.WriteLine($"Part two: {DoPartTwo()}");
    }

    private char[][] InputAsArray()
    {
        char[][] result = new char[Input.Length][];
        for (int i = 0; i < Input.Length; i++)
        {
            result[i] = Input[i].ToCharArray();
        }
        return result;
    }

    private bool[][] InitHeatMap()
    {
        bool[][] result = new bool[_input.Length][];
        for (int i = 0; i < _input.Length; i++)
        {
            result[i] = new bool[_input[i].Length];
        }
        return result;
    }

    private List<DigitIndexPair> SurroundedIndexes(List<DigitIndexPair> inputList)
    {
        List<DigitIndexPair> result = new List<DigitIndexPair>();
        foreach (DigitIndexPair digit in inputList)
        {
            if (IsSurrounded(digit))
            {
                result.Add(digit);
            }
        }
        return result;
    }

    private List<int> GetAllPartNumbers(List<DigitIndexPair> list)
    {
        List<int> result = new List<int>();
        foreach (DigitIndexPair digit in list)
        {
            result.Add(GetCompleteNumber(digit));
        }
        return result;
    }

    private int GetCompleteNumber(DigitIndexPair index)
    {
        if (bools[index.I][index.J])
        {
            return 0;
        }
        char[] chars = GetChars(index);
        int number = GetNumber(chars, index);
        return number;
    }

    private int GetNumber(char[] chars, DigitIndexPair index)
    {
        if (chars.Length != 5)
        {
            return int.MinValue;
        }

        bool left = char.IsAsciiDigit(chars[1]);
        bool right = char.IsAsciiDigit(chars[3]);

        if (left && right)
        {
            bools[index.I][index.J - 1] = true;
            bools[index.I][index.J] = true;
            bools[index.I][index.J + 1] = true;
            return int.Parse(chars[1].ToString()) * 100 + int.Parse(chars[2].ToString()) * 10 + int.Parse(chars[3].ToString());
        }

        if (left || !right)
        {
            int miniresult = 0;
            bool lefter = char.IsAsciiDigit(chars[0]);
            if (lefter)
            {
                bools[index.I][index.J - 2] = true;
                miniresult += int.Parse(chars[0].ToString()) * 100;
            }
            bools[index.I][index.J - 1] = true;
            miniresult += int.Parse(chars[1].ToString()) * 10;
            bools[index.I][index.J] = true;
            miniresult += int.Parse(chars[2].ToString());
            return miniresult;
        }

        if (right || !left)
        {
            int miniresult = 0;
            bool rightest = char.IsAsciiDigit(chars[4]);
            if (rightest)
            {
                bools[index.I][index.J + 2] = true;
                miniresult += int.Parse(chars[4].ToString());
            }
            bools[index.I][index.J + 1] = true;
            miniresult += int.Parse(chars[3].ToString()) * 10;
            bools[index.I][index.J] = true;
            miniresult += int.Parse(chars[2].ToString()) * 100;
            return miniresult;
        }

        if (!left || !right)
        {
            bools[index.I][index.J] = true;
            return int.Parse(chars[2].ToString());
        }
        return int.MinValue;
    }

    private int[] GetNums(char[] chars)
    {
        int[] result = new int[5];
        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsAsciiDigit(chars[i]))
            {
                result[i] = int.Parse(chars[i].ToString());
            }
        }
        return result;
    }

    private char[] GetChars(DigitIndexPair digit)
    {
        char[] result = new char[5];
        int leftmostIndex = digit.J - 2;
        int leftIndex = digit.J - 1;
        int rightIndex = digit.J + 1;
        int rightmostIndex = digit.J + 2;

        if (IsIndexOutOfBounds(digit.I, leftmostIndex))
        {
            result[0] = '.';
        }
        else
        {
            result[0] = _input[digit.I][leftmostIndex];
        }

        if (IsIndexOutOfBounds(digit.I, leftIndex))
        {
            result[1] = '.';
        }
        else
        {
            result[1] = _input[digit.I][leftIndex];
        }

        if (IsIndexOutOfBounds(digit.I, digit.J))
        {
            result[2] = '.';
        }
        else
        {
            result[2] = _input[digit.I][digit.J];
        }

        if (IsIndexOutOfBounds(digit.I, rightIndex))
        {
            result[3] = '.';
        }
        else
        {
            result[3] = _input[digit.I][rightIndex];
        }

        if (IsIndexOutOfBounds(digit.I, rightmostIndex))
        {
            result[4] = '.';
        }
        else
        {
            result[4] = _input[digit.I][rightmostIndex];
        }
        return result;
    }

    private bool IsIndexOutOfBounds(int i, int j)
    {
        if (i < 0 || j < 0)
        {
            return true;
        }

        if (i >= _input.Length || j >= _input[i].Length)
        {
            return true;
        }
        return false;
    }

    private List<DigitIndexPair> WalkInput()
    {
        List<DigitIndexPair> result = new List<DigitIndexPair>();
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[i].Length; j++)
            {
                if (char.IsAsciiDigit(_input[i][j]))
                {
                    result.Add(new DigitIndexPair { Value = int.Parse(_input[i][j].ToString()), I = i, J = j });
                }
            }
        }
        return result;
    }

    private bool IsSurrounded(DigitIndexPair digit)
    {
        bool leftUpCorner = IsIndexSymbol(digit.I - 1, digit.J - 1);
        bool Up = IsIndexSymbol(digit.I - 1, digit.J);
        bool rightUpCorner = IsIndexSymbol(digit.I - 1, digit.J + 1);
        bool left = IsIndexSymbol(digit.I, digit.J - 1);
        bool right = IsIndexSymbol(digit.I, digit.J + 1);
        bool leftDownCorner = IsIndexSymbol(digit.I + 1, digit.J - 1);
        bool down = IsIndexSymbol(digit.I + 1, digit.J);
        bool rightDownCorner = IsIndexSymbol(digit.I + 1, digit.J + 1);
        return leftUpCorner || Up || rightUpCorner || left || right || leftDownCorner || down || rightDownCorner;
    }

    private bool IsIndexSymbol(int i, int j)
    {
        if (IsIndexOutOfBounds(i, j))
        {
            return false;
        }

        if (IsSymbol(_input[i][j]))
        {
            return true;
        }
        return false;
    }

    private bool IsSymbol(char c)
    {
        if (char.IsAsciiDigit(c) || c.Equals('.'))
        {
            return false;
        }
        return true;
    }

    private class DigitIndexPair
    {
        public int Value { get; set; }
        public int I { get; set; }
        public int J { get; set; }

        public override string ToString()
        {
            return $"Value: {Value}, X: {I}, Y: {J}";
        }
    }
}