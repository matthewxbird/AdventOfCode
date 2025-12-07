using System.Text;
using Common;

namespace Day3;

public class Day3 : IDay
{
    private readonly string[] _banks = File.ReadAllLines("input.txt");
    
    public string Part1()
    {
        long result = 0;
        foreach (var bank in _banks)
        {
            int largest = -1;
            int largestIndex = 0;
            int nextLargest = -1;
            for (var index = 0; index < bank.Length; index++)
            {
                var battery = int.Parse(bank[index].ToString());

                if (battery > largest)
                {
                    largest = battery;
                    largestIndex = index;
                }
            }
            
            for (var index = largestIndex+1; index < bank.Length; index++)
            {
                var battery = int.Parse(bank[index].ToString());

                if (battery > nextLargest)
                {
                    nextLargest = battery;
                }
            }

            if (nextLargest == -1)
            {
                nextLargest = largest;
                largest = -1;
                for (var index = 0; index < bank.Length; index++)
                {
                    var battery = int.Parse(bank[index].ToString());

                    if (battery > largest && battery != nextLargest)
                    {
                        largest = battery;
                    }
                }
            }

            string shovedTogether = string.Concat(largest, nextLargest);
            int shovedTogetherInt = int.Parse(shovedTogether);
            result += shovedTogetherInt;
        }
        
        return result.ToString();
    }

    public string Part2()
    {
        long sum = 0;
        foreach (var bank in _banks)
        {
            int n = bank.Length;
            var stack = new Stack<char>();

            for (int i = 0; i < n; i++)
            {
                char d = bank[i];
                
                while (stack.Count > 0 && stack.Peek() < d && stack.Count - 1 + (n - i) >= 12)
                {
                    stack.Pop();
                }
                
                if (stack.Count < 12)
                {
                    stack.Push(d);
                }
            }
            
            char[] result = stack.ToArray();
            Array.Reverse(result);
            sum += long.Parse(string.Concat(result));
        }

        return sum.ToString();
    }
}