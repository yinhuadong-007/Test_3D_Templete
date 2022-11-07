namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    //检测头部和地面的高度
    public class BodyDetect : MonoBehaviour
    {
        float m_Groundtime = 1.5f;   //弹跳间隔
        float m_Groundtimer;
        float m_Airtimer;
        Player mplayer;
        [HideInInspector]
        public float mheadDistanceToGround;//头部和地面的距离
        public float m_GravityFactor = 1.5f;
        [HideInInspector]
        public bool mCanJumpWallRebound = false;//判断是否可以在爬墙的时候反弹
        [HideInInspector]
        public BodyState mbodystate;//身体所处得状态（地面/空中）

        StayStanding m_StayStanding;
        C_Movement m_Cmovement;

        Rigidbody mhead;
        Rigidbody mhip;
        PunchForce[] m_coll;
        RaycastHit mhit;


        void Start()
        {
            mplayer = GetComponent<Player>();
            m_StayStanding = mplayer.m_StayStanding;
            m_Cmovement = mplayer.m_Movement;
            m_coll = GetComponentsInChildren<PunchForce>();
            mhead = mplayer.m_JointAnimationMgr.m_head;
        }

        void FixedUpdate()
        {
            HeadRayCast();

            // if (!IsBodyGround() && m_Groundtimer < m_Groundtime || (m_Cmovement.m_state == State.up
            //     || m_Cmovement.m_state == State.rightUp || m_Cmovement.m_state == State.leftUp || m_Cmovement.m_IsInWater))
            // {
            //     return;
            // }
            if (!IsBodyGround() || (m_Cmovement.m_state == State.up || m_Cmovement.m_state == State.rightUp
            || m_Cmovement.m_state == State.leftUp))
            {
                return;
            }
            // Debug.Log("mheadDistanceToGround = " + mheadDistanceToGround);
            m_StayStanding.StandBalance();
        }


        //头部检测与地面的距离

        void HeadRayCast()
        {
            Ray ray = new Ray(mhead.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out mhit, 20, 1 << TagLayer.layer_Plane))
            {
                mheadDistanceToGround = mhead.transform.position.y - mhit.point.y;
            }
        }

        public bool IsBodyGround()
        {
            for (int i = 0; i < m_coll.Length; i++)
            {
                PunchForce item = m_coll[i];
                if (item.IsCollisionGround)
                {
                    // if (item.GetComponent<LeftKnee>() != null || item.GetComponent<RightKnee>() != null || item.GetComponent<Hip>() != null
                    //     || item.GetComponent<LeftLeg>() != null || item.GetComponent<RightLeg>() != null)
                    // {
                    //     //播放跑步特效
                    //     if ((item.GetComponent<LeftKnee>() != null || item.GetComponent<RightKnee>() != null)
                    //         && (m_Cmovement.m_state == State.forward || m_Cmovement.m_state == State.back))
                    //     {
                    //         PlayGroundEffect(item.groundPoint);
                    //     }
                    //     m_StayStanding.m_gravityFactor = 0;
                    //     mheadDistanceToGround = mhead.transform.position.y - item.groundPoint.y;
                    //     mCanJumpWallRebound = false;
                    //     return true;
                    // }
                    // continue;

                    m_StayStanding.m_gravityFactor = 0;
                    mheadDistanceToGround = mhead.transform.position.y - item.groundPoint.y;

                    mCanJumpWallRebound = false;
                    return true;
                }
            }
            // if (m_Cmovement.m_state == State.forward)
            // {
            //     m_StayStanding.m_gravityFactor = 0;
            //     mheadDistanceToGround += 1f;
            //     return true;
            // }
            // else
            // {
            m_StayStanding.m_gravityFactor += m_GravityFactor * Time.fixedDeltaTime;
            if (m_StayStanding.m_gravityFactor >= 2)
            {
                m_StayStanding.m_gravityFactor = 2;
            }
            // }
            StartCoroutine(ExternalGravity());
            mCanJumpWallRebound = true;
            return false;
        }

        //播放跑步特效
        void PlayGroundEffect(Vector3 pos)
        {
            // xingnengMgr.getMe().createTexiaopaobu(pos, Quaternion.identity);
        }

        IEnumerator ExternalGravity()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            m_StayStanding.ExternalGravity();
        }


    }
}
