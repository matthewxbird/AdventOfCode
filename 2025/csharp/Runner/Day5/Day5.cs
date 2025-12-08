using Common;

namespace Day5;

public class Day5 : IDay
{
    private string[] _input = File.ReadAllLines("input.txt");
    
    public string Part1()
    {
        HashSet<long> freshIngredients = new HashSet<long>();
        List<Tuple<long, long>> ranges = [];
        bool listIngredients = true;
        foreach (var database in _input)
        {
            if (database == "")
            {
                listIngredients = false;
                continue;
            }

            if (listIngredients)
            {
                var range = database.Split("-");
                Tuple<long, long> t = new(long.Parse(range[0]), long.Parse(range[1]));
                ranges.Add(t);
            }
            else
            {
                var id = long.Parse(database);

                foreach (var r in ranges)
                {
                    if (id >= r.Item1 && id <= r.Item2)
                    {
                        freshIngredients.Add(id);
                    }
                }
            }
        }

        return freshIngredients.Count.ToString();
    }

    /// <summary>
    /// GG take - 64GB enough of memory not enough to brute force this. Guess we gotta get clever and not project the whole sequence...
    /// </summary>
    /// <returns></returns>
    public string Part2()
    {
        List<(long start, long end)> ranges = [];
        
        foreach (var database in _input)
        {
            if (database == "")
            {
                break;
            }

            var range = database.Split("-");
            ranges.Add(new ValueTuple<long, long>(long.Parse(range[0]), long.Parse(range[1])));
        }
        
        var sorted = ranges.OrderBy(r => r.start).ToList();
        var merged = new List<(long start, long end)>();

        foreach (var range in sorted)
        {
            if (merged.Count == 0)
            {
                merged.Add(range);
            }
            else
            {
                var last = merged[merged.Count - 1];
                if (range.start <= last.end + 1) // overlap or touching
                {
                    merged[merged.Count - 1] = (last.start, Math.Max(last.end, range.end));
                }
                else
                {
                    merged.Add(range);
                }
            }
        }

        var result = merged.Sum(r => (long)(r.end - r.start + 1));
        return result.ToString();
    }
}