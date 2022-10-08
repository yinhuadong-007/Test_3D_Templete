using System;

/// <summary> 游戏事件类型 </summary>
public static class GameEventType
{
    /// <summary> 通知敌人开启碰撞 </summary>
    public const string Enemy_Change_Collider_State = "Enemy_Change_Collider_State";
}

/// <summary>
/// 游戏发射器类型
/// </summary>
[Serializable]
public enum EShotMachineType
{
    /// <summary> 触摸发射 </summary>
    Touch_Shot,

    /// <summary> 点击发射 </summary>
    Tap_Shot,
}

public enum MoveModule
{
    StraightLine,//直线
    BezierLine_2,//二次贝塞尔曲线
    BezierLine_3,//三次贝塞尔曲线
}

public static class ESoundName
{
    /// <summary> 炮台发射 </summary>
    public const string tower_shoot = "tower_shoot";
    //子弹1~9音效
    public const string bullet_hit="bullet_hit_";

}


public static class EPrefabName
{
    //胜利ui
    public const string Success = "SuccessUI";

    //失败ui
    public const string Fail = "FailedUI";
}


