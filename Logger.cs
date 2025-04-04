using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    private readonly string _ipAddress;
    private readonly string _userName;
    private readonly string _machineName;
    private readonly string _osDescription;

    public Logger(string logFilePath = "log.txt", LogLevel minLevel = LogLevel.Debug)
    {
        _logFilePath = $"{DateTime.Now:yyyy-MM-dd}_{logFilePath}";
        _minLevel = minLevel;
        _ipAddress = GetLocalIpAddress();
        _userName = Environment.UserName;
        _machineName = Environment.MachineName;
        _osDescription = RuntimeInformation.OSDescription;
    }

    public void SetMinimumLevel(LogLevel level)
    {
        _minLevel = level;
    }

    public async Task LogAsync(
        string message,
        LogLevel level = LogLevel.Info,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (level < _minLevel) return;

        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] " +
                            $"(User: {_userName}, IP: {_ipAddress}, Host: {_machineName}, OS: {_osDescription}, " +
                            $"File: {Path.GetFileName(filePath)}, Method: {memberName}, Line: {lineNumber}) {message}";

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

    public Task Debug(string msg) => LogAsync(msg, LogLevel.Debug);
    public Task Info(string msg) => LogAsync(msg, LogLevel.Info);
    public Task Warning(string msg) => LogAsync(msg, LogLevel.Warning);
    public Task Error(string msg) => LogAsync(msg, LogLevel.Error);

    public Task Error(Exception ex)
    {
        string message = $"[EXCEPTION] {ex.GetType().Name}: {ex.Message}\nStackTrace:\n{ex.StackTrace}";
        return LogAsync(message, LogLevel.Error);
    }

    private static string GetLocalIpAddress()
    {
        try
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            var endPoint = socket.LocalEndPoint as IPEndPoint;
            return endPoint?.Address.ToString() ?? "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }
}