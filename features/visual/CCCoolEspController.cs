using catclientv3.misc;
//using Il2Cpp;
using UnityEngine;
using Object = UnityEngine.Object;

namespace catclientv3.features.visual;

public class CCCoolEspController
{
    internal static bool line;
    internal static bool linevr;
    internal static bool box;
    internal static bool bone;
    internal static bool enabled => box || line || linevr || bone;
    /*internal static bool vrCompatMode 
    {
        get
        {
            var a = CCPersistance.tryGetSetting("vrCompatMode", out bool b);
            return a && b;
        }
        set
        {
            CCPersistance.setSetting("vrCompatMode", value);
        }
    }*/

    internal static void playerinit(Player player)
    {
        if (player.playerApi.isLocal)
            return;
        var esp = player.playerObject.AddComponent<CCCoolEsp>();
        esp.CCPlayer = player;
    }
}