using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenSequence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //动画队列 依次执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.PrependCallback(() => { Debug.Log("TweenSequence PrependCallback"); });
        sequence.AppendCallback(() => { Debug.Log("TweenSequence AppendCallback"); });
        sequence.Append(transform.DOMove(new Vector3(2, 2, 2), 2));
        sequence.Join(transform.DOScale(Vector3.one * 3, 4));   //加入到上一个动画一起执行 ↑

        sequence.AppendInterval(2);//等待两秒
        sequence.Append(transform.DOMove(new Vector3(2, 0, 8), 2));
        sequence.Join(transform.DOScale(Vector3.one, 2));
        sequence.Insert(2, transform.DOMove(-Vector3.one, 1)); //插入动画 1：哪个时间插入 2：动画；一秒后移动到-1,-1,-1
        sequence.Prepend(transform.DOMove(new Vector3(-2, -2, -2), 2)); // 预添加 后添加先执行
        sequence.InsertCallback(5, () => { Debug.Log("TweenSequence InsertCallback"); }); //在第五秒插入回调函数
    }

    // Update is called once per frame
    void Update()
    {

    }
}
