using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

public class Logger
{
    private readonly string _logFilePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private LogLevel _minLevel;

    public Logger(string logFilePath = "log.txt", LogLevel minLevel = LogLevel.Debug)
    {
        _logFilePath = logFilePath;
        _minLevel = minLevel;
    }

    public void SetMinimumLevel(LogLevel level)
    {
        _minLevel = level;
    }

    public async Task LogAsync(string message, LogLevel level = LogLevel.Info)
    {
        if (level < _minLevel) return;

        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

        await _semaphore.WaitAsync();
        try
        {
            await File.AppendAllTextAsync(_logFilePath, logMessage + Environment.NewLine);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // Métodos por nível de log
    public Task Debug(string msg) => LogAsync(msg, LogLevel.Debug);
    public Task Info(string msg) => LogAsync(msg, LogLevel.Info);
    public Task Warning(string msg) => LogAsync(msg, LogLevel.Warning);
    public Task Error(string msg) => LogAsync(msg, LogLevel.Error);

    // Log de exceções com mais detalhes
    public Task Error(Exception ex)
    {
        string message = $"[EXCEPTION] {ex.GetType().Name}: {ex.Message}\nStackTrace:\n{ex.StackTrace}";
        return LogAsync(message, LogLevel.Error);
    }
}
