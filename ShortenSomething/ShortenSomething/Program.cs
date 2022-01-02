using ShortenSomething;

var url = "https://github.com/catcherwong";

Console.WriteLine("============ToBase62Util===============");
Console.WriteLine(ToBase62Util.ConvertToBase62(int.MaxValue));
Console.WriteLine(ToBase62Util.ConvertToBase62(long.MaxValue));
Console.WriteLine("============ToBase62Util===============\n");

Console.WriteLine("============GetMurmurHash===============");
Console.WriteLine(MurmurhashUtil.GetMurmurHash(url));
Console.WriteLine(url.GetMurmurHash().ConvertToBase62());
Console.WriteLine("============GetMurmurHash===============\n");

Console.WriteLine("============WebiBoAlgo.GetShortCodes===============");
var codes = WebiBoAlgo.GetShortCodes(url);
foreach (var item in codes)
{
    Console.WriteLine(item);
}
Console.WriteLine("============WebiBoAlgo.GetShortCodes===============\n");

Console.ReadKey();
