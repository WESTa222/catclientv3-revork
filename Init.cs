using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Reflection;
using catclientv3.features.general;
using catclientv3.features.movement;
using catclientv3.features.network;
using catclientv3.features.qol;
using catclientv3.features.visual;
using catclientv3.features.worlds;
using catclientv3.misc;
using catclientv3.patches;
using Il2CppInterop.Runtime.Injection;
using TMPro;
using VRC.Core;
using VRC.SDKBase;
//using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.Groups;
using WorldAPI.ButtonAPI.QM.Extras;
using PhotonHandler = Photon.Pun.PhotonHandler;

namespace catclientv3;

[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
public class Init
{
    
    public static void initUI()
    {
        UIHelper.QMDashContent =
            APIBase.QuickMenu.transform.Find(
                "CanvasGroup/Container/Window/QMParent/Body/Menu_QM_Launchpad/ScrollRect/Viewport/VerticalLayoutGroup");
        UIHelper.QMDashContent.Find("Carousel_Banners").gameObject.SetActive(false);
        UIHelper.QMDashContent.Find("Buttons_QuickLinks/Button_LiveNow").gameObject.SetActive(false);
        Transform userSelectedMenu = UIHelper.VRCUserSelMenu = APIBase.QuickMenu.transform.Find(
            "CanvasGroup/Container/Window/QMParent/Body/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/");
        
        var iconbase = APIBase.UserInterface.Find(
            "UnscaledUI/HudContent/HUD_UI 2(Clone)/VR Canvas/Container/Left/Icons/Dynamic Icons/Generic & Notification Icons/Notification Icons/Friend Request");
        var invisdeserializeicon =  GameObject.Instantiate(iconbase, iconbase.parent);
        invisdeserializeicon.name = "sex icon";
        UnityEngine.Object.Destroy(invisdeserializeicon.GetComponent<HudIcon_Notifications>());
        invisdeserializeicon.localPosition = new Vector3(-73.7875f, 90.9029f, -44.1711f);
        var actualicondi = UIHelper.ghosticonhud = invisdeserializeicon.transform.GetChild(0);
        actualicondi.gameObject.GetComponent<Image>().sprite = UIHelper.ghost;
        
        var qmLanchpadTitle = APIBase.QuickMenu.transform.Find(
            "CanvasGroup/Container/Window/QMParent/Body/Menu_QM_Launchpad/Header_H1/LeftItemContainer/Text_Title");
        var qmLaunchpadTitleTMPro = qmLanchpadTitle.gameObject.GetComponent<TextMeshProUGUI>();
        qmLaunchpadTitleTMPro.text = $"Welcome, {APIUser.CurrentUser.displayName}.";
        qmLaunchpadTitleTMPro.enableAutoSizing = true;
        
        if(CCSettings.QMConsoleEnabled)
            OtherUI.CCQMConsole.Init();
        
        //main page
        var page = UIHelper.CCMain = new VRCPage("catclient v3");
         
        if (!CCSettings.tabEnabled && !CCSettings.buttonEnabled)
            CCSettings.tabEnabled = true;
        
        //tab/launchpad button
        if(CCSettings.tabEnabled)
         new Tab(page, "catclient", UIHelper.placeholder);
        if(CCSettings.buttonEnabled)
            new VRCButton(UIHelper.QMDashContent.Find("Buttons_QuickLinks"), "CCv3", "meow :3", page.OpenMenu, false,
            icon: UIHelper.placeholder);
        
        //pages (by category)
        var pageGen = new VRCPage("General");
        var pageQol = new VRCPage("QOL");
        var pageMove = new VRCPage("Movement");
        var pageVis = new VRCPage("Visual");
        var pageNet = new VRCPage("Network");
        var pageWorld = new VRCPage("Games");
        var pageMisc = new VRCPage("CC Settings");
        
        //Main CC QM Page stuff
        var grpMainFeatures = UIHelper.CCRootGrp = new ButtonGroup(page, "grp1", true);
        new VRCButton(grpMainFeatures, "General", "general page", pageGen.OpenMenu);
        new VRCButton(grpMainFeatures, "QOL", "qol page", pageQol.OpenMenu);
        new VRCButton(grpMainFeatures, "Movement", "movement page", pageMove.OpenMenu);
        new VRCButton(grpMainFeatures, "Visual", "visual page", pageVis.OpenMenu);
        new VRCButton(grpMainFeatures, "Network", "network page", pageNet.OpenMenu);
        new VRCButton(grpMainFeatures, "Games", "games page", pageWorld.OpenMenu);
        new VRCButton(grpMainFeatures, "CC Settings", "setting on your ings", pageMisc.OpenMenu);
        
        //groups (by group)
        //grp/general
        var grpGeneral = new ButtonGroup(pageGen, "General", true);
        new VRCToggle(grpGeneral, "Force grab", ForceGrab.handleToggle, ForceGrab.enabled);
        new VRCButton(grpGeneral, "Set Avi Height", "I hate you all",
            () => UIHelper.CcInputmenu("set avi height like a fucking faggot I HATE YOU",
                s => AviSizeShitterOrSomethingFuckYou.setSize(float.Parse(s)), InputPopupType.Numeric));
        new VRCToggle(grpGeneral, "Anti TP", b => AntiTp.enabled = b);
        //new VRCToggle(grpGeneral, "Anti Theft", b => AntiTheft.enabled = b);
        new VRCToggle(grpGeneral, "Economy Spoof", b => EcoSpoof.enabled = b, EcoSpoof.enabled);
        //new VRCToggle(grpGeneral, "Block Avi Loading", b => BlockAviLoad.enabled = b, BlockAviLoad.enabled);
        PickupsMenu.Init(grpGeneral);
        
        //grp/qol
        var grpQol = new ButtonGroup(pageQol, "QOL", true);
        //new VRCToggle(grpQol, "Join/Leave notifs", JoinLeaveNotifs.togglehandle, true);
        new VRCToggle(grpQol, "QM Freeze", b => Freeze.qmFreeze = b, Freeze.qmFreeze);
        new VRCToggle(grpQol, "VR gestures", b => CCKeybindManager.vrKeybinds = b, CCKeybindManager.vrKeybinds);
        new VRCToggle(grpQol, "VR 2xjump fly", b => CCKeybindManager.doubletap = b, CCKeybindManager.doubletap);
        new VRCToggle(grpQol, "Desktop Binds", b => CCKeybindManager.deskyKeybinds = b, CCKeybindManager.deskyKeybinds);
        InstanceHistoryMenu.init(grpQol, pageQol);
        new VRCButton(grpQol, "Clear History", "self explanitory", CCWorldUtils.instanceHistory.Clear);
        new VRCButton(grpQol, "Join Instance\nBy ID", "join by id", () => UIHelper.CcInputmenu("instance id", s => VRC.SDKBase.Networking.GoToRoom(s), defaultText:"worldid:instanceid"));
        new VRCButton(grpQol, "Set Avatar\nBy ID", "set by id", () => UIHelper.CcInputmenu("avtr id", Utils.CloneAvatarEVILassTEMPLEOSISOmethodGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEX_the_fitnessgram_pacer_test_is_a_multistage_aerobic_capacity_test_that_progressively_gets_more_difficult_as_it_continues_the_20_meter_pacer_test_will_begin_in_30_seconds_line_up_at_the_start_the_running_speed_starts_slowly_but_gets_faster_each_minute_after_you_hear_this_signal_boop_a_single_lap_should_be_completed_each_time_you_hear_this_sound_ding_remember_to_run_in_a_straight_line_and_run_as_long_as_possible_the_second_time_you_fail_to_complete_a_lap_before_the_sound_your_test_is_over_the_test_will_begin_on_the_word_start_on_your_mark_get_ready_start , defaultText:"meow"));
        
        
        //grp/movement
        var grpMovement = new ButtonGroup(pageMove, "Movement", true);
        Fly.toggle = new VRCToggle(grpMovement, "Fly", Fly.handleToggle);
        new VRCToggle(grpMovement, "Roblox Fly", s => Fly.roblox = s);
        new VRCToggle(grpMovement, "Speed", Speed.togglehandle);
        new VRCToggle(grpMovement, "Force jump", Forcejump.togglehandle);
        new VRCToggle(grpMovement, "Infinite jump", b => Infjump.enabled = b);
        Freeze.toggle = new VRCToggle(grpMovement, "Freeze", Freeze.togglehandle);
        new VRCToggle(grpMovement, "Snapback", Snapback.togglehandle);
        
        new VRCSlider(pageMove, "Fly speed", "Multiplier for fly speed", Fly.handleSlider, 1, 0.5f, 5);
        new VRCSlider(pageMove, "Speed", "Multiplier for speed", Speed.set, 2f, 0.5f, 10f);
        
        //grp/visual
        var grpVisual = new ButtonGroup(pageVis, "Visual", true);
        new VRCToggle(grpVisual, "ESP", ESP.togglehandle);
        new VRCToggle(grpVisual, "Line ESP", b => CCCoolEspController.line = b);
        new VRCToggle(grpVisual, "Compatible Line ESP", b => CCCoolEspController.linevr = b);
        new VRCToggle(grpVisual, "Box ESP", b => CCCoolEspController.box = b);
        new VRCToggle(grpVisual, "Bone ESP", b => CCCoolEspController.bone = b);
        new VRCToggle(grpVisual, "Item ESP", ItemESP.togglehandle);
        
        //grp/network
        var grpNetwork = new ButtonGroup(pageNet, "Network/Udon", true);
        new VRCToggle(grpNetwork, "Udon Blocker", b => UdonBlocker.enabled = b);
        new VRCToggle(grpNetwork, "<-- Block Sync?", b => UdonBlocker.enabled = b);
        new VRCToggle(grpNetwork, "Block Local Udon Invocation", b => UdonBlocker.invokation = b);
        new VRCToggle(grpNetwork, "Udon Logger", b => UdonLogger.enabled = b);
        new VRCToggle(grpNetwork, "<-- Log Invocation", b => UdonLogger.logInvokation = b);
        new VRCButton(grpNetwork, "Dump Udon", "Dumps all udon events in this scene",
            () => UdonDumper.dumpAll(new CCFileLogger($"CCData/Dumps/{DateTime.Now.ToString("s", DateTimeFormatInfo.InvariantInfo)}.udon.txt")));
        Deserialize.toggle = new VRCToggle(grpNetwork, "Deserialize", Deserialize.handleToggle);
        new VRCToggle(grpNetwork, "Name Spoof", b => NameSpoof.spoof = b);
        new VRCButton(grpNetwork, "Custom Name", "empty to reset",
            () => UIHelper.CcInputmenu("Custom Name", NameSpoof.customSpoof));
        new VRCToggle(grpNetwork, "Fast Sync", Fastsync.handlefag, Fastsync.enabled);
        E12Invis.btn = new VRCToggle(grpNetwork, "Invisibility", E12Invis.handleToggle);
        
        //usr
        var selUserMenu = UIHelper.CCUserSelGrp = new CollapsibleButtonGroup(userSelectedMenu, "Catclient User Menu", true).buttonGroup;
        new VRCButton(selUserMenu, "tp to", "zip", Teleport.TPToSelectedPlayer);
        new VRCButton(selUserMenu, "copy avi id", "copy their id", () =>  Utils.GetPlayerByUser(Utils.selectedPlayerName()).vrcPlayer.field_Private_ApiAvatar_0.id.CopyToClipboard2() );
        new VRCButton(selUserMenu, "delete player", "block without the pleasure", () => Utils.GetPlayerByUser(Utils.selectedPlayerName()).playerObject.SetActive(false));
        //new VRCButton(selUserMenu, "delete player", "block without the pleasure", () => Utils.GetPlayerByUser(Utils.selectedPlayerName()).playerObject.SetActive(false));
        
        //grp/world
        var grpWorld = new ButtonGroup(pageWorld, "World", true);
        Murder4.MurderUi(grpWorld, selUserMenu);
        AmongUs.AmongUsUi(grpWorld, selUserMenu);
        
        //grp/CC settttings/ui
        var grpMiscMenuBtn = new ButtonGroup(pageMisc, "UI");
        new VRCToggle(grpMiscMenuBtn, "CC Tab", b => CCSettings.tabEnabled = b, CCSettings.tabEnabled);
        new VRCToggle(grpMiscMenuBtn, "CC Button", b => CCSettings.buttonEnabled = b, CCSettings.buttonEnabled);
        new VRCToggle(grpMiscMenuBtn, "QM Console", b => CCSettings.QMConsoleEnabled = b, CCSettings.QMConsoleEnabled);
        
        
        //new VRCLable(grpMisc, "NOTICE",
            //"these will only update when you restart your game\nif both are disabled, tab will be re-enabled");
        
        
        
        //init done
        CCEventSystem.CCInitializedInvoke();
    }

    public static void initFeatures()
    {
        var uid = APIUser.CurrentUser.id;
        ModMain.instance.Log.LogInfo(uid);
        var response = new HttpClient()
            .GetAsync(
                $"https://catnotadog.dev/catclient/User/Authenticate?key={ModMain.apikey.Trim()}&user={uid}&checkFor=default")
            .Result;
        //ModMain.instance.Log.LogMessage(response.ToString());
        //var authSuccess = bool.TryParse(response, out var success);
        if (response.StatusCode != HttpStatusCode.Accepted)
        {
            Application.Quit(0);
        }
        var staff = new HttpClient()
            .GetAsync(
                $"https://catnotadog.dev/fs/staff_hashes.txt")
            .Result.Content.ReadAsStringAsync().Result.Split('\n');
        ModMain.staffHashes = staff.ToList();
        CCEventSystem.OnFixedUpdate += Fly.update;
        CCEventSystem.OnSceneLoaded += Fly.sceneLoaded;
        CCEventSystem.OnSceneLoaded += AntiTheft.OnSceneLoaded;
        CCEventSystem.OnSceneUnloaded += Fly.sceneUnloaded;
        CCEventSystem.OnSceneUnloaded += Speed.sceneunload;
        CCEventSystem.OnSceneUnloaded += Forcejump.sceneunload;
        CCEventSystem.OnSceneUnloaded += CCPickupUtils.handleSceneUnload;
        CCEventSystem.OnSceneUnloaded += ItemESP.onSceneChanged;
        //CCEventSystem.OnSceneUnloaded += PickupsMenu.onsceneunload;
        CCEventSystem.OnUpdate += Infjump.OnUpdate;
        CCEventSystem.OnUpdate += Freeze.update;
        CCEventSystem.OnUpdate += CCKeybindManager.update;
        CCEventSystem.OnUpdate += Fastsync.imHomophobic;
        CCEventSystem.OnUpdate += AntiTheft.onupdate;
        PlayerHooks.patch();
        InputHelper.setup();
        ClassInjector.RegisterTypeInIl2Cpp<CCCoolEsp>();
        ClassInjector.RegisterTypeInIl2Cpp<CCCoolEspManager>();
        ClassInjector.RegisterTypeInIl2Cpp<CCNameplate>();
        ModMain.CCObj = new GameObject("CCv3");
        UnityEngine.Object.DontDestroyOnLoad(ModMain.CCObj.AddComponent<CCCoolEspManager>());
    }
}