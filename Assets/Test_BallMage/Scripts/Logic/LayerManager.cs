using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public static LayerManager instance;

    // [Tooltip("敌人层")] public Transform EnemyLayer;
    [Tooltip("打击模型层")] public Transform ModelLayer;

    [Tooltip("道具层")] public Transform PropLayer;

    // [Tooltip("玩家层")] public Transform PlayerLayer;

    [Tooltip("技能特效层")] public Transform SkillLayer;

    [Tooltip("辅助线层")] public Transform GuideLayer;

    [Tooltip("小球层")] public Transform BallLayer;

    [Tooltip("玩家ui文本展示")] public Canvas TextCanvas;
    [Tooltip("UI图片")] public Canvas ImageCanvas;


    [Tooltip("UI弹窗层")] public Transform UIPopLayer;
    [Tooltip("UI3D特效层")] public Transform UIParticleRootLayer;

    private void Awake()
    {
        instance = this;
    }

    public void Clear()
    {
        Tools.ClearChildren(ModelLayer);
        Tools.ClearChildren(PropLayer);
        Tools.ClearChildren(SkillLayer);
        Tools.ClearChildren(GuideLayer);
        Tools.ClearChildren(BallLayer);
        Tools.ClearChildren(TextCanvas.transform);
        Tools.ClearChildren(UIPopLayer);
        Tools.ClearChildren(UIParticleRootLayer);
    }

}
