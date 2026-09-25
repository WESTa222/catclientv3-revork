using System.Collections;
using System.Reflection;
using System.Text;
using catclientv3;
using catclientv3.misc;
using catclientv3.OtherUI.EdenToast;
using catclientv3.patches;
//using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime;
using VRC.Udon;
using UnityEngine.Events;
using UnityEngine.Playables;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.Groups;
//using BepInEx;
//using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine.SceneManagement;
//using BepInEx.Unity.IL2CPP.Utils.Collections;
using MelonLoader;
using Object = System.Object;

[assembly: MelonInfo(typeof(ModMain), "catclient v3", "0.0.1", "catnotadog")]

namespace catclientv3;

//[BepInPlugin("catclientv3", "catclientv3", "0.0.2")]
public class ModMain : MelonMod
{
    public static ModMain instance;
    public HarmonyLib.Harmony HarmonyInstance;
    public static string apikey;
    public static List<string> staffHashes = new List<string>();
    public static GameObject CCObj;
    public CCLogger Log = new CCLogger();

    public override void OnInitializeMelon()
    {
        //ClassInjector.RegisterTypeInIl2Cpp<CCEventSystem>();
        instance = this;
        HarmonyInstance = new HarmonyLib.Harmony("catclientv3");
        SceneManagerPatches.Patch();
        CCPersistance.loadSettings();
        Console.OutputEncoding = Encoding.UTF8;
        Log.LogMessage("HIIIII IM LOADED, waiting for QM");
        Directory.CreateDirectory("CCData/Dumps");
        /*if (!File.Exists("UserLibs/Newtonsoft.Json.dll"))
        {
            File.WriteAllBytes("UserLibs/Newtonsoft.Json.dll", new HttpClient().GetByteArrayAsync("https://cdn.nightshade.ltd/81a340ec-bae4-40ae-a2fc-cb5acad81668/5a06b37e-3195-4cac-904b-3fbb6a4cd902/Newtonsoft.Json.dll").Result);
        }*/
        if (File.Exists("CCData/CCKey"))
        {
            apikey = File.ReadAllText("CCData/CCKey");
        }
        else
        {
            apikey = "nofile";
        }
        ClassInjector.RegisterTypeInIl2Cpp<CCEventSystem>();
        var shit = new GameObject("meowdy");
        var piss = shit.AddComponent<CCEventSystem>();
        UnityEngine.Object.DontDestroyOnLoad(piss);
        piss.enabled = true;
        MelonCoroutines.Start(WaitForQM());
        //BepInEx.Unity.IL2CPP.Utils.MonoBehaviourExtensions.
        //HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        AvatarLoadingPatch.Patch();
        PhotonReceivedPatch.Patch();
        PickupsPatches.Patch();
        VRCPlusSpoof.Patch();
        //Init.initFeatures();
        PhotonSentPatch.Patch();
        CloningAllowedPatch.Patch();
        //SHUTTHEFUCKUPVRCHAT.Patch();
        UdonInvokePatch.Patch();
        RMEnterFaggotPatch9000.Patch();
        TeleportPatch.Patch();
        RespawnPatch.Patch();
        EcoPatchOfDOOMandDESPAIR.Patch();
        RPCPatch.Patch();
        OwnershipPatch.Patch();
        CCWorldUtils.loadHistory();
    }

    public override void OnApplicationQuit()
    {
        CCPersistance.saveSettings();
    }

    public static IEnumerator WaitForQM()
    {
        while (GameObject.Find("Canvas_QuickMenu(Clone)") == null)
        {
            yield return null;
        }

        WorldAPI.APIBase.IsReady();
        instance.Log.LogMessage("QM found!!!!");
        Init.initFeatures();
        Init.initUI();
    }
}

public class CCEventSystem : MonoBehaviour
{
    public static CCEventSystem instance;
    public delegate void OnUpdateHandler();
    public delegate void OnFixedUpdateHandler();
    public delegate void OnSceneLoadedHandler(int index, string name);
    public delegate void OnSceneUnloadedHandler(int index, string name);
    
    public delegate void OnCCDoneLoading(ModMain mainmod);
    public delegate void OnCCDoneInitializing();
    
    public static event OnUpdateHandler? OnUpdate;
    public static event OnFixedUpdateHandler? OnFixedUpdate;
    public static event OnSceneLoadedHandler? OnSceneLoaded;
    public static event OnSceneUnloadedHandler? OnSceneUnloaded;
    public static event OnCCDoneLoading? CCLoaded;
    public static event OnCCDoneInitializing? CCInitialized;

    private void Update()
    {
        //ModMain.instance.Log.LogMessage("CCEventSystem exists");
        OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }

    private void Awake()
    {
        instance = this;
        ModMain.instance.Log.LogMessage("CCEventSystem Awake");
        CCLoadedInvoke(ModMain.instance); 
        OnSceneLoaded += CCRPCManager.sceneLoad; //special treatment
        //SceneManager.sceneLoaded += sceneloaded;
        //SceneManager.sceneUnloaded += sceneunloaded;
        //nameof(SceneManager.load)
    }
 
    public static void sceneloaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke(scene.buildIndex, scene.name);
    }

    public static void sceneunloaded(Scene scene)
    {
        OnSceneUnloaded?.Invoke(scene.buildIndex, scene.name);
    }

    internal static void CCLoadedInvoke(ModMain mainmod)
    {
        CCLoaded?.Invoke(mainmod);
    }

    internal static void CCInitializedInvoke()
    {
        CCInitialized?.Invoke();
    }

    public static void StartCoroutinePls(IEnumerator coroutine)
    {
        MelonCoroutines.Start(coroutine);
        //instance.StartCoroutine(coroutine.ca);
    }
}