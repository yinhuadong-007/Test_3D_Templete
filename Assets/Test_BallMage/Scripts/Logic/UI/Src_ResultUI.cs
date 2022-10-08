using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_ResultUI : MonoBehaviour
{
    public void OnClickNext()
    {
        Time.timeScale = GameManager.instance.timeScale;
        // ReportUi.OnVictoryNext(MainGameScene.instance.currentLVIndex + 1);
        MainGameScene.instance.EnterNextChapter();
    }

    public void OnClickReset()
    {
        Time.timeScale = GameManager.instance.timeScale;
        // ReportUi.OnDefeatNext(MainGameScene.instance.currentLVIndex + 1);
        MainGameScene.instance.ResetCurrentChapter();
    }
}
