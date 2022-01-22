using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransOut", (string branch_id, string gid, string op, TransRequest req) => 
{
    // 进行 数据库操作
    Console.WriteLine($"用户【{req.UserId}】转出【{req.Amount}】正向操作，gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutCompensate", (string branch_id, string gid, string op, TransRequest req) =>
{
    // 进行 数据库操作
    Console.WriteLine($"用户【{req.UserId}】转出【{req.Amount}】补偿操作，gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10000");
