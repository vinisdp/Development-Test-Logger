# 📝 LoggerApp

Um **logger assíncrono** simples e extensível em **C#**, com suporte a múltiplos níveis de log, escrita em arquivo e informações úteis sobre o ambiente da aplicação.

---

## 🚀 Funcionalidades

- ✅ **Log assíncrono** com controle de concorrência usando `SemaphoreSlim`
- ✅ Suporte a múltiplos **níveis de log**: `Debug`, `Info`, `Warning`, `Error`
- ✅ Registro de **exceções** com stack trace
- ✅ Definição de **nível mínimo de log**
- ✅ Captura de informações do sistema:
  - Sistema operacional
  - Nome do usuário
  - Nome da máquina
  - Endereço IP local
- ✅ Suporte a `traceKey` para rastreamento entre serviços
- ✅ Identificação do **source** (serviço de origem) do log
- ✅ Informações de chamada: nome do método, arquivo e linha

---

## 💻 Como usar

```csharp
var traceKey = Guid.NewGuid().ToString();
var logger = new Logger(source: "MinhaAplicacao", logFilePath: ".log", minLevel: LogLevel.Info);

// Exemplos de log
await logger.Info("Aplicação iniciada", traceKey);
await logger.Warning("Aviso: uso elevado de memória", traceKey);
await logger.Error("Erro crítico detectado", traceKey);

// Tratamento de exceção
try
{
    int x = 0;
    int y = 10 / x;
}
catch (Exception ex)
{
    await logger.Error(ex, traceKey);
}
