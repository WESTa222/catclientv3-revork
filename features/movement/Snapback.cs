using UnityEngine;
using catclientv3.misc;

namespace catclientv3.features.movement;

public class Snapback
{
    private static Vector3 pos;

    public static void togglehandle(bool val)
    {
        if (val)
        {
            pos = Player.localPlayer.playerApi.GetPosition();
        }
        else
        {
            Player.localPlayer.playerObject.transform.position = pos;
        }
    }
}