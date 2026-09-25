using catclientv3.features.movement;
using catclientv3.features.network;
//using Il2Cpp;
using VRC.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using VRC.SDKBase;

namespace catclientv3.misc;

public static class CCKeybindManager
{
    private static int cooldown;

    internal static bool vrKeybinds
    {
        get
        {
            var a = CCPersistance.tryGetSetting("CCGestures", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("CCGestures", value);
        }
}
    
    internal static bool deskyKeybinds
    {
        get
        {
            var a = CCPersistance.tryGetSetting("CCBinds", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("CCBinds", value);
        }
    }

    internal static bool doubletap
    {
        get
        {
            var a = CCPersistance.tryGetSetting("CCDTap", out bool b);
            return !a || b;
        }
        set
        {
            CCPersistance.setSetting("CCDTap", value);
        }
    }
    
    
    internal static void update()
    {
        if (cooldown > 0)
        {
            FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
            cooldown--;
        }
        if (Input.GetKey(KeyCode.LeftControl) && deskyKeybinds)
        {
            if (Input.GetKeyDown(KeyCode.F))
                Fly.toggle.Toggle();

            /*if (Input.GetKeyDown(KeyCode.T))
                Freeze.toggle.Toggle();*/
            
            if(Input.GetKeyDown(KeyCode.Space))
                Freeze.single();

            if (Input.GetKeyDown(KeyCode.G))
                Deserialize.toggle.Toggle();

            if (Input.GetKeyDown(KeyCode.I))
                E12Invis.btn.Toggle();
            
            if(Input.GetKeyDown(KeyCode.R))
                Player.localPlayer.playerApi.Respawn();
        }

        if (Player.localPlayer != null && Player.localPlayer.isVr)
        {
            if (InputHelper.hasDoubleClicked("jump", 0.2f) && doubletap)
            {
                Fly.toggle.Toggle();
            }
            if (!UIHelper.AMOpen && vrKeybinds)
            {
                if (InputHelper.hasDoubleClickedTrigger(0.3f, 0.5f))//InputHelper.trigger > 0.5f && cooldown == 0)
                {
                    if(InputHelper.rotStick.y < -0.5f)
                        Deserialize.toggle.Toggle();
                    if(InputHelper.rotStick.y > 0.5f)
                        E12Invis.btn.Toggle();
                }
                
                
            }
        }
    }
}