namespace TestConfigJointDoll
{


    using System.Collections;

    using System.Collections.Generic;

    using UnityEngine;


    ///<summary>

    ///第三人称角色控制器

    ///</summary>

    public class ThirdPersonController : MonoBehaviour

    {

        public float speed = 3;

        public float turnSpeed = 10;

        public Transform targetPerson;

        public Camera myCamera;

        //镜头旋转速度

        public float camRotateSpeed = 20;


        //记录当前摄像机的旋转角度

        float cameraRotate;


        Vector3 offset;


        private Animator anim;

        private Rigidbody rigid;


        private Transform m_Cam;

        private Vector3 m_CamForward;


        Vector3 move;


        Vector3 gravityVelocity;


        Vector3 m_GroundNormal;

        bool m_IsGrounded;

        public float m_GroundCheckDistance = 0.1f;


        float m_GravityMultiplier = 2f;


        float forwardAmount;

        float turnAmount;

        C_Movement m_movement;
        BalanceRotation m_BalanceRotation;



        private void Start()

        {

            anim = GetComponent<Animator>();
            m_movement = GetComponent<C_Movement>();
            m_BalanceRotation = GetComponent<BalanceRotation>();
            if (targetPerson == null) targetPerson = this.transform;

            // rigid = GetComponent<Rigidbody>();


            //记录人物初始位置

            offset = targetPerson.position;


            if (Camera.main != null)

            {

                m_Cam = Camera.main.transform;

            }

        }


        private void Update()
        {

            float h = Input.GetAxis("Horizontal");

            float v = Input.GetAxis("Vertical");

            if (m_Cam != null)

            {

                // calculate camera relative direction to move:

                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

                move = v * m_CamForward + h * m_Cam.right;

            }

            else

            {

                // we use world-relative directions in the case of no main camera

                move = v * Vector3.forward + h * Vector3.right;

            }


            if (move.magnitude > 1f) move.Normalize();

            UpdateAnim();

        }


        private void FixedUpdate()
        {
            //跳跃和下蹲
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_movement.Jump();
            }
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                m_movement.Down();
            }

            if (move == Vector3.zero)
            {
                m_movement.Wait();
                return;
            }

            CheckGroundStatus();

            Vector3 localMove = targetPerson.InverseTransformVector(move);

            //localMove = Vector3.ProjectOnPlane(move, m_GroundNormal);

            // rigid.velocity = forwardAmount * transform.forward * speed;

            // rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));

            // if (m_movement.m_state == State.down)
            // {
            //     forwardAmount = localMove.y;
            //     turnAmount = Mathf.Atan2(localMove.x, localMove.y);
            //     // Debug.Log("down turnAmount= " + turnAmount);
            //     // m_BalanceRotation.SetTargetRotate(Quaternion.Euler(0, 0, turnAmount * turnSpeed), State.forwardDown);
            //     m_movement.DownForward(localMove);
            // }
            // else
            // {
            //     forwardAmount = localMove.z;
            //     turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            //     // Debug.Log("forward turnAmount= " + turnAmount);
            //     // m_BalanceRotation.SetTargetRotate(Quaternion.Euler(0, turnAmount * turnSpeed, 0), State.forward);
            //     m_BalanceRotation.SetTargetRotate2(m_Cam.forward, State.forward);
            //     if (localMove.z != 0)
            //     {
            //         m_movement.Forward((int)Mathf.Sign(localMove.z));
            //     }
            //     if (localMove.x != 0)
            //     {
            //         m_movement.LeftRight((int)Mathf.Sign(localMove.x));
            //     }

            // }

            State lastStatue = m_movement.m_state;
            forwardAmount = localMove.z;
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            // Debug.Log("forward turnAmount= " + turnAmount);
            // m_BalanceRotation.SetTargetRotate(Quaternion.Euler(0, turnAmount * turnSpeed, 0), State.forward);
            m_BalanceRotation.SetTargetRotate2(m_Cam.forward, State.forward);
            if (localMove.z != 0)
            {
                m_movement.Forward((int)Mathf.Sign(localMove.z));
            }
            if (localMove.x != 0)
            {
                m_movement.LeftRight((int)Mathf.Sign(localMove.x));
            }
            if (lastStatue == State.down)
            {
                m_movement.m_state = State.forwardDown;
            }

            //重力

            // if (!m_IsGrounded)

            // {

            //     gravityVelocity.y += Physics.gravity.y * Time.deltaTime;

            //     targetPerson.Translate(gravityVelocity * Time.deltaTime);

            // }

        }


        private void LateUpdate()

        {

            //摄像机跟随人物移动，移动量=人物当前位置-人物上一位置

            myCamera.transform.position += targetPerson.position - offset;

            offset = targetPerson.position;


            //摄像机视角旋转

            float mouseX = Input.GetAxis("Mouse X") * camRotateSpeed * Time.deltaTime;

            float mouseY = Input.GetAxis("Mouse Y") * camRotateSpeed * Time.deltaTime;

            myCamera.transform.RotateAround(targetPerson.position, Vector3.up, mouseX * camRotateSpeed);

            //myCamera.transform.Rotate(mouseY, 0, 0, Space.Self);


            //记录摄像机当前旋转角度，并转换为弧度

            cameraRotate = myCamera.transform.eulerAngles.y * Mathf.Deg2Rad;

        }


        private void UpdateAnim()

        {

            // anim.SetFloat("run", move.magnitude);

        }


        void CheckGroundStatus()

        {

            RaycastHit hitInfo;

#if UNITY_EDITOR

            // helper to visualise the ground check ray in the scene view

            Debug.DrawLine(transform.position, transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));

#endif

            // 0.1f is a small offset to start the ray from inside the character

            // it is also good to note that the transform position in the sample assets is at the base of the character

            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, m_GroundCheckDistance))

            {

                m_GroundNormal = hitInfo.normal;

                m_IsGrounded = true;

                //anim.applyRootMotion = true;

            }

            else

            {

                m_IsGrounded = false;

                m_GroundNormal = Vector3.up;

                //anim.applyRootMotion = false;

            }

        }

    }
}
//作者：羊村村长c https://www.bilibili.com/read/cv14011835 出处：bilibili