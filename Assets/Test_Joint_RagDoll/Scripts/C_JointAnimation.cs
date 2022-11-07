namespace TestRagDoll
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //处理下肢跑步的动画
    public class C_JointAnimation : MonoBehaviour
    {

        public JointAnimationsInfo animatioinInfo;
        private Rigidbody rig;
        private Rigidbody hip;

        void Awake()
        {
            rig = GetComponent<Rigidbody>();
            hip = transform.root.GetComponentInChildren<Hip>().GetComponent<Rigidbody>();
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
                dir = this.hip.velocity.normalized;
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
            if (animatioinInfo.animationsType == JointAnimationsInfo.AnimationsType.Single)
            {
                if ((leftForward && animatioinInfo.IsBodyLeftSide) || (!leftForward && !animatioinInfo.IsBodyLeftSide))
                {
                    this.rig.AddTorque(dir * Time.deltaTime * -animatioinInfo.backForceMultiplier, ForceMode.Acceleration);
                }
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

        public enum AnimationsType
        {
            Torque,
            Force,
            Single
        }
        public enum BodyDirection
        {
            MainBodyVelocity,    //和身体运动方向(hip)一致
            Swing,               //摆动  
        }
    }


}