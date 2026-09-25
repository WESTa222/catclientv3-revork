using System.Collections;
using catclientv3.features.network;
using catclientv3.features.qol;
using catclientv3.features.visual;
using catclientv3.OtherUI.EdenToast;
//using Il2Cpp;
//using Il2CppExitGames.Client.Photon;
using VRC.Core;
using VRC.SDK3.Props.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Wrapper.Modules;
//using MelonLoader;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;

namespace catclientv3.misc;

public class Player
{
    public GameObject playerObject;
    public CCNameplate namplate;
    public VRCPlayer vrcPlayer;
    public VRCPlayerApi playerApi;

    public APIUser apiuser;
    //public IPlayer iplayer;
    public IUser iuser;
    public VRC.Player player;
    public string username;
    public IkBone? root;
    public bool isVr;
    public static Player? localPlayer;
    public static List<Player> players = new List<Player>();

    public Player(VRC.Player ip)
    {
        /*ModMain.instance.Log.LogMessage("player init start for " + ip.prop_APIUser_0.username);
        ModMain.instance.Log.LogMessage("VRCPLAYER: " + ip._vrcplayer);*/
        //iplayer = ip;
        playerApi = ip.prop_VRCPlayerApi_0;
        player = ip;
        vrcPlayer = ip.prop_VRCPlayer_0;
        iuser = ip.prop_IUser_0;
        apiuser = ip.field_Private_APIUser_0;
        username = apiuser.displayName;
        //playerApi = Utils.GetPlayerApiByUser(username);
        //vrcPlayer = Utils.GetVRCPlayer(playerApi);
        //player = Utils.GetVRCDotPlayer(playerApi);
        isVr = playerApi.IsUserInVR();
        playerObject = ip.gameObject;
        players.Add(this);
        try
        {
            namplate = playerObject.AddComponent<CCNameplate>();
            namplate.init(this);
            namplate.AddText(isVr ? "[<color=blue>VR</color>]" : "[<color=red>FLATSCREEN</color>]");
            if (ModMain.staffHashes.Contains(Utils.GetStringSha256Hash(apiuser.id)))
                namplate.AddText("[<color=purple>CC Staff</color>]");
            /*namplate.AddText("[JD VANCE]", text =>
            {
                if (text.parent.player.player.field_Private_APIUser_0 != null)
                {
                    text.text.text = "JOINED " + text.parent.player.player.field_Private_APIUser_0.date_joined;
                    text.updateAction = null;
                }
            });*/
            namplate.AddText("[PUBLIC NUDITY]", text =>
            {
                if (text.parent.player.vrcPlayer.field_Private_ApiAvatar_0 != null)
                {
                    var isPublic = text.parent.player.vrcPlayer.field_Private_ApiAvatar_0.releaseStatus == "public";
                    text.text.text = $"[{(isPublic ? "<color=green>PUBLIC" : "<color=red>PRIVATE")} AVATAR</color>]";
                }
            });
        }
        catch
        {
            ModMain.instance.Log.LogMessage("NAMEPLATE HAD ISSUES");
        }
    }
    
}

public class CCPlayerUtils
{
    public static void playerinit(VRC.Player ip)
    {
        string Meowmeow = "faggots should burn hehe";
        var player = new Player(ip);
        if (player.playerApi.isLocal)
        {
            Player.localPlayer = player;
        }
            
        ESP.playerjoined(player);
        CCCoolEspController.playerinit(player);
        if(JoinLeaveNotifs.enabled)
            ToastNotification.ForceToast($"{player.username} has joined and initialized", icon: UIHelper.placeholder);
    }

    public static void playerleft(VRC.Player ip)
    {
        var rp = Player.players.Find(x => x.player == ip);
        if(JoinLeaveNotifs.enabled)
            ToastNotification.ForceToast($"{ip.prop_IUser_0.prop_String_1} has left", icon: UIHelper.placeholder);
        if (rp != null)
        {
            Player.players.Remove(rp);
        }
        
    }
    
    internal static void handleSceneUnload(int index, string name)
    {
        if (index == -1)
        {
            Player.players.Clear();
        }
    }
}