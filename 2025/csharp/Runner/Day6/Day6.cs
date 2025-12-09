using System.Numerics;
using System.Text;
using Common;

namespace Day6;

public class Day6 : IDay
{
    private string[] _input = File.ReadAllLines("input.txt");
    private string _worksheet = File.ReadAllText("input.txt");

    public string Part1()
    {
        string[] symbolInputs = _input.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        char[] symbols = symbolInputs.Select(s => s[0]).ToArray();

        var dim = _input[0].Trim().Split().Length - 1;
        int[,] matrix = new int[_input.Length - 1, dim];
        for (int i = 0; i < _input.Length - 1; i++)
        {
            int[] row = _input[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            for (int j = 0; j < row.Length; j++)
            {
                matrix[i, j] = row[j];
            }
        }

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        int[,] pivoted = new int[cols, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                pivoted[j, i] = matrix[i, j];
            }
        }

        List<long> totals = new List<long>();
        for (int i = 0; i < cols; i++)
        {
            char symbol = symbols[i % symbols.Length];

            List<long> values = new List<long>();
            for (int j = 0; j < rows; j++)
            {
                values.Add(pivoted[i, j]);
            }

            if (symbol == '+')
            {
                totals.Add(values.Sum());
            }
            else if (symbol == '*')
            {
                long product = values.Aggregate(1L, (acc, x) => acc * x);
                totals.Add(product);
            }
        }

        long grandTotal = totals.Sum();

        return grandTotal.ToString();
    }

    public string Part2()
    {
        // Normalize lines and pad to equal width
        var rawLines = _worksheet.Replace("\r", "").Split('\n');

        int rows = rawLines.Length;
        int width = rawLines.Max(l => l.Length);
        var lines = new string[rows];
        for (int r = 0; r < rows; r++)
        {
            lines[r] = rawLines[r].PadRight(width, ' ');
        }

        // Identify contiguous groups of columns that contain any non-space character
        var nonEmpty = new bool[width];
        for (int c = 0; c < width; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if (lines[r][c] != ' ')
                {
                    nonEmpty[c] = true;
                    break;
                }
            }
        }

        var groups = new List<(int start, int end)>();
        int idx = 0;
        while (idx < width)
        {
            if (!nonEmpty[idx]) { idx++; continue; }
            int start = idx;
            while (idx < width && nonEmpty[idx]) idx++;
            int end = idx - 1;
            groups.Add((start, end));
        }

        BigInteger grandTotal = BigInteger.Zero;

        foreach (var (start, end) in groups)
        {
            char op = '\0';
            for (int c = start; c <= end; c++)
            {
                char ch = lines[rows - 1][c];
                if (ch != ' ')
                {
                    op = ch;
                    break;
                }
            }
            if (op == '\0')
            {
                continue;
            }

            // Collect numbers: for each column in the group, read top-to-bottom (excluding bottom operator row)
            var numbers = new List<BigInteger>();
            for (int c = start; c <= end; c++)
            {
                var sb = new StringBuilder();
                for (int r = 0; r < rows - 1; r++) // exclude bottom operator row
                {
                    char ch = lines[r][c];
                    if (ch != ' ')
                    {
                        // Accept digits only; ignore any other characters
                        if (char.IsDigit(ch))
                            sb.Append(ch);
                    }
                }
                
                if (sb.Length > 0)
                {
                    numbers.Add(BigInteger.Parse(sb.ToString()));
                }
            }

            if (numbers.Count == 0) continue;

            // Evaluate the problem using the operator
            BigInteger result = 0;
            if (op == '+')
            {
                result = BigInteger.Zero;
                foreach (var n in numbers) result += n;
            } 
            
            if (op == '*')
            {
                result = BigInteger.One;
                foreach (var n in numbers) result *= n;
            }

            grandTotal += result;
        }

        return grandTotal.ToString();
    }
}