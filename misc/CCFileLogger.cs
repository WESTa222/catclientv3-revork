using System;
using System.Globalization;

namespace catclientv3.misc;

public class CCFileLogger : Logger
{
    private readonly string _path;

    public CCFileLogger(string logFilePath)
    {
        _path = logFilePath;
    }

    private void WriteLog(string level, object message)
    {
        using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, _path), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        using (var writer = new StreamWriter(stream))
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            writer.WriteLine($"[{timestamp}] [{level}] {message}");
        }
    }

    public void LogMessage(params object[] args) => Array.ForEach(args, a => WriteLog("msg", a));
    public void LogInfo(params object[] args)    => Array.ForEach(args, a => WriteLog("inf", a));
    public void LogWarning(params object[] args) => Array.ForEach(args, a => WriteLog("warn", a));
    public void LogError(params object[] args)   => Array.ForEach(args, a => WriteLog("error", a));
}