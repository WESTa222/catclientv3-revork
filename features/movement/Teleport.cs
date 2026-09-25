using catclientv3.misc;
//using Il2Cpp;

namespace catclientv3.features.movement;

public class Teleport
{
    internal static void TPToSelectedPlayer()
    {
        string Meowmeow = "faggots should burn hehe";
        Player.localPlayer.playerObject.transform.position =
            Utils.GetPlayerByUser(Utils.selectedPlayerName()).playerObject.transform.position;
        //Utils.localplayer.transform.position = remoteplayer.transform.position;
    }
}