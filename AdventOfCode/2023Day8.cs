namespace AdventOfCode;
internal class _2023Day8 : Solution
{
    private RepeatingString _instruction = new RepeatingString();
    private string[] clean = new string[0];

    public _2023Day8(string inputPath) : base(inputPath)
    {
    }

    public override object DoPartOne()
    {
        StateMachine machine = new StateMachine(clean);
        return machine.RunSingleState(_instruction, "AAA", "ZZZ");
    }

    public override object DoPartTwo()
    {
        StateMachine machine = new StateMachine(clean);
        return machine.DoPartTwo(_instruction);
    }

    public override void Run()
    {
        Init();
        ParseInput();
        Console.WriteLine($"2023 Day 8");
        Console.WriteLine($"Part one: {DoPartOne()}");
        Console.WriteLine($"Part two: {DoPartTwo()}");
    }

    private void ParseInput()
    {
        string[] cleanLines = Input.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        _instruction = new RepeatingString(cleanLines[0]);
        string[] states = new string[cleanLines.Length - 1];
        Array.Copy(cleanLines, 1, states, 0, cleanLines.Length - 1);
        clean = states;
    }

    private class RepeatingString
    {
        public RepeatingString(string input)
        {
            _input = input;
        }

        public RepeatingString()
        {
            _input = "";
        }

        private readonly string _input;

        public string Input => new string(_input);

        public IEnumerator<char> GetEnumerator()
        {
            //Make it repeat forever
            while (true)
            {
                foreach (var item in _input)
                {
                    yield return item;
                }
            }
        }

        //Implement a reverse iterator
        public IEnumerable<char> GetReverseEnumerator()
        {
            //Make it repeat forever
            while (true)
            {
                for (int i = _input.Length - 1; i >= 0; i--)
                {
                    yield return _input[i];
                }
            }
        }
    }

    private class StateMachine
    {
        private Dictionary<string, State> _states = new Dictionary<string, State>();

        private State _currentState = new State();

        public StateMachine(string[] input)
        {
            _states = ParseInput(input);
        }

        private Dictionary<string, State> ParseInput(string[] input)
        {
            Dictionary<string, State> states = new Dictionary<string, State>();
            foreach (string line in input)
            {
                State state = new State(line);
                states.Add(state.Name, state);
            }
            return states;
        }

        public long RunSingleState(RepeatingString instructions, string startingPoint, string endPoint)
        {
            long steps = 0;
            _currentState = _states[startingPoint];
            foreach (char c in instructions)
            {
                _currentState = GetDirectionalState(_currentState, c);
                steps++;

                if (_currentState.Name.Equals(endPoint))
                {
                    break;
                }
            }
            return steps;
        }

        public long DoPartTwo(RepeatingString instructions)
        {
            List<State> states = new List<State>();
            _states.Where(pair => pair.Key.EndsWith('A')).ToList().ForEach(pair => states.Add(pair.Value));
            return RunMultiState(instructions, states);
        }

        private long RunMultiState(RepeatingString instructions, List<State> startPoint)
        {
            List<long> cyclelengths = new List<long>();
            foreach (State s in startPoint)
            {
                cyclelengths.Add(CalculateCycleLength(instructions, s));
            }
            long result = cyclelengths.Aggregate(Lcm);
            return result;
        }

        private long CalculateCycleLength(RepeatingString instructions, State state)
        {
            long steps = 0;
            _currentState = state;
            foreach (char c in instructions)
            {
                _currentState = GetDirectionalState(_currentState, c);
                steps++;

                if (IsDestination(_currentState))
                {
                    break;
                }
            }
            return steps;
        }

        private bool IsDestination(State state)
        {
            return state.Name.EndsWith('Z');
        }

        private long Gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private long Lcm(long a, long b)
        {
            return (a / Gcf(a, b)) * b;
        }

        private State GetDirectionalState(State current, char direction)
        {
            if (direction == 'L')
            {
                if (_states.TryGetValue(current.Left, out State? value))
                {
                    return value;
                }
                else
                {
                    throw new Exception("Invalid State found in left Tree");
                }
            }

            if (direction == 'R')
            {
                if (_states.TryGetValue(current.Right, out State? value))
                {
                    return value;
                }
                else
                {
                    throw new Exception("Invalid State found in right Tree");
                }
            }
            throw new Exception("Invalid input");
        }

        private class State
        {
            public State(string name, string left, string right)
            {
                Name = name;
                Left = left;
                Right = right;
            }

            public State(string line)
            {
                string[] split = line.Split('=');
                Name = split[0].Trim();
                //Get what is contained in () as a substring
                string cleaned = split[1].Trim().Substring(1, split[1].Length - 3);
                string[] sides = cleaned.Split(',');
                Left = sides[0].Trim();
                Right = sides[1].Trim();
            }

            public State()
            {
                Name = "";
                Left = "";
                Right = "";
            }

            public string Name { get; }
            public string Left { get; set; }
            public string Right { get; set; }
        }
    }
}
