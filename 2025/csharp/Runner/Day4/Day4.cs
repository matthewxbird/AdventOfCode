using Common;

namespace Day4;

public class Day4 : IDay
{
    private string[] input = File.ReadAllLines("input.txt");
    
    public string Part1()
    {
        int rows = input.Length;
        int cols = input[0].Length;

        int accessibleRolls = 0;
        
        var directions = new List<(int dr, int dc)>
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                //Don't care not a roll
                if(input[r][c] != '@')
                    continue;

                int neighbours = 0;

                foreach ((int dr, int dc)  in directions)
                {
                    int nr = r + dr;
                    int nc = c + dc;
                    
                    //Make sure we're within bounds...
                    if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
                    {
                        if (input[nr][nc] == '@')
                            neighbours++;
                    }
                }
                
                if(neighbours < 4)
                {
                    accessibleRolls++;
                }
            }
        }
        
        return accessibleRolls.ToString();
    }

    public string Part2()
    {
        int rows = input.Length;
        int cols = input[0].Length;

        int lastTotalHaveRemoved = -1;
        int totalHaveRemoved = 0;
        
        var directions = new List<(int dr, int dc)>
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        };

        while (totalHaveRemoved > lastTotalHaveRemoved)
        {
            lastTotalHaveRemoved = totalHaveRemoved;
            var toBeReplaced = new List<(int x, int y)>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    //Don't care not a roll
                    if (input[r][c] != '@')
                        continue;

                    int neighbours = 0;

                    foreach ((int dr, int dc) in directions)
                    {
                        int nr = r + dr;
                        int nc = c + dc;

                        //Make sure we're within bounds...
                        if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
                        {
                            if (input[nr][nc] == '@')
                            {
                                neighbours++;

                            }
                        }
                    }

                    if (neighbours < 4)
                    {
                        toBeReplaced.Add((r, c));
                    }
                }
            }
            
            foreach (var (row, col) in toBeReplaced)
            {
                char[] chars = input[row].ToCharArray();
                chars[col] = '.';
                input[row] = new string(chars);
                totalHaveRemoved++;
            }
        }

        return totalHaveRemoved.ToString();
    }
}