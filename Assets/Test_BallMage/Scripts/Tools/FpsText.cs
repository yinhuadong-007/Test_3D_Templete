using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FPS 显示于OnGUI 
/// </summary>
public class FpsText : MonoBehaviour
{
    public static bool show = false;

    public UnityEngine.UI.Text label;
    public int fps = -1;
    float updateInterval = 1.0f;   //当前时间间隔
    private float accumulated = 0.0f;  //在此期间累积 
    private float frames = 0;    //在间隔内绘制的帧 
    private float timeRemaining;   //当前间隔的剩余时间
    private float curFps = 30.0f;    //当前帧 Current FPS
    private float lastSample;

    void Start()
    {
        if (fps > 0)
        {
            Application.targetFrameRate = fps;
        }
        if (show)
        {
            DontDestroyOnLoad(this.gameObject); //不销毁此游戏对象，在哪个场景都可以显示，，不需要则注释
        }

        timeRemaining = updateInterval;
        lastSample = Time.realtimeSinceStartup; //实时自启动

    }

    void Update()
    {
        if (!show) return;
        ++frames;
        float newSample = Time.realtimeSinceStartup;
        float deltaTime = newSample - lastSample;
        lastSample = newSample;
        timeRemaining -= deltaTime;
        accumulated += 1.0f / deltaTime;

        if (timeRemaining <= 0.0f)
        {
            curFps = accumulated / frames;
            timeRemaining = updateInterval;
            accumulated = 0.0f;
            frames = 0;
        }
    }

    void OnGUI()
    {
        if (!show) return;
        var str = string.Format("FPS:{0:0.}", curFps);//"FPS:" + curFps.ToString("f0")
        if (label != null && label.gameObject != null)
        {
            label.text = str;
        }
        else
        {
            GUIStyle style = new GUIStyle
            {
                border = new RectOffset(10, 10, 10, 10),
                fontSize = 50,
                fontStyle = FontStyle.BoldAndItalic,
                alignment = TextAnchor.UpperLeft,
            };
            //自定义宽度 ，高度大小 颜色，style

            GUI.Label(new Rect(50, 50, 200, 100),
                string.Format("<color=#ff0000><size=50>{0}</size></color>", str), style);
        }
    }
}