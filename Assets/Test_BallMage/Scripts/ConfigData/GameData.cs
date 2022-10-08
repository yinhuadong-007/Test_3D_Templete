using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData m_instance = null;

    public static GameData instance
    {
        get
        {
            // if (m_instance == null)
            // {
            //     GameObject conf = ResManager.LoadPrefab("GameDataConf");
            //     m_instance = Instantiate(conf).GetComponent<GameData>();
            //     DontDestroyOnLoad(m_instance);
            // }
            return m_instance;
        }
    }

    [Tooltip("全局的重力加速度 - 游戏中")] public Vector3 gravityValue = new Vector3(0, -50f, 0);
    [Tooltip("全局的重力加速度 - 结算界面粒子掉落")] public Vector3 gravityValueResult = new Vector3(0, -9.8f, 0);
    [Tooltip("正常游戏速度")] public float gameTimeScale = 1;

    /// /////////////////////////////////////////////////////////
    [Header("炮塔配置")]
    [SerializeField, Tooltip("炮塔的转动角度范围, 取值范围 0-360")] public float[] shotAngleRange = { 5, 175 };
    [SerializeField, Tooltip("触摸失效阈值")] public float shotInvalidTouch = 60;
    [Tooltip("发射器移动速度")] public float shotMachineMoveSpeed = 30;


    /// /////////////////////////////////////////////////////////
    [Header("小球配置")]
    [Tooltip("球移动速度")] public float ballSpeed = 10;
    [Tooltip("球发射间隔")] public float ballShotDeltaTime = 0.1f;
    [Tooltip("球回收时的移动速度")] public int ballRecoverSpeed = 30;
    // [Tooltip("球的弹性系数"), Range(0, 1)] public float ballBounciness = 1f;


    /// /////////////////////////////////////////////////////////
    [Header("游戏加速功能")]
    [Tooltip("游戏加速功能是否启用")] public bool gameSpeedOpen = false;
    [Tooltip("游戏加速值-游戏快进速率,功能开启后生效")] public float gameSpeedTimeScale = 2;
    [Tooltip("游戏加速触发值-发射小球后多久显示按钮，功能开启后生效")] public float triggerGameSpeedTimeValue = 5;


    /// /////////////////////////////////////////////////////////
    [Header("道具")]
    [Tooltip("总球数增加")] public int totalBallNum;
    [Tooltip("反射球角度")] public float reflectAngle;
    [Tooltip("反射球额外数量")] public int reflectNum;
    [Tooltip("爆炸半径")] public float bombRadius;
    [Tooltip("爆炸威力")] public int bombDamage;


    /// /////////////////////////////////////////////////////////
    [Header("相机配置")]
    [Tooltip("镜头切换时间")] public float cameraDollyRunTime = 0.5f;


    private void Awake()
    {
        m_instance = this;
    }

    public void SetInstance()
    {
        m_instance = this;
    }
}
