namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// 控制人物站立类
    /// </summary>
    public class StayStanding : MonoBehaviour
    {

        public float m_UpMuliForceFadeSpeed = 500;                                          //额外上拉力的衰减速度
        // [HideInInspector]
        public float m_StandUpForce;                                                         //人物平衡时候给头部向上的力
        // [HideInInspector]
        public float m_StandUpMultiForce;                                                   //物体在stand平衡之前为了达到平衡给头部的额外的上拉力
        // [HideInInspector]
        public float m_gravityFactor = 0;                                                    //模拟额外重力的因子
        // [HideInInspector]
        public float m_gravityForceMultiplayer = 4000;
        public float m_headBalanceHeight = 1.9f;               //静止时头部在平衡状态下到地面的距离
        public float m_moveHeadBalanceHeight = 1.9f;               //移动时头部在平衡状态下到地面的距离

        JointAnimationMgr m_jointAnimationMgr;
        Rigidbody[] m_rigids;
        Rigidbody m_head;
        BodyDetect m_bodyDetect;
        Player m_player;

        public float m_curHeadBalanceHeight;

        void Awake()
        {

        }
        void Start()
        {
            m_player = GetComponent<Player>();
            m_jointAnimationMgr = m_player.m_JointAnimationMgr;
            m_bodyDetect = m_player.m_BodyDetect;
            m_rigids = m_jointAnimationMgr.m_rigids;
            m_head = m_jointAnimationMgr.m_head;
            m_StandUpForce = GetTotalMassInChild();
            Debug.Log("m_StandUpForce= " + m_StandUpForce);
            m_curHeadBalanceHeight = m_headBalanceHeight;
        }


        void FixedUpdate()
        {
            m_StandUpMultiForce = m_UpMuliForceFadeSpeed * (m_curHeadBalanceHeight - m_bodyDetect.mheadDistanceToGround);
        }

        /// <summary>
        /// 物体在空中的时候添加的额外的向下的力
        /// </summary>
        public void ExternalGravity()
        {
            Debug.Log("--->StayStanding  ExternalGravity");
            for (int i = 0; i < m_rigids.Length; i++)
            {
                m_rigids[i].AddForce(Vector3.down * m_gravityForceMultiplayer * m_gravityFactor * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }

        /// <summary>
        /// 为了平衡，给头部添加的向上的力
        /// </summary>
        public void StandBalance()
        {
            Debug.Log("--->StayStanding  StandBalance");
            m_head.AddForce(Vector3.up * (m_StandUpForce + m_StandUpMultiForce), ForceMode.Acceleration);
        }

        /// <summary>
        /// 计算角色整体的质量
        /// </summary>
        /// <returns></returns>
        float GetTotalMassInChild()
        {
            for (int i = 0; i < m_rigids.Length; i++)
            {
                m_StandUpForce += m_rigids[i].mass;
            }
            return m_StandUpForce;
        }

    }
}