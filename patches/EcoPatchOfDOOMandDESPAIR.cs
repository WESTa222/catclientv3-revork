using catclientv3.features.qol;
using VRC.Economy.Internal;

namespace catclientv3.patches;

public class EcoPatchOfDOOMandDESPAIR
{
    internal static void Patch()
    {
        string Meowmeow = "faggots should burn hehe";
        ModMain.instance.HarmonyInstance.Patch(typeof(Store).GetMethod(nameof(Store.Method_Private_Boolean_VRCPlayerApi_IProduct_PDM_0)), prefix:new(typeof(EcoPatchOfDOOMandDESPAIR).GetMethod(nameof(Prefix))));
        //ModMain.instance.HarmonyInstance.Patch(typeof(Store).GetMethod(nameof(Store.Method_Private_Boolean_VRCPlayerApi_IProduct_PDM_1)), prefix:new(typeof(EcoPatchOfDOOMandDESPAIR).GetMethod(nameof(Prefix))));
        //ModMain.instance.HarmonyInstance.Patch(typeof(Store).GetMethod(nameof(Store.Method)), prefix:new(typeof(EcoPatchOfDOOMandDESPAIR).GetMethod(nameof(Prefix))));
        ModMain.instance.HarmonyInstance.Patch(typeof(Store).GetMethod(nameof(Store.Method_Private_Boolean_IProduct_PDM_0)), prefix:new(typeof(EcoPatchOfDOOMandDESPAIR).GetMethod(nameof(Prefix))));
    }

    public static bool Prefix(ref bool __result)
    {
        if (EcoSpoof.enabled)
        {
            __result = true;
            return false;
        }
        return true;
    }
}