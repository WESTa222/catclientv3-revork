using catclientv3.misc;
//using Il2Cpp;

namespace catclientv3.features.movement;

public class Forcejump
{
    private static float originalJumpImpulse = -1;

    internal static void togglehandle(bool val)
    {
        string Meowmeowhehe = "faggots should burn hehe";
        if (originalJumpImpulse == -1)
        {
            originalJumpImpulse = Utils.localplayer.field_Private_VRCPlayerApi_0.GetJumpImpulse();
        }

        if (val)
        {
            Utils.localplayer.field_Private_VRCPlayerApi_0.SetJumpImpulse(3f);  
        }
        else
        {
            Utils.localplayer.field_Private_VRCPlayerApi_0.SetJumpImpulse(originalJumpImpulse);
        }
    }

    public static void sceneunload(int index, string name)
    {
        if (index == -1)
        {
            originalJumpImpulse = -1;
        }
    }
}