using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified;
using catclientv3.features.general;
using UnityEngine;
using VRC.Core;

namespace catclientv3.patches;

public class AvatarLoadingPatch
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(VRCAvatarManager).GetMethod(nameof(VRCAvatarManager.Method_Private_Boolean_ApiAvatar_GameObject_0)));
    }

    public static bool Prefix(VRCAvatarManager __instance, ApiAvatar __0, GameObject __1)
    {
        return !BlockAviLoad.enabled;
    }
}