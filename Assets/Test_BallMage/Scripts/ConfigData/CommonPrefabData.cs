using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPrefabData : MonoBehaviour
{
    public static CommonPrefabData m_instance = null;

    public static CommonPrefabData instance
    {
        get
        {
            // if (m_instance == null)
            // {
            //     GameObject conf = ResManager.LoadPrefab("CommonPrefabDataConf");
            //     m_instance = Instantiate(conf).GetComponent<CommonPrefabData>();
            //     DontDestroyOnLoad(m_instance);
            // }
            return m_instance;
        }
    }
    [Header("预制体")]
    [Tooltip("游戏失败UI")] public GameObject uiGameFail;
    [Tooltip("游戏胜利UI")] public GameObject uiGameSuccess;
    [Tooltip("引导手")] public GameObject uiGuideHand;

    [Tooltip("爆炸粒子特效")] public GameObject particleBomb;
    [Tooltip("击中粒子特效")] public GameObject particleCube;

    [Header("物理材质")]
    [Tooltip("方块材质")] public PhysicMaterial blockPhysicsMat;

    private void Awake()
    {
        m_instance = this;
    }
}
