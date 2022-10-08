using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_GM : MonoBehaviour
{
    [SerializeField, Tooltip("GM content")] private GameObject m_GMContent;

    private int m_clickTotalCount = 1;
    private int m_clickShowGMCount = 0;

    private bool m_showGM = false;

    private void Start()
    {
        m_showGM = false;
        m_GMContent.SetActive(false);
    }


    public void OnClickShowGM()
    {
        m_clickShowGMCount++;
        if (m_clickShowGMCount == 1)
        {
            Tween.delay(0.5f, () =>
            {
                m_clickShowGMCount = 0;
                Print.Log("-----显示/隐藏GM----- reset");
            }).start();
        }
        if (m_clickShowGMCount >= m_clickTotalCount)
        {

            m_clickShowGMCount = 0;
            if (m_showGM)
            {
                Print.Log("-----隐藏GM----- success");
                m_showGM = false;
            }
            else
            {
                Print.Log("-----显示GM----- success");
                m_showGM = true;
            }
            m_GMContent.SetActive(m_showGM);
        }
    }

    private int m_clickResetCount = 0;
    public void OnClickReset()
    {
        m_clickResetCount++;
        if (m_clickResetCount == 1)
        {
            Tween.delay(0.5f, () =>
            {
                m_clickResetCount = 0;
                Print.Log("-----重置关卡----- reset");
            }).start();
        }
        if (m_clickResetCount >= m_clickTotalCount)
        {
            Print.Log("-----重置关卡----- success");
            m_clickResetCount = 0;
            // GameGlobal.GetInstance().ResetCurrentScene();
            MainGameScene.instance.ResetCurrentChapter();
        }
        Print.Log("-----重置关卡----- count : " + m_clickResetCount);
    }

    private int m_clickNextCount = 0;
    public void OnClickNext()
    {
        m_clickNextCount++;
        if (m_clickNextCount == 1)
        {
            Tween.delay(0.5f, () =>
            {
                m_clickNextCount = 0;
                Print.Log("-----下个关卡----- reset");
            }).start();
        }
        if (m_clickNextCount >= m_clickTotalCount)
        {
            Print.Log("-----下个关卡----- success");
            m_clickNextCount = 0;
            // GameGlobal.GetInstance().EnterNextScene();
            MainGameScene.instance.EnterNextChapter();
        }
        Print.Log("-----下个关卡----- count : " + m_clickNextCount);
    }
}
