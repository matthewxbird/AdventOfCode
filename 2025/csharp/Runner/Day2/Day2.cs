using Common;

namespace Day2;

public class Day2 : IDay
{
    private readonly string _input = File.ReadAllText("input.txt");
    
    public string Part1()
    {
        string[] idRanges = _input.Split(',');

        long total = 0;
        foreach (var idRange in idRanges)
        {
            var range = idRange.Split('-');
            var lower = long.Parse(range[0]);
            var upper = long.Parse(range[1]);

            for (long i = lower; i <= upper; i++)
            {
                if (!i.ToString().StartsWith('0') && !IsRepeatingGroup(i)) 
                    continue;
                
                Console.WriteLine($"{i} is invalid");
                total += i;
            }
        }
        
        return total.ToString();
    }

    bool IsRepeatingGroup(long number)
    {
        string s = number.ToString();
        int len = s.Length;

        // Only even-length strings can be split into equal groups
        if (len % 2 != 0) return false;

        // Must be identical halves
        return s[..(len / 2)] ==  s[(len / 2)..];
    }
    

    public string Part2()
    {
        string[] idRanges = _input.Split(',');

        long total = 0;
        foreach (var idRange in idRanges)
        {
            var range = idRange.Split('-');
            var lower = long.Parse(range[0]);
            var upper = long.Parse(range[1]);

            for (long i = lower; i <= upper; i++)
            {
                if (!IsInvalidId(i.ToString())) 
                    continue;
                
                Console.WriteLine($"{i} is invalid");
                total += i;
            }
        }
        
        bool IsInvalidId(string id)
        {
            int len = id.Length;

            // Try all possible group lengths up to half the string
            for (int groupLen = 1; groupLen <= len / 2; groupLen++)
            {
                if (len % groupLen != 0) continue; // must divide evenly

                string group = id[..groupLen];
                string repeated = string.Concat(Enumerable.Repeat(group, len / groupLen));

                if (repeated == id)
                    return true; // invalid
            }

            return false; // valid
        }
        
        return total.ToString();
    }
}