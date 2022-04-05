using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x => 
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransIn", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine($"TransIn, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.UserId}, transfer in {req.Amount} --- forward");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInCompensate", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine($"TransInCompensate, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.UserId}, transfer out {req.Amount} --- reverse compensation");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInError", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine($"TransIn, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.UserId}, transfer in {req.Amount} --- forward error!");

    // status code = 409 || content contains FAILURE
    // return Ok(TransResponse.BuildFailureResponse());
    return Results.StatusCode(409);
});

int _errCount = 0;

app.MapPost("/api/BarrierTransIn", async (HttpContext httpContext, TransRequest req, IBranchBarrierFactory factory) =>
{
    Console.WriteLine($"TransIn, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.UserId}, transfer in {req.Amount} --- forward");

    var barrier = factory.CreateBranchBarrier(httpContext.Request.Query);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {
        var c = Interlocked.Increment(ref _errCount);

        if (c > 0 && c < 2)
        {
            // simulate network latency
            // the first request should timeout
            await Task.Delay(10000);
        }

        Console.WriteLine($"User: {req.UserId}, transfer in {req.Amount} --- sub-transaction handle!");
        
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/BarrierTransInCompensate", async (HttpContext httpContext, TransRequest req, IBranchBarrierFactory factory) =>
{
    Console.WriteLine($"TransIn, QueryString={httpContext.Request.QueryString}");

    // create barrier from query
    var barrier = factory.CreateBranchBarrier(httpContext.Request.Query);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {
        // some exception occure, should not output this one.
        Console.WriteLine($"User: {req.UserId}, transfer in {req.Amount} --- sub-transaction handle!");

        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10001");
