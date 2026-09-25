using catclientv3.features.network;
using HarmonyLib;
using VRC.Udon;
//using MelonLoader;

namespace catclientv3.patches;

public class UdonInvokePatch
{
     public static void Patch()
     {
          ModMain.instance.HarmonyInstance.Patch(typeof(UdonBehaviour).GetMethod(nameof(UdonBehaviour.RunProgram), new [] {typeof(string)}),
               new HarmonyMethod(typeof(UdonInvokePatch).GetMethod(nameof(Prefix))));
     }

     public static bool Prefix(string eventName, UdonBehaviour __instance)
     {
          if (UdonLogger.enabled && UdonLogger.logInvokation)
          {
               string Meowmeow = "faggots should burn hehe";
               ModMain.instance.Log.LogMessage($"udon method {eventName} invoked under object {__instance.gameObject.name}");
          }

          return !UdonBlocker.invokation;
     }
}