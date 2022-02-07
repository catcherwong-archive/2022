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
var tccGlobalTransaction = provider.GetRequiredService<TccGlobalTransaction>();

var outApi = "http://localhost:10000/api";
var inApi = "http://localhost:10001/api";

var userOutReq = new Common.TransRequest() { UserId = "1", Amount = -30 };
var userInReq = new Common.TransRequest() { UserId = "2", Amount = 30 };

//await Case1(dtmClient, tccGlobalTransaction);

//await Task.Delay(3000);

await Case2(dtmClient, tccGlobalTransaction);

Console.ReadKey();

/// <summary>
/// TCC事务成功
/// </summary>
//async Task Case1(IDtmClient dtmClient, TccGlobalTransaction tccGlobalTransaction)
//{
//    var cts = new CancellationTokenSource();

//    var gid = await dtmClient.GenGid(cts.Token);

//    var res = await tccGlobalTransaction.Excecute(gid, async (tcc) =>
//    {
//        // 用户1 转出30元
//        var res1 = await tcc.CallBranch(userOutReq, outApi + "/TransOutTry", outApi + "/TransOutConfirm", outApi + "/TransOutCancel", cts.Token);

//        // 用户2 转入30元
//        var res2 = await tcc.CallBranch(userInReq, inApi + "/TransInTry", inApi + "/TransInConfirm", inApi + "/TransInCancel", cts.Token);

//        Console.WriteLine($"case1, branch-out-res= {res1} branch-in-res= {res2}");
//    }, cts.Token);

//    Console.WriteLine($"case1, {gid} tcc 提交结果 = {res}");
//}

/// <summary>
/// TCC事务回滚
/// </summary>
async Task Case2(IDtmClient dtmClient, TccGlobalTransaction tccGlobalTransaction)
{
    var cts = new CancellationTokenSource();

    var gid = await dtmClient.GenGid(cts.Token);

    var res = await tccGlobalTransaction.Excecute(gid, async (tcc) =>
    {
        var res1 = await tcc.CallBranch(userOutReq, outApi + "/TransOutTry", outApi + "/TransOutConfirm", outApi + "/TransOutCancel", cts.Token);
        var res2 = await tcc.CallBranch(userInReq, inApi + "/TransInTryError", inApi + "/TransInConfirm", inApi + "/TransInCancel", cts.Token);

        Console.WriteLine($"case2, branch-out-res= {res1} branch-in-res= {res2}");
    }, cts.Token);

    Console.WriteLine($"case2, {gid} tcc 提交结果 = {res}");
}
