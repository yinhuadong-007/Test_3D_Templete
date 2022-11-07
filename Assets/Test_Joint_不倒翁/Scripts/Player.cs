namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        public StayStanding m_StayStanding;
        public BodyDetect m_BodyDetect;
        public JointAnimationMgr m_JointAnimationMgr;
        public PlayerRigidBodyMgr m_playerRigidBodyMgr;
        public C_Movement m_Movement;

        public bool GameStart;
        // Start is called before the first frame update
        void Awake()
        {
            m_StayStanding = GetComponent<StayStanding>();
            m_BodyDetect = GetComponent<BodyDetect>();
            m_JointAnimationMgr = GetComponent<JointAnimationMgr>();
            m_playerRigidBodyMgr = GetComponent<PlayerRigidBodyMgr>();
            m_Movement = GetComponent<C_Movement>();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            GameStart = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
