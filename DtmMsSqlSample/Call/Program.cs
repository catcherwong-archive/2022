using Dtmcli;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var provider = services.BuildServiceProvider();
var dtmClient = provider.GetRequiredService<IDtmClient>();

var outApi = "http://localhost:10000/api";
var inApi = "http://localhost:10001/api";

var userOutReq = new Common.TransRequest() { UserId = "1", Amount = -30 };
var userInReq = new Common.TransRequest() { UserId = "2", Amount = 30 };

//await Case3(dtmClient);

await Case4(dtmClient);

Console.ReadKey();


/// <summary>
/// 转出成功，转入失败，转入补偿(Barrier)，转出补偿，SAGA事务失败
/// </summary>
//async Task Case3(IDtmClient dtmClient)
//{
//    var ct = new CancellationToken();

//    var gid = await dtmClient.GenGid(ct);
//    var saga = new Saga(dtmClient, gid)
//        .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
//        .Add(inApi + "/TransInError", inApi + "/BarrierTransInCompensate", userInReq)
//        ;

//    var flag = await saga.Submit(ct);

//    Console.WriteLine($"case3, {gid} saga 提交结果 = {flag}");
//}


/// <summary>
/// 转出成功，转入成功+重试，SAGA事务成功
/// </summary>
async Task Case4(IDtmClient dtmClient)
{
    var ct = new CancellationToken();

    var gid = await dtmClient.GenGid(ct);
    var saga = new Saga(dtmClient, gid)
        .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
        .Add(inApi + "/BarrierTransIn", inApi + "/BarrierTransInCompensate", userInReq)
        ;

    var flag = await saga.Submit(ct);

    Console.WriteLine($"case4, {gid} saga 提交结果 = {flag}");
}