using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    FixedJoint fixedJoint;
    Collider triggerCollider;
    private void Awake()
    {
        triggerCollider = GetComponent<SphereCollider>();
    }
    public void SetConnectedBody(Rigidbody rb)
    {
        if (rb != null)
        {
            if (fixedJoint != null) return;
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = rb;
            DisableTrigger();
        }
        else
        {
            if (fixedJoint == null) return;
            Destroy(fixedJoint);
            EnableTrigger();
        }

    }
    private void EnableTrigger()
    {
        triggerCollider.enabled = true;
    }
    private void DisableTrigger()
    {
        triggerCollider.enabled = false;
    }
}
