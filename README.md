# 📝 LoggerApp

Um **logger assíncrono** leve e thread-safe em **C#**, implementado com o padrão Singleton. Possui suporte a `traceKey`, múltiplos níveis de log e identificação do serviço de origem.

A lightweight and thread-safe **asynchronous logger** in **C#**, using the Singleton pattern. Supports `traceKey`, multiple log levels, and service source identification.

---

## 🌍 Idiomas | Languages

- 🇧🇷 [Português](#-funcionalidades)
- 🇺🇸 [English](#-features)

---

## ✅ Funcionalidades

- Log assíncrono com `SemaphoreSlim`
- Singleton: única instância de logger em toda a aplicação
- Suporte a múltiplos níveis: `Debug`, `Info`, `Warning`, `Error`
- Registro com `traceKey` para rastreamento
- Identificação do serviço de origem (`source`) e endereço HTTP
- Escrita em arquivo com nome baseado na data (`log_YYYY-MM-DD.log`)
- 📁 Suporte a rotação de log (backup automático quando arquivo atinge tamanho limite)
- 🖥️ Suporte opcional a saída no console

---

## 💻 Como usar

```csharp
using System;
using System.Threading.Tasks;

class Program
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
            logFilePath: "app.log",
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

```

## ✅ Features

- Asynchronous logging using `SemaphoreSlim`  
- Singleton: only one logger instance throughout the application  
- Supports multiple log levels: `Debug`, `Info`, `Warning`, `Error`  
- Logging with `traceKey` for request tracing  
- Identification of the service source (`source`) and HTTP address  
- File writing with date-based filename (`log_YYYY-MM-DD.log`)  
- 📁 Log rotation support (automatic backup when file reaches size limit)  
- 🖥️ Optional support for console output  


---

## 💻 How to Use

```csharp
using System;
using System.Threading.Tasks;

class Program
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
            logFilePath: "app.log",
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
```