using catclientv3.features.movement;
using catclientv3.misc;
using catclientv3.OtherUI.EdenToast;
//using Il2Cpp;
using VRC.SDKBase;
using VRC.Networking;
using WorldAPI.ButtonAPI.Buttons;

namespace catclientv3.features.network;

public class Deserialize
{
    internal static VRCToggle toggle;
    internal static bool enabled = false;
    internal static void handleToggle(bool val)
    {
        enabled = val;
        //Networking.LocalPlayer.gameObject.GetComponent<MonoBehaviour1PrivateIFlatBufferNetworkSerializerILoggableClassHa1ObVeObBoVeSpBoSyUnique>().enabled = !val;
        //Networking.LocalPlayer.gameObject.GetComponent<FlatBufferNetworkSerializer>().enabled = !val;
        ToastNotification.ForceToast("deserialize " + val,  icon: UIHelper.ghost);
        if (!E12Invis.enabled)
        {
            UIHelper.ghosticonhud.gameObject.SetActive(val);
        }
    }
}