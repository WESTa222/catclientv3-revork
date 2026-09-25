
//using Il2Cpp;
using VRC.Core;
using Newtonsoft.Json;

namespace catclientv3.misc;


public static class CCWorldUtils
{
    public static ApiWorld currentWorld;
    public static ApiWorldInstance currentInstance;
    public static List<CCWorldInstance> instanceHistory = new List<CCWorldInstance>();
    public static void handleEnterFagWorldOrSomethingLmaoThisIsJustToPissOfKami_HiKami_FuckYouKami()
    {
        //empty
    }

    public static void loadHistory()
    {
        if (CCPersistance.tryGetSetting<CCWorldInstance[]>("instanceHistory", out var gaysex))
        {
            FAGGOTstringsofDOOMandDESPAIR.meowmeowcantdeletethis();
            instanceHistory = gaysex.ToList();
            CCPersistance.setSetting("instanceHistory", instanceHistory);
        }
    }
}

public class CCWorldInstance
{
    public string worldName;
    public string instanceName;
    public string instanceId;
    public string worldId;

    [JsonConstructor]
    public CCWorldInstance(string worldName, string instanceName, string instanceId, string worldId)
    {
        this.worldName = worldName;
        this.instanceName = instanceName;
        this.instanceId = instanceId;
        this.worldId = worldId;
    }

    public CCWorldInstance(ApiWorldInstance intannce)
    {
        worldName = intannce.world.name;
        instanceName = intannce.name;
        instanceId = intannce.instanceId;
        worldId = intannce.worldId;
    }
}