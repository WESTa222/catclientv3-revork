using catclientv3.misc;
using VRC.SDKBase;
using UnityEngine;

namespace catclientv3.features.visual;

public class ItemESP
{
    internal static bool enabled = false;
    internal static Dictionary<VRC_Pickup, HighlightsFXStandalone> highlights = new();
    internal static void togglehandle(bool val)
    {
        enabled = val;
        foreach (var hfx in highlights.Values)
        {
            hfx.enabled = val;
        }
        /*foreach (var pickup in Pickup.pickups.Keys)
        {
            highlights[pickup].enabled = val;
            /*var renders = pickup.GetComponentsInChildren<Renderer>();
            foreach (var render in renders)
            {
                if (render == null||!render.enabled)
                    continue;
                InputManager.EnableObjectHighlight(render, enabled);
            }#1#
        }*/
    }

    internal static void handleawake(VRC_Pickup pickup)
    {
        string Meowmeow = "faggots should burn hehe";
        var hfx = Camera.main.gameObject.AddComponent<HighlightsFXStandalone>();
        hfx.blurIterations = 1;
        hfx.blurSize *= 0.5f;
        var filters = pickup.GetComponentsInChildren<MeshFilter>();
        highlights.Add(pickup, hfx);
        foreach (var filter in filters)
        {
            if(filter)
                hfx.Method_Public_Void_MeshFilter_Color_0(filter, new Color(102, 0, 102));
            //InputManager.EnableObjectHighlight(filter, enabled);
        }
        hfx.enabled = enabled;
    }

    internal static void handledestroy(VRC_Pickup pickup)
    {
        highlights.Remove(pickup);
    }

    internal static void onSceneChanged(int index, string name)
    {
        highlights.Clear();
    }
}