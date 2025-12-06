using Common;

namespace Day1;

public class Day1 : IDay
{
    private readonly string[] _input = File.ReadAllLines("input.txt");

    public string Part1()
    {
        int password = 0;
        int position = 50;
        
        foreach (var line in _input)
        {
            Console.WriteLine("Processing line: " + line);
            var dir = line[..1];
            var amount = int.Parse(line[1..]);
            
            while (amount != 0)
            {
                amount--;
                switch (dir)
                {
                    case "L":
                        position--;
                        break;
                    case "R":
                        position++;
                        break;
                }

                position = position switch
                {
                    100 => 0,
                    -1 => 99,
                    _ => position
                };
            }
            
            if (position == 0)
                password++;
        }
        
        return password.ToString();
    }

    public string Part2()
    {
        int password = 0;
        int position = 50;
        
        foreach (var line in _input)
        {
            Console.WriteLine("Processing line: " + line);
            var dir = line[..1];
            var amount = int.Parse(line[1..]);
            
            while (amount != 0)
            {
                amount--;
                switch (dir)
                {
                    case "L":
                        position--;
                        break;
                    case "R":
                        position++;
                        break;
                }

                position = position switch
                {
                    100 => 0,
                    -1 => 99,
                    _ => position
                };
                
                if (position == 0)
                    password++;
            }
        }
        
        return password.ToString();
    }
}