using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Src_Move : MonoBehaviour
{
    public Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(targetPos, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
