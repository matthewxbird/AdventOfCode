using Common;

namespace Day6;

public class Day6 : IDay
{
    private string[] _input = File.ReadAllLines("input.txt");
    public string Part1()
    {
        string[] symbolInputs = _input.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        char[] symbols = symbolInputs.Select(s => s[0]).ToArray();

        var dim = _input[0].Trim().Split().Length -1;
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
            }else if (symbol == '*'){
                long product = values.Aggregate(1L, (acc, x) => acc * x);
                totals.Add(product);
            }
        }

        long grandTotal = totals.Sum();

        return grandTotal.ToString();
    }

    public string Part2()
    {
        throw new NotImplementedException();
    }
}