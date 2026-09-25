using catclientv3.misc;

namespace catclientv3.features.qol;

public class EcoSpoof
{
    //public static bool enabled = false;
    public static bool enabled 
    {
        get
        {
            var a = CCPersistance.tryGetSetting("ecoSpoofEnabled", out bool b);
            return a && b;
        }
        set
        {
            CCPersistance.setSetting("ecoSpoofEnabled", value);
        }
    }
}