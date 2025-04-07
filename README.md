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
- Identificação do serviço de origem (`source`)
- Escrita em arquivo com nome baseado na data (`log_YYYY-MM-DD.txt`)

---

## 💻 Como usar

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var traceKey = Guid.NewGuid().ToString();

        Logger.Instance.Configure(source: "MinhaAplicacao", httpAddress: "http://localhost");

        await Logger.Instance.Info("Aplicação iniciada", traceKey);
        await Logger.Instance.Warning("Aviso: consumo elevado", traceKey);
        await Logger.Instance.Error("Erro crítico detectado", traceKey);

        try
        {
            int x = 0;
            int y = 10 / x;
        }
        catch (Exception ex)
        {
            await Logger.Instance.Error(ex, traceKey);
        }
    }
}

```

## ✅ Features

- Asynchronous logging using `SemaphoreSlim`
- Singleton: only one logger instance throughout the application
- Supports multiple levels: `Debug`, `Info`, `Warning`, `Error`
- Logging with `traceKey` for request tracing
- Identification of the `source` (originating service) and `httpAddress`
- File writing with date-based log filename (`log_YYYY-MM-DD.txt`)

---

## 💻 How to Use

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var traceKey = Guid.NewGuid().ToString();

        Logger.Instance.Configure(source: "MyApplication", httpAddress: "http://localhost");

        await Logger.Instance.Info("Application started", traceKey);
        await Logger.Instance.Warning("Warning: high resource usage", traceKey);
        await Logger.Instance.Error("Critical error detected", traceKey);

        try
        {
            int x = 0;
            int y = 10 / x;
        }
        catch (Exception ex)
        {
            await Logger.Instance.Error(ex, traceKey);
        }
    }
}
```