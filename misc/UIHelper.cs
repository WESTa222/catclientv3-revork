//using Il2Cpp;

using System.Reflection;
//using Cpp2IL.Core.Extensions;
using VRC.UI.Elements.Menus;
using UnityEngine;
using VRC.DataModel;
using VRC.Localization;
using WorldAPI;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Groups;

namespace catclientv3.misc;

public class UIHelper
{
    public static VRCPage CCMain;
    public static ButtonGroup CCRootGrp;

    public static Transform QMDashContent;

    public static Transform AML;
    public static Transform AMR;

    public static Transform VRCUserSelMenu;
    public static ButtonGroup CCUserSelGrp;

    public static Transform ghosticonhud;
    
    

    public static Sprite placeholder => Utils.MakeSprite(Assembly.GetExecutingAssembly().GetManifestResourceStream("catclientv3.icons.wm1.png").GetBytes());
    public static Sprite ghost => Utils.MakeSprite(Assembly.GetExecutingAssembly().GetManifestResourceStream("catclientv3.icons.ghostcat.png").GetBytes());

    internal static bool keyboardOpen
    {
        get
        {
            var obj = APIBase.UserInterface.Find(
                "Canvas_MainMenu(Clone)/Container/MMParent/HeaderOffset/Modal_MM_Keyboard(Clone)");
            return obj != null && obj.GetComponent<Canvas>().enabled;
        }
    }
    
    internal static bool QMOpen
    {
        get
        {
            var obj = APIBase.QuickMenu;
            return obj != null && obj.GetComponent<Canvas>().enabled;
        }
    }

    internal static bool AMOpen
    {
        get
        {
            if (!AML || !AMR)
            {
                AML = APIBase.UserInterface.Find("ActionMenu/Container/MenuL/ActionMenu");
                AMR = APIBase.UserInterface.Find("ActionMenu/Container/MenuR/ActionMenu");
            }
            return AMR.gameObject.active ||  AML.gameObject.active;
        }
    }
    
    public static KeyboardComponent keyComp;
    public static void CcInputmenu(string text, System.Action<string> action, InputPopupType Ktype = 0, string defaultText = "text")
    {
        Utils.VRCUiPopupManager.AskInGameInput(text, "OK", action, defaultText);
        //Utils.VRCUiPopupManager.field_Public_VRCUiPopupInput_0.
        return;
        if (keyComp == null)
        {
            keyComp = new GameObject("CCboard").AddComponent<KeyboardComponent>();
            UnityEngine.Object.DontDestroyOnLoad(keyComp);
        }
        var keyDat = new KeyboardData();
        keyDat.Method_Public_KeyboardData_LocalizableString_LocalizableString_String_LocalizableString_LocalizableString_PDM_0(text.Localize(), defaultText.Localize(), "", "Submit".Localize(), "Cancel".Localize());
        keyDat.Method_Public_KeyboardData_Action_1_String_Action_1_String_Action_Boolean_PDM_0(null, action, null);
        //typeof(KeyboardData).GetMethod("Method_Public_KeyboardData_InputType_ContentType_Int32_Boolean_Boolean_InterfacePublicAbstractBoStVoAc1VoAcSt1BoUnique_PDM_0").Invoke(keyDat, [0, 0, 0, false, false]);
        //keyDat.Method_Public_KeyboardData_InputType_ContentType_Int32_Boolean_Boolean_InterfacePublicAbstractBoStVoAc1VoAcSt1BoUnique_PDM_0(0, 0, 0, false, false);
        keyDat.Method_Public_KeyboardData_InputType_ContentType_Int32_Boolean_Boolean_PDM_0(0, 0, 0, false, false);
        keyDat.Method_Public_KeyboardData_InputPopupType_Boolean_PDM_0(Ktype, true);
        keyDat._isWorldKeyboard = true;
        keyComp._keyboardData = keyDat;
        keyComp.Method_Private_Void_0();
        keyComp.Method_Private_Void_1();
        keyComp.Method_Private_Void_2();
        keyComp.Method_Private_Void_3();
    }
    
    
    
}