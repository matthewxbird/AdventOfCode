using System.Numerics;
using Common;

namespace Day7;

public class Day7 : IDay
{
    private string[] inputLines = File.ReadAllLines("input.txt"); 
    
    private void PrettyPrint(char[][] diagram)
    {
        foreach (var row in diagram)
        {
            foreach (var ch in row)
            {
                Console.Write(ch);
            }
            Console.WriteLine();
        }
    }
    
    public string Part1()
    {
        char[][] original = inputLines.Select(r => r.ToCharArray()).ToArray();
        int rows = original.Length;
        int cols = original[0].Length;

        // find S
        int sr = -1, sc = -1;
        for (int r = 0; r < rows; r++)
        {
            int c = inputLines[r].IndexOf('S');
            if (c >= 0)
            {
                sr = r;
                sc = c;
                break;
            }
        }
        if (sr == -1) throw new ArgumentException("No S found in input");

        // active beams are positions (r,c) processed this step
        var active = new HashSet<(int r, int c)>();
        if (sr + 1 < rows) active.Add((sr + 1, sc));

        var countedSplitters = new HashSet<(int r, int c)>();
        var persistentVisited = new HashSet<(int r, int c)>(); // accumulate all beam positions for overlay
        int splits = 0;
        int step = 0;

        while (active.Count > 0)
        {
            // Add current active positions to persistent trace
            foreach (var p in active) persistentVisited.Add(p);

            var nextActive = new HashSet<(int r, int c)>();

            // Process each active beam
            foreach (var pos in active)
            {
                int r = pos.r;
                int c = pos.c;

                // If out of bounds horizontally or vertically, beam exits immediately
                if (c < 0 || c >= cols || r < 0 || r >= rows) continue;

                char cell = original[r][c];

                if (cell == '^')
                {
                    // If this splitter hasn't been counted yet, count it and spawn left/right beams
                    if (!countedSplitters.Contains((r, c)))
                    {
                        countedSplitters.Add((r, c));
                        splits++;
                    }

                    int leftC = c - 1;
                    int rightC = c + 1;
                    // Spawn beams at same row (they will be printed next iteration and then move down)
                    if (leftC >= 0) nextActive.Add((r, leftC));
                    if (rightC < cols) nextActive.Add((r, rightC));
                    // original beam stops here
                }
                else
                {
                    // Beam passes through this cell; it will continue downward next step
                    int downR = r + 1;
                    if (downR < rows)
                    {
                        nextActive.Add((downR, c));
                    }
                    // if downR >= rows, beam exits manifold
                }
            }

            // Move to next step
            active = nextActive;
        }
        
        return splits.ToString();
    }
    

    public string Part2()
    {
        int rows = inputLines.Length;
        int cols = inputLines[0].Length;

        // find S
        int sr = -1, sc = -1;
        for (int r = 0; r < rows; r++)
        {
            int c = inputLines[r].IndexOf('S');
            if (c >= 0)
            {
                sr = r;
                sc = c;
                break;
            }
        }
        if (sr == -1) throw new ArgumentException("No S found in input");

        // start position is the cell immediately below S (if inside grid)
        int startR = sr + 1;
        int startC = sc;

        var memo = new Dictionary<(int r, int c), BigInteger>();
        var visiting = new HashSet<(int r, int c)>(); // cycle detection (shouldn't occur in valid inputs)

        return DetermineTimeline(startR, startC, cols, rows, memo, visiting).ToString();
    }

    private BigInteger DetermineTimeline(int r, int c, int cols, int rows, Dictionary<(int r, int c), BigInteger>? memo, HashSet<(int r, int c)>? visiting)
    {
        // out of horizontal bounds -> beam exits immediately: one timeline
        if (c < 0 || c >= cols) return BigInteger.One;
        // out of vertical bounds -> beam exited: one timeline
        if (r < 0 || r >= rows) return BigInteger.One;

        var key = (r, c);
        if (memo.TryGetValue(key, out var val)) return val;
        if (visiting.Contains(key))
        {
            // cycle detected; to be safe, treat as zero (or throw). Valid AoC inputs do not contain cycles.
            throw new InvalidOperationException($"Cycle detected at {r},{c} - input likely invalid for this model.");
        }

        visiting.Add(key);

        char ch = inputLines[r][c];
        BigInteger count = BigInteger.Zero;

        if (ch == '^')
        {
            // splitter: branch left and right (starting at same row)
            // if branch goes out of bounds, that branch counts as an exit (1)
            count += DetermineTimeline(r, c - 1, cols, rows, memo, visiting);
            count += DetermineTimeline(r, c + 1, cols, rows, memo, visiting);
        }
        else
        {
            // empty space or S or any other char: beam continues downward
            count += DetermineTimeline(r + 1, c, cols, rows, memo, visiting);
        }

        visiting.Remove(key);
        memo[key] = count;
        return count;
    }
}