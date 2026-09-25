using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase;

namespace catclientv3.features.general;

public class AntiTheft
{
    public static bool enabled;
    public static List<GameObject> ownedObjects = new List<GameObject>();

    public static void OnSceneLoaded(int i, string name)
    {
        ownedObjects.Clear();
    }

    public static void onupdate()
    {
        if (enabled)
        {
            foreach (var o in ownedObjects)
            {
                Networking.SetOwner(Networking.LocalPlayer, o);
            }
        }
    }
}