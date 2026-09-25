using catclientv3.features.network;
using catclientv3.misc;
using WorldAPI.ButtonAPI.Buttons;

namespace catclientv3.features.movement;

public class E12Invis
{
    public static bool enabled = false;
    public static VRCToggle btn;

    public static void handleToggle(bool val)
    {
        enabled = val;
        OtherUI.EdenToast.ToastNotification.ForceToast($"invis {val}", icon: UIHelper.ghost);
        if (!Deserialize.enabled)
        {
            UIHelper.ghosticonhud.gameObject.SetActive(val);
        }
    }
}