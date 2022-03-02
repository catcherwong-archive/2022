using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x => 
{
    x.DtmUrl = "http://localhost:36789";

    // ָ�� ���������� ���ݿ�����Ϊ sql server
    x.DBType = DtmCommon.Constant.Barrier.DBTYPE_SQLSERVER;

    // ָ�� ���������ϱ�
    x.BarrierTableName = "[test].[dbo].[barrier]";
});

var app = builder.Build();

app.MapPost("/api/TransIn", (string branch_id, string gid, string op, TransRequest req) =>
{
    Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}�����������gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInCompensate", (string branch_id, string gid, string op, TransRequest req) =>
{
    Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}������������gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInError", (string branch_id, string gid, string op, TransRequest req) =>
{
    Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}���������--ʧ�ܣ�gid={gid}, branch_id={branch_id}, op={op}");

    return Results.Ok(TransResponse.BuildFailureResponse());
});

int _errCount = 0;

app.MapPost("/api/BarrierTransIn", async (string branch_id, string gid, string op, string trans_type, TransRequest req, IBranchBarrierFactory factory) =>
{
    Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}���������ˣ����� gid={gid}, branch_id={branch_id}, op={op}");

    var barrier = factory.CreateBranchBarrier(trans_type, gid, branch_id, op);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {
        var c = Interlocked.Increment(ref _errCount);

        if (c > 0 && c < 2)
        {
            // ģ��һ����ʱִ��
            await Task.Delay(10000);
        }

        Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}�����������gid={gid}, branch_id={branch_id}, op={op}");
        // tx ���������񣬿ɺͱ�������һ���ύ�ع�
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
        // ת��ʧ�ܵ�����£���Ӧ������������
        Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}������������gid={gid}, branch_id={branch_id}, op={op}");
        // tx ���������񣬿ɺͱ�������һ���ύ�ع�
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10001");
