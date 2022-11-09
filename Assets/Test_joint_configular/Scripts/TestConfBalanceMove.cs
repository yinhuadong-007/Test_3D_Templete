
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceMove : MonoBehaviour
{
    //移动一步的过程步骤
    //1. 出右腿60、右小腿-20、身体Hip同步移动

    [ChineseLabel("移动一次需要的时间")] public float moveOnceTime = 1;
    // [ChineseLabel("角色盆骨距离地面的高度")] public float hipHightToPlane = 1.4f;
    public bool isLeftSideForward;
    public Vector3 velocityDirection;
    public Vector3 animateDirection;

    TestConfBalanceAnimate[] testConfBalanceAnimates;

    Rigidbody m_hip;

    private void Start()
    {
        m_hip = transform.Find("RigidBodies/Hip").GetComponent<Rigidbody>();

        testConfBalanceAnimates = transform.GetComponentsInChildren<TestConfBalanceAnimate>();

        Vector3 pos = m_hip.transform.position;
        // m_hip.GetComponent<TestConfBalanceJoint>().joint.connectedAnchor = new Vector3(pos.x, hipHightToPlane, pos.z);
    }

    float timer;
    private void FixedUpdate()
    {
        velocityDirection = Vector3.zero;
        animateDirection = Vector3.zero;
        bool isL_R = false;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            velocityDirection = Input.GetKey(KeyCode.W) ? m_hip.transform.forward : -m_hip.transform.forward;
            animateDirection = Input.GetKey(KeyCode.W) ? Vector3.forward : Vector3.back;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            velocityDirection += Input.GetKey(KeyCode.A) ? -m_hip.transform.right : m_hip.transform.right;
            animateDirection += Input.GetKey(KeyCode.A) ? Vector3.left : Vector3.right;
            isL_R = true;
        }
        // Debug.Log("velocityDirection = " + velocityDirection + ", " + m_hip.transform.forward + ", " + m_hip.transform.right);
        if (velocityDirection != Vector3.zero)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                StopAnimation();
                timer = this.moveOnceTime;
                if (isL_R)
                {
                    isLeftSideForward = Input.GetKey(KeyCode.A) ? true : false;
                }
                else
                {
                    isLeftSideForward = !isLeftSideForward;
                }

            }
            else
            {
                RunAnimation();
            }
        }
        else
        {
            timer = 0;
            StopAnimation();
        }
    }

    /// <summary>
    /// 控制人物的跑步
    /// </summary>
    public void RunAnimation()
    {
        for (int i = 0; i < testConfBalanceAnimates.Length; i++)
        {
            testConfBalanceAnimates[i].MoveJoint(velocityDirection, this.isLeftSideForward);
            testConfBalanceAnimates[i].JointAnimate(animateDirection, this.isLeftSideForward);
        }

    }
    /// <summary>
    /// 停止人物移动
    /// </summary>
    public void StopAnimation()
    {
        for (int i = 0; i < testConfBalanceAnimates.Length; i++)
        {
            testConfBalanceAnimates[i].StopJointAnimate();
        }
    }
}

