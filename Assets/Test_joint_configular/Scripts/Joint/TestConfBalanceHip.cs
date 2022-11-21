using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 盆骨
/// </summary>
public class TestConfBalanceHip : TestConfBalanceAnimate
{
    // [ChineseLabel("每步的距离")] public float m_stepDistance = 2;

    bool lastDir;

    /// <summary>
    /// 盆骨
    /// </summary>
    /// <param name="leftForward"></param>
    // public override void JointAnimate(Vector3 direction, bool leftForward)
    // {
    //     if ((leftForward && isBodyLeftSide) || (!leftForward && !isBodyLeftSide))
    //     {
    //         jointVal.joint.targetPosition -= direction * m_stepDistance;
    //     }
    // }

    /// <summary>
    /// 盆骨
    /// </summary>
    /// <param name="leftForward"></param>
    public override void MoveJoint(Vector3 direction, bool leftForward)
    {
        // if ((leftForward && isBodyLeftSide) || (!leftForward && !isBodyLeftSide))
        // {
        // Vector3 euler = new Vector3(direction.z * forwardRotateAngle, 0, 0);
        // jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(euler), jointVal.StartLocalRotation);
        this.rig.AddForce(direction * Time.fixedDeltaTime * forceMultiplier, ForceMode.Acceleration);
        // }

        // if (lastDir != leftForward)
        // {
        //     lastDir = leftForward;
        //     jointVal.joint.connectedAnchor += direction * m_stepDistance;
        // }
    }

    public override void StopJointAnimate()
    {

    }
    public void TurnBody(Quaternion euler)
    {
        jointVal.joint.SetTargetRotationLocal(euler, jointVal.StartLocalRotation);
    }
}
