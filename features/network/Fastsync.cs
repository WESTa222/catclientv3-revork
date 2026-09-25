using catclientv3.misc;
using VRC.Networking;

namespace catclientv3.features.network;

public class Fastsync
{
    //public static bool enabled = false;
    public static bool enabled 
    {
        get
        {
            var a = CCPersistance.tryGetSetting("FSEnabled", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("FSEnabled", value);
        }
    }
    public static void handlefag(bool ni)
    {
        enabled = ni;
        VRC.Player.prop_Player_0.gameObject.GetComponent<FlatBufferNetworkSerializer>().RequireFastRate = ni;
    }

    public static void imHomophobic()
    {
        try
        {
            VRC.Player.prop_Player_0.gameObject.GetComponent<FlatBufferNetworkSerializer>().RequireFastRate = enabled;
        }
        catch
        {
            //sybauuuuu
        }
    }
}