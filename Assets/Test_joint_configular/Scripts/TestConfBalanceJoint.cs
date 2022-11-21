using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceJoint : MonoBehaviour
{
    public static float strength = 10;

    [ChineseLabel("绑定的骨骼")] public Transform boneTrans;

    [ChineseLabel("移动的旋转驱动速度 力/秒")] public float rotateDriveSpring = 0;

    [Header("数据展示")]
    public Quaternion StartLocalRotation;
    public ConfigurableJoint joint;
    public Transform dollTrans;
    public Transform dollPosNodeTrans;

    //骨骼相关
    public Vector3 boneDollOffsetPos;
    public Quaternion boneDollOffsetRotation;
    //对象缩放时属性
    public Vector3 dollInitScale;
    public Vector3 dollConnectedAnchor;
    public Vector3 dollAnchor;
    //用于恢复骨骼初始位置
    public Vector3 boneIdleLocalPosition;


    void Awake()
    {
        dollTrans = this.transform;
        dollPosNodeTrans = dollTrans.Find("Position");
        StartLocalRotation = transform.rotation;

        joint = GetComponent<ConfigurableJoint>();
        InitBone();
        // rotateDriveSpring = joint.angularXDrive.positionSpring;
        // if (rotateDriveSpring == 0)
        // {
        //     rotateDriveSpring = joint.slerpDrive.positionSpring;
        // }
    }

    private void InitBone()
    {
        if (boneTrans == null) return;
        Debug.Log("name= " + transform.name + ", boneTrans.position = " + boneTrans.position + ", dollTrans.position = " + dollTrans.position);
        boneDollOffsetPos = boneTrans.position - dollPosNodeTrans.position;
        boneDollOffsetRotation = Quaternion.Inverse(dollTrans.rotation) * boneTrans.rotation;

        dollInitScale = dollTrans.localScale;
        dollConnectedAnchor = joint.connectedAnchor;
        dollAnchor = joint.anchor;

        boneIdleLocalPosition = boneTrans.localPosition;
    }
    private void FixedUpdate()
    {
        if (boneTrans == null) return;

        // float lerpValue = Time.fixedDeltaTime * strength;
        // Vector3 targetPosition = Vector3.Lerp(boneTrans.position, dollTrans.position + boneDollOffsetPos, lerpValue);
        // boneTrans.position = targetPosition;

        // Quaternion targetRotation = Quaternion.Lerp(dollTrans.rotation, dollTrans.rotation * boneDollOffsetRotation, lerpValue);
        // boneTrans.rotation = targetRotation;

        boneTrans.rotation = dollTrans.rotation * boneDollOffsetRotation;
        boneTrans.position = dollPosNodeTrans.position + boneDollOffsetPos;

        // boneTrans.position = dollTrans.position;
        // boneTrans.rotation = dollTrans.rotation;
    }

    JointDrive m_initXAngularDrive;
    JointDrive m_initYZAngularDrive;
    JointDrive m_initSlerpDrive;
    public Rigidbody m_rb;


    public void InitBalanceArgs(float moveOnceTimeFactor, float gravityFactor)
    {
        m_initXAngularDrive = joint.angularXDrive;
        m_initYZAngularDrive = joint.angularYZDrive;
        m_initSlerpDrive = joint.slerpDrive;

        float spring = rotateDriveSpring / moveOnceTimeFactor * gravityFactor;
        m_initXAngularDrive.positionSpring = spring;
        m_initYZAngularDrive.positionSpring = spring;
        m_initSlerpDrive.positionSpring = spring;

        EnableBalance();

        m_rb = GetComponent<Rigidbody>();
    }

    public void EnableBalance()
    {
        joint.angularXDrive = m_initXAngularDrive;
        joint.angularYZDrive = m_initYZAngularDrive;
        joint.slerpDrive = m_initSlerpDrive;
    }

    public void DisableBalance()
    {
        if (this.name == "Hip" || this.name == "Head")
        {
            this.m_rb.AddForce(-this.transform.forward.normalized * 1000, ForceMode.Acceleration);
        }

        JointDrive tempX = m_initXAngularDrive;
        tempX.positionSpring = 0;
        JointDrive tempYZ = m_initYZAngularDrive;
        tempYZ.positionSpring = 0;
        JointDrive tempSlerp = m_initSlerpDrive;
        tempSlerp.positionSpring = 0;
        joint.angularXDrive = tempX;
        joint.angularYZDrive = tempYZ;
        joint.slerpDrive = tempSlerp;
    }
}
