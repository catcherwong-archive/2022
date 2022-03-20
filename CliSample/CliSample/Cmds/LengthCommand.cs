using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;

public class LengthCommand : Command
{
    public LengthCommand()
        : base("len", "Get length of input string")
    {
        var arg = new Argument<string>("string", "The input string you want to get the length");

        AddArgument(arg);

        this.SetHandler((InvocationContext context) =>
        {
            var str = context.ParseResult.GetValueForArgument<string>(arg);

            var res = GetLength(str);

            context.Console.WriteLine($"The result is {res}");
        });
    }

    private int GetLength(string str)
        => str.Length;
}