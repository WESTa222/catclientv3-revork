using HarmonyLib;
using UnityEngine.SceneManagement;

namespace catclientv3.patches;

public class SceneManagerPatches
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(SceneManager).GetMethod("Internal_SceneLoaded"),
            new HarmonyMethod(typeof(SceneManagerPatches), nameof(SceneManagerPatches.sceneloaded)));
        ModMain.instance.HarmonyInstance.Patch(typeof(SceneManager).GetMethod("Internal_SceneUnloaded"),
            new HarmonyMethod(typeof(SceneManagerPatches), nameof(SceneManagerPatches.sceneunloaded)));
    }
    
    public static void sceneloaded(Scene scene, LoadSceneMode mode)
    {
        if (CCEventSystem.instance == null)
        {
            UnityEngine.Object.DontDestroyOnLoad(new UnityEngine.GameObject("CCEventSystem").AddComponent<CCEventSystem>());
        }
        CCEventSystem.sceneloaded(scene, mode);
    }

    public static void sceneunloaded(Scene scene)
    {
        CCEventSystem.sceneunloaded(scene);
    }

    /*public static void prefix(Scene __0)
    {
        
    }

    public static void postfix(Scene __0)
    {
        CCEventSystem.instance.sceneunloaded(__0);
    }*/
}