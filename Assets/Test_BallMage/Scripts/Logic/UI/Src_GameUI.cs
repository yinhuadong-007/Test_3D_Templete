using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Src_GameUI : MonoBehaviour
{
    [Tooltip("首页面")] public GameObject m_frontPage;

    [Tooltip("等级UI前缀图片")] public RectTransform uiLevelImageRectTrans;
    [Tooltip("等级UI后缀文本")] public Text uiLevelLabel;
    // [Tooltip("加速按钮")] public GameObject uiSpeedBtn;
    [Tooltip("Play按钮")] public GameObject uiPlayBtn;

    [Tooltip("球数量根节点")] public GameObject uiBallNumBg;
    [Tooltip("球数量文本")] public Text uiBallNum;

    [Header("UI相机")]
    public Camera cameraNoDes;

    /// <summary> 引导手 </summary>
    private GameObject uiGuideHand;

    private void Awake()
    {
        m_frontPage.SetActive(true);
        // initSpeedBtn();

        //按钮事件
        // Tools.AddButtonEvent(uiSpeedBtn, this.OnClickEventSpeedTime);
        Tools.AddButtonEvent(uiPlayBtn, this.OnClickGameStart);
    }

    public void Init()
    {
        // initSpeedBtn();
    }

    public void SetLevel()
    {
        uiLevelLabel.text = "" + (MainGameScene.instance.currentLVIndex + 1);

        //位置重置
        ContentSizeFitter lvFitter = uiLevelLabel.GetComponent<ContentSizeFitter>();
        RectTransform uiLevelLabelRectTrans = this.uiLevelLabel.GetComponent<RectTransform>();

        lvFitter.SetLayoutHorizontal();

        float halfWidth = uiLevelLabelRectTrans.sizeDelta.x / 2 + 10;
        Print.Log("UIGame  halfWidth= " + halfWidth + ", Image= " + uiLevelImageRectTrans.sizeDelta.x);
        Vector2 pos1 = uiLevelImageRectTrans.anchoredPosition;
        pos1.x = -halfWidth;
        uiLevelImageRectTrans.anchoredPosition = pos1;
        Print.Log("pos1 = " + pos1);

        Vector2 pos2 = uiLevelLabelRectTrans.anchoredPosition;
        pos2.x = pos1.x + uiLevelImageRectTrans.sizeDelta.x / 2 + halfWidth;
        uiLevelLabelRectTrans.anchoredPosition = pos2;
        Print.Log("pos2 = " + pos2);
    }

    /// <summary>加速触发器</summary>
    private float m_runTime;
    private int m_curRoundCount;
    public void SelfUpdate()
    {
        //加速按钮刷新
        // if (GameData.instance.gameSpeedOpen && GameManager.instance.isFired && GameManager.instance.roundCount != m_curRoundCount && !GameManager.instance.gameEnd)
        // {
        //     //加速按钮时间统计
        //     m_runTime += Time.deltaTime;
        //     if (m_runTime >= GameData.instance.triggerGameSpeedTimeValue)
        //     {
        //         m_runTime = 0;
        //         ShowSpeedBtn();
        //     }
        // }
    }

    Vector3 lastPos;

    public void BallUIPosUpdate(GameObject obj, Vector3 m_offset, bool refresh = false)
    {
        //获取模型的世界坐标
        Vector3 curPos = obj.transform.position + m_offset;
        if (lastPos == curPos && refresh == false) return;

        //将3D世界坐标转屏幕空间坐标
        var screenPos = Camera.main.WorldToScreenPoint(curPos);
        //获取UI Camera对应的 Canvas的RectTransform
        var rt = (RectTransform)cameraNoDes.transform.parent.transform;
        Vector3 uiScreenPos;
        //将屏幕空间坐标 转 UI Canvas平面坐标uiScreenPos
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPos, cameraNoDes, out uiScreenPos);
        //将ui的位置同步
        var pos = uiBallNumBg.transform.position;
        pos.x = uiScreenPos.x;
        uiBallNumBg.transform.position = pos;

        lastPos = curPos;
    }

    public void ShowGuideHand(bool isShow)
    {
        if (isShow)
        {
            int needShow = PlayerPrefs.GetInt("IsShowGuide", 0);
            Print.Log("needShow= " + needShow);
            if (needShow != 0) return;

            if (uiGuideHand == null)
            {
                uiGuideHand = Instantiate(CommonPrefabData.instance.uiGuideHand, LayerManager.instance.ImageCanvas.transform);
            }
            uiGuideHand.SetActive(true);
            PlayerPrefs.SetInt("IsShowGuide", 1);
        }
        else
        {
            if (uiGuideHand == null) return;
            Destroy(uiGuideHand);
            uiGuideHand = null;
        }
    }

    ///////////////////按钮事件//////////////////////

    /// 加速按钮
    // private void initSpeedBtn()
    // {
    //     m_runTime = 0;
    //     uiSpeedBtn.SetActive(false);

    //     Time.timeScale = GameManager.instance.timeScale;
    //     m_speedCount = 0;
    // }

    // private void ShowSpeedBtn()
    // {
    //     if (GameManager.instance.isFired == true)
    //     {
    //         m_curRoundCount = GameManager.instance.roundCount;
    //         uiSpeedBtn.SetActive(true);
    //     }
    // }
    // public void HideSpeedBtn()
    // {
    //     uiSpeedBtn.SetActive(false);
    // }


    /// <summary> 游戏加速 </summary>
    // private int m_speedCount = 0;
    // public void OnClickEventSpeedTime()
    // {
    //     if (GameManager.instance.isFired == true)
    //     {
    //         if (m_speedCount == 0)
    //         {
    //             Time.timeScale = GameData.instance.gameSpeedTimeScale;
    //             m_speedCount = 1;
    //             uiSpeedBtn.SetActive(false);
    //         }
    //         else
    //         {
    //             Time.timeScale = GameManager.instance.timeScale;
    //             m_speedCount = 0;
    //         }
    //     }
    // }

    ////开始游戏
    public void OnClickGameStart()
    {
        m_frontPage.SetActive(false);
        GameObject.Destroy(m_frontPage);

        GameManager.instance.gameStart = true;

        //镜头移动
        GameManager.instance.MoveCamera();
    }

}
