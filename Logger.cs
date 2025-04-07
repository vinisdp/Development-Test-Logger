using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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
    private readonly string _source;
    private readonly string _httpAddress;
    private LogLevel _minLevel;

    public Logger(string source, string httpAddress, string logFilePath = "log.txt", LogLevel minLevel = LogLevel.Debug)
    {
        _source = source;
        _httpAddress = httpAddress;
        _logFilePath = $"{DateTime.Now:yyyy-MM-dd}_{logFilePath}";
        _minLevel = minLevel;
    }

    public void SetMinimumLevel(LogLevel level)
    {
        _minLevel = level;
    }

    public async Task LogAsync(
        string message,
        string traceKey,
        LogLevel level = LogLevel.Info)
    {
        if (level < _minLevel) return;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"{timestamp} [{level}] [traceKey: {traceKey}] [source: {_source}] [http: {_httpAddress}] {message}";

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

    public Task Debug(string msg, string traceKey) => LogAsync(msg, traceKey, LogLevel.Debug);
    public Task Info(string msg, string traceKey) => LogAsync(msg, traceKey, LogLevel.Info);
    public Task Warning(string msg, string traceKey) => LogAsync(msg, traceKey, LogLevel.Warning);
    public Task Error(string msg, string traceKey) => LogAsync(msg, traceKey, LogLevel.Error);

    public Task Error(Exception ex, string traceKey)
    {
        string message = $"[EXCEPTION] {ex.GetType().Name}: {ex.Message}\nStackTrace:\n{ex.StackTrace}";
        return LogAsync(message, traceKey, LogLevel.Error);
    }
}
