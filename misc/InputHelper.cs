//using Il2Cpp;
using Valve.VR;
//using MelonLoader;
using UnityEngine;

namespace catclientv3.misc;

public class InputHelper
{
    internal static SteamVR_Action_Boolean Sjump;
    internal static SteamVR_Action_Boolean SopenQM;
    internal static SteamVR_Action_Boolean SmicToggle;
    internal static SteamVR_Action_Boolean Sgrab;
    internal static SteamVR_Action_Single Strigger;
    internal static SteamVR_Action_Vector2 SmoveStick;
    internal static SteamVR_Action_Vector2 SrotStick;
    internal static readonly Dictionary<string, float> lastTime = new Dictionary<string, float>();
    internal static bool triggerReady = true;
    internal static readonly Dictionary<string, SteamVR_Action_Boolean?> SVRbtns = new Dictionary<string, SteamVR_Action_Boolean?>();

    internal static void setup()
    {
    Sjump = SteamVR_Input.GetBooleanAction("jump", false);
    SopenQM = SteamVR_Input.GetBooleanAction("Menu", false);
    SmicToggle = SteamVR_Input.GetBooleanAction("Toggle Microphone", false);
    Sgrab = SteamVR_Input.GetBooleanAction("Grab", false);
    Strigger = SteamVR_Input.GetSingleAction("gesture_trigger_axis", false);
    SmoveStick = SteamVR_Input.GetVector2Action("Move", false);
    SrotStick = SteamVR_Input.GetVector2Action("Rotate", false);
    }
    internal static bool jump
    {
        get
        {
            return Sjump.state || Input.GetKey(KeyCode.Space);
        }
    }
    internal static bool openQM
    {
        get
        {
            return SopenQM.state || Input.GetKey(KeyCode.Escape);
        }
    }
    internal static bool micToggle
    {
        get
        {
            return SmicToggle.state || Input.GetKey(KeyCode.V);
        }
    }
    internal static bool grab
    {
        get
        {
            return Sgrab.state || Input.GetKey(KeyCode.Mouse0);
        }
    }

    internal static Single trigger
    {
        get
        {
            return Strigger.axis;
        }
    }
    

    internal static Vector2 moveStick
    {
        get
        {
            return SmoveStick.axis;
        }
    }
    
    internal static Vector2 rotStick
    {
        get
        {
            return SrotStick.axis;
        }
    }
    
    public static bool hasDoubleClicked(string input, float threshold)
    {
        /*OVRInput.GetResolvedAxis2D(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.RawAxis2D.Any,
            OVRInput.Controller.All);*/
        if (!SVRbtns.ContainsKey(input))
        {
            SVRbtns.Add(input, SteamVR_Input.GetBooleanAction(input, false));
        }
        var btn =  SVRbtns[input];
        if(btn == null || !btn.stateDown)
            return false;
        if (!lastTime.ContainsKey(input))
        {
            lastTime.Add(input, Time.time);
        }
        bool doubleClicked = Time.time - lastTime[input] <= threshold;
        if (doubleClicked)
        {
            lastTime[input] =  Time.time - threshold * 2f;
            return true;
        }
        lastTime[input] = Time.time;
        return false;
    }
    
    public static bool hasDoubleClickedTrigger(float timeThreshold, float triggerThreshold)
        {
            /*OVRInput.GetResolvedAxis2D(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.RawAxis2D.Any,
                OVRInput.Controller.All);*/
            /*if (!SVRbtns.ContainsKey(input))
            {
                SVRbtns.Add(input, SteamVR_Input.GetBooleanAction(input, false));
            }*/
            var input = "trigger";
            var btn = Strigger;
            if (!(btn.axis > triggerThreshold))
            {
                triggerReady = true;
                return false;
            }

            if (!triggerReady)
                return false;
            
            if (!lastTime.ContainsKey(input))
            {
                lastTime.Add(input, Time.time);
            }
            bool doubleClicked = Time.time - lastTime[input] <= timeThreshold;
            if (doubleClicked)
            {
                triggerReady = false;
                lastTime[input] =  Time.time - timeThreshold * 2f;
                return true;
            }
            triggerReady = false;
            lastTime[input] = Time.time;
            return false;
        }
}