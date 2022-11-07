using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove2 : MonoBehaviour
{
    float timer;
    bool flag = false;
    Rigidbody rigidbody;
    SetJointTargetVal jointVal;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        jointVal = GetComponent<SetJointTargetVal>();
    }

    // Update is called once per frame
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 2f;
            flag = !flag;
        }
        if (Input.GetKey(KeyCode.P))
        {
            if (flag)
            {
                jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(-60, 0, 0), jointVal.StartLocalRotation);
            }
            else
            {
                jointVal.joint.SetTargetRotationLocal(Quaternion.Euler(60, 0, 0), jointVal.StartLocalRotation);
            }
        }
    }

}
