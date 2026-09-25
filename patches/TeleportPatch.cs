using catclientv3.features.general;
using HarmonyLib;
using VRC.SDKBase;

namespace catclientv3.patches;

public static class TeleportPatch
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(VRCPlayerApi).GetMethod(nameof(VRCPlayerApi.TeleportTo), new []{typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion)}),
            new HarmonyMethod(typeof(TeleportPatch).GetMethod(nameof(Prefix))));
        ModMain.instance.HarmonyInstance.Patch(typeof(VRCPlayer).GetMethod(nameof(VRCPlayer.Method_Public_Void_Vector3_Quaternion_0), new []{typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion)}),
            new HarmonyMethod(typeof(TeleportPatch).GetMethod(nameof(Prefix))));
    }

    public static bool Prefix()
    {
        return !AntiTp.enabled;
    }
}