namespace catclientv3.misc;

public interface Logger
{
    public void LogMessage(params object[] args);
    public void LogInfo(params object[] args);
    public void LogWarning(params object[] args);
    public void LogError(params object[] args);
    
}