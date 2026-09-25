using System.Reflection.Emit;
using catclientv3.features.general;
using catclientv3.misc;
using HarmonyLib;
using VRC.SDKBase;
using VRC.Udon.Wrapper.Modules;

namespace catclientv3.patches;

public class PickupsPatches
{
    internal static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(VRC_Pickup).GetMethod(nameof(VRC_Pickup.Awake)), postfix: new HarmonyMethod(typeof(PickupsPatches), nameof(pfAwake)));
        ModMain.instance.HarmonyInstance.Patch(typeof(VRC_Pickup).GetMethod(nameof(VRC_Pickup.OnDestroy)), postfix: new HarmonyMethod(typeof(PickupsPatches), nameof(pfDestroy)));
        ModMain.instance.HarmonyInstance.Patch(typeof(VRCPlayerApi).GetMethod(nameof(VRCPlayerApi.EnablePickups)), new HarmonyMethod(typeof(PickupsPatches), nameof(noRun)));
        ModMain.instance.HarmonyInstance.Patch(typeof(ExternVRCSDK3ComponentsVRCPickup).GetMethod(nameof(ExternVRCSDK3ComponentsVRCPickup.__set_pickupable__SystemBoolean)), new HarmonyMethod(typeof(PickupsPatches), nameof(noRunIfForceGrab)));
        ModMain.instance.HarmonyInstance.Patch(typeof(ExternVRCSDK3ComponentsVRCPickup).GetMethod(nameof(ExternVRCSDK3ComponentsVRCPickup.__set_DisallowTheft__SystemBoolean)), new HarmonyMethod(typeof(PickupsPatches), nameof(noRunIfForceGrab)));
        ModMain.instance.HarmonyInstance.Patch(typeof(ExternVRCSDK3ComponentsVRCPickup).GetMethod(nameof(ExternVRCSDK3ComponentsVRCPickup.__set_allowManipulationWhenEquipped__SystemBoolean)), new HarmonyMethod(typeof(PickupsPatches), nameof(noRunIfForceGrab)));
        ModMain.instance.HarmonyInstance.Patch(typeof(ExternVRCSDK3ComponentsVRCPickup).GetMethod(nameof(ExternVRCSDK3ComponentsVRCPickup.__set_proximity__SystemSingle)), new HarmonyMethod(typeof(PickupsPatches), nameof(noRunIfForceGrab)));
    }

    internal static bool noRun() => false;
    
    internal static bool noRunIfForceGrab() => !ForceGrab.enabled && !ForceGrab.toggling;

    internal static void pfAwake(VRC_Pickup __instance)
    {
        CCPickupUtils.handleNewPickup(__instance);
    }

    internal static void pfDestroy(VRC_Pickup __instance)
    {
        CCPickupUtils.handlePickupDeletion(__instance);
    }
}