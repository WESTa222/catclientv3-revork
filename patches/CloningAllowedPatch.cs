using HarmonyLib;
using AccessTools = HarmonyLib.AccessTools;

namespace catclientv3.patches;

public class CloningAllowedPatch //thank god for astrum
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(
            AccessTools.PropertySetter(typeof(VRC.Core.APIUser),
                nameof(VRC.Core.APIUser.allowAvatarCopying)),
            new HarmonyMethod(typeof(CloningAllowedPatch), nameof(hk)));
    }
    
    public static void hk(ref bool __0) =>  __0 = true;
}