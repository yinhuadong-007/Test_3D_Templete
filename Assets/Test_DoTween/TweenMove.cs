using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using System.Linq;

public class TweenMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Tweener _tweener;

    public Transform[] positions;
    async void Start()
    {
        // this.transform.DOMove(new Vector3(3, 3, 3), 1);
        // this.transform.DOMove(new Vector3(-3, -3, -3), 3);
        //后面的DOMove覆盖前面的

        // this.transform.DOLocalMove(new Vector3(-3, -3, -3), 3);

        // this.transform.DOLookAt(new Vector3(-3, -3, -3), 3);

        // this.transform.DORotate(new Vector3(0, 90, 0), 3);
        // this.transform.DOScale(new Vector3(2, 2, 2), 3);

        //来回弹跳 第一个参数：位置 方向 第二个参数：持续时间 第三个参数：弹跳次数（频率） 第四个参数：越大向相反方向移动越大
        // transform.DOPunchPosition(new Vector3(0, 3, 0), 4, 3, 0);
        // transform.DOPunchRotation(new Vector3(0, 90, 0), 4, 3, 0);

        ///shake 1:持续时间 2：范围 3：频率
        // transform.DOShakePosition(2, new Vector3(0, 3, 0), 10);
        // transform.DOShakeRotation(2, new Vector3(90, 90, 90), 2);

        //blend 动画混合 增量动画
        // transform.DOBlendableMoveBy(new Vector3(2, 2, 2), 2);
        // transform.DOBlendableMoveBy(new Vector3(-1, -1, -2), 2);

        //循环
        // transform.DOMove(new Vector3(3, 3, 3), 2).SetLoops(-1, LoopType.Incremental);

        // transform.DOMove(new Vector3(2, 2, 2), 2).From();//从目标点到起始点
        // transform.DOMove(Vector3.one, 2).SetDelay(3);//设置延迟多少秒后执行动画 单位秒
        // transform.DOMove(Vector3.one, 2).SetSpeedBased();//2代表移动速度 设置以速度为基准
        // transform.DOMove(Vector3.one * 3, 2).SetRelative(true);//相对运动 即增量运动
        // transform.DOMove(Vector3.one * 3, 2).SetId("Ani1");//设置动画ID 方便再次使用
        // transform.DOMove(Vector3.one * 2, 2).SetUpdate(UpdateType.Fixed); // 设置帧动画更新类型 normal普通 fixed late

        // Time.timeScale = 0;
        // transform.DOMove(Vector3.one * 10, 5).SetUpdate(UpdateType.Normal, true); //不受unity Time.timeScale影响

        // transform.DOMove(Vector3.one * 5, 2).SetEase(Ease.InBack);//设置动画速度变化的运动曲线

        // transform.DOMove(Vector3.one * 5, 2).SetEase(Ease.Flash, 10, 1); //10 震动次数 1 从大到小

        //生命周期的回调函数
        // transform.DOMove(Vector3.one * 2, 2).SetLoops(3, LoopType.Yoyo)
        //    .OnRewind(() => { Debug.Log("OnRewind"); })
        //    .OnStart(() => { Debug.Log("OnStart"); })
        //    .OnUpdate(() => { Debug.Log("OnUpdate"); })
        //    .OnPlay(() => { Debug.Log("OnPlay"); })
        //    .OnComplete(() => { Debug.Log("OnComplete"); })
        //    .OnStepComplete(() => { Debug.Log("OnStepComplete"); })
        //    .OnKill(() => { Debug.Log("OnKill"); })
        //    .OnPause(() => { Debug.Log("OnPause"); });


        // //控制函数
        // transform.DOMove(Vector3.one * 2, 2);
        // await Task.Delay(TimeSpan.FromSeconds(1));
        // transform.DOPause();//暂停
        // await Task.Delay(TimeSpan.FromSeconds(1));
        // transform.DOPlay();//开始播放
        // transform.DORestart();//重新播放
        // transform.DORewind();//倒播 一下子回到起始点  播放完不回倒
        // transform.DOSmoothRewind();//倒播 平滑的回到起始点 播放完不回倒

        // transform.DOMove(Vector3.one * 2, 2);
        // transform.DOFlip();  //动画翻转 终点变为起始点

        //transform.DOGoto(1,true);//跳转到动画第几秒，true 接着播放动画
        // transform.DOPlayBackwards();//倒放
        //transform.DOTogglePause();//暂停开始开关；暂停是调用则播放，播放时调用则暂停

        // List<Tween> list = DOTween.PausedTweens(); // 获取所有暂停的动画
        //List<Tween> list2 = DOTween.PlayingTweens();//获取所有正在播放的动画
        //DOTween.TweensByTarget(transform,true);  获取目标的动画，为true表示正在播放的动画

        //Tweener tweener = transform.DOMove(Vector3.one * 2, 2);
        //await Task.Delay(TimeSpan.FromSeconds(1));
        //Debug.Log(tweener.fullPosition);
        //await Task.Delay(TimeSpan.FromSeconds(1));
        //tweener.fullPosition = 0; // 动画播放的时间点 （可读可写） 修改后可以从另一个时间点运动
        //tweener.Elapsed(); // 已运动的时间  true 包括循环 false 不包括
        //tweener.ElapsedPercentage();//同上 不过是百分比

        //通过协程执行回调
        // _tweener = transform.DOMove(Vector3.one * 2, 2).SetLoops(3);
        // StartCoroutine(Wait());


        //路径动画
        var posiList = positions.Select(t => t.position).ToArray();
        //路径点 持续时间 路线类型（直线 曲线） 路径类型 路线精细程度 路线颜色 
        transform.DOPath(posiList, 5, PathType.Linear, PathMode.Full3D, 50, Color.blue)
            .SetOptions(true, AxisConstraint.None, AxisConstraint.None); //true收尾相连 锁定x轴位移（x值不发生变化） 锁定Y轴旋转
    }

    private IEnumerator Wait()
    {
        //yield return _tweener.WaitForCompletion(); // 动画执行完成
        yield return _tweener.WaitForElapsedLoops(3); // 循环执行几次后
        Debug.Log("DDD");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
