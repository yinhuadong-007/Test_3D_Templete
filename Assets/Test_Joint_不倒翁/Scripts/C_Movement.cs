namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    ////角色 行走 跳跃 跑墙 控制

    public class C_Movement : MonoBehaviour
    {
        public State m_state;
        public bool m_IsInWater;
        public float stepEmptyTime = 0.15f;
        public float m_HorizonralexternalForce = 45;                                           //控制物体水平移动的外力
        public float m_Up_Horizon = 100;                                                        //控制物体斜跳的水平力
        public float m_VerticalexternalForce = 200;                                             //向下的力
        public float m_jumpForce = 30;                                                         //控制物体跳起的外力
        public float m_HorizonalWalkForce = 10;                                                //控制物体左走的外力
        public float m_jumpForceMultiplier = 25;                                              //弹跳系数
        public float m_torsoForce = 0;                                                        //支撑脖子的力
        public float m_Down_Vertical = 80;                                                     //控制物体匍匐前进的下压力
        public float m_Down_MoveForce = 200;                                             //匍匐前行的力


        public Face m_face;     //朝向

        private BodyDetect m_bodydect;  //检测头部和地面的高度类脚本引用
        private BalanceRotation m_balanceRotation;
        private Rigidbody[] m_rigidbodys;
        private PunchForce[] m_collisionForGround;


        private Vector3 m_wallnormal;        //身体碰撞到墙壁之后得到的碰撞点的法线向量

        private float m_jumptimer;                                        //跳跃的计时器
        private float m_jumptime = 0.3f;                                  //跳跃的时间间隔

        Player m_player;
        JointAnimationMgr m_jointAnimationMgr;
        Rigidbody m_torso;
        Rigidbody m_hip;
        Rigidbody m_rightLeg;
        Rigidbody m_leftLeg;
        StayStanding m_standing;


        void Start()
        {
            m_player = GetComponent<Player>();
            m_jointAnimationMgr = m_player.m_JointAnimationMgr;
            m_rigidbodys = m_jointAnimationMgr.m_rigids;
            m_collisionForGround = GetComponentsInChildren<PunchForce>();
            m_bodydect = GetComponent<BodyDetect>();
            m_balanceRotation = GetComponent<BalanceRotation>();
            m_jumptimer = 0;
            m_hip = m_jointAnimationMgr.m_hip;
            m_torso = m_jointAnimationMgr.m_torso;
            m_rightLeg = m_jointAnimationMgr.m_rightLeg;
            m_leftLeg = m_jointAnimationMgr.m_leftLeg;
            m_standing = m_player.m_StayStanding;
        }
        void Update()
        {
            m_jumptimer += Time.deltaTime;
        }

        // private void FixedUpdate()
        // {
        //     OnMove();
        // }

        private void OnMove()
        {
            m_state = State.wait;
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                Down_Left();
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                Down_Right();
            }
            else if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.W))
            {
                UP_Left();
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.W))
            {
                UP_Right();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
                return;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Down();
                return;
            }
        }

        /// <summary>
        /// 右跑
        /// </summary>
        public void MoveRight()
        {
            m_face = Face.right;
            m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(90, Vector3.up);
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {

                m_rigidbodys[i].AddForce(Vector3.right * m_HorizonralexternalForce * Time.fixedDeltaTime, ForceMode.Acceleration);

            }
            m_torso.AddForce(-Vector3.right * m_torsoForce, ForceMode.Force);
            m_state = State.right;
        }

        /// <summary>
        /// 左跑
        /// </summary>
        public void MoveLeft()
        {
            m_face = Face.left;
            m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(270, Vector3.up);
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {

                m_rigidbodys[i].AddForce(-Vector3.right * m_HorizonralexternalForce * Time.fixedDeltaTime, ForceMode.Acceleration);

            }
            m_torso.AddForce(Vector3.right * m_torsoForce, ForceMode.Force);
            m_state = State.left;
        }

        //左走
        public void WalkLeft()
        {
            m_face = Face.left;
            m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(270, Vector3.up);
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_rigidbodys[i].AddForce(-Vector3.right * m_HorizonalWalkForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            m_torso.AddForce(Vector3.right * m_torsoForce, ForceMode.Force);
            m_state = State.left;
        }

        //右走
        public void WalkRight()
        {
            m_face = Face.right;
            m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(90, Vector3.up);
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_rigidbodys[i].AddForce(Vector3.right * m_HorizonalWalkForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            m_torso.AddForce(-Vector3.right * m_torsoForce, ForceMode.Force);
            m_state = State.right;
        }

        /// <summary>
        /// 跳跃
        /// </summary>
        public bool Jump()
        {
            if (m_jumptimer < m_jumptime)
            {
                return false;
            }
            if (m_bodydect.IsBodyGround())
            {
                PlayJumpEffect();
                for (int i = 0; i < m_rigidbodys.Length; i++)
                {
                    m_rigidbodys[i].AddForce(Vector3.up * m_jumpForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
                    m_jumptimer = 0;
                }
                m_standing.m_gravityFactor = 0;
                m_state = State.up;
            }
            return true;
        }


        void PlayJumpEffect()
        {
            // float x = m_hip.position.x;
            // float y = (m_rightLeg.position.y + m_leftLeg.position.y) / 2;
            // float z = m_hip.position.z;
            // Vector3 pos = new Vector3(x, y, z);
            // GameObject go = xingnengMgr.getMe().createTexiaoJump(pos, Quaternion.identity);
            // if (go == null)
            //     return;
            // go.transform.position = pos;
            // go.transform.rotation = Quaternion.LookRotation(Vector3.up);
            // Destroy(go, 0.5f);
        }


        /// <summary>
        /// 判断是否能爬墙
        /// </summary>
        /// <returns></returns>
        bool Do_WallJump()
        {
            for (int i = 0; i < m_collisionForGround.Length; i++)
            {
                if (m_collisionForGround[i].IsCollisionWall)
                {
                    this.m_wallnormal = m_collisionForGround[i].wallnormal;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 下蹲
        /// </summary>
        public void Down()
        {
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_standing.m_StandUpMultiForce = 0;
                m_rigidbodys[i].AddForce(Vector3.down * m_VerticalexternalForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            m_state = State.down;
        }

        /// <summary>
        /// 左上跳
        /// </summary>
        public void UP_Left()
        {
            m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(270, Vector3.up);
            Jump();
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_rigidbodys[i].AddForce(-Vector3.right * m_Up_Horizon * Time.fixedDeltaTime, ForceMode.Acceleration);
            }

            m_face = Face.left;
            m_state = State.leftUp;
        }

        /// <summary>
        /// 右上方跳
        /// </summary>
        public void UP_Right()
        {
            m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(90, Vector3.up);
            Jump();
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_rigidbodys[i].AddForce(Vector3.right * m_Up_Horizon * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            m_face = Face.right;
            m_state = State.rightUp;
        }
        /// <summary>
        /// 右匍匐前进
        /// </summary>
        public void Down_Right()
        {
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_standing.m_StandUpMultiForce = 0;
                m_rigidbodys[i].AddForce(Vector3.down * m_Down_Vertical * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            MoveRight();
            m_face = Face.right;
            m_state = State.rightDown;
        }

        /// <summary>
        /// 左匍匐前进
        /// </summary>
        public void Down_Left()
        {
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_standing.m_StandUpMultiForce = 0;
                m_rigidbodys[i].AddForce(Vector3.down * m_Down_Vertical * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            MoveLeft();
            m_face = Face.left;
            m_state = State.leftDown;
        }

        /// <summary>
        /// 向前方移动
        /// </summary>
        public void Forward(int sign)
        {
            // m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(0, Vector3.up);
            // Vector3 dir = m_hip.transform.forward.normalized;
            Vector3 dir = m_balanceRotation.mTarget.forward.normalized;
            // Debug.Log("m_hip.transform.forward = " + dir);
            dir.y = 0;

            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_rigidbodys[i].AddForce(sign * dir * m_HorizonralexternalForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            m_torso.AddForce(-dir * sign * m_torsoForce, ForceMode.Force);
            m_state = State.forward;
        }

        /// <summary>
        /// 左右方移动
        /// </summary>
        public void LeftRight(int sign)
        {
            // m_balanceRotation.mTarget.rotation = Quaternion.AngleAxis(0, Vector3.up);
            // Vector3 dir = m_hip.transform.forward.normalized;
            Vector3 dir = m_balanceRotation.mTarget.right.normalized;
            // Debug.Log("m_hip.transform.forward = " + dir);
            dir.y = 0;

            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_rigidbodys[i].AddForce(sign * dir * m_HorizonralexternalForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            m_torso.AddForce(-dir * sign * m_torsoForce, ForceMode.Force);
            m_state = State.forward;
        }

        /// <summary>
        /// 匍匐向前方移动
        /// </summary>
        public void DownForward(Vector3 move)
        {
            Vector3 rightDir = m_hip.transform.right * (int)Mathf.Sign(move.x);
            Vector3 upDir = m_hip.transform.up * (int)Mathf.Sign(move.y);
            // Debug.Log("m_hip.transform.rightDir = " + rightDir + ", upDir = " + upDir);
            rightDir.y = 0;
            upDir.y = 0;
            for (int i = 0; i < m_rigidbodys.Length; i++)
            {
                m_standing.m_StandUpMultiForce = 0;
                m_rigidbodys[i].AddForce(rightDir * m_Down_MoveForce * Time.fixedDeltaTime, ForceMode.Acceleration);
                m_rigidbodys[i].AddForce(upDir * m_Down_MoveForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            // m_torso.AddForce(-dir * m_torsoForce, ForceMode.Force);
            m_state = State.forwardDown;
        }

        public void Wait()
        {
            m_state = State.wait;
        }

    }


}
