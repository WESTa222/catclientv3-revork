using System.Diagnostics.CodeAnalysis;
using catclientv3.misc;
//using Il2Cpp;
using OscCore;
using Il2CppSystem.Collections;
using Il2CppSystem.Text.RegularExpressions;
using TMPro;
using VRC.SDKBase;
using VRC.Udon;
//using MelonLoader;
using UnityEngine;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Groups;
using Utils = catclientv3.misc.Utils;

namespace catclientv3.features.worlds;

[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
public static class Murder4
{

    private static VRCPage _userPage;
    private static VRCPage _mainPage;
    private static ButtonGroup _userRootButtonGroup;
    private static ButtonGroup _mainRootButtonGroup;
    internal static bool godMode = false;
    internal static bool rapidFire = false;
    internal static bool rapidFireAll = false;
    internal static bool spawnMurdererWithGun = false;
    internal static List<CCNameplateText> nameplateTexts = new List<CCNameplateText>();
    public static bool roleNameplate = true;
    private static GameObject gameLogic => GameObject.Find("Game Logic");

    internal static void MurderUi(ButtonGroup main, ButtonGroup user)
    {
        //init
        _userRootButtonGroup = user;
        _mainRootButtonGroup = main;
        _mainPage = new VRCPage("Murder 4");
        _userPage = new VRCPage("Murder 4 user menu", parentMenuName:"QuickMenuSelectedUserLocal");
        
        //main page
        new VRCButton(_mainRootButtonGroup, "Murder 4", "Murder 4 haxor skilz", _mainPage.OpenMenu);
        _mainPage.BackButtonPress = _mainPage.CloseMenu;
        var grpSelf = new CollapsibleButtonGroup(_mainPage, "Self", true);
        new VRCButton(grpSelf, "Set Bystander", "set yourself to bystander", selfB);
        new VRCButton(grpSelf, "Set Murderer", "set yourself to murderer", selfM);
        new VRCButton(grpSelf, "Set Detective", "set yourself to detective", selfD);
        new VRCToggle(grpSelf, "god mode", val => godMode = val);
        new VRCToggle(grpSelf, "Anti-Blind", handleAntiBlind);
        
        var grpGame = new CollapsibleButtonGroup(_mainPage, "Game");
        new VRCButton(grpGame, "Start Game", "start the game", gameStart);
        new VRCButton(grpGame, "Abort Game", "stop the game", gameAbort);
        new VRCButton(grpGame, "B win", "bystanders win", gameBWin);
        new VRCButton(grpGame, "M win", "murder murders wow", gameMWin);
        
        var grpItems = new CollapsibleButtonGroup(_mainPage, "Items");
        new VRCButton(grpItems, "Bring gun", "brings revolver", bringGun);
        new VRCButton(grpItems, "Bring shotgun", "brings shotgun", bringShotgun);
        new VRCButton(grpItems, "Bring luger", "brings luger", bringLuger);
        new VRCButton(grpItems, "Bring smoke", "brings smoke", bringSmoke);
        new VRCButton(grpItems, "Bring frag", "brings frag", bringGrenade);
        new VRCButton(grpItems, "Bring camera", "brings camera", bringCamera);
        
        var grpOther = new CollapsibleButtonGroup(_mainPage, "Other");
        new VRCToggle(grpOther, "rapid fire", b => rapidFire = b);
        new VRCToggle(grpOther, "rapid fire (all)", b => rapidFireAll = b);
        new VRCToggle(grpOther, "Murder shotty", b => spawnMurdererWithGun = b);
        new VRCToggle(grpOther, "Role Nameplate", b => roleNameplate = b, true);
        
        
        //user profile
        new VRCButton(_userRootButtonGroup, "Murder 4", "fuck this guy bro", _userPage.OpenMenu);
        
        var grpRoles = new CollapsibleButtonGroup(_userPage, "Roles", true);
        new VRCButton(grpRoles, "Bystander", "set user to bystander", selUserB);
        new VRCButton(grpRoles, "Murderer", "set user to murderer", selUserM);
        new VRCButton(grpRoles, "Detective", "set user to detective", selUserD);
        
        var grpUserOther = new  CollapsibleButtonGroup(_userPage, "Other");
        new VRCButton(grpUserOther, "Kill", "kill this fucking guy bro", selUserKill);
        new VRCButton(grpUserOther, "Flash", "PENIS", selUserFlash);
        new VRCButton(grpUserOther, "Blow up", "tps grenade to this guy then blows him up", selUserBlow);

    }
    #region self roles
    internal static void selfB()
    {
        sendUserEvent(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_IUser_0.prop_String_1, "SyncAssignB");
    }

    internal static void selfM()
    {
        sendUserEvent(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_IUser_0.prop_String_1, "SyncAssignM");
    }

    internal static void selfD()
    {
        sendUserEvent(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_IUser_0.prop_String_1, "SyncAssignD");
    }
    #endregion
    
    #region game events

    internal static void gameStart()
    {
        sendGameEvent("Btn_Start");
        sendGameEvent("SyncStartGame");
    }

    internal static void gameAbort()
    {
        sendGameEvent("SyncAbort");
    }

    internal static void gameBWin()
    {
        sendGameEvent("SyncVictoryB");
    }

    internal static void gameMWin()
    {
        sendGameEvent("SyncVictoryM");
    }
    #endregion
    
    #region items

    internal static void bringGun()
    {
        TPWeapon("Revolver", Networking.LocalPlayer);
    }

    internal static void bringShotgun()
    {
        TPUnlockable("Shotgun", Networking.LocalPlayer);
    }

    internal static void bringLuger()
    {
        TPUnlockable("Luger",  Networking.LocalPlayer);
    }

    internal static void bringGrenade()
    {
        TPUnlockable("Frag",  Networking.LocalPlayer);
    }

    internal static void bringSmoke()
    {
        TPUnlockable("Smoke",  Networking.LocalPlayer);
    }
    internal static void bringCamera()
    {
        CCPickupUtils.tpPickupToPlayer(gameLogic.transform.Find("Polaroids Unlock Camera/FlashCamera").gameObject, Networking.LocalPlayer);
    }
    #endregion
    
    #region seluser roles

    internal static void selUserB()
    {
        sendUserEvent(Utils.selectedPlayerName(), "SyncAssignB");
    }

    internal static void selUserM()
    {
        sendUserEvent(Utils.selectedPlayerName(), "SyncAssignM");
    }

    internal static void selUserD()
    {
        sendUserEvent(Utils.selectedPlayerName(), "SyncAssignD");
    }

    internal static void spawnWithGunHandle(GameObject node)
    {
        CCEventSystem.StartCoroutinePls(SpawnMurdererWithGun(node));

    }

    internal static System.Collections.IEnumerator SpawnMurdererWithGun(GameObject node)
    {
        yield return new WaitForSeconds(1.5f);
        /*var nodenum = int.Parse(Regex.Match(node.name, @"\d+").Value);*/
        /*var player = PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray()
            .FirstOrDefault(x => x.field_Private_VRCPlayerApi_0.displayName == GameObject
                .Find("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry (" + nodenum +
                      ")/Player Name Text").GetComponent<TextMeshProUGUI>().text);*/
        var player = node.GetPlayerApiFromNode();
        while (player.gameObject.transform.position.z < 50)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        TPUnlockable("Shotgun", player);
    }

    #endregion
    
    #region seluser other

    internal static void selUserKill()
    {
        sendUserEvent(Utils.selectedPlayerName(), "SyncKill");
    }

    internal static void selUserBlow()
    {
        TPUnlockable("Frag", Utils.GetPlayerByUser(Utils.selectedPlayerName()).playerApi);
        GameObject.Find("Game Logic/Weapons/Unlockables/Frag (0)").GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "Explode");
    }
    
    internal static void selUserFlash()
    {
        sendUserEvent(Utils.selectedPlayerName(), "SyncFlashbang");
    }
    #endregion
    
    
    internal static void sendUserEvent(string player, string eventname)
    {
        GetPlayerNode(player).InvokeUdon(eventname, true);
        /*for (int i = 0; i < 24; i++)
        {
            if (GameObject.Find("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry (" + i + ")/Player Name Text").GetComponent<TextMeshProUGUI>().text.Equals(player))
            {
                ModMain.instance.LoggerInstance.Msg("Sent " + player + " "  + eventname);
                gameLogic.transform.FindChild("Player Nodes/Player Node (" + i + ")").GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, eventname);
            }
        }*/
    }

    public static void gameEnded()
    {
        ModMain.instance.Log.LogMessage("game ended");
        foreach (var npt in nameplateTexts)
        {
            try
            {
                npt.dest();
            }
            catch (Exception e)
            {
                ModMain.instance.Log.LogError(e.ToString());
            }
            
        }
        nameplateTexts.Clear();
    }

    public static void interceptAssign(string role, GameObject node)
    {
        if (roleNameplate)
        {
            if (nameplateTexts.Any(x =>
                    x.parent.player == Player.players.First(x => x.playerApi.Equals(node.GetPlayerApiFromNode()))))
            {
                var a = nameplateTexts.First(x =>
                    x.parent.player == Player.players.First(x => x.playerApi.Equals(node.GetPlayerApiFromNode())));
                nameplateTexts.Remove(a);
                a.dest();
            }
            nameplateTexts.Add(Player.players.First(x => x.playerApi.Equals(node.GetPlayerApiFromNode())).namplate.AddText(role.Remove(0,10))); //why the fuck is this one line??? kill yourself??????
        }

        if (spawnMurdererWithGun && role.Contains('M'))
        {
            spawnWithGunHandle(node);
        }
    }

    public static void handleAntiBlind(bool val)
    {
        GameObject.Find("Game Logic/Player HUD/Blind HUD Anim").SetActive(!val);
        GameObject.Find("Game Logic/Player HUD/Flashbang HUD Anim").SetActive(!val);
    }

    internal static void sendGameEvent(string eventname)
    {
        gameLogic.InvokeUdon(eventname, true);
    }

    internal static void TPUnlockable(string unlockable, VRCPlayerApi player)
    {
        CCPickupUtils.tpPickupToPlayer(gameLogic.transform.Find("Weapons/Unlockables/" + unlockable + " (0)").gameObject.gameObject, player);
    }
    
    internal static void TPWeapon(string weapon, VRCPlayerApi player)
    {
        CCPickupUtils.tpPickupToPlayer(gameLogic.transform.Find("Weapons/" + weapon).gameObject, player);
    }

    public static VRCPlayerApi? GetPlayerApiFromNode(this GameObject node)
    {
        return node.GetHeapVal<VRCPlayerApi>("playerApi");
    }

    public static GameObject? GetPlayerNode(string player)
    {
        var thePlayer = Player.players.First(x => x.username.Contains(player.Trim()));
        foreach (var node in GameObject.Find("Game Logic/Player Nodes").GetComponentsInChildren<UdonBehaviour>())
        { 
            var meow = node.GetHeapVal<VRCPlayerApi>("playerApi"); 
            if (meow != null && meow.Equals(thePlayer.playerApi))
            { 
                return node.gameObject;
            }
        }
        ModMain.instance.Log.LogMessage("there is no player node (lie, there always is a fucking player node)");
        return null;
        //return GameObject.Find("Game Logic/Player Nodes").GetComponentsInChildren<UdonBehaviour>().First(x => (x.gameObject.GetHeapVal<VRCPlayerApi>("playerApi")!).displayName.Equals(player)).gameObject;
    }
}