using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
    public static MainGameScene instance;

    [SerializeField, Tooltip("场景列表")] public GameObject[] sceneDataList;

    [SerializeField, Tooltip("编辑器模式下，第一个运行的场景序号 0,1,2 ...")] public int currentLVIndex = 0;

    [HideInInspector, Tooltip("游戏循环次数上限")] private int m_totalLoopCount = 1;
    [HideInInspector, Tooltip("编辑器模式下，当前游戏循环次数, 值大于0")] public int curLoopCount = 1;

    public int totalLoopCount { get { return m_totalLoopCount; } }

    private GameObject curChapterObj;
    public GameObject curChapterRoot
    {
        get { return curChapterObj; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (sceneDataList.Length == 0)
        {
            Print.LogError("没有配置场景，请查看GameChapterConfig节点配置！");
            return;
        }
        // ReportPage.OnPageShow("首页");
        // ReportPage.OnPageShow("游戏页");

        if (curLoopCount < 1) curLoopCount = 1;

#if UNITY_EDITOR
        int index = currentLVIndex + (curLoopCount - 1) * sceneDataList.Length;
#else
        int index = PlayerPrefs.GetInt("CurrentLVIndexByIndex", 0);
        Print.Log("cache CurrentLVIndexByIndex = " + index);
#endif
        currentLVIndex = index;
        if (currentLVIndex >= sceneDataList.Length * m_totalLoopCount)
        {
            currentLVIndex = 0;
        }

        StartChapter();
    }

    /// <summary> 开始章节 </summary>
    public void StartChapter()
    {

        Clear();
        if (curChapterObj != null)
        {
            Destroy(curChapterObj);
        }
        int sceneLvIndex = currentLVIndex % sceneDataList.Length;
        curLoopCount = currentLVIndex / sceneDataList.Length + 1;

        //关卡开始
        // ReportUtil.OnLevelStart(currentLVIndex + 1);

        curChapterObj = new GameObject($"LvRoot_{curLoopCount}_{sceneLvIndex}");

        // ReportUtil.OnLevelStart(currentLVIndex + 1);

        Instantiate(sceneDataList[sceneLvIndex], curChapterObj.transform);

        GameManager.instance.InitChapter();

        GraphicsManager.instance.Init();
    }

    /// <summary> 重置当前章节 </summary>
    public void ResetCurrentChapter()
    {
        StartChapter();
    }

    /// <summary> 下一个章节 </summary>
    public void EnterNextChapter()
    {
        currentLVIndex++;
        if (currentLVIndex >= sceneDataList.Length * m_totalLoopCount)
        {
            currentLVIndex = 0;
        }

        StartChapter();
#if UNITY_EDITOR
#else
        PlayerPrefs.SetInt("CurrentLVIndexByIndex", currentLVIndex);
        PlayerPrefs.Save();
#endif
    }

    private void Clear()
    {
        Tween.Clear();
        GameManager.instance.Clear();
        BallManager.ins.Clear();
        ParticleManager.Instance.Clear();
        AudioManager.Instance.Clear();
        LayerManager.instance.Clear();
        Src_BaseBlock.Clear();
    }

}



