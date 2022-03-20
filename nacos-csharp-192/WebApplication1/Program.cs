var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var nacosConfig = builder.Configuration.GetSection("nacos");
builder.Configuration.AddNacosV2Configuration(nacosConfig, Nacos.YamlParser.YamlConfigurationStringParser.Instance);
Console.WriteLine(string.Join("\n", builder.Configuration.AsEnumerable().Where(c => c.Key.StartsWith("datasource", StringComparison.OrdinalIgnoreCase)).Select(p => p.Key + "=" + p.Value)));

app.MapGet("/", () =>
{
    return "ok";
});

app.Run("http://*:9090");
