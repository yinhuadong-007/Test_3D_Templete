
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceMove : MonoBehaviour
{
    //移动一步的过程步骤
    //1. 出右腿60、右小腿-20、身体Hip同步移动
    [ChineseLabel("重力因子")] public float m_gravityFactor = 1;
    [ChineseLabel("移动一次需要的时间缩放因子")] public float moveOnceTimeFactor = 1;
    [ChineseLabel("跳跃的力")] public float jumpForce = 5000;
    // [ChineseLabel("角色盆骨距离地面的高度")] public float hipHightToPlane = 1.4f;

    [Header("数据展示")]
    public bool isLeftSideForward;
    public Vector3 velocityDirection;
    public Vector3 animateDirection;

    TestConfBalanceAnimate[] testConfBalanceAnimates;
    TestConfBalanceJoint[] testConfBalanceJoints;
    public bool m_isDown;

    Rigidbody m_hip;

    public Vector3 gravity;

    private void Awake()
    {
        gravity = Physics.gravity;
        Physics.gravity = gravity * m_gravityFactor;
    }

    private void Start()
    {
        m_hip = transform.Find("RigidBodies/Hip").GetComponent<Rigidbody>();

        testConfBalanceAnimates = transform.GetComponentsInChildren<TestConfBalanceAnimate>();

        testConfBalanceJoints = transform.GetComponentsInChildren<TestConfBalanceJoint>();

        Vector3 pos = m_hip.transform.position;

        for (int i = 0; i < testConfBalanceJoints.Length; i++)
        {
            testConfBalanceJoints[i].InitBalanceArgs(moveOnceTimeFactor, m_gravityFactor);
        }

        // m_hip.GetComponent<TestConfBalanceJoint>().joint.connectedAnchor = new Vector3(pos.x, hipHightToPlane, pos.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !m_isDown)//被击飞
        {
            Down();
        }
        else if (Input.GetKeyDown(KeyCode.Space))//站起来，跳跃
        {
            if (m_isDown)
            {
                Stand();
            }
            else
            {
                Jump();
            }
        }
    }

    float timer;
    private void FixedUpdate()
    {
        Physics.gravity = gravity * m_gravityFactor;
        if (!m_isDown)
        {
            for (int i = 0; i < testConfBalanceJoints.Length; i++)
            {
                testConfBalanceJoints[i].InitBalanceArgs(moveOnceTimeFactor, m_gravityFactor);
            }
        }

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
        if (velocityDirection != Vector3.zero)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                StopAnimation();
                timer = 0.3f * moveOnceTimeFactor;
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


        // Debug.Log("velocityDirection = " + velocityDirection + ", " + m_hip.transform.forward + ", " + m_hip.transform.right);

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

    public void Down()
    {
        for (int i = 0; i < testConfBalanceJoints.Length; i++)
        {
            testConfBalanceJoints[i].DisableBalance();
        }
        m_isDown = true;
    }

    public void Stand()
    {
        for (int i = 0; i < testConfBalanceJoints.Length; i++)
        {
            testConfBalanceJoints[i].EnableBalance();
        }
        m_isDown = false;
    }

    public void Jump()
    {
        for (int i = 0; i < testConfBalanceJoints.Length; i++)
        {
            TestConfBalanceJoint joint = testConfBalanceJoints[i];
            if (joint.gameObject.name == "Head")
            {
                joint.m_rb.AddForce(Vector3.up * jumpForce * m_gravityFactor, ForceMode.Acceleration);
            }
        }
    }
}

