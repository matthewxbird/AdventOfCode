namespace Host;

public static class Program
{
    private static void Main(string[] args)
    {
        var day = new Day2.Day2();
        var result1 = day.Part1();
        Console.WriteLine("Part1: " + result1);
        
        var result2 = day.Part2();
        Console.WriteLine("Part2: " + result2);
        Console.ReadKey();
    }
}