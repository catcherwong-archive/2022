using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;

public class PowCommand : Command
{
    public PowCommand()
        : base("pow", "Get a number raised to the specified power.")
    {
        var numOptioin = new Option<double>(new string[] { "-n", "--num" }, "The number")
        {
            IsRequired = true,
        };

        var powOption = new Option<double>(new string[] { "-p", "--pow" }, "The power")
        { 
            IsRequired = true
        };

        AddOption(numOptioin);
        AddOption(powOption);

        this.SetHandler((InvocationContext context) =>
        {
            var num1 = context.ParseResult.GetValueForOption<double>(numOptioin);
            var num2 = context.ParseResult.GetValueForOption<double>(powOption);

            var res = GetPow(num1, num2);

            context.Console.WriteLine($"The result is {res}");
        });
    }

    private double GetPow(double x, double y)
    {
        return Math.Pow(x, y);
    }
}