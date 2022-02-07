using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x => 
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransInTry", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】Try操作，bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInConfirm", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】Confirm操作，bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInTryError", (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】Try--失败，bb={bb}");

    return Results.Ok(TransResponse.BuildFailureResponse());
});

app.MapPost("/api/TransInCancel", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"用户【{req.UserId}】转入【{req.Amount}】Cancel操作，bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10001");
