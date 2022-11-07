namespace TestConfigJointDoll
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class C_JointAnimation : MonoBehaviour
    {
        public JointAnimationsInfo animatioinInfo;
        private Rigidbody rig;
        private Rigidbody hip;

        SetJointTargetVal jointVal;

        void Awake()
        {
            rig = GetComponent<Rigidbody>();
            jointVal = GetComponent<SetJointTargetVal>();
        }

        /// <summary>
        /// 控制角色跑步身体关节的运动
        /// </summary>
        /// <param name="leftForward"></param>
        public void JointAnimate(bool leftForward)
        {
            Vector3 dir = Vector3.zero;
            if (animatioinInfo.bodyDirection == JointAnimationsInfo.BodyDirection.Swing)
            {
                dir = transform.right;
            }
            if (animatioinInfo.bodyDirection == JointAnimationsInfo.BodyDirection.MainBodyVelocity)
            {
                dir = JointAnimationMgr.instance.m_hip.velocity.normalized;
                // dir = BalanceRotation.instance.mTarget.forward.normalized;
            }
            if (animatioinInfo.animationsType == JointAnimationsInfo.AnimationsType.Torque)
            {
                if ((leftForward && animatioinInfo.IsBodyLeftSide) || (!leftForward && !animatioinInfo.IsBodyLeftSide))
                {
                    this.rig.AddTorque(dir * Time.deltaTime * -animatioinInfo.backForceMultiplier, ForceMode.Acceleration);
                }
                else
                {
                    this.rig.AddTorque(dir * Time.deltaTime * -animatioinInfo.forwardForceMultiplier, ForceMode.Acceleration);
                }
            }
            if (animatioinInfo.animationsType == JointAnimationsInfo.AnimationsType.Force)
            {
                if ((leftForward && animatioinInfo.IsBodyLeftSide) || (!leftForward && !animatioinInfo.IsBodyLeftSide))
                {
                    this.rig.AddForce(dir * Time.deltaTime * -animatioinInfo.backForceMultiplier, ForceMode.Acceleration);
                }
                else
                {
                    this.rig.AddForce(dir * Time.deltaTime * -animatioinInfo.forwardForceMultiplier, ForceMode.Acceleration);
                }
            }
            if (animatioinInfo.animationsType == JointAnimationsInfo.AnimationsType.TargetRotate && jointVal != null)
            {
                if ((leftForward && animatioinInfo.IsBodyLeftSide) || (!leftForward && !animatioinInfo.IsBodyLeftSide))
                {
                    float rotateAngle = animatioinInfo.rotateAngle;
                    jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(rotateAngle, 0, 0), jointVal.StartLocalRotation);
                }
                else
                {
                    float rotateAngle = animatioinInfo.isNegate ? -animatioinInfo.rotateAngle : animatioinInfo.rotateAngle;
                    jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(rotateAngle, 0, 0), jointVal.StartLocalRotation);
                }
            }
        }

        /// <summary>
        /// 控制角色跑步身体关节的运动
        /// </summary>
        /// <param name="leftForward"></param>
        public void StopJointAnimate()
        {
            if (animatioinInfo.animationsType == JointAnimationsInfo.AnimationsType.TargetRotate && jointVal != null)
            {
                jointVal.joint.SetTargetRotationLocal(jointVal.StartLocalRotation, jointVal.StartLocalRotation);
            }

        }
    }


    //角色动画数据
    //u3d面板 配置
    /// <summary>
    /// 配置角色的Animation信息
    /// </summary>
    [Serializable]
    public class JointAnimationsInfo
    {

        public bool IsBodyLeftSide;                         //是否是身体部位的左侧，在外部配置好的参数
        public AnimationsType animationsType;               //动画模式
        public BodyDirection bodyDirection;                 //身体部位

        public float forwardForceMultiplier;                //向前方向的扭矩参数
        public float backForceMultiplier;                   //向后方向的扭矩参数

        public float rotateAngle;
        public bool isNegate = true;

        public enum AnimationsType
        {
            Torque,
            Force,
            TargetRotate
        }
        public enum BodyDirection
        {
            MainBodyVelocity,    //和身体运动方向(hip)一致
            Swing,               //摆动  
        }
    }


}