using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 发射器基类 - 玩法基类
/// </summary>
public class Src_BaseShotMachine : MonoBehaviour
{
    /// <summary> 发射器类型 </summary>
    [HideInInspector, Tooltip("发射器类型")] public EShotMachineType shotType;

    ///<summary> 旋转节点（炮管） </summary>
    [SerializeField, Tooltip("旋转节点（炮管）")] protected Transform m_rotateNode;

    ///<summary> 发射出生点 </summary>
    [SerializeField, Tooltip("发射出生点")] public Transform lifePoint;

    ///<summary> 皮肤根节点 </summary>
    [SerializeField, Tooltip("皮肤根节点")] private Transform skinRoot;

    ///<summary> 发射器内小球出生点 </summary>
    [SerializeField, Tooltip("发射器内小球出生点")] public Transform littleBallLifePoint;


    [Tooltip("当前使用的皮肤")] public GameObject curSkin;

    private Animator m_curShotAni;

    // [HideInInspector] public UIText uiText;

    protected Vector3 m_initPaoHeadAngle;

    public Transform rotateNode
    {
        get { return m_rotateNode; }
    }

    public virtual void Init()
    {
        for (int i = 0, length = skinRoot.childCount; i < length; i++)
        {
            skinRoot.GetChild(i).gameObject.SetActive(false);
        }

        m_initPaoHeadAngle = this.m_rotateNode.eulerAngles;
    }

    /// <summary>
    /// 检查旋转是否在范围内
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckRotateInRange()
    {
        return true;
    }

    /// <summary>
    /// 设置旋转
    /// </summary>
    /// <param name="angle">角度</param>
    /// <returns>是否成功改变旋转</returns>
    public bool SetRotateBySubAngleY(float angle)
    {
        Vector3 lastAngle = this.m_rotateNode.eulerAngles;
        this.m_rotateNode.eulerAngles = new Vector3(m_initPaoHeadAngle.x, angle - m_initPaoHeadAngle.y, m_initPaoHeadAngle.z);
        if (CheckRotateInRange())
        {
            return true;
        }
        this.m_rotateNode.eulerAngles = lastAngle;
        return false;
    }

    /// <summary>
    /// 发射器炮管旋转到目标角度
    /// </summary>
    public void RotateToTargetAngle(float targetAngleY, float t, Action cbk = null)
    {
        Vector3 targetAngle = this.m_rotateNode.eulerAngles;
        targetAngle.y = targetAngleY;
        Tween.target(this.m_rotateNode)
        .toRotation(
            targetAngle,
            t,
            () =>
            {
                if (cbk != null) cbk();
            })
        .start(true);
    }



    void SetTrigger(string trigger)
    {
        Print.Log("---Shot SetTrigger---  " + trigger);
        Tools.ResetAllTriggers(m_curShotAni);

        m_curShotAni.SetTrigger(trigger);
    }

    public void PlayShotAni()
    {
        m_curShotAni.SetTrigger("shot");
    }
    public void StopShotAni()
    {
        m_curShotAni.SetTrigger("idle");
    }

}
