using UnityEngine;
/**
 * 标签 层级 保存
 */
public class TagLayer
{
    public const string tag_Untagged = "Untagged";
    public const string tag_Respawn = "Respawn";
    public const string tag_Finish = "Finish";
    public const string tag_EditorOnly = "EditorOnly";
    public const string tag_MainCamera = "MainCamera";
    public const string tag_Player = "Player";
    public const string tag_GameController = "GameController";

    private static int _layer_Default = -1;
    public static int layer_Default { get { if (_layer_Default == -1){_layer_Default = LayerMask.NameToLayer("Default");}  return _layer_Default; } }

    private static int _layer_TransparentFX = -1;
    public static int layer_TransparentFX { get { if (_layer_TransparentFX == -1){_layer_TransparentFX = LayerMask.NameToLayer("TransparentFX");}  return _layer_TransparentFX; } }

    private static int _layer_IgnoreRaycast = -1;
    public static int layer_IgnoreRaycast { get { if (_layer_IgnoreRaycast == -1){_layer_IgnoreRaycast = LayerMask.NameToLayer("IgnoreRaycast");}  return _layer_IgnoreRaycast; } }

    private static int _layer_Water = -1;
    public static int layer_Water { get { if (_layer_Water == -1){_layer_Water = LayerMask.NameToLayer("Water");}  return _layer_Water; } }

    private static int _layer_UI = -1;
    public static int layer_UI { get { if (_layer_UI == -1){_layer_UI = LayerMask.NameToLayer("UI");}  return _layer_UI; } }

    private static int _layer_CaveSide = -1;
    public static int layer_CaveSide { get { if (_layer_CaveSide == -1){_layer_CaveSide = LayerMask.NameToLayer("CaveSide");}  return _layer_CaveSide; } }

    private static int _layer_Falling = -1;
    public static int layer_Falling { get { if (_layer_Falling == -1){_layer_Falling = LayerMask.NameToLayer("Falling");}  return _layer_Falling; } }

    private static int _layer_Plane = -1;
    public static int layer_Plane { get { if (_layer_Plane == -1){_layer_Plane = LayerMask.NameToLayer("Plane");}  return _layer_Plane; } }
}
