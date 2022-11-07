
namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    /// <summary>
    /// 处理碰撞类
    /// </summary>
    public class PunchForce : MonoBehaviour
    {

        public bool IsCollisionGround = false;

        public bool IsCollisionWall = false;
        [HideInInspector]
        public Vector3 groundPoint; //地面碰撞点
        [HideInInspector]
        public Vector3 wallPoint;   //墙面碰撞点
        [HideInInspector]
        public Vector3 wallnormal;  //碰撞法线方向

        [HideInInspector]
        public float m_StemEmptyTime;
        public float m_StemEmptyTimer;//踩空时间

        private Collider _collider;


        void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.root == base.transform.root) //如果是自己碰自己，返回
            {
                return;
            }
            if (collision.collider.tag == "Rigidbodys" || collision.collider.tag == "wall")
            {
                return;
            }
            if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) > 95f) //朝碰撞体反面跳
            {
                return;
            }
            _collider = collision.collider;
            if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < 70 && collision.collider.tag == "Ground")
            {
                IsCollisionGround = true;
                m_StemEmptyTimer = 0;
                groundPoint = collision.contacts[0].point;
            }
            if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) > 75)
            {
                if (collision.collider.tag == "bullet" || collision.collider.tag == "weapon" || collision.collider.tag == "airwall")
                    return;
                this.wallnormal = collision.contacts[0].normal;
                IsCollisionWall = true;
                m_StemEmptyTimer = 1;
                wallPoint = collision.contacts[0].point;
            }
        }


        private void OnCollisionStay(Collision collision)
        {
            if (collision.transform.root == base.transform.root) //如果是自己碰自己，返回
            {
                return;
            }
            if (collision.collider.CompareTag("Rigidbodys") || collision.collider.tag == "wall")
            {
                return;
            }
            if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) > 95f) //朝碰撞体反面跳
            {
                return;
            }
            _collider = collision.collider;

            if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < 70 && collision.collider.tag == "Ground")
            {
                IsCollisionGround = true;
                groundPoint = collision.contacts[0].point;
            }
            if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) > 75)
            {
                if (collision.collider.tag == "bullet" || collision.collider.tag == "weapon" || collision.collider.tag == "airwall")
                    return;
                this.wallnormal = collision.contacts[0].normal;
                IsCollisionWall = true;
                m_StemEmptyTimer = 1;
                wallPoint = collision.contacts[0].point;
            }
        }


        void OnCollisionExit(Collision other)
        {
            if (other.transform.root == transform.root)
            {
                return;
            }
            if (other.collider.tag == "Ground" || other.collider.CompareTag("Rigidbodys"))
            {
                IsCollisionGround = false;
                IsCollisionWall = false;
            }
        }

        void Exit()
        {
            IsCollisionGround = false;
            IsCollisionWall = false;
        }

        private void Update()
        {
            if (_collider == null)
            {
                Exit();
            }
            if (!IsCollisionGround)
            {
                m_StemEmptyTimer += Time.deltaTime;
            }
        }
    }
}
