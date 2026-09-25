using System.Drawing;
using MelonLoader;
using VRC.Udon.Serialization.OdinSerializer.Utilities;

namespace catclientv3.misc;

public class CCLogger : Logger
{
    
    /*public static void Log(params object[] args) 
    {
        Console.ForegroundColor = ConsoleColor.White;
        //ModMain.instance.Log.
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("] [");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("catclientv3");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("] ");
        foreach (var arg in args)
        {
            try
            {
                Console.ForegroundColor = (ConsoleColor)arg;
            }
            catch
            {
                Console.Write((string)arg);
            }
        }
    }*/
    public CCLogger()
    {
        
    }
    
    public void LogMessage(params object[] args)
    {
        
        foreach (var arg in args)
        {
            MelonLogger.Msg(arg.ToString());
        }
    }
    
    public void LogInfo(params object[] args)
    {
        foreach (var arg in args)
        {
            MelonLogger.Msg(arg.ToString());
        }
    }
    
    public void LogWarning(params object[] args)
    {
        foreach (var arg in args)
        {
            MelonLogger.Warning(arg.ToString());
        }
    }
    
    public void LogError(params object[] args)
    {
        foreach (var arg in args)
        {
            MelonLogger.Error(arg.ToString());
        }
    }
}