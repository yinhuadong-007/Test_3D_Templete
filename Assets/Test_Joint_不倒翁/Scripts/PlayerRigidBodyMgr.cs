namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// 人物刚体编号管理
    /// </summary>
    public class PlayerRigidBodyMgr : MonoBehaviour
    {
        public Rigidbody[] m_AllRigidBodies;
        public Rigidbody m_HipRigidBody;

        private void Awake()
        {
            InitRigidBodies();
            m_HipRigidBody = transform.Find("RigidBodies/Hip").GetComponent<Rigidbody>();
        }

        private void InitRigidBodies() //初始化刚体，编号
        {
            this.m_AllRigidBodies = base.GetComponentsInChildren<Rigidbody>();
            int length = this.m_AllRigidBodies.Length;
            for (byte i = 0; i < length; i = (byte)(i + 1))
            {
                Rigidbody rigidbody = this.m_AllRigidBodies[i];
                // rigidbody.gameObject.AddComponent<RigidBodyIndexHolder>().InitIndex(i);
            }
            IgnoreCollision();
        }

        public void IgnoreCollision()
        {
            for (int i = 0; i < m_AllRigidBodies.Length - 1; i++)
            {
                for (int j = i + 1; j < m_AllRigidBodies.Length; j++)
                {
                    Collider collider1 = m_AllRigidBodies[i].GetComponent<Collider>();
                    Collider collider2 = m_AllRigidBodies[j].GetComponent<Collider>();
                    Physics.IgnoreCollision(collider1, collider2, true);
                }
            }
        }

    }
}
