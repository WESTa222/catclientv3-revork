using catclientv3.features.movement;
using catclientv3.features.network;
using catclientv3.features.worlds;
//using Il2Cpp;
//using ExitGames.Client.Photon;
using VRC.Udon;
//using MelonLoader;
using Photon.Client;
using Photon.Pun;
using Sentry.Ben.BlockingDetector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace catclientv3.misc;

public static class PhotonHandler
{
    //-2 = failure, abort, -1 = success, abort, 0 = unhandled, continue, 1 = success, continue, 2 = failure, continue
    public static int handleRecv(ref EventData eDat)
    {
        try
        {
            switch (eDat.Code)
            {
                case 18:
                    return handleUdonRecv(eDat);
                case 10:
                    return handlee10recv(eDat);
                case 11:
                    return handlee11recv(eDat);
                case 17:
                    return handlee17recv(eDat);
                default:
                    return 0; //unhandled
            }
        }
        catch (Exception e)
        {
            ModMain.instance.Log.LogError(e);
            return 2;
        }
    }

    public static int handleSent(ref byte code, ref Il2CppSystem.Object owobject, ref RaiseEventOptions eOp, ref SendOptions sOp)
    {
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        try
        {
            switch (code)
            {
                case 18:
                    return handleUdonSent(owobject, eOp, sOp);
                case 11:
                    return handlee11sent(owobject, eOp, sOp);
                case 12:
                    return handlee12sent(ref owobject);
                default:
                    return 0; //unhandled
            }
        }
        catch (Exception e)
        {
            ModMain.instance.Log.LogMessage(e);
            return 2;
        }
        
    }

    public static int handleUdonRecv(EventData param_1)  //ty void
    {
        if (param_1.Parameters == null || !param_1.Parameters.ContainsKey(param_1.CustomDataKey))
            return 2; //aborted, allow method execution
        var dict = param_1.Parameters[param_1.CustomDataKey].Cast<Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object>>();
        if (dict == null! || !dict.TryGetValue(2, out var objPhotonViewId) || !dict.TryGetValue(1, out var objEntrypointHash))
            return 2;
        int photonViewId = objPhotonViewId.Unbox<int>();
        int entrypointHash = objEntrypointHash.Unbox<int>();
        var photonView = PhotonView.Method_Public_Static_PhotonView_Int32_0(photonViewId);
        //var photonView = PhotonView.Method_Public_Static_PhotonView_Int32_0(photonViewId);
        //var photonView = PhotonView.Method_Public_Static_PhotonView_Int32_0((Il2CppSystem.Int32)objPhotonViewId);
        if (photonView == null)
            return 2;
        var udonBehaviour = photonView.GetComponent<UdonBehaviour>();
        if (udonBehaviour == null || !udonBehaviour.TryGetEntrypointNameFromHash((uint)entrypointHash, out var entrypointName))
            return 2;
        var player = Utils.GetPlayerByActorID(param_1.sender);
        if (UdonLogger.enabled)
        {
            ModMain.instance.Log.LogMessage("Received udon event " + entrypointName + " from " + player!.field_Private_VRCPlayerApi_0.displayName + " targeted towards gameobject " + udonBehaviour.gameObject.name);
            
        }
            
        if (Murder4.godMode && entrypointName.Equals("SyncKill"))
            return -1;
        if (Murder4.rapidFire && Murder4.rapidFireAll && entrypointName.Equals("SyncDryFire"))
            udonBehaviour.SendCustomNetworkEvent(0, "SyncFire");
        if (entrypointName.Contains("SyncAssign"))
        {
            Murder4.interceptAssign(entrypointName, udonBehaviour.gameObject);
        }
        if(entrypointName.Contains("SyncVictory") || entrypointName.Equals("SyncAbort"))
            Murder4.gameEnded();
        if (UdonBlocker.enabled)
            return -1; //success, but don't continue execution
        return 1; //success, method fully executed
    }
    
    public static int handleUdonSent(Il2CppSystem.Object param_2, RaiseEventOptions param_3, Photon.Client.SendOptions param_4) //ty void
    {
        var managedDict = param_2.Cast<Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object>>();
        if (managedDict == null || !managedDict.TryGetValue(2, out var objPhotonViewId) || !managedDict.TryGetValue(1, out var objEntrypointHash))
                return 2;
        int photonViewId = objPhotonViewId.Unbox<int>();
        int entrypointHash = objEntrypointHash.Unbox<int>();
        var photonView = PhotonView.Method_Public_Static_PhotonView_Int32_0(photonViewId);
        if (photonView == null)
            return 2;
        var udonBehaviour = photonView.GetComponent<UdonBehaviour>();
        if (udonBehaviour == null || !udonBehaviour.TryGetEntrypointNameFromHash((uint)entrypointHash, out var entrypointName))
            return 2;
        //var player = VRCPlayer.field_Internal_Static_VRCPlayer_0;
        if (UdonLogger.enabled)
            ModMain.instance.Log.LogMessage("Sent udon event " + entrypointName + " to " + udonBehaviour.gameObject.name);
        if (Murder4.rapidFire && entrypointName.Equals("SyncDryFire"))
            udonBehaviour.SendCustomNetworkEvent(0, "SyncFire");
        if (entrypointName.Contains("SyncAssign"))
        {
            Murder4.interceptAssign(entrypointName, udonBehaviour.gameObject);
        }
        if(entrypointName.Contains("SyncVictory") || entrypointName.Equals("SyncAbort"))
            Murder4.gameEnded();
        return 1; //success
    }

    public static int handlee11sent(Il2CppSystem.Object owobject, RaiseEventOptions eOp, SendOptions sOp)
    {
        return 0;
    }

    public static int handlee11recv(EventData eDat)
    {
        return UdonBlocker.enabled && UdonBlocker.blockSync ? -1 : 1;
    }
    
    
    public static int handlee10recv(EventData eDat)
    {
        return UdonBlocker.enabled && UdonBlocker.blockSync ? -1 : 1;
    }
    
    public static int handlee17recv(EventData eDat)
    {
        return UdonBlocker.enabled && UdonBlocker.blockSync ? -1 : 1;
    }

    public static int handlee12sent(ref Il2CppSystem.Object owobject)
    {
        if (E12Invis.enabled)
        {
            try
            {
                byte[] VecData = Utils.Vector3ToBytes(new Vector3(400000 + System.Random.Shared.NextSingle(), -200000  + System.Random.Shared.NextSingle(), 300000  + System.Random.Shared.NextSingle()));
                var arraything2 = Utils.Il2CppToByteArray(owobject);
                    
                var currentPos = Player.localPlayer.playerObject.transform.position;
                float epsilon = 0.3f; 
                for (int i = 0; i <= arraything2.Length - 12; i++) {
                    float packetX = BitConverter.ToSingle(arraything2, i);
                    float packetY = BitConverter.ToSingle(arraything2, i + 4);
                    float packetZ = BitConverter.ToSingle(arraything2, i + 8);
                        
                    if (Math.Abs(packetX - currentPos.x) < epsilon &&
                        Math.Abs(packetY - currentPos.y) < epsilon &&
                        Math.Abs(packetZ - currentPos.z) < epsilon) {
                            
                        Buffer.BlockCopy(VecData, 0, arraything2, i, 12);
                        break; 
                    }
                }
                owobject = Utils.IL2CPPFromByteArray<Il2CppSystem.Object>(arraything2);
            }
            catch (Exception ex)
            {
                ModMain.instance.Log.LogError(ex);
            }
        }
        return Deserialize.enabled ? -1 : 1;
    }
    
}