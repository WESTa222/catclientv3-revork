using System.Diagnostics.CodeAnalysis;
using System.Linq;
using catclientv3.misc;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Groups;
using Utils = catclientv3.misc.Utils;

namespace catclientv3.features.worlds;

[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
public static class AmongUs
{
	private static VRCPage _userPage;
	private static VRCPage _mainPage;
	private static ButtonGroup _userRootButtonGroup;
	private static ButtonGroup _mainRootButtonGroup;
	private static GameObject gameLogic => GameObject.Find("Game Logic");

	internal static void AmongUsUi(ButtonGroup main, ButtonGroup user)
	{
		_userRootButtonGroup = user;
		_mainRootButtonGroup = main;
		_mainPage = new VRCPage("Among Us");
		_userPage = new VRCPage("Among Us user menu", parentMenuName: "QuickMenuSelectedUserLocal");

		new VRCButton(_mainRootButtonGroup, "Among Us", "Among Us world controls", _mainPage.OpenMenu);
		_mainPage.BackButtonPress = _mainPage.CloseMenu;

		var grpSelf = new CollapsibleButtonGroup(_mainPage, "Self", true);
		new VRCButton(grpSelf, "Crewmate", "Set yourself to crewmate", selfCrewmate);
		new VRCButton(grpSelf, "Impostor", "Set yourself to impostor", selfImpostor);

		var grpGame = new CollapsibleButtonGroup(_mainPage, "Game");
		new VRCButton(grpGame, "Start Game", "Begin the round", gameStart);
		new VRCButton(grpGame, "Abort Game", "Abort the round", gameAbort);
		new VRCButton(grpGame, "Emergency Meeting", "Force emergency meeting", gameMeeting);
		new VRCButton(grpGame, "Report Body", "Force a body report", gameReport);

		var grpVictory = new CollapsibleButtonGroup(_mainPage, "Force Win");
		new VRCButton(grpVictory, "Crewmate Win", "Trigger crewmate victory", victoryCrewmate);
		new VRCButton(grpVictory, "Impostor Win", "Trigger impostor victory", victoryImpostor);

		var grpSabotage = new CollapsibleButtonGroup(_mainPage, "Sabotage");
		new VRCButton(grpSabotage, "Lights", "Sabotage lights", sabotageLights);
		new VRCButton(grpSabotage, "Oxygen", "Sabotage oxygen", sabotageOxygen);
		new VRCButton(grpSabotage, "Reactor", "Sabotage reactor", sabotageReactor);
		new VRCButton(grpSabotage, "Comms", "Sabotage communications", sabotageComms);
		new VRCButton(grpSabotage, "Lock All Doors", "Lock every door", sabotageDoorsAll);

		var grpRepair = new CollapsibleButtonGroup(_mainPage, "Repairs");
		new VRCButton(grpRepair, "Fix Lights", "Repair lights", repairLights);
		new VRCButton(grpRepair, "Fix Oxygen", "Repair oxygen", repairOxygen);
		new VRCButton(grpRepair, "Fix Reactor", "Repair reactor", repairReactor);
		new VRCButton(grpRepair, "Fix Comms", "Repair communications", repairComms);
		new VRCButton(grpRepair, "Cancel Sabotage", "Cancel all sabotage", cancelSabotage);

		new VRCButton(_userRootButtonGroup, "Among Us", "Targeted actions", _userPage.OpenMenu);

		var grpRoles = new CollapsibleButtonGroup(_userPage, "Roles", true);
		new VRCButton(grpRoles, "Crewmate", "Set user to crewmate", selUserCrewmate);
		new VRCButton(grpRoles, "Impostor", "Set user to impostor", selUserImpostor);

		var grpUserOther = new CollapsibleButtonGroup(_userPage, "Other");
		new VRCButton(grpUserOther, "Kill", "Kill selected user", selUserKill);
		new VRCButton(grpUserOther, "Mark Reporter", "Make user the reporter", selUserReporter);
		new VRCButton(grpUserOther, "Report Body", "Report a body for user", selUserReport);
	}

	#region self roles

	internal static void selfCrewmate()
	{
		sendUserEvent(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_IUser_0.prop_String_1, "SyncAssignB");
	}

	internal static void selfImpostor()
	{
		sendUserEvent(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_IUser_0.prop_String_1, "SyncAssignM");
	}

	#endregion

	#region game events

	internal static void gameStart()
	{
		sendGameEvent("Btn_Start");
		sendGameEvent("SyncStart");
	}

	internal static void gameAbort()
	{
		sendGameEvent("SyncAbort");
	}

	internal static void gameMeeting()
	{
		sendGameEvent("SyncEmergencyMeeting");
	}

	internal static void gameReport()
	{
		sendGameEvent("SyncReport");
	}

	internal static void victoryCrewmate()
	{
		sendGameEvent("SyncVictoryC");
	}

	internal static void victoryImpostor()
	{
		sendGameEvent("SyncVictoryI");
	}

	#endregion

	#region sabotage

	internal static void sabotageLights()
	{
		sendGameEvent("SyncDoSabotageLights");
	}

	internal static void sabotageOxygen()

	{
		sendGameEvent("SyncDoSabotageOxygen");
	}

	internal static void sabotageReactor()
	{
		sendGameEvent("SyncDoSabotageReactor");
	}

	internal static void sabotageComms()
	{
		sendGameEvent("SyncDoSabotageComms");
	}

	internal static void sabotageDoorsAll()
	{
		sendGameEvent("SyncDoSabotageDoorsCafeteria");
		sendGameEvent("SyncDoSabotageDoorsElectrical");
		sendGameEvent("SyncDoSabotageDoorsLower");
		sendGameEvent("SyncDoSabotageDoorsMedbay");
		sendGameEvent("SyncDoSabotageDoorsSecurity");
		sendGameEvent("SyncDoSabotageDoorsStorage");
		sendGameEvent("SyncDoSabotageDoorsUpper");
	}

	#endregion

	#region repairs

	internal static void repairLights()
	{
		sendGameEvent("SyncRepairLights");
	}

	internal static void repairOxygen()
	{
		sendGameEvent("SyncRepairOxygen");
		sendGameEvent("SyncRepairOxygenA");
		sendGameEvent("SyncRepairOxygenB");
	}

	internal static void repairReactor()
	{
		sendGameEvent("SyncRepairReactor");
	}

	internal static void repairComms()
	{
		sendGameEvent("SyncRepairComms");
	}

	internal static void cancelSabotage()
	{
		sendGameEvent("CancelAllSabotage");
	}

	#endregion

	#region seluser

	internal static void selUserCrewmate()
	{
		sendUserEvent(Utils.selectedPlayerName(), "SyncAssignB");
	}

	internal static void selUserImpostor()
	{
		sendUserEvent(Utils.selectedPlayerName(), "SyncAssignM");
	}

	internal static void selUserKill()
	{
		sendUserEvent(Utils.selectedPlayerName(), "SyncKill");
	}

	internal static void selUserReporter()
	{
		sendUserEvent(Utils.selectedPlayerName(), "SyncPlayerIsReporter");
	}

	internal static void selUserReport()
	{
		sendUserEvent(Utils.selectedPlayerName(), "SyncReport");
	}

	#endregion

	internal static void sendUserEvent(string player, string eventname)
	{
		var node = GetPlayerNode(player);
		node?.InvokeUdon(eventname, true);
	}

	internal static void sendGameEvent(string eventname)
	{
		gameLogic.InvokeUdon(eventname, true);
	}

	private static GameObject? GetPlayerNode(string player)
	{
		var thePlayer = Player.players.First(x => x.username.Contains(player.Trim()));
		foreach (var node in GameObject.Find("Game Logic/Player Nodes").GetComponentsInChildren<UdonBehaviour>())
		{
			var playerApi = node.GetHeapVal<VRCPlayerApi>("playerApi");
			if (playerApi != null && playerApi.Equals(thePlayer.playerApi))
			{
				return node.gameObject;
			}
		}

		ModMain.instance.Log.LogMessage("Player node not found for " + player);
		return null;
	}
}