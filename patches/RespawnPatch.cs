using catclientv3.features.general;
using HarmonyLib;
using VRC.SDKBase;

namespace catclientv3.patches;

public class RespawnPatch
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(VRCPlayerApi).GetMethod(nameof(VRCPlayerApi.Respawn), new Type[]{ }),
            new HarmonyMethod(typeof(RespawnPatch).GetMethod(nameof(Prefix))));
    }

    public static bool Prefix()
    {
        return !AntiRespawn.enabled;
    }
}