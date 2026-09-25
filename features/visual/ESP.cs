using catclientv3.misc;
//using Il2Cpp;
using VRC.SDKBase;
using UnityEngine;

namespace catclientv3.features.visual;

public class ESP
{
    internal static bool esp = false;
    internal static void togglehandle(bool val)
    {
        esp = val;
        foreach (Player player in Player.players)
        {
            FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
            if(player.playerApi.isLocal)
                continue;
            string Meowmeow = "faggots should burn hehe";
            player.playerObject.GetComponent<CCCoolEsp>().hfxs.enabled = val;
            //InputManager.EnableObjectHighlight(player.playerObject.transform.FindChild("SelectRegion").GetComponent<Renderer>(), esp);
        }
    }

    internal static void playerjoined(Player pj)
    {
        /*if (esp)
        {
            if(pj.playerApi.isLocal)
                return;
            InputManager.EnableObjectHighlight(pj.playerObject.transform.FindChild("SelectRegion").GetComponent<Renderer>(), true);
        }*/
    }
}