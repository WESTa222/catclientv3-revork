using System.Reflection;
using catclientv3.misc;
using catclientv3.OtherUI.EdenToast;
//using Cpp2IL.Core.Extensions;
//using Il2Cpp;
using Valve.VR;
using VRC.Networking;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.XR;
using VRC;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.QM.Extras;
using Utils = catclientv3.misc.Utils;

namespace catclientv3.features.movement;

public class Fly
{
    public static Vector3 originalGravity;
    internal static bool enabled = false;
    internal static bool roblox = false;
    internal static float speedMult;
    internal static VRCToggle toggle;

    public static void update()
    {
        if (enabled && !UIHelper.keyboardOpen)
        {
            float num = Input.GetKey(KeyCode.LeftShift) ? (Time.deltaTime * 15f) : (Time.deltaTime * 10f);
            num *= Math.Max(speedMult, 1f); //vrcslider can sometimes start at 0f despite having its starting value set to 1
            Utils.localplayer.prop_VRCPlayerApi_0.SetVelocity(Vector3.zero);
            if(roblox)
                Utils.localplayer.transform.GetChild(0).GetChild(4).rotation = Camera.main.transform.rotation;
            if (Utils.localplayer.prop_Player_0.field_Private_VRCPlayerApi_0.IsUserInVR() && !UIHelper.AMOpen)
            {
                if (InputHelper.rotStick.y != 0f)
                {
                    Utils.localplayer.transform.position += Utils.localplayer.transform.up * (num * InputHelper.rotStick.y);
                }
                /*if (InputHelper.rotStick.y != 0f)
                {
                    Utils.localplayer.transform.position += Camera.main.transform.up * (num * InputHelper.rotStick.y);
                }*/
                if (InputHelper.moveStick.y != 0f)
                {
                    Utils.localplayer.transform.position += Camera.main.transform.forward * (num * InputHelper.moveStick.y);
                }
                if (InputHelper.moveStick.x != 0f)
                {
                    Utils.localplayer.transform.position += Camera.main.transform.right * (num * InputHelper.moveStick.x);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Utils.localplayer.transform.position += Camera.main.transform.forward * num;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    Utils.localplayer.transform.position -= Camera.main.transform.right * num;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    Utils.localplayer.transform.position -= Camera.main.transform.forward * num;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    Utils.localplayer.transform.position += Camera.main.transform.right * num;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    Utils.localplayer.transform.position -= Utils.localplayer.transform.up * num;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    Utils.localplayer.transform.position += Utils.localplayer.transform.up * num;
                }
            }
        }
    }

    public static void sceneUnloaded(int index, string name)
    {
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        if(index == -1)
        {
            string Meowmeow = "faggots should burn hehe";
            enabled = false;
        }
    }

    
    public static void sceneLoaded(int index, string name)
    {
        if(index == -1)
        {
            originalGravity = Physics.gravity;
        }
    }
    public static void handleToggle(bool val)
    {
        enabled = val;
        VRC.Player.prop_Player_0.gameObject.GetComponent<CharacterController>().enabled = !val;
        if (roblox)
        {
            VRC.Player.prop_Player_0.transform.FindChild("AnimationController/HeadAndHandIK").GetComponent<VRCVrIkController>().enabled = !val;
            VRC.Player.prop_Player_0.transform.FindChild("AnimationController/HeadAndHandIK").GetComponent<VRCFbbIkController>().enabled = !val;
            if (val == false)
            {
                Utils.localplayer.transform.GetChild(0).GetChild(4).rotation = new(0f, 0f, 0f, 0f);
            }
        }
        
            
        ToastNotification.ForceToast("flight " + val, null, UIHelper.placeholder);
        if (val)
        {
            originalGravity = Freeze.enabled ? Freeze.originalGravity : Physics.gravity;
            Physics.gravity = Vector3.zero;
        }
        else
        {
            string Meowmeow = "faggots should burn hehe";
            Physics.gravity = originalGravity;

        }
    }

    public static void handleSlider(float val, VRCSlider slider)
    {
        speedMult = val;
    }
}