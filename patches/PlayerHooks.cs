using catclientv3.features.network;
using catclientv3.features.visual;
using catclientv3.misc;
using catclientv3.OtherUI.EdenToast;
using HarmonyLib;
using Il2CppInterop.Runtime;
using UnityEngine;
using UnityEngine.Events;
using VRC;
using VRC.Core;
using VRC.Networking;
using VRC.SDKBase;

namespace catclientv3.patches;

public class PlayerHooks
{
    //string 0 - user id, string 1 - username, string 2 - primary language?, string 5 - avatar id, string 6 - fallback id
    public static void patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_0)), new HarmonyMethod(typeof(PlayerHooks), nameof(playerleft)));
        ModMain.instance.HarmonyInstance.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_PDM_1)), new HarmonyMethod(typeof(PlayerHooks), nameof(playerinit)));
        ModMain.instance.HarmonyInstance.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_PDM_0)), new HarmonyMethod(typeof(PlayerHooks), nameof(playerjoined)));
        /*UnityAction<IPlayer> joinAction = (UnityAction<IPlayer>)((IPlayer player) => {playerjoined(player);});
        NetworkManager.field_Internal_Static_NetworkManager_0..field_Private_VRCEventDelegate_IPlayer_1
            .field_Private_HashSet_1_UnityAction_1_T_0.Add(joinAction);
        UnityAction<IPlayer> initAction = (UnityAction<IPlayer>)((IPlayer player) => {playerinit(player);});
        NetworkManager.field_Internal_Static_NetworkManager_0.field_Private_VRCEventDelegate_IPlayer_0
            .field_Private_HashSet_1_UnityAction_1_T_0.Add(initAction);
        UnityAction<IPlayer> leaveAction = (UnityAction<IPlayer>)((IPlayer player) => {playerleft(player);});
        NetworkManager.field_Internal_Static_NetworkManager_0.field_Private_VRCEventDelegate_IPlayer_2
            .field_Private_HashSet_1_UnityAction_1_T_0.Add(leaveAction);*/
    }

    public static void playerjoined(VRC.Player __0)
    {
        if (__0.prop_VRCPlayerApi_0 == Networking.LocalPlayer && NameSpoof.spoof)
        {
            CCEventSystem.StartCoroutinePls(NameSpoof.waitForLocalPlayer());
        }
        ModMain.instance.Log.LogMessage("playerjoined " + __0.field_Private_APIUser_0.displayName);
        if (__0.field_Private_APIUser_0.hasModerationPowers)
        {
            ModMain.instance.Log.LogMessage("USER " + __0.field_Private_APIUser_0.displayName + " HAS MODERATION POWERS");
            ToastNotification.ForceToast($"USER {__0.field_Private_APIUser_0.displayName} HAS MODERATION POWERS", "be careful", UIHelper.placeholder);
        }
    }

    public static void playerinit(VRC.Player __0)
    {
        ModMain.instance.Log.LogMessage("playerinit " + __0.field_Private_APIUser_0.displayName);
        CCPlayerUtils.playerinit(__0);
    }
    
    public static void playerleft(VRC.Player __0)
    {
        ModMain.instance.Log.LogMessage("playerleft " + __0.field_Private_APIUser_0.displayName);
        CCPlayerUtils.playerleft(__0);
    }
}