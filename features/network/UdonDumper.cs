using System.Text;
using catclientv3.misc;
using Il2CppInterop.Runtime;
using Il2CppSystem.Collections;
using VRC.Udon;
//using MelonLoader;
//using MelonLoader.Logging;
    //using MelonLoader.Pastel;
using Unity.Collections;
using UnityEngine;
using Logger = catclientv3.misc.Logger;

namespace catclientv3.features.network;

public class UdonDumper
{
    internal static void dumpAll(Logger? logger = null)
    {
        foreach (var udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
        {
            dumpSingle(udon.gameObject, logger);
        }
    }

    public static void dumpSingle(GameObject gayobject, Logger? logger = null)
    {
        if (logger == null)
        {
            logger = ModMain.instance.Log;
        }
        //StringBuilder faggot;
        //string writethistofile;
        foreach (var udon in gayobject.GetComponents<UdonBehaviour>())
        {
            logger.LogMessage($"---{Utils.GetGameObjectPath(udon.gameObject)}---");
            logger.LogMessage("---EVENTS---");
            foreach (var key in udon._eventTable)
            {
                logger.LogMessage($"Found udon event {key.key}");
            }
            logger.LogMessage("---HEAP---");
            for (uint i = 0; i < udon._program.Heap.GetHeapCapacity(); i++)
            {
                try
                {
                    if (udon._program.Heap.TryGetHeapVariable(i, out Il2CppSystem.Object obj))
                    {
                        logger.LogMessage($"symbol: {(udon._program.SymbolTable.TryGetSymbolFromAddress(i, out var s) ? s : "no symbol")}, address: {i}, type: {obj.GetIl2CppType().ToString()},  value: {obj.ToString()}");
                        /*CCLogger.Log(ConsoleColor.Gray, "symbol: ",
                            ConsoleColor.Red, udon._program.SymbolTable.TryGetSymbolFromAddress(i, out var str) ? str : "no symbol",
                            ConsoleColor.Gray, "   address: ",
                            ConsoleColor.White, i.ToString(),
                            ConsoleColor.Gray, "  type: ",
                            ConsoleColor.Cyan,  obj.GetIl2CppType().ToString()/*udon._program.Heap.GetHeapVariableType(i).ToString()#1#,
                            ConsoleColor.Gray, "  value: ",
                            ConsoleColor.White, obj.ToString(), "\n");*/
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    // this faggot bro
                }
            }
            logger.LogMessage($"---END---");
            Console.ResetColor();
        }
    }
}