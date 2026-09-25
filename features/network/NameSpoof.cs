using System.Collections;
//using BepInEx;
//using Cpp2IL.Core.OutputFormats;
using VRC.SDKBase;

namespace catclientv3.features.network;

public class NameSpoof
{
    public static bool spoof = false;
    public static string? spoofTo = null;

    public static IEnumerator waitForLocalPlayer()
    {
        while (Networking.LocalPlayer == null)
        {
            yield return null;
        }
        Networking.LocalPlayer.displayName = spoofTo ?? RoomManager.field_Internal_Static_ApiWorld_0.authorName;
        ModMain.instance.Log.LogMessage($"spoofed name to {Networking.LocalPlayer.displayName}");
    }

    public static void customSpoof(string customSpoof)
    {
        spoofTo = string.IsNullOrWhiteSpace(customSpoof) ? null : customSpoof;
    }
}