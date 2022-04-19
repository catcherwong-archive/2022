using StackExchange.Redis;


var str = "localhost";

var connectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(str));

var prefix = new string[] { "foo", "bar", "cat", "dog", "apple", "king", "lib", "jet", "aot", "jit", "net", "redis", "cass", "mysql" };

AddString(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(10000, 50000));
AddList(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(5000, 10000));
AddSet(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(5000, 10000));
AddZSet(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(5000, 10000));
AddHash(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(10000, 30000));
AddStream(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(10000, 30000));


//AddString(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(5000, 25000));
//AddList(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(500, 1000));
//AddSet(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(500, 1000));
//AddZSet(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(500, 1000));
//AddHash(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(1000, 3000));
//AddStream(connectionMultiplexer.GetDatabase(new Random().Next(0, 3)), prefix, new Random().Next(1000, 3000));

static void AddString(IDatabase database, string[] prefix, int num)
{
    Parallel.For(0, num, i =>
    {
        var key = $"{prefix[new Random().Next(0, prefix.Length - 1)]}:{Guid.NewGuid().ToString("N")}";

        var rate = new Random().NextDouble();

        if (rate > 0.5d)
        {
            var time = TimeSpan.FromMinutes(new Random().Next(10, 500));

            database.StringSet($"s{key}", GetString(new Random().Next(3, 8)), time);
        }
        else
        {
            database.StringSet($"s{key}", GetString(new Random().Next(3, 8)));
        }
    });

    Console.WriteLine($"done {num} string");
}

static void AddList(IDatabase database, string[] prefix, int num)
{
    Parallel.For(0, num, i =>
    {
        var key = $"{prefix[new Random().Next(0, prefix.Length - 1)]}:{Guid.NewGuid().ToString("N")}";
        Parallel.For(0, new Random().Next(100, 500), j =>
        {
            database.ListLeftPush($"l{key}", GetString(new Random().Next(3, 8)));
        });
    });

    Console.WriteLine($"done list with {num} item");
}

static void AddSet(IDatabase database, string[] prefix, int num)
{
    Parallel.For(0, num, i =>
    {
        var key = $"{prefix[new Random().Next(0, prefix.Length - 1)]}:{Guid.NewGuid().ToString("N")}";

        Parallel.For(0, new Random().Next(100, 500), j =>
        {
            database.SetAdd($"s{key}", GetString(new Random().Next(3, 8)));
        });
    });

    Console.WriteLine($"done set with {num} item");
}

static void AddZSet(IDatabase database, string[] prefix, int num)
{
    Parallel.For(0, num, i =>
    {
        var key = $"{prefix[new Random().Next(0, prefix.Length - 1)]}:{Guid.NewGuid().ToString("N")}";

        Parallel.For(0, new Random().Next(100, 500), j =>
        {
            database.SortedSetAdd($"z{key}", GetString(new Random().Next(3, 8)), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        });

    });

    Console.WriteLine($"done zset with {num} item");
}

static void AddHash(IDatabase database, string[] prefix, int num)
{
    Parallel.For(0, num, i =>
    {
        var key = $"{prefix[new Random().Next(0, prefix.Length - 1)]}:{Guid.NewGuid().ToString("N")}";

        Parallel.For(0, new Random().Next(100, 500), j => 
        {
            database.HashSet($"h{key}", GetString(new Random().Next(3, 8)), GetString(new Random().Next(3, 8)));
        });
    });

    Console.WriteLine($"done {num} hash");
}

static void AddStream(IDatabase database, string[] prefix, int num)
{
    Parallel.For(0, num, i =>
    {
        var key = $"{prefix[new Random().Next(0, prefix.Length - 1)]}:{Guid.NewGuid().ToString("N")}";

        Parallel.For(0, new Random().Next(100, 1000), j =>
        {
            database.StreamAdd($"ss{key}", GetString(new Random().Next(3, 8)), GetString(new Random().Next(3, 8)));
        });
    });

    Console.WriteLine($"done {num} stream");
}

static string GetString(int num)
{
    var sb = new System.Text.StringBuilder();
    for (int i = 0; i < num; i++)
    {
        sb.Append(Guid.NewGuid().ToString());
    }

    return sb.ToString();
}

