using catclientv3.features.network;
using catclientv3.features.worlds;
using catclientv3.misc;
using HarmonyLib;
//using Il2CppExitGames.Client.Photon;
using VRC.Udon;
using Photon.Pun;
using Photon.Realtime;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Photon.Client;
using Int32 = Il2CppSystem.Int32;
using PhotonHandler = catclientv3.misc.PhotonHandler;

namespace catclientv3.patches;

public class PhotonReceivedPatch
{
    internal static void Patch()
    {
        //typeof(client)
        ModMain.instance.HarmonyInstance.Patch(
            typeof(LoadBalancingClient).GetMethod(
                FAGGOTstringsofDOOMandDESPAIR.faggotphoton), prefix:new HarmonyMethod(typeof(PhotonReceivedPatch), nameof(Prefix)));
    }

    
    internal static bool Prefix(ref EventData param_1) // thanks void
    {
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        return PhotonHandler.handleRecv(ref param_1) >= 0;
    }
    
    /*internal static bool Prefix(EventData param_1) // thanks void
    {
        int region = 0;
        try
        {
            if (param_1.Code == 18)
            {
                if (param_1.Parameters == null || !param_1.Parameters.ContainsKey(param_1.CustomDataKey))
                    return true;
                region++;
                var parameters = param_1.Parameters;
                var dict = Utils.FromIL2CPPToManaged<Dictionary<byte, object>>(parameters[param_1.CustomDataKey]);
                //var dict = (Il2CppSystem.Collections.Generic.Dictionary<byte, object>)param_1.customData;
                //var il2cppDict = param_1.Parameters[param_1.CustomDataKey].Cast<Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object>>();
                
                /*var dict = new Dictionary<byte, object>();
                foreach (var kvp in il2cppDict)
                {
                    dict.Add(kvp.Key, kvp.Value);
                }#1#
                region++;
                if (dict == null || !dict.TryGetValue(2, out var objPhotonViewId) ||
                    !dict.TryGetValue(1, out var objEntrypointHash))
                    return true;
                region++;
                var photonView = PhotonView.Method_Public_Static_PhotonView_Int32_0((int)objPhotonViewId);
                region++;
                if (photonView == null)
                    return true;
                region++;
                var udonBehaviour = photonView.GetComponent<UdonBehaviour>();
                region++;
                if (udonBehaviour == null ||
                    !udonBehaviour.TryGetEntrypointNameFromHash((uint)(int)objEntrypointHash, out var entrypointName))
                    return true;
                region++;
                var player = Utils.GetPlayerByActorID(param_1.sender);
                region++;
                if (UdonLogger.enabled)
                    ModMain.instance.LoggerInstance.Msg("Received udon event " + entrypointName + " from " +
                                                        player.field_Private_VRCPlayerApi_0.displayName);
            }
        }
        catch(Exception ex)
        {
            ModMain.instance.LoggerInstance.Msg("Exception " + ex + " at region " + region);
        }
        if (param_1.Code == 18 && UdonBlocker.enabled)
        {
            return false;
        }
        return true;
    }*/
}