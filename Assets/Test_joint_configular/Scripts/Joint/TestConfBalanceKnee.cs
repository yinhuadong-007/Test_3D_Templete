using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小腿
/// </summary>
public class TestConfBalanceKnee : TestConfBalanceAnimate
{
    /// <summary>
    /// 小腿
    /// </summary>
    /// <param name="leftForward"></param>
    public override void JointAnimate(Vector3 direction, bool leftForward)
    {
        if ((leftForward && isBodyLeftSide) || (!leftForward && !isBodyLeftSide))
        {
            if (direction.z != 0)
            {
                direction.z = direction.z > 0 ? 1 : -1;
            }
            Vector3 euler = new Vector3(direction.z * forwardRotateAngle, 0, 0);
            jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(euler), jointVal.StartLocalRotation);
        }
    }
}
