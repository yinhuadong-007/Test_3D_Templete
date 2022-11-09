using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AngularDrives : MonoBehaviour
{
    public Vector3[] targetAngle;
    public float deltaTime = 1;
    ConfigurableJoint m_joint;
    Quaternion startPosition;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_joint = GetComponent<ConfigurableJoint>();
        startPosition = transform.rotation;
        StartCoroutine(AddIndex());
        DoRotate();
    }

    IEnumerator AddIndex()
    {
        while (true)
        {
            yield return new WaitForSeconds(deltaTime);
            index++;
            if (index >= targetAngle.Length)
            {
                index = 0;
            }
            DoRotate();
        }
    }

    void DoRotate()
    {
        m_joint.SetTargetRotationLocal(Quaternion.Euler(targetAngle[index]), startPosition);
    }
}
