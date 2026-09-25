using System.Collections;
using catclientv3.misc;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.Groups;

namespace catclientv3.features.qol;

public static class InstanceHistoryMenu
{
    public static VRCPage mainPage;
    public static ButtonGroup group;
    public static void init(ButtonGroup grp, VRCPage qolpage)
    {
        mainPage = new VRCPage("Instance History");
        new VRCButton(grp, "Instance History", "read", openmenu);
        group = new ButtonGroup(mainPage, "blabla", true);
    }

    public static void openmenu()
    {
        /*group.gameObject.DestroyChildren();
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        /*foreach (var instance in CCWorldUtils.instanceHistory)
        {
            new VRCButton(InstanceHistoryMenu.group, $"{instance.worldName} ({instance.instanceName})", "join",
                () => Networking.GoToRoom($"{instance.worldId}:{instance.instanceId}"));
        }#1#
        for (int i = CCWorldUtils.instanceHistory.Count - 1; i >= 0; i--)
        {
            var instance = CCWorldUtils.instanceHistory[i];
            new VRCButton(InstanceHistoryMenu.group, $"{instance.worldName} ({instance.instanceName})", "join",
                () => Networking.GoToRoom($"{instance.worldId}:{instance.instanceId}"));
        }*/
        CCEventSystem.StartCoroutinePls(loadObjectsInIHMenu());
        mainPage.OpenMenu();
    }

    public static IEnumerator loadObjectsInIHMenu()
    {
        group.gameObject.DestroyChildren();
        FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
        /*foreach (var instance in CCWorldUtils.instanceHistory)
        {
            new VRCButton(InstanceHistoryMenu.group, $"{instance.worldName} ({instance.instanceName})", "join",
                () => Networking.GoToRoom($"{instance.worldId}:{instance.instanceId}"));
        }*/
        for (int i = CCWorldUtils.instanceHistory.Count - 1; i >= 0; i--)
        {
            var instance = CCWorldUtils.instanceHistory[i];
            new VRCButton(InstanceHistoryMenu.group, $"{instance.worldName} ({instance.instanceName})", "join",
                () => Networking.GoToRoom($"{instance.worldId}:{instance.instanceId}"));
            yield return new WaitForEndOfFrame();
        }
    }
}