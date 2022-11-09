
namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class JointAnimationMgr : MonoBehaviour
    {
        public static JointAnimationMgr instance;
        public Rigidbody[] m_rigids;
        public Rigidbody m_head;
        public Rigidbody m_hip;
        public Rigidbody m_torso;
        public Rigidbody m_rightLeg;
        public Rigidbody m_leftLeg;

        public bool m_IsleftSideForward = false;          //身体的左侧是否在前方
        public bool m_CanStepCross = true;                                               //跑步的时候双腿是否可以交叉
        C_JointAnimation[] m_jointAnimations;
        public float m_LegAngle = 90;                                                  //双腿之间的夹角
        public float m_switchTime = 0.2f;                                                //双腿交替的频率
        private float m_counter;
        Player m_player;

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
            m_player = GetComponent<Player>();

            this.m_rigids = GetComponentsInChildren<Rigidbody>();
            m_head = transform.Find("RigidBodies/Head").GetComponent<Rigidbody>();
            m_hip = transform.Find("RigidBodies/Hip").GetComponent<Rigidbody>();
            m_torso = transform.Find("RigidBodies/Torso").GetComponent<Rigidbody>();
            m_rightLeg = transform.Find("RigidBodies/Leg_Right").GetComponent<Rigidbody>();
            m_leftLeg = transform.Find("RigidBodies/Leg_Left").GetComponent<Rigidbody>();
            m_jointAnimations = GetComponentsInChildren<C_JointAnimation>();
        }


        float timer;
        bool flag = false;

        /// <summary>
        /// 控制跑步时候腿的交叉
        /// </summary>
        void FixedUpdate()
        {
            // m_counter += Time.deltaTime;
            // float angle = Vector3.Angle(this.m_leftLeg.transform.up, this.m_rightLeg.transform.up);
            // if (m_leftLeg && m_rightLeg && this.m_counter > this.m_switchTime && angle > m_LegAngle && m_CanStepCross)
            // {
            //     m_IsleftSideForward = !m_IsleftSideForward;
            //     m_counter = 0;
            // }
            if (!m_player.GameStart) return;

            float angle = Vector3.Angle(this.m_leftLeg.transform.up, this.m_rightLeg.transform.up);
            if (m_player.m_Movement.m_state != State.wait && m_player.m_Movement.m_state != State.down
            && m_player.m_Movement.m_state != State.forwardDown
            && m_CanStepCross)
            {
                m_player.m_StayStanding.m_curHeadBalanceHeight = m_player.m_StayStanding.m_moveHeadBalanceHeight;
                timer -= Time.deltaTime;
                if (timer <= 0 && angle > m_LegAngle)
                {
                    timer = this.m_switchTime;
                    m_IsleftSideForward = !m_IsleftSideForward;
                }
                RunAnimation();
            }
            else
            {
                StopAnimation();
                StartCoroutine(DelayDoStand());
            }

            // timer -= Time.deltaTime;
            // if (timer <= 0)
            // {
            //     timer = this.m_switchTime;
            //     m_IsleftSideForward = !m_IsleftSideForward;
            // }
            // RunAnimation();
        }

        IEnumerator DelayDoStand()
        {
            yield return new WaitForSeconds(0.05f);
            m_player.m_StayStanding.m_curHeadBalanceHeight = m_player.m_StayStanding.m_headBalanceHeight;
        }
        /// <summary>
        /// 控制人物的跑步
        /// </summary>
        public void RunAnimation()
        {
            for (int i = 0; i < m_jointAnimations.Length; i++)
            {
                m_jointAnimations[i].JointAnimate(this.m_IsleftSideForward);
            }
        }
        /// <summary>
        /// 停止人物移动
        /// </summary>
        public void StopAnimation()
        {
            for (int i = 0; i < m_jointAnimations.Length; i++)
            {
                m_jointAnimations[i].StopJointAnimate();
            }
        }
    }
}
