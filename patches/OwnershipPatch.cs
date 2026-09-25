using catclientv3.features.general;
using HarmonyLib;
using UnityEngine;
using VRC.SDKBase;

namespace catclientv3.patches;

public class OwnershipPatch
{

    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(Networking).GetMethod(nameof(Networking.SetOwner)),
            new HarmonyMethod(typeof(OwnershipPatch).GetMethod(nameof(Prefix))));
    }

    public static bool Prefix(VRCPlayerApi __0, GameObject __1)
    {
        if (__0 == Networking.LocalPlayer && ! AntiTheft.ownedObjects.Contains(__1))
        {
            AntiTheft.ownedObjects.Add(__1);
        }
        /*if (AntiTheft.enabled && Networking.IsOwner(__1) && __0 != Networking.LocalPlayer)
        {
            Networking.SetOwner(Networking.LocalPlayer, __1);
            return false;
        }*/
        if (!AntiTheft.enabled && __0 != Networking.LocalPlayer && AntiTheft.ownedObjects.Contains(__1))
        {
            AntiTheft.ownedObjects.Remove(__1);
        }
        else if (AntiTheft.enabled && __0 != Networking.LocalPlayer && AntiTheft.ownedObjects.Contains(__1))
        {
            Networking.SetOwner(Networking.LocalPlayer, __1);
            return false;
        }
        return true;
    }
}

