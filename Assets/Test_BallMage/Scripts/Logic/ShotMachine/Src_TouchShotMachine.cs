using System;
using UnityEngine;

/// <summary>
/// 玩法B - 触摸发射
/// </summary>
public class Src_TouchShotMachine : Src_BaseShotMachine
{
    ///<summary> 移动节点（底座） </summary>
    [SerializeField] private Transform m_moveNode;

    [SerializeField] private Transform m_widthNode;

    public Transform moveNode
    {
        get { return m_moveNode; }
    }

    private float[] m_paoMoveRange;

    public override void Init()
    {
        base.Init();
        shotType = EShotMachineType.Touch_Shot;
        m_paoMoveRange = new float[2];
        float paoBottomHalfWidth = this.m_widthNode.lossyScale.x / 2;
        m_paoMoveRange[0] = Src_Environment.WallLeftValue + paoBottomHalfWidth;
        m_paoMoveRange[1] = Src_Environment.WallRightValue - paoBottomHalfWidth;
        Print.Log("m_paoMoveRange= " + m_paoMoveRange[0] + ", " + m_paoMoveRange[1]);
    }

    /// <summary>
    /// 检查旋转是否在范围内
    /// </summary>
    /// <returns></returns>
    public override bool CheckRotateInRange()
    {
        Print.Log("CheckRotateInRange= " + lifePoint.position + ", m_paoMoveRange= " + m_paoMoveRange[0] + ", " + m_paoMoveRange[1]);
        if (lifePoint.position.x > m_paoMoveRange[1] || lifePoint.position.x < m_paoMoveRange[0])
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 发射器底座移动到目标位置
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="cbk"></param>
    public void MoveToTargetPos(Vector3 targetPos, Action cbk = null)
    {
        targetPos.y = this.m_moveNode.position.y;
        targetPos.z = this.m_moveNode.position.z;

        targetPos.x = targetPos.x > m_paoMoveRange[1] ? m_paoMoveRange[1] : targetPos.x;
        targetPos.x = targetPos.x < m_paoMoveRange[0] ? m_paoMoveRange[0] : targetPos.x;

        float t = Mathf.Abs(targetPos.x - this.m_moveNode.position.x) / GameData.instance.shotMachineMoveSpeed;

        RotateToTargetAngle(m_initPaoHeadAngle.y, t);

        Tween.target(this.m_moveNode)
        .toPosition(
            targetPos,
            t,
            () =>
            {
                if (cbk != null) cbk();
            })
        .start();


    }

}
