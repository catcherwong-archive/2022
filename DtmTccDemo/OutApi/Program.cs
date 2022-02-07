using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransOutTry", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) => 
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"用户【{req.UserId}】转出【{req.Amount}】Try 操作，bb={bb}");
        // tx 参数是事务，可和本地事务一起提交回滚
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutConfirm", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"用户【{req.UserId}】转出【{req.Amount}】Confirm操作，bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutCancel", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"用户【{req.UserId}】转出【{req.Amount}】Cancel操作，bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10000");
