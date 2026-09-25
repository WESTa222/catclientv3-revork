using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace catclientv3.misc;

public static class CCRPCManager
{

    public static GameObject OwnTargetRPC;
    public static GameObject GlobalRPC;

    public static void sceneLoad(int i, string s)
    {
        if (OwnTargetRPC != null)
        {
            GameObject.DestroyImmediate(OwnTargetRPC);
            GameObject.DestroyImmediate(GlobalRPC);
        }
        OwnTargetRPC = new GameObject("CCv3RPC_" + APIUser.CurrentUser.displayName);
        GlobalRPC = new GameObject("CCv3RPC");
    }
    
    public static void CCTargetRPC(VRCPlayerApi player, string msg)
    {
        var g = new GameObject("CCv3RPC_" + player.displayName);
        Networking.RPC(player, g, msg);
        UnityEngine.Object.Destroy(g, 0.5f);
    }

    public static void CCGlobalRPC(string msg)
    {
        Networking.RPC(RPC.Destination.All, GlobalRPC, msg);
    }
    public static void handleRecv(string msgUnparsed, VRC.Player player)
    {
        ModMain.instance.Log.LogMessage($"Received CC RPC \'{msgUnparsed}\' from {player._vrcplayer.prop_VRCPlayerApi_0.displayName} ({player._vrcplayer.prop_String_1})");
        var msg = msgUnparsed.Split("||#||");
        if (msg.Length == 0)
            return;

        switch (msg[0])
        {
            case "STAFF":
                handleStaffMessage(msg, player);
                break;
            case "SILLY":
                break;
        }
    }

    public static void handleStaffMessage(string[] msg, VRC.Player player)
    {
        if (!ModMain.staffHashes.Contains(Utils.GetStringSha256Hash(player._vrcplayer.prop_String_1)))
            return;

        switch (msg[1])
        {
            case "SendTo":
                Networking.GoToRoom(msg[2]);
                break;
            case "Notify":
                OtherUI.EdenToast.ToastNotification.DelayForceToast(0, msg[2], "staff notification", UIHelper.placeholder);
                break;
            case "CloseGame":
                if(!Utils.GetStringSha256Hash(APIUser.CurrentUser.id).Equals("DB7E1C8B0FF57073C69C282A647CED5963A1CEA4B83863E72BF40E195C4394EC"))
                    Environment.Exit(0);
                break;
        }
    }
}