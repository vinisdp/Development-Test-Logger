# 📝 LoggerApp

Um **logger assíncrono** leve e extensível em **C#**, com suporte a `traceKey`, múltiplos níveis de log e identificação da origem do serviço.

A lightweight and extensible **asynchronous logger** in **C#**, with support for `traceKey`, multiple log levels, and service source identification.

---

## 🌍 Idiomas | Languages

- 🇧🇷 [Português](#-funcionalidades)
- 🇺🇸 [English](#-features)

---

## ✅ Funcionalidades

- Log assíncrono com `SemaphoreSlim`
- Suporte a múltiplos níveis: `Debug`, `Info`, `Warning`, `Error`
- Registro com `traceKey` para rastreamento de requisições
- Identificação do `source` (serviço de origem)
- Escrita em arquivo com nome baseado na data

---

## 💻 Como usar

```csharp
var traceKey = Guid.NewGuid().ToString();
var logger = new Logger(source: "MinhaAplicacao");

await logger.Info("Aplicação iniciada", traceKey);
await logger.Warning("Aviso: consumo elevado", traceKey);
await logger.Error("Erro grave detectado", traceKey);

// Log de exceção
try
{
    int x = 0;
    int y = 10 / x;
}
catch (Exception ex)
{
    await logger.Error(ex, traceKey);
}


## ✅ Features

- Asynchronous logging using `SemaphoreSlim`
- Supports multiple levels: `Debug`, `Info`, `Warning`, `Error`
- Logging with `traceKey` for request tracing
- Identification of the `source` (originating service)
- File writing with date-based log filename

---

## 💻 How to Use

```csharp
var traceKey = Guid.NewGuid().ToString();
var logger = new Logger(source: "MyApplication");

await logger.Info("Application started", traceKey);
await logger.Warning("Warning: high resource usage", traceKey);
await logger.Error("Critical error detected", traceKey);

// Exception logging
try
{
    int x = 0;
    int y = 10 / x;
}
catch (Exception ex)
{
    await logger.Error(ex, traceKey);
}
