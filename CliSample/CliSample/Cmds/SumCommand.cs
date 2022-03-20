using System.CommandLine;

public class SumCommand : Command
{
    public SumCommand()
        : base("sum", "Get sum of input numbers")
    {
        var arg = new Argument<List<int>>("numbers", "The numbers you want to get the sum");
                
        AddArgument(arg);

        this.SetHandler((List<int> x) =>
        {
            Add(x);
        }, arg);
    }

    public void Add(List<int> x)
    {
        int res = 0;
        foreach (int i in x)
        { 
            res = res + i;
        }

        Console.WriteLine(res);
    }
}
