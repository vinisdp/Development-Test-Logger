using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string traceKey = Guid.NewGuid().ToString();
        string source = "LoggerApp";
        string httpAddress = "http://localhost:5000";
        bool logToConsole = false;

        Logger.Instance.Configure(
            source: source,
            httpAddress: httpAddress,
            logFilePath: ".log",
            minLevel: LogLevel.Debug,
            logToConsole: logToConsole
        );

        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            int taskId = i;
            tasks.Add(Task.Run(async () =>
            {
                string localTraceKey = Guid.NewGuid().ToString();
                await Logger.Instance.Debug($"[Task {taskId}] Debug log", localTraceKey);
                await Logger.Instance.Info($"[Task {taskId}] Info log", localTraceKey);
                await Logger.Instance.Warning($"[Task {taskId}] Warning log", localTraceKey);
                await Logger.Instance.Error($"[Task {taskId}] Error log", localTraceKey);
            }));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine("✅ Todos os logs foram registrados de forma assíncrona.");
    }
}
