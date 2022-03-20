using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var root = new RootCommand
        {
            new SumCommand(),
            new LengthCommand(),
            new PowCommand(),

        };

        root.Description = "cli-sample is a cli sample";

        var parser = new CommandLineBuilder(root)
            .UseHelp()
            .UseVersionOption(new[] { "-v", "--version" })
            .UseSuggestDirective()
            .UseParseErrorReporting()
            .UseExceptionHandler()
            .Build();

        if (args.Length == 0)
        {
            var helpBld = new HelpBuilder(LocalizationResources.Instance, Console.WindowWidth);
            helpBld.Write(root, Console.Out);
            return 0;
        }

        return await parser.InvokeAsync(args);
    }
}
