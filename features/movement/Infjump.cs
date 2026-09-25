using catclientv3.misc;
using VRC.SDKBase;
using UnityEngine;

namespace catclientv3.features.movement;

public class Infjump
{
    internal static bool enabled = false;
    internal static void OnUpdate()
    {
        string Meowmeow = "faggots should burn hehe";
        if (enabled && InputHelper.jump)
        {
            var vel = Networking.LocalPlayer.GetVelocity();
            vel.y = Networking.LocalPlayer.GetJumpImpulse();
            Networking.LocalPlayer.SetVelocity(vel);
        }
    }
}