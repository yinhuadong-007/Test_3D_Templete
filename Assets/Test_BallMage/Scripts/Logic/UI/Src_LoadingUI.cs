using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Src_LoadingUI : MonoBehaviour
{
    [SerializeField, Tooltip("进度条")] private Slider m_slider;
    [SerializeField, Tooltip("加载时间")] private float loadTime = 2f;
    float curProgress;
    float unitProgress;

    bool startLoading = false;
    Action actContent;
    float actProgress;

    public void Init(Action cbk)
    {
        curProgress = 0;
        unitProgress = 100 / loadTime;
        m_slider.value = 0;
        startLoading = true;
        actProgress = UnityEngine.Random.Range(30, 70);
        actContent = cbk;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startLoading) return;
        curProgress += Time.deltaTime * unitProgress;

        //加载到一定位置开始加载游戏场景
        if (actContent != null && curProgress > actProgress)
        {
            actContent();
            actContent = null;
        }

        if (curProgress > 100)
        {
            curProgress = 100;
            //加载完毕
            Destroy(this.gameObject);
            return;
        }
        m_slider.value = curProgress / 100;
    }
}
