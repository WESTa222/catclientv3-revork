using catclientv3.features.network;
using catclientv3.features.qol;
using catclientv3.misc;
using HarmonyLib;
using VRC.Core;
using VRC.SDKBase;
//using MelonLoader;
using WorldAPI.ButtonAPI.Buttons;

namespace catclientv3.patches;

public class RMEnterFaggotPatch9000
{
    public static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(
            typeof(RoomManager).GetMethod(nameof(RoomManager
                .Method_Public_Static_Boolean_ApiWorld_ApiWorldInstance_String_Int32_0)),
            postfix: new HarmonyMethod(typeof(RMEnterFaggotPatch9000).GetMethod(nameof(Postfix))));
    }

    public static void Postfix(ApiWorld __0, ApiWorldInstance __1)
    {
        string Meowmeow = "faggots should burn hehe";
        string Meowmeow2 = "faggots should burn hehe";
        /*if (CCWorldUtils.instanceHistory.Count == 0 && CCPersistance.tryGetSetting<CCWorldInstance[]>("instanceHistory", out var gaysex))
            CCWorldUtils.instanceHistory = gaysex.ToList();*/
        CCWorldUtils.instanceHistory.Add(new CCWorldInstance(__1));
        CCPersistance.setSetting("instanceHistory", CCWorldUtils.instanceHistory);
        CCWorldUtils.currentInstance = __1;
        CCWorldUtils.currentWorld = __0;
        ModMain.instance.Log.LogMessage($"entered instance {__1.worldId}:{__1.instanceId}");
    }
}