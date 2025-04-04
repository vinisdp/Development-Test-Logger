# 📝 LoggerApp

Um logger assíncrono simples e extensível em C#, com suporte a múltiplos níveis de log (`Debug`, `Info`, `Warning`, `Error`), escrita em arquivo e registro de informações do sistema.

## 🚀 Funcionalidades

- ✅ Log assíncrono com `SemaphoreSlim` para escrita segura
- ✅ Suporte a múltiplos níveis de log
- ✅ Registro de exceções com stack trace
- ✅ Definição de nível mínimo de log
- ✅ Captura de informações do sistema (SO, arquitetura, framework)

## 💻 Como usar

```csharp
var logger = new Logger("app.log");

// Exemplo de uso
await logger.Info("Aplicação iniciada");
await logger.Warning("Aviso de uso elevado de memória");
await logger.Error(new Exception("Erro de teste"));
await logger.LogSystemInfo(); // Loga infos do sistema
