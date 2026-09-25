using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppMicrosoft.Extensions.Logging;
using ZLogger;
//using Microsoft.Extensions.Logging;

namespace catclientv3.patches;

public class SHUTTHEFUCKUPVRCHAT
{
    internal static void Patch()
    {
        ModMain.instance.HarmonyInstance.Patch(
            typeof(VRC.Core.VRCLogger).GetMethod(nameof(VRC.Core.VRCLogger.Log), new Type[]{typeof(ILogger), typeof(ZLoggerDebugInterpolatedStringHandler)}),
            new HarmonyMethod(typeof(SHUTTHEFUCKUPVRCHAT), nameof(noRun)));
        ModMain.instance.HarmonyInstance.Patch(
            typeof(VRC.Core.ZLoggerHandlerLogger).GetMethod(nameof(VRC.Core.ZLoggerHandlerLogger.LogFormat), new[] { typeof(UnityEngine.LogType), typeof(UnityEngine.Object), typeof(string), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) }),
            new HarmonyMethod(typeof(SHUTTHEFUCKUPVRCHAT), nameof(noRun)));
        //ModMain.instance.HarmonyInstance.Patch(AccessTools.Method("VRC.Core.ZLoggerHandlerLogger:LogFormat", new[] { typeof(UnityEngine.LogType), typeof(UnityEngine.Object), typeof(string), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) }), new HarmonyMethod(typeof(SHUTTHEFUCKUPVRCHAT), nameof(noRun)));
    }
    
    internal static bool noRun() => false;
}