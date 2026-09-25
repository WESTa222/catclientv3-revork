using System.Collections;
using catclientv3.misc;
//using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using RootMotion.FinalIK;
using VRC.Core;
using VRC.SDKBase;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace catclientv3.features.visual;

public class CCCoolEsp : MonoBehaviour
{
    public Player CCPlayer;
    private VRC.Player player;
    private APIUser apiUser;
    private Renderer sRR; // select region renderer
    private Color color;
    private VRIK? ik;
    public HighlightsFXStandalone hfxs;
    public LineRenderer lineRenderer;
    private void Start()
    {
        player = GetComponent<VRC.Player>();
        apiUser = player.field_Private_APIUser_0;
        sRR = transform.FindChild("SelectRegion").GetComponent<Renderer>();
        color = apiUser.playerTColor();
        //ik = player.transform.GetChild(0).FindChild("Avatar").GetComponent<VRIK>();
        CCEventSystem.StartCoroutinePls(waitForSelRegFilter(this));
        string Meowmeow = "faggots should burn hehe";
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        /*hfxs = Camera.main.gameObject.AddComponent<HighlightsFXStandalone>();
        hfxs.Method_Public_Void_MeshFilter_Color_0(sRR.gameObject.GetComponent<MeshFilter>(), color);
        hfxs.enabled = ESP.esp;
        hfxs.blurSize *= 0.5f;
        hfxs.blurIterations = 1;*/
    }

    public static IEnumerator waitForSelRegFilter(CCCoolEsp instance)
    {
        while (instance.sRR == null || instance.sRR.GetComponent<MeshFilter>() == null)
        {
            instance.sRR = instance.transform.FindChild("SelectRegion").GetComponent<Renderer>();
            yield return null;
        }
        instance.hfxs = Camera.main.gameObject.AddComponent<HighlightsFXStandalone>();
        instance.hfxs.Method_Public_Void_MeshFilter_Color_0(instance.sRR.GetComponent<MeshFilter>(), instance.color);
        instance.hfxs.enabled = ESP.esp;
        instance.hfxs.blurSize *= 0.5f;
        instance.hfxs.blurIterations = 1;
    }

    private void OnRenderObject()
    {
        lineRenderer.enabled = CCCoolEspController.linevr;
        
        if (!CCCoolEspController.enabled)
            return;
        
        Vector3 wpos = sRR.bounds.center;
        //line esp
        if(CCCoolEspController.line)
            CCGuiBullshit.lineDrawinator(Utils.localplayer.transform.position, wpos, color);
        
        //vr compat line esp
        if (CCCoolEspController.linevr)
        {
            lineRenderer.SetPosition(0, Utils.localplayer.transform.position);
            lineRenderer.SetPosition(1, wpos);
        }
        
        //box esp
        if(CCCoolEspController.box)
            CCGuiBullshit.boxAroundBounds(sRR.bounds, color);
        
        //bone esp
        if(CCCoolEspController.bone && ik != null && CCPlayer.root != null && ik.animator.avatar.isHuman)
            boneEspHelper(CCPlayer.root);
    }

    private void Update()
    {
        //im to lazy to make this good, fuck you
        if (ik == null || CCPlayer.root == null || !CCPlayer.root.bone)
        {
            try
            {
                if (player.transform.GetChild(0).FindChild("Avatar"))
                {
                    ik = player.transform.GetChild(0).FindChild("Avatar").GetComponent<VRIK>();
                    CCPlayer.root = CCIkUtils.buildSkeleton(ik.references);
                }
                else
                    return;
            }
            catch
            {
                //sex
            }
            
        }
    }

    private void OnDestroy()
    {
        Destroy(hfxs);
    }

    private void boneEspHelper(IkBone bone, IkBone? parent = null)
    {
        if (parent != null)
        {
            CCGuiBullshit.lineDrawinator(parent.bone.position, bone.bone.position, color);
        }
        foreach (var child in bone.children)
        {
            boneEspHelper(child, bone);
        }
    }
}



public class CCCoolEspManager : MonoBehaviour
{
    static CCCoolEspManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }
    private void OnRenderObject()
    {
        CCGuiBullshit.RenderQueuedLines();
    }

    private void LateUpdate()
    {
        CCGuiBullshit.ClearQueue();
    }
}