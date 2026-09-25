using System.Collections;
using catclientv3.misc;
using UnityEngine;
using VRC.SDKBase;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.Groups;

namespace catclientv3.features.general;

public static class PickupsMenu
{
    public static ButtonGroup faggrp;
    public static VRCPage meow;
    //public static Dictionary<VRC_Pickup, VRCButton> pickupbuttons = new Dictionary<VRC_Pickup, VRCButton>();
    
    /*public static void handlepickup(VRC_Pickup pickup)
    {
        pickupbuttons.Add(pickup, new VRCButton(faggrp, pickup.transform.name, "bring this pickup", () => CCPickupUtils.tpPickupToPlayer(pickup.gameObject, Networking.LocalPlayer)));
    }

    public static void handlepickupdestruction(VRC_Pickup pickup)
    {
        GameObject.DestroyImmediate(pickupbuttons[pickup].gameObject);
    }*/

    public static void openmenu()
    {
        CCEventSystem.StartCoroutinePls(loadbuttons());
        meow.OpenMenu();
    }

    public static IEnumerator loadbuttons()
    {
        faggrp.gameObject.DestroyChildren();
        foreach (var pickup in Pickup.pickups)
        {
            new VRCButton(faggrp, pickup.Key.name, "bring this pickup", () => CCPickupUtils.tpPickupToPlayer(pickup.Key.gameObject, Networking.LocalPlayer));
        }
        yield return new WaitForEndOfFrame();
    }

    public static void Init(ButtonGroup grp)
    {
        meow = new VRCPage("pickups");
        new VRCButton(grp, "Pickup Menu", "bring yourself pickups", () => openmenu());
        faggrp = new ButtonGroup(meow, "Bring Pickup");
    }
}