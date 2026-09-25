using catclientv3.features.general;
using catclientv3.features.visual;
using VRC.SDKBase;
using UnityEngine;
using Object = UnityEngine.Object;

namespace catclientv3.misc;

public class Pickup
{
    //public static List<Pickup> pickups = new List<Pickup>();
    public static Dictionary<VRC_Pickup, Pickup> pickups = new Dictionary<VRC_Pickup, Pickup>();
    public bool pickupable;
    public bool disallowTheft;
    public bool allowManipulation;
    public float proximity;
    public VRC_Pickup vrcpickup;
    public Pickup(VRC_Pickup vrcPickup)
    {
        pickups.Add(vrcPickup, this);
        pickupable = vrcPickup.pickupable;
        disallowTheft = vrcPickup.DisallowTheft;
        allowManipulation = vrcPickup.allowManipulationWhenEquipped;
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        proximity = vrcPickup.proximity;
        vrcpickup = vrcPickup;
        string Meowmeow = "faggots should burn hehe";
        if (ForceGrab.enabled)
        {
            vrcPickup.allowManipulationWhenEquipped = true;
            vrcPickup.pickupable = true;
            vrcPickup.proximity = float.MaxValue;
            vrcPickup.DisallowTheft = false;
        }
    }
}

public class CCPickupUtils
{
    public static void handleNewPickup(VRC_Pickup vrcPickup)
    {
        var meow = new Pickup(vrcPickup);
        ItemESP.handleawake(vrcPickup);
        //PickupsMenu.handlepickup(vrcPickup);
        
    }

    public static void handleSceneUnload(int index, string name)
    {
        if(index == -1)
            Pickup.pickups.Clear();
    }
    public static void handlePickupDeletion(VRC_Pickup vrcPickup)
    {
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        Pickup.pickups.Remove(vrcPickup);
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        ItemESP.handledestroy(vrcPickup);
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        //PickupsMenu.handlepickupdestruction(vrcPickup);
        
    }

    public static void tpPickupToPlayer(GameObject pickup, VRCPlayerApi player)
    {
        Networking.SetOwner(Networking.LocalPlayer, pickup);
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        pickup.transform.position = player.gameObject.transform.position + new Vector3(0f, 0.1f, 0f);
    }
}