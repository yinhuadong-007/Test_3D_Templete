using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public Camera camera;

    public Rigidbody Head;
    public Rigidbody Body;
    public Rigidbody Pelvis;
    public Rigidbody Arm_R_1;
    public Rigidbody Arm_L_1;
    public Rigidbody Elbow_R_2;
    public Rigidbody Elbow_L_2;
    public Rigidbody Leg_R_1;
    public Rigidbody Leg_L_1;
    public Rigidbody Leg_R_2;
    public Rigidbody Leg_L_2;
    public Rigidbody Foot_R;
    public Rigidbody Foot_L;

    public Transform COM;
    public Transform CameraPos;

    public SetJointTargetVal JointVal_Leg_R_1;
    public SetJointTargetVal JointVal_Leg_L_1;
    public SetJointTargetVal JointVal_Leg_R_2;
    public SetJointTargetVal JointVal_Leg_L_2;

    private Vector3 com_position;


    // Start is called before the first frame update
    void Start()
    {
        Head = transform.Find("Head").GetComponent<Rigidbody>();
        Body = transform.Find("Body").GetComponent<Rigidbody>();
        Pelvis = transform.Find("Pelvis").GetComponent<Rigidbody>();
        Arm_R_1 = transform.Find("Arm_R_1").GetComponent<Rigidbody>();
        Arm_L_1 = transform.Find("Arm_L_1").GetComponent<Rigidbody>();
        Elbow_R_2 = transform.Find("Elbow_R_2").GetComponent<Rigidbody>();
        Elbow_L_2 = transform.Find("Elbow_L_2").GetComponent<Rigidbody>();
        Leg_R_1 = transform.Find("Leg_R_1").GetComponent<Rigidbody>();
        Leg_L_1 = transform.Find("Leg_L_1").GetComponent<Rigidbody>();
        Leg_R_2 = transform.Find("Leg_R_2").GetComponent<Rigidbody>();
        Leg_L_2 = transform.Find("Leg_L_2").GetComponent<Rigidbody>();
        Foot_R = transform.Find("Foot_R").GetComponent<Rigidbody>();
        Foot_L = transform.Find("Foot_L").GetComponent<Rigidbody>();

        COM = transform.Find("COM").GetComponent<Transform>();
        CameraPos = transform.Find("CameraPos").GetComponent<Transform>();

        JointVal_Leg_R_1 = Leg_R_1.GetComponent<SetJointTargetVal>();
        JointVal_Leg_L_1 = Leg_L_1.GetComponent<SetJointTargetVal>();
        JointVal_Leg_R_2 = Leg_R_2.GetComponent<SetJointTargetVal>();
        JointVal_Leg_L_2 = Leg_L_2.GetComponent<SetJointTargetVal>();
    }

    void FixedUpdate()
    {

        CameraPos.transform.position = Vector3.Lerp(CameraPos.transform.position, Body.transform.position, 2 * Time.unscaledDeltaTime);

        if (Input.GetKey(KeyCode.W))
        {

        }

        if (Input.GetKey(KeyCode.D))
        {
            JointVal_Leg_R_1.joint.SetTargetRotationLocal(Quaternion.Euler(-60, 0, 0), JointVal_Leg_R_1.StartLocalRotation);
            JointVal_Leg_L_1.joint.SetTargetRotationLocal(Quaternion.Euler(60, 0, 0), JointVal_Leg_L_1.StartLocalRotation);

            JointVal_Leg_R_2.joint.SetTargetRotationLocal(Quaternion.Euler(20, 0, 0), JointVal_Leg_R_1.StartLocalRotation);
            JointVal_Leg_L_2.joint.SetTargetRotationLocal(Quaternion.Euler(20, 0, 0), JointVal_Leg_L_1.StartLocalRotation);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            JointVal_Leg_R_1.joint.SetTargetRotationLocal(Quaternion.Euler(60, 0, 0), JointVal_Leg_R_1.StartLocalRotation);
            JointVal_Leg_L_1.joint.SetTargetRotationLocal(Quaternion.Euler(-60, 0, 0), JointVal_Leg_L_1.StartLocalRotation);

            JointVal_Leg_R_2.joint.SetTargetRotationLocal(Quaternion.Euler(20, 0, 0), JointVal_Leg_R_1.StartLocalRotation);
            JointVal_Leg_L_2.joint.SetTargetRotationLocal(Quaternion.Euler(20, 0, 0), JointVal_Leg_L_1.StartLocalRotation);
        }
        else
        {
            JointVal_Leg_R_2.joint.SetTargetRotationLocal(Quaternion.Euler(0, 0, 0), JointVal_Leg_R_1.StartLocalRotation);
            JointVal_Leg_L_2.joint.SetTargetRotationLocal(Quaternion.Euler(0, 0, 0), JointVal_Leg_L_1.StartLocalRotation);
        }
    }

}
