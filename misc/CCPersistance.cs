using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace catclientv3.misc;

public static class CCPersistance
{
    private static Dictionary<string, object> persistantSettings = new();

    public static T getSetting<T>(string setting)
    {
        if (typeof(T).IsArray)
        {
            return ((JArray)persistantSettings[setting]).ToObject<T>();
        }
        return (T)persistantSettings[setting];
    }
    
    public static bool tryGetSetting<T>(string setting, out T result)
    {
        if (!persistantSettings.ContainsKey(setting))
        {
            result = default;
            return false;
        }
        else
        {
            if (typeof(T).IsArray)
            {
                result = ((JArray)persistantSettings[setting]).ToObject<T>(); 
                return true;
            }
            result = (T)persistantSettings[setting];
            return true;
        }
    }

    public static void setSetting<T>(string setting, T value)
    {
        persistantSettings[setting] = value;
        saveSettings();
    }

    public static void saveSettings()
    {
        string meow = JsonConvert.SerializeObject(persistantSettings);
        File.WriteAllText("CCData/CC.settings", meow);
    }

    public static void loadSettings()
    {
        if (!File.Exists("CCData/CC.settings"))
            return;
        //var a = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText("UserData/CC.settings"));
        var e = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText("CCData/CC.settings"));
        persistantSettings = e; //?? new Dictionary<string, object>();
    }
}