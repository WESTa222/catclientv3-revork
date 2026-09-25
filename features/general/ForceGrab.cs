using catclientv3.misc;

namespace catclientv3.features.general;

public class ForceGrab
{
    //internal static bool enabled = false;
    public static bool enabled 
    {
        get
        {
            var a = CCPersistance.tryGetSetting("forceGrabEnabled", out bool b);
            return a && b;
        }
        set
        {
            CCPersistance.setSetting("forceGrabEnabled", value);
        }
    }
    internal static bool toggling = false;

    internal static void handleToggle(bool val)
    {
        enabled = val;
        toggling = true;
        foreach (var kvp in Pickup.pickups)
        {
            kvp.Key.DisallowTheft = !val && kvp.Value.disallowTheft;
            kvp.Key.pickupable = val || kvp.Value.pickupable;
            kvp.Key.allowManipulationWhenEquipped = val || kvp.Value.allowManipulation;
            kvp.Key.proximity = val ? float.MaxValue : kvp.Key.proximity;
        }
        toggling = false;
    }
}