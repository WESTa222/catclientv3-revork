//using BepInEx.Logging;
using catclientv3.misc;
using MelonLoader;
using MelonLoader.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI;

namespace catclientv3.OtherUI;

public static class CCQMConsole
{
    public static bool initialized = false;
    public static Transform textTransform;
    public static TextMeshProUGUI consoleText;
    public static List<string> activeFaggots = new List<string>();
    
    public static void Init()
    {
        var fpsping =
            APIBase.QuickMenu.transform.Find(FAGGOTstringsofDOOMandDESPAIR.performanceStatz);
        var qmconsolebase = GameObject.Instantiate(fpsping, APIBase.QuickMenu.transform.Find(FAGGOTstringsofDOOMandDESPAIR.qmSmthIdk));
        qmconsolebase.name = "QmConsole";
        qmconsolebase.gameObject.SetActive(true);
        qmconsolebase.gameObject.AddComponent<Mask>();
        qmconsolebase.gameObject.AddComponent<RectMask2D>();
        var rectbase = qmconsolebase.gameObject.GetComponent<RectTransform>();
        rectbase.pivot = new Vector2(-0.02f, 0.85f);
        rectbase.sizeDelta = new Vector2(990f, 390f);
        qmconsolebase.GetChild(1).gameObject.SetActive(false);
        textTransform = qmconsolebase.GetChild(2);
        consoleText = textTransform.gameObject.GetComponent<TextMeshProUGUI>();
        consoleText.alignment = TextAlignmentOptions.BottomLeft;
        consoleText.richText = true;
        //consoleText.enableWordWrapping = false;
        consoleText.fontSize = 20;
        //consoleText.overflowMode = TextOverflowModes.Truncate;
        var consoleRect = textTransform.gameObject.GetComponent<RectTransform>();
        consoleRect.sizeDelta = new Vector2(960f, -10f);
        consoleRect.pivot = new Vector2(0.5f, 0f);
        List<string> fuh = new();
        MelonLogger.MsgDrawingCallbackHandler += HandleMelonMessage;
        MelonLogger.WarningCallbackHandler += HandleMelonWarning;
        MelonLogger.ErrorCallbackHandler += HandleMelonError;
        /*foreach (var logSource in BepInEx.Logging.Logger.Sources)
        {
            if (!fuh.Contains(logSource.SourceName))
            {
                logSource.LogEvent += HandleBepinLog;
                fuh.Add(logSource.SourceName);
            }
        }*/
        //ModMain.instance.Log.LogEvent += HandleBepinLog;
        
        initialized = true;

        //consoleText.text = "HAIII this is a test\nmeowmeowmeowmeow\nfaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaags";
    }

    private static void HandleMelonMessage(ColorARGB arg1, ColorARGB arg2, string arg3, string arg4)
    {
        LogText($"<color=white>[{arg3}]</color> {arg4}");
    }
    private static void HandleMelonWarning(string arg3, string arg4)
    {
        LogText($"<color=yellow>[{arg3}]</color> {arg4}");
    }
    private static void HandleMelonError(string arg3, string arg4)
    {
        LogText($"<color=red>[{arg3}]</color> {arg4}");
    }

    public static void LogText(string text)
    {
        if (!initialized)
            return;
        activeFaggots.Add(text);
        if (activeFaggots.Count > 15)
            activeFaggots.RemoveAt(0);
        
        consoleText.text = string.Join("\n", activeFaggots) + "\n\n";
    }
    
    /*private static void HandleBepinLog(object? sender, LogEventArgs e)
    {
        if (e.Level >= LogLevel.Info)
            return;
        string color = "white";
        switch (e.Level)
        {
            case LogLevel.Warning:
                color = "yellow";
                break;
            case LogLevel.Error:
                color = "red";
                break;
        }
        
            LogText($"<color={color}>[{e.Source.SourceName}]</color> {e.Data.ToString()}");
    }*/
    
}