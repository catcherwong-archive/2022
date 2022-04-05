using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransOut", (HttpContext httpContext, TransRequest req) => 
{
    Console.WriteLine($"TransOut, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.UserId}, transfer out {req.Amount} --- forward");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutCompensate", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine($"TransOutCompensate, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.UserId}, transfer out {req.Amount} --- reverse compensation");
    
    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10000");
