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

// await Case1(dtmClient);

// await Case2(dtmClient);

await Case3(dtmClient);

// await Case4(dtmClient);


Console.ReadKey();

// /// <summary>
// /// All forward operations are successful, the global transaction succeeds.
// /// </summary>
// async Task Case1(IDtmClient dtmClient)
// {
//    var cts = new CancellationTokenSource();

//    var gid = await dtmClient.GenGid(cts.Token);
//    var saga = new Saga(dtmClient, gid)
//        .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
//        .Add(inApi + "/TransIn", inApi + "/TransInCompensate", userInReq)
//        .EnableWaitResult()
//        ;

//    await saga.Submit(cts.Token);

//    Console.WriteLine($"Submit SAGA transaction, gid is {gid}.");
// }

// /// <summary>
// /// transfer out succeed, transfer in fail, transfer in compensate, transfer out compensate, the global transaction fails.
// /// </summary>
// async Task Case2(IDtmClient dtmClient)
// {
//    var ct = new CancellationToken();

//    var gid = await dtmClient.GenGid(ct);
//    var saga = new Saga(dtmClient, gid)
//        .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
//        .Add(inApi + "/TransInError", inApi + "/TransInCompensate", userInReq)
//        .EnableWaitResult()
//        ;

//    await saga.Submit(ct);

//    Console.WriteLine($"Submit SAGA transaction, gid is {gid}.");
// }


/// <summary>
/// transfer out succeed, transfer in fail, transfer in compensate(sub-transaction barrier), transfer out compensate, the global transaction fails.
/// </summary>
async Task Case3(IDtmClient dtmClient)
{
   var ct = new CancellationToken();

   var gid = await dtmClient.GenGid(ct);
   var saga = new Saga(dtmClient, gid)
       .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
       .Add(inApi + "/TransInError", inApi + "/BarrierTransInCompensate", userInReq)
       .EnableWaitResult()
       ;

   await saga.Submit(ct);

   Console.WriteLine($"Submit SAGA transaction, gid is {gid}.");
}


// /// <summary>
// /// transfer out succeed, transfer in with retry the global transaction succeeds.
// /// </summary>
// async Task Case4(IDtmClient dtmClient)
// {
//     var ct = new CancellationToken();

//     var gid = await dtmClient.GenGid(ct);
//     var saga = new Saga(dtmClient, gid)
//         .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
//         .Add(inApi + "/BarrierTransIn", inApi + "/BarrierTransInCompensate", userInReq)
//         ;

//     await saga.Submit(ct);

//     Console.WriteLine($"Submit SAGA transaction, gid is {gid}.");
// }