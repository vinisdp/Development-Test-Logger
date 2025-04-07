using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string traceKey = Guid.NewGuid().ToString(); // Gerando um traceKey único
        string source = "LoggerApp"; // Nome do serviço ou aplicação
        string httpAddress = "http://localhost:5000"; // Endereço HTTP do serviço
        bool logToConsole = false;

        // Configura o singleton
        Logger.Instance.Configure(
            source: source,
            httpAddress: httpAddress,
            logFilePath: "app.log",
            minLevel: LogLevel.Debug,
            logToConsole: logToConsole
        );

        // Criar várias tarefas concorrentes de logging
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

        // Aguardar todas as tarefas terminarem
        await Task.WhenAll(tasks);

        Console.WriteLine("✅ Todos os logs foram registrados de forma assíncrona.");
    }
}
