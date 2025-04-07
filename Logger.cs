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

public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = new(() => new Logger());
    public static Logger Instance => _instance.Value;

    private string _logFilePath = $"log_{DateTime.Now:yyyy-MM-dd}.log";
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private string _source = "DefaultSource";
    private string _httpAddress = "http://localhost";
    private LogLevel _minLevel = LogLevel.Debug;
    private bool _logToConsole = false;

    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    private Logger() { }

    public void Configure(string source, string httpAddress, string logFilePath = "app.log", LogLevel minLevel = LogLevel.Debug, bool logToConsole = false)
    {
        _source = source;
        _httpAddress = httpAddress;
        _logFilePath = $"{DateTime.Now:yyyy-MM-dd}_{logFilePath}";
        _minLevel = minLevel;
        _logToConsole = logToConsole;
    }

    public void SetMinimumLevel(LogLevel level)
    {
        _minLevel = level;
    }

    public async Task LogAsync(string message, string traceKey, LogLevel level = LogLevel.Info)
    {
        if (level < _minLevel) return;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"{timestamp} [{level}] [traceKey: {traceKey}] [source: {_source}] [http: {_httpAddress}] {message}";

        await _semaphore.WaitAsync();
        try
        {
            RotateLogFile();

            await File.AppendAllTextAsync(_logFilePath, logMessage + Environment.NewLine);

            if (_logToConsole) Console.WriteLine(logMessage);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void RotateLogFile()
    {
        if (!File.Exists(_logFilePath))
            return;

        var fileInfo = new FileInfo(_logFilePath);
        if (fileInfo.Length >= MaxFileSizeBytes)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupFilePath = _logFilePath.Replace(".log", $"_{timestamp}.log");

            File.Move(_logFilePath, backupFilePath);
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
