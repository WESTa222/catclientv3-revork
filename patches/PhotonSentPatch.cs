using catclientv3.features.network;
using catclientv3.features.worlds;
using catclientv3.misc;
//using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes;
using Photon.Client;
using VRC.Udon;
using Photon.Pun;
using VRC.Networking;
using PhotonHandler = catclientv3.misc.PhotonHandler;

namespace catclientv3.patches;

public class PhotonSentPatch
{
    internal static void Patch()
    {
        string Meowmeow = "faggots should burn hehe";
        ModMain.instance.HarmonyInstance.Patch(typeof(LoadBalancingClient).GetMethod(nameof(LoadBalancingClient.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0)), prefix:new(typeof(PhotonSentPatch).GetMethod(nameof(Prefix))));
    }

    public static bool Prefix(ref byte param_1, ref Il2CppSystem.Object param_2, ref RaiseEventOptions param_3, ref SendOptions param_4)
    {
        return PhotonHandler.handleSent(ref param_1, ref param_2, ref param_3, ref param_4) >= 0;
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
    }
}