using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfBalanceJoint : MonoBehaviour
{
    [ChineseLabel("移动的旋转驱动速度 力/秒")] public float rotateDriveSpring = 0;
    public Quaternion StartLocalRotation;

    public ConfigurableJoint joint;

    void Awake()
    {
        StartLocalRotation = transform.rotation;

        joint = GetComponent<ConfigurableJoint>();
    }
    private void Start()
    {
        SetDrive();
    }
    private void Update()
    {
        SetDrive();
    }
    private void SetDrive()
    {
        // joint.rotationDriveMode = RotationDriveMode.XYAndZ;
        // joint.angularXDrive = new JointDrive() { positionSpring = rotateDriveSpring / TestConfBalanceMove.instance.moveOnceTime };
        // joint.angularYZDrive = new JointDrive() { positionSpring = rotateDriveSpring / TestConfBalanceMove.instance.moveOnceTime };

        // Debug.Log("SetDrive : " + joint.angularXDrive.positionSpring + ", " + joint.angularYZDrive.positionSpring);
    }
}
