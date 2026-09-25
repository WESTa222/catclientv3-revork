using catclientv3.misc;
using HarmonyLib;
using VRC.SDKBase;

namespace catclientv3.patches;

public class RPCPatch
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(typeof(VRC_EventDispatcherRFC).GetMethod(
            nameof(VRC_EventDispatcherRFC.Method_Public_Boolean_Player_VrcEvent_VrcBroadcastType_0)), postfix:new HarmonyMethod(typeof(RPCPatch).GetMethod(nameof(Postfix))));
    }

    public static void Postfix(VRC.Player __0, VRC_EventHandler.VrcEvent __1, VRC_EventHandler.VrcBroadcastType __2)
    {
        if (__1.ParameterObject.name.Contains("CCv3RPC") && __0 != VRC.Player.prop_Player_0)
        {
            
            //__1.
            CCRPCManager.handleRecv(__1.ParameterString, __0);
        }
    }
}