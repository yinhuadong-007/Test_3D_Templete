using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetJointTargetVal : MonoBehaviour
{
    public Quaternion StartLocalRotation;

    public ConfigurableJoint joint;

    // Start is called before the first frame update
    void Awake()
    {
        StartLocalRotation = transform.rotation;

        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
