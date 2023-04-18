using System.Net.Sockets;
using Serilog;
using Serilog.Sinks.Fluentd;

Console.WriteLine("Hello, World!");

Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Fluentd(new FluentdSinkOptions("localhost", 24224, "portal.test")
        {
            LingerEnabled = false
        })
        .WriteTo.Console()
        .CreateLogger();


for (var x = 0; x < 10; x++)
{
    // test log big item
    Log.Information(
        "heya Test");

    Thread.Sleep(100);
}

Log.CloseAndFlush();