using HarmonyLib;
//using Il2Cpp;
using VRC.Core;

namespace catclientv3.patches;

public class VRCPlusSpoof //thanks cyril
{
    internal static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(
            typeof(VRCPlusStatus).GetProperty(nameof(VRCPlusStatus.prop_ReactiveProperty_1_Boolean_0)).GetGetMethod(),
            postfix: new HarmonyMethod(typeof(VRCPlusSpoof), nameof(pfstatus)));
    }
    internal static void pfstatus(ref ReactiveProperty<bool> __result)
    {
        __result.field_Protected_T_0 = true;
    }
}