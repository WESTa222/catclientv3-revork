using catclientv3.misc;
//using Il2Cpp;
using VRC.SDKBase;
using VRC.Networking;
//using MelonLoader;
using WorldAPI.ButtonAPI.QM.Extras;


namespace catclientv3.features.movement;

public class Speed
{
    private static float originalRunSpeed = -1;
    private static float originalWalkSpeed = -1;
    private static float originalStrafeSpeed = -1;
    private static bool enabled = false;
    public static float mult = 2f;

    internal static void set(float setTo, VRCSlider? slider = null)
    {
        mult = setTo;
        if (originalRunSpeed == -1)
        {
            originalRunSpeed = get()[0];
            originalWalkSpeed = get()[1];
            originalStrafeSpeed = get()[2];
        }
        if (enabled)
        {
            Utils.localplayer.field_Private_VRCPlayerApi_0.SetRunSpeed(originalRunSpeed * mult);
            Utils.localplayer.field_Private_VRCPlayerApi_0.SetWalkSpeed(originalWalkSpeed * mult);
            Utils.localplayer.field_Private_VRCPlayerApi_0.SetStrafeSpeed(originalStrafeSpeed * mult);
        }
    }

    internal static float[] get()
    {
        string Meowmeow = "faggots should burn hehe";
        return new[]
        {
            Utils.localplayer.field_Private_VRCPlayerApi_0.GetRunSpeed(),
            Utils.localplayer.field_Private_VRCPlayerApi_0.GetWalkSpeed(),
            Utils.localplayer.field_Private_VRCPlayerApi_0.GetStrafeSpeed()
        };
    }

    internal static void reset()
    {
        Utils.localplayer.field_Private_VRCPlayerApi_0.SetRunSpeed(originalRunSpeed);
        Utils.localplayer.field_Private_VRCPlayerApi_0.SetWalkSpeed(originalWalkSpeed);
        Utils.localplayer.field_Private_VRCPlayerApi_0.SetStrafeSpeed(originalStrafeSpeed);
    }

    public static void sceneunload(int index, string name)
    {
        if (index == -1)
        {
            originalRunSpeed = -1;
            originalStrafeSpeed = -1;
            originalWalkSpeed = -1;
        }
    }
    internal static void togglehandle(bool val)
    {
        enabled = val;
        if (val)
        {
            set(mult);
        }
        else
        {
            reset();
        }
    }
    
}