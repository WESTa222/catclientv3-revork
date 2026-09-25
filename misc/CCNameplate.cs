using System.Collections;
//using Il2Cpp;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using Logger = VRC.Core.Logger;

namespace catclientv3.misc;

public class CCNameplate : MonoBehaviour
{
    public Player player;
    private Transform playerNameplateContent;
    public Transform customNameplate;
    public Transform textPreset;
    public List<CCNameplateText> texts = new();

    public void Update()
    {
        foreach (var text in texts)
        {
            text.upd();
        }
    }
    
    public void init(Player p, float offset = 28.3f)
    {
        player = p;
        playerNameplateContent = p.vrcPlayer.field_Public_PlayerNameplate_0.field_Public_GameObject_0.transform;
        customNameplate = GameObject.Instantiate(playerNameplateContent.FindChild(FAGGOTstringsofDOOMandDESPAIR.quickfaps), playerNameplateContent);
        customNameplate.name = "CCNameplate";
        for (int i = 0; i < customNameplate.childCount; i++)
        {
            customNameplate.GetChild(i).gameObject.SetActive(false);
        }
        customNameplate.localPosition += new Vector3(0, offset, 0);
        customNameplate.gameObject.SetActive(true);
        textPreset = customNameplate.transform.GetChild(1);
        /*//for testing
        AddText("testnum", (x) =>
        {
            x.vars.TryAdd("intthing", 0);
            x.text.text = x.vars["intthing"].ToString();
            var num = (int)x.vars["intthing"];
            num++;
            x.vars["intthing"] = num;
        });*/
    }
    
    

    public CCNameplateText AddText(string it = "", Action<CCNameplateText>? updaction = null, Action<CCNameplateText>? startaction = null)
    {
        CCNameplateText? sep = null;
        if (!it.Equals(" | ") && texts.Count > 0)
        {
            sep = AddText(" | ");
        }

        var npt = new CCNameplateText(this, it, updaction, startaction, sep);
        texts.Add(npt);
        return npt;
    }
    
    public CCNameplateText AddText(CCNameplateText text)
    {
        CCNameplateText? sep = null;
        sep = AddText(" | ");
        texts.Add(text);
        return text;
    }
}

public class CCNameplateText
{
    public Transform textobj;
    public NameplateTextMeshProUGUI text;
    public CCNameplate parent;
    public Action<CCNameplateText>? updateAction;
    public Dictionary<string,object> vars = new();
    public CCNameplateText? sep;

    public CCNameplateText(CCNameplate np, string t, Action<CCNameplateText>? ua = null, Action<CCNameplateText>? sa = null, CCNameplateText? sep = null)
    {
        textobj = GameObject.Instantiate(np.textPreset, np.customNameplate);
        textobj.gameObject.SetActive(true);
        text = textobj.GetComponent<NameplateTextMeshProUGUI>();
        text.color = Color.white;
        text.richText = true;
        text.text = t;
        parent = np;
        updateAction = ua;
        if (sa != null)
            sa.Invoke(this);
        if (sep != null)
            this.sep = sep;

    }

    public void upd()
    {
        if (updateAction != null)
        {
            updateAction(this);
        }
    }

    public void dest()
    {
        //var sep = parent.texts[parent.texts.IndexOf(this) - 1];
        if (textobj != null)
        {
            UnityEngine.Object.DestroyImmediate(textobj.gameObject);
            parent.texts.Remove(this);
        }
        if (sep != null)
        {
            UnityEngine.Object.DestroyImmediate(sep.textobj.gameObject);
            parent.texts.Remove(sep);
        }
        
    }
}