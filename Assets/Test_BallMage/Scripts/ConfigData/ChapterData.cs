using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterData : MonoBehaviour
{
    public static ChapterData m_instance = null;

    public static ChapterData instance
    {
        get
        {
            // if (m_instance == null)
            // {
            //     GameObject conf = ResManager.LoadPrefab("PrefabDataConf");
            //     m_instance = Instantiate(conf).GetComponent<ChapterData>();
            //     DontDestroyOnLoad(m_instance);
            // }
            return m_instance;
        }
    }

    private void Awake()
    {
        m_instance = this;
        InitData();

        Print.Log("awake " + m_instance.name);

    }

    private void InitData()
    {
    }

    //数据
    [Header("数据")]
    [Tooltip("行数")] public int rows;
    [Tooltip("列数")] public int columns;

    //主体
    [Header("预制体")]
    [Tooltip("环境预制")] public GameObject environmentPrefab;
    [Tooltip("小球预制")] public GameObject ballPrefab;
    [Tooltip("发射器预制")] public GameObject shotMachinePrefab;
    [Tooltip("引导点预制")] public GameObject guidePointPrefab;

    //特效

    //材质
    [Header("材质")]
    // [Tooltip("天空球材质")] public Material matSky;
    // [Tooltip("地板材质")] public Material matPlane;
    // [Tooltip("边框材质")] public Material matFrame;
    // [Tooltip("障碍材质")] public Material matObstacle;
    // [Tooltip("背景建筑材质")] public Material matBgBuild;
    [Tooltip("引导材质")] public Material matGuide;

    [Header("小球颜色材质")]
    [Tooltip("小球颜色材质列表")] public Material[] ballColorMaterialList;
    [Tooltip("拖尾颜色材质列表")] public Material[] trailColorMaterialList;


    //展示数据
    [Header("展示数据 - 配置无效")]
    [Tooltip("当前小球数量 - 不可配置")] public int curBallCount = 1;

}

