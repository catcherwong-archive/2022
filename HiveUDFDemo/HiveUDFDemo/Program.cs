using System.Security.Cryptography;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var p = "raw";
        if (args.Length > 0)
        { 
            p = args[0];
        }

        string line;
        try
        {
            while ((line = Console.ReadLine()) != null)
            {
                line = line.TrimEnd('\n');

                Handle(line, p);
            }
        }
        catch (Exception ex)
        {
            //bad format or end of line so do nothing
        }
    }

    private static void Handle(string input, string type)
    {
        if (type.Equals("raw", StringComparison.OrdinalIgnoreCase))
        {
            RawHandle(input);
        }
        else if (type.Equals("md5", StringComparison.OrdinalIgnoreCase))
        {
            Md5Handle(input);
        }
        else if (type.Equals("pre", StringComparison.OrdinalIgnoreCase))
        {
            AddPrefixHandle(input);
        }
        else
        {
            RawHandle(input);
        }
    }

    public static void AddPrefixHandle(string input)
    {
        var field = input.Split('\t');

        var builder = new StringBuilder(512);

        for (int i = 0; i < field.Length; i++)
        {
            builder.Append($"pre-{field[i]}\t");
        }

        Console.WriteLine(builder.ToString().TrimEnd('\t'));
    }

    public static void Md5Handle(string input)
    {
        var field = input.Split('\t');

        var builder = new StringBuilder(512);

        for (int i = 0; i < field.Length; i++)
        {
            builder.Append($"{GetMD5Hash(field[i])}\t");
        }

        Console.WriteLine(builder.ToString().TrimEnd('\t'));
    }

    public static void RawHandle(string input)
    {
        var field = input.Split('\t');

        var builder = new StringBuilder(512);

        for (int i = 0; i < field.Length; i++)
        {
            builder.Append($"{field[i]}\t");
        }

        Console.WriteLine(builder.ToString().TrimEnd('\t'));        
    }

    private static string GetMD5Hash(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder(64);
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
}
