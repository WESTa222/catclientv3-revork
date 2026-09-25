using catclientv3.misc;
using catclientv3.OtherUI.EdenToast;
using UnityEngine;
using WorldAPI.ButtonAPI.Buttons;

namespace catclientv3.features.movement;

public class Freeze
{
    internal static bool enabled;
    internal static bool qmFreeze {
        get
        {
            var a = CCPersistance.tryGetSetting("CCQMFreeze", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("CCQMFreeze", value);
        }
}
    internal static VRCToggle toggle;
    public static Vector3 originalGravity;
    
    internal static void update()
    {
        if(enabled || qmFreeze && UIHelper.QMOpen)
            Utils.localplayer.field_Private_VRCPlayerApi_0.SetVelocity(Vector3.zero);
    }

    internal static void single()
    {
        Utils.localplayer.field_Private_VRCPlayerApi_0.SetVelocity(Vector3.zero);
    }

    internal static void togglehandle(bool val)
    {
        enabled = val;
        originalGravity = val ? (Fly.enabled ? Fly.originalGravity : Physics.gravity) : originalGravity;
        Physics.gravity = val ? Vector3.zero : originalGravity;
        ToastNotification.ForceToast("freeze " + val, icon: UIHelper.placeholder);
    }
}