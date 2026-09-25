using System.Runtime.Serialization.Formatters.Binary;
//using Il2Cpp;
using TMPro;
using Valve.VR;
using VRC.SDKBase;
using UnityEngine;
using VRC;
using System.Linq;
using System.Reflection;
using catclientv3.features.network;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Photon.Client;
using VRC.Core;
using VRC.DataModel;
using VRC.Localization;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace catclientv3.misc;

public static class Utils
{
    public static string? selectedPlayerName() => (WorldAPI.APIBase.QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Body/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/UserProfile_Compact/UserDetails/Info/Text_Username_NonFriend"))?.GetComponent<TextMeshProUGUI>().text.ToString();
    public static VRCPlayer GetVRCPlayer(VRCPlayerApi playerapi) => playerapi.gameObject.GetComponent<VRCPlayer>();
    public static VRC.Player GetVRCDotPlayer(VRCPlayerApi playerapi) => playerapi.gameObject.GetComponent<VRC.Player>();

    public static VRCPlayerApi GetPlayerApiByUser(string user)
    {
        foreach (VRCPlayerApi playerapi in VRCPlayerApi.AllPlayers)
        {
            if (playerapi.displayName == user)
                return playerapi;
        }
        return null;
    }
    
    public static Player GetPlayerByUser(string user)
    {
        foreach (Player player in Player.players)
        {
            if (player.username == user)
                return player;
        }
        return null;
    }

    public static VRC.Player? GetPlayerByActorID(int id)
    {
        return PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray()
            .FirstOrDefault(x => x.field_Private_VRCPlayerApi_0.playerId == id);
    }
    public static VRCPlayer localplayer
    {
        get
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }
    }

    public static SteamVR_Action_Vector2 leftcontroller => SteamVR_Input.GetVector2Action("Move", false);
    public static SteamVR_Action_Vector2 rightcontroller => SteamVR_Input.GetVector2Action("Rotate", false);

    
    
    //voids photon stuff
    
    public static byte[] ToByteArray(object obj)
    {
        if (obj == null)
        {
            return null;
        }

#pragma warning disable SYSLIB0011 // Type or member is obsolete
        BinaryFormatter binaryFormatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }
    
    public static byte[] Il2CppToByteArray(Il2CppSystem.Object obj)
    {
        if (obj == null)
        {
            return null;
        }

#pragma warning disable SYSLIB0011 // Type or member is obsolete
        Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        Il2CppSystem.IO.MemoryStream memoryStream = new Il2CppSystem.IO.MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }

    public static T FromByteArray<T>(byte[] data)
    {
        if (data == null)
        {
            return default(T);
        }

#pragma warning disable SYSLIB0011 // Type or member is obsolete
        BinaryFormatter binaryFormatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(data))
        {
            object obj = binaryFormatter.Deserialize(memoryStream);
            return (T)obj;
        }
    }

    public static T? IL2CPPFromByteArray<T>(byte[] data) where T : Il2CppObjectBase
    {
        if (data == null)
        {
            return default(T);
        }
        Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        Il2CppSystem.IO.MemoryStream memoryStream = new Il2CppSystem.IO.MemoryStream(data);
        Il2CppSystem.Object obj = binaryFormatter.Deserialize(memoryStream);
        return obj.TryCast<T>();
    }

    public static T FromIL2CPPToManaged<T>(Il2CppSystem.Object obj) => FromByteArray<T>(Il2CppToByteArray(obj));

    public static T FromManagedToIL2CPP<T>(object obj) where T : Il2CppObjectBase => IL2CPPFromByteArray<T>(ToByteArray(obj));
    
    public static byte[] GetByteArray(int sizeInKb)
    {
        System.Random random = new System.Random();
        byte[] array = new byte[sizeInKb * 1024];
        random.NextBytes(array);
        return array;
    }
    public static UnityEngine.Object ByteArrayToObjectUnity2(byte[] arrBytes)
    {
        Il2CppStructArray<byte> il2CppStructArray = new Il2CppStructArray<byte>((long)arrBytes.Length);
        arrBytes.CopyTo(il2CppStructArray, 0);
        Il2CppSystem.Object @object = new Il2CppSystem.Object(il2CppStructArray.Pointer);
        return new UnityEngine.Object(@object.Pointer);
    }
    
    //default/fuels stuff
    
    public static byte[] Vector3ToBytes(UnityEngine.Vector3 vector3)
    {
        byte[] buffer = new byte[12];
            
        System.Buffer.BlockCopy(BitConverter.GetBytes(vector3.x), 0, buffer, 0, 4);
        System.Buffer.BlockCopy(BitConverter.GetBytes(vector3.y), 0, buffer, 4, 4);
        System.Buffer.BlockCopy(BitConverter.GetBytes(vector3.z), 0, buffer, 8, 4);
        return buffer;
    }

    public static UnityEngine.Vector3 BytesToVector3(byte[] data)
    {
        byte[] ByteX = data.Take(4).ToArray();
        byte[] ByteY = data.Skip(4).Take(4).ToArray();
        byte[] ByteZ = data.Skip(8).Take(4).ToArray();
            
        return new UnityEngine.Vector3(BitConverter.ToSingle(ByteX, 0), BitConverter.ToSingle(ByteY, 0), BitConverter.ToSingle(ByteZ, 0));
    }

    /*public static T Cast<T>(this object val)
    {
        return (T)Convert.ChangeType(val, typeof(T));
    }*/
    
    public static string GetGameObjectPath(GameObject obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        string path = "/" + obj.name;
        Transform currentTransform = obj.transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
            path = "/" + currentTransform.name + path;
        }

        return path;
    }
    
    public enum trust
    {
        v,
        nU,
        u,
        kU,
        tU,
        DEV
    }
    public static trust getTrust(this APIUser apiUser)
    {
        if (apiUser.tags.Count > 0)
        {
            if (apiUser.tags.Contains("system_trust_developer") && apiUser.tags.Contains("system_trust_dev"))
                return trust.DEV;
                
            if (apiUser.tags.Contains("system_trust_veteran") && apiUser.tags.Contains("system_trust_trusted"))
                return trust.tU;

            if (apiUser.tags.Contains("system_trust_trusted"))
                return trust.kU;

            if (apiUser.tags.Contains("system_trust_known"))
                return trust.u;

            if (apiUser.tags.Contains("system_trust_basic"))
                return trust.nU;
        }

        return trust.v;
    }
    public static bool isFren(this APIUser a, APIUser b)
    {
        if (b == null)
            return false;

        return a.friendIDs.Contains(b.id);
    }
    public static Color playerTColor(this APIUser apiUser)
    {
        if (apiUser == null)
            return Color.white;

        if (APIUser.CurrentUser.isFren(apiUser))
            return Color.yellow;

        switch (apiUser.getTrust())
        {
            case trust.nU:
                return new Color(21f / 255f, 91f / 255f, 190f / 255f);
            case trust.u:
                return new Color(39f / 255f, 199f / 255f, 86f / 255f);
            case trust.kU:
                return new Color(252f / 255f, 120f / 255f, 65f / 255f);
            case trust.tU:
                return new Color(119f / 255f, 62f / 255f, 216f / 255f);
            case trust.DEV:
                return Color.red;
            default: //visitor/nuisance
                return Color.white;
        }
    }

    public static Transform getNameplateContents(this GameObject faggot)
    {
        return faggot.transform.Find("PlayerNameplate/Canvas/NameplateGroup/Nameplate/Contents");
    }

    public static T? GetHeapVal<T>(this GameObject go, string blalba)// where T : Il2CppObjectBase
    {
        var ub = go.GetComponent<UdonBehaviour>();
        return ub._program.Heap.GetHeapVariable<T>(ub._program.SymbolTable.GetAddressFromSymbol(blalba));
        /*if (ub && ub._program.SymbolTable.TryGetAddressFromSymbol(blalba, out var address))
        {
            return ub._program.Heap.GetHeapVariable<T>(address);
        }
        return null;*/
    }

    public static void SetHeapVal<T>(this GameObject go, string blalba, T value)
    {
        var ub = go.GetComponent<UdonBehaviour>();
        if (ub && ub._program.SymbolTable.TryGetAddressFromSymbol(blalba, out var address))
        {
            ub._program.Heap.SetHeapVariable(address, value);
        }
    }
    
    public static T GetHeapVal<T>(this UdonBehaviour ub, string blalba)// where T : Il2CppObjectBase
    {
        ub._program.SymbolTable.TryGetAddressFromSymbol(blalba, out var address);
        return ub._program.Heap.GetHeapVariable<T>(ub._program.SymbolTable.GetAddressFromSymbol(blalba));
        /*if (ub && )
        {
            
        }
        return null;*/
    }

    public static void SetHeapVal<T>(this UdonBehaviour ub, string blalba, T value)
    {
        if (ub && ub._program.SymbolTable.TryGetAddressFromSymbol(blalba, out var address))
        {
            ub._program.Heap.SetHeapVariable(address, value);
        }
    }
    
    public static void SetProgVar<T>(this UdonBehaviour ub, string blalba, T value)
    {
        Il2CppType.Of<Il2CppSystem.Int32>();
        if (ub)
        {
            ub.SetProgramVariable(blalba, value);
        }
    }
    
    public static T? GetProgVar<T>(this UdonBehaviour ub, string blalba)// where T : Il2CppObjectBase
    {
        ub._program.SymbolTable.TryGetAddressFromSymbol(blalba, out var address);
        return ub._program.Heap.GetHeapVariable<T>(ub._program.SymbolTable.GetAddressFromSymbol(blalba));
        /*if (ub && ub.TryGetProgramVariable<T>(blalba, out var value))
        {
            return value;
        }
        return null;*/
    }
    
    public static void SetProgVar<T>(this GameObject go, string blalba, T value)
    {
        var ub = go.GetComponent<UdonBehaviour>();
        if (ub)
        {
            ub.SetProgramVariable(blalba, value);
        }
    }
    
    public static T? GetProgVar<T>(this GameObject go, string blalba)// where T : Il2CppObjectBase
    {
        var ub = go.GetComponent<UdonBehaviour>();
        return ub._program.Heap.GetHeapVariable<T>(ub._program.SymbolTable.GetAddressFromSymbol(blalba));
        /*if (ub && ub.TryGetProgramVariable<T>(blalba, out var value))
        {
            return value;
        }
        return null;*/
    }

    public static void DumpUdonForObject(this GameObject FAGGOT)
    {
        UdonDumper.dumpSingle(FAGGOT);
    }

    public static void InvokeUdon(this GameObject go, string evnt, bool network = false, int target = 0)
    {
        var udon = go.GetComponent<UdonBehaviour>();
        if (udon)
        {
            if (network)
            {
                udon.SendCustomNetworkEvent((NetworkEventTarget)target, evnt);
            }
            else
            {
                udon.SendCustomEvent(evnt);
            }
        }
    }
    
    public static void InvokeUdon(this UdonBehaviour udon, string evnt, bool network = false, int target = 0)
    {
        //var udon = go.GetComponent<UdonBehaviour>();
        if (udon)
        {
            if (network)
            {
                udon.SendCustomNetworkEvent((NetworkEventTarget)target, evnt);
            }
            else
            {
                udon.SendCustomEvent(evnt);
            }
        }
    }
    
    public static Sprite MakeSprite(byte[] data)
    {
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(data);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        return Sprite.Create(tex,new Rect(0, 0, tex.width, tex.height),new Vector2(0.5f, 0.5f),100f);
    }

    public static void temp()
    {
         foreach (var udon in GameObject.Find("_GIMMICKS /CheesePickups").GetComponentsInChildren<UdonBehaviour>())
         {
             udon.gameObject.InvokeUdon("_interact");
         }
    }
    
    public static void CloneAvatarEVILassTEMPLEOSISOmethodGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEXGAYSEX_the_fitnessgram_pacer_test_is_a_multistage_aerobic_capacity_test_that_progressively_gets_more_difficult_as_it_continues_the_20_meter_pacer_test_will_begin_in_30_seconds_line_up_at_the_start_the_running_speed_starts_slowly_but_gets_faster_each_minute_after_you_hear_this_signal_boop_a_single_lap_should_be_completed_each_time_you_hear_this_sound_ding_remember_to_run_in_a_straight_line_and_run_as_long_as_possible_the_second_time_you_fail_to_complete_a_lap_before_the_sound_your_test_is_over_the_test_will_begin_on_the_word_start_on_your_mark_get_ready_start(string avatarId) 
        => PageAvatar.Method_Public_Static_Void_ApiAvatar_String_0(new ApiAvatar { id = avatarId });
    
    public static string GetStringSha256Hash(string text)
    {
        if (String.IsNullOrEmpty(text))
            return String.Empty;

        using (var sha = new System.Security.Cryptography.SHA256Managed())
        {
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }

    public static byte[] GetBytes(this string text)
    {
        return System.Text.Encoding.UTF8.GetBytes(text);
    }

    public static void CopyToClipboard(this string text)
    {
        typeof(GUIUtility).GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic)
            .SetValue(null, text, null);
    }
    
    public static void CopyToClipboard2(this string text)
    {
        TextEditor te = new TextEditor();
        te.text = text;
        te.SelectAll();
        te.Copy();
    }

    public static void chatbox(string chat)
    {
        PhotonNetwork.Method_Public_Static_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0(43,
            "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n",
            new RaiseEventOptions { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public static byte[] GetBytes(this Stream stream)
    {
        using(MemoryStream ms = new MemoryStream())
        {
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
    
    public static VRCUiPopupManager VRCUiPopupManager
    {
        get
        {
            return VRCUiPopupManager.prop_VRCUiPopupManager_0;
        }
    }
    public static void AskInGameInput(this VRCUiPopupManager instance, string title, string okButtonName, Action<string> onSuccess, string placeholder = null)
    {
        instance.InputPopUp(title, okButtonName, new Action<string>((g) =>
        {
            onSuccess(g);
        }), placeholder);
    }

    private static void InputPopUp(this VRCUiPopupManager instance, string title, string okButtonName, Action<string> onSuccess, string placeholder = null)
    {
        var titleString = LocalizableStringExtensions.Localize(title);
        var emptyString = LocalizableStringExtensions.Localize("");
        var okString = LocalizableStringExtensions.Localize(okButtonName);
        var placeholderString = LocalizableStringExtensions.Localize(placeholder ?? "");

        instance.Method_Public_Void_LocalizableString_LocalizableString_InputType_Boolean_LocalizableString_Action_3_String_List_1_KeyCode_TextMeshProUGUIEx_Action_LocalizableString_Boolean_Action_1_MonoBehaviour1PublicObImObBuAcSiCoSiAc1Unique_Boolean_Int32_0(
            titleString,                              // Title
            emptyString,                              // Description (unused)
            TMPro.TMP_InputField.InputType.Standard,  // Input type
            false,                                     // Multiline
            okString,                                  // Ok button label
            new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, TextMeshProUGUIEx>((input, _, __) =>
            {
                if (string.IsNullOrWhiteSpace(input)) input = placeholder;
                onSuccess?.Invoke(input);
                VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");
            }),
            new Action(() => // close action
            {
                VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");
            }),                                      
            placeholderString,                         // Placeholder text DO NOT CHANGE
            false,                                     
            null,                                      
            false,                                     
            0                                          
        );
    }


}