namespace catclientv3.features.qol;

public class JoinLeaveNotifs
{
    internal static bool enabled;

    internal static void togglehandle(bool val)
    {
        enabled = val;
    }
}