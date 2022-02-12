using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x => 
{
    x.DtmUrl = "http://localhost:36789";
});

// 指定 子事务屏障 数据库类型为 sql server
Dtmcli.DtmImp.DbSpecialDelegate.Instance.SetCurrentDBType("sqlserver");

// 指定 子事务屏障表
Dtmcli.BranchBarrier.SetBarrierTableName("[test].[dbo].[barrier]");

var app = builder.Build();

app.MapPost("/api/TransIn", (string branch_id, string gid, string op, TransRequest req) =>
{
    Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】正向操作，gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInCompensate", (string branch_id, string gid, string op, TransRequest req) =>
{
    Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】补偿操作，gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInError", (string branch_id, string gid, string op, TransRequest req) =>
{
    Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】正向操作--失败，gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildFailureResponse());
    //return Results.BadRequest();
});

int _errCount = 0;

app.MapPost("/api/BarrierTransIn", async (string branch_id, string gid, string op, string trans_type, TransRequest req, IBranchBarrierFactory factory) =>
{
    Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】请求来了！！！ gid={gid}, branch_id={branch_id}, op={op}");

    var barrier = factory.CreateBranchBarrier(trans_type, gid, branch_id, op);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {
        var c = Interlocked.Increment(ref _errCount);

        if (c > 0 && c < 2)
        {
            // 模拟一个超时执行
            await Task.Delay(10000);
        }

        Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】正向操作，gid={gid}, branch_id={branch_id}, op={op}");
        // tx 参数是事务，可和本地事务一起提交回滚
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});


app.MapPost("/api/BarrierTransInCompensate", async (string branch_id, string gid, string op, string trans_type, TransRequest req, IBranchBarrierFactory factory) =>
{
    var barrier = factory.CreateBranchBarrier(trans_type, gid, branch_id, op);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {
        // 转入失败的情况下，不应该输出下面这个
        Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】补偿操作，gid={gid}, branch_id={branch_id}, op={op}");
        // tx 参数是事务，可和本地事务一起提交回滚
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10001");
