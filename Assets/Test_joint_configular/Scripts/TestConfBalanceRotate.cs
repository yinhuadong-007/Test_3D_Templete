using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceRotate : MonoBehaviour
{
    private Transform mtarget;                           //世界中作为角色转向的标杆
    public Transform mTarget
    {
        get
        {
            if (!mtarget)
            {
                mtarget = transform.Find("TargetRotation");
            }
            return mtarget;
        }
    }
    private Vector3 mAngle_direction;                //叉乘向量
    private float mAngle;
    public int TorqueForce = 30;
    private Rigidbody mhip;
    private bool canTorque = true;
    private TestConfBalanceHip hipAni;

    void Start()
    {
        mhip = transform.Find("RigidBodies/Hip").GetComponent<Rigidbody>();
        hipAni = mhip.GetComponent<TestConfBalanceHip>();
    }

    void FixedUpdate()
    {
        // if (mhip == null || !canTorque) return;
        hipAni.TurnBody(mTarget.transform.localRotation);

        // if (mplayer.m_Movement.m_state >= State.down && mplayer.m_Movement.m_state <= State.backDown)
        // {
        //     mAngle = Vector3.Angle(mhip.transform.up, mTarget.up);
        //     mAngle_direction = Vector3.Cross(mhip.transform.up, mTarget.up);
        //     if (mAngle_direction.z > 0)
        //     {
        //         mhip.AddTorque(Vector3.forward * mAngle * TorqueForce, ForceMode.Acceleration);
        //     }
        //     else
        //     {
        //         mhip.AddTorque(-Vector3.forward * mAngle * TorqueForce, ForceMode.Acceleration);
        //     }
        //     if (mAngle < 2)
        //     {
        //         canTorque = false;
        //     }
        // }
        // else
        // {
        // mAngle = Vector3.Angle(mhip.transform.forward, mTarget.forward);                //实时获取TargetRotation与hip的角度差
        // mAngle_direction = Vector3.Cross(mhip.transform.forward, mTarget.forward);
        // AddTorqueWithHip();
        // if (mAngle < 2)
        // {
        //     canTorque = false;
        // }
        // }
    }

    /// <summary>
    /// 根据target点和角色旋转的角度差给对应方向，对应大小力的扭矩
    /// </summary>
    void AddTorqueWithHip()
    {
        if (mAngle_direction.y > 0)
        {
            mhip.AddTorque(Vector3.up * mAngle * TorqueForce, ForceMode.Acceleration);
        }
        else
        {
            mhip.AddTorque(-Vector3.up * mAngle * TorqueForce, ForceMode.Acceleration);
        }
    }

    // public void SetTargetRotate(Quaternion eulerAngle)
    // {
    //     if (mplayer.m_Movement.m_state != st)
    //     {
    //         mTarget.rotation = Quaternion.Euler(0, 0, 0);
    //     }
    //     mTarget.rotation = mTarget.rotation * eulerAngle;
    //     // mTarget.rotation = eulerAngle;
    //     canTorque = true;
    // }

    public void SetTargetRotate2(Vector3 eulerAngle)
    {
        eulerAngle.y = 0;
        mTarget.transform.forward = eulerAngle;
        // mTarget.rotation = eulerAngle;
        canTorque = true;
    }
}
