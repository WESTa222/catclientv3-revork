namespace catclientv3.misc;

public class CCSettings
{
    public static bool tabEnabled 
    {
        get
        {
            var a = CCPersistance.tryGetSetting("tabEnabled", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("tabEnabled", value);
        }
    }
    public static bool buttonEnabled 
    {
        get
        {
            var a = CCPersistance.tryGetSetting("buttonEnabled", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("buttonEnabled", value);
        }
    }
    
    public static bool QMConsoleEnabled 
        {
            get
            {
                var a = CCPersistance.tryGetSetting("QMConsoleEnabled", out bool b);
                return !a || b;
            }
            set
            {
                CCPersistance.setSetting("QMConsoleEnabled", value);
            }
        }
}