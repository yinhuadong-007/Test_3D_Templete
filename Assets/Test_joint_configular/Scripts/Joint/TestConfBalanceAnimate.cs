using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceAnimate : MonoBehaviour
{
    [Tooltip("是不是身体左侧")] public bool isBodyLeftSide;
    [Tooltip("正向旋转角度")] public float forwardRotateAngle;
    [Tooltip("反向旋转角度")] public float backRotateAngle;
    [Tooltip("左右是否相反")] public bool isNegate;

    protected Rigidbody rig;
    protected Rigidbody hip;

    protected TestConfBalanceJoint jointVal;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        jointVal = GetComponent<TestConfBalanceJoint>();
    }

    /// <summary>
    /// 控制角色跑步身体关节的运动
    /// </summary>
    /// <param name="leftForward"></param>
    public virtual void JointAnimate(Vector3 direction, bool leftForward)
    {

    }

    /// <summary>
    /// 移动关节
    /// </summary>
    /// <param name="leftForward"></param>
    public virtual void MoveJoint(Vector3 direction, bool leftForward)
    {

    }

    /// <summary>
    /// 停止运动
    /// </summary>
    /// <param name="leftForward"></param>
    public virtual void StopJointAnimate()
    {
        if (jointVal != null)
        {
            jointVal.joint.SetTargetRotationLocal(jointVal.StartLocalRotation, jointVal.StartLocalRotation);
        }

    }

}
