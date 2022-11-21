using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceAnimate : MonoBehaviour
{
    [Tooltip("是不是身体左侧")] public bool isBodyLeftSide;
    [Tooltip("正向旋转角度")] public float forwardRotateAngle;
    [Tooltip("反向旋转角度")] public float backRotateAngle;
    [Tooltip("左右是否相反")] public bool isNegate;

    [ChineseLabel("受到的力")] public float forceMultiplier = 0;

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
        if ((leftForward && isBodyLeftSide) || (!leftForward && !isBodyLeftSide))
        {
            if (direction.z != 0)
            {
                direction.z = direction.z > 0 ? 1 : -1;
            }
            if (direction.x != 0)
            {
                direction.x = direction.x > 0 ? 1 : -1;
            }
            if (direction.y != 0)
            {
                direction.y = direction.y > 0 ? 1 : -1;
            }
            Vector3 euler = new Vector3(direction.z, -direction.x, direction.y) * forwardRotateAngle;
            jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(euler), jointVal.StartLocalRotation);
        }
    }

    /// <summary>
    /// 移动关节
    /// </summary>
    /// <param name="leftForward"></param>
    public virtual void MoveJoint(Vector3 direction, bool leftForward)
    {
        this.rig.AddForce(direction * Time.fixedDeltaTime * forceMultiplier, ForceMode.Acceleration);
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

    public virtual void DownJointAnimate()
    {

    }

}
