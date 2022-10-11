using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_PhysicsCircleCave : MonoBehaviour
{
    public Transform caveDown;
    public Transform caveGround;
    public Vector3 caveDownSize;
    public Vector3 caveGroundSize;
    public float minRadius;
    public float maxRadius;
    // Start is called before the first frame update
    void Start()
    {
        // Vector3 s = caveDown.gameObject.GetComponent<BoxCollider>().size;
        // caveDownSize.x = caveDown.transform.lossyScale.x * s.x;
        // caveDownSize.y = caveDown.transform.lossyScale.y * s.y;
        // caveDownSize.z = caveDown.transform.lossyScale.z * s.z;
        caveDownSize = caveDown.transform.lossyScale;
        caveGroundSize = caveGround.transform.lossyScale;

        minRadius = Mathf.Max(Mathf.Max(caveDownSize.x, caveDownSize.z) / 2, Mathf.Sqrt(caveDownSize.x / 2 * caveDownSize.x / 2 + caveDownSize.z / 2 * caveDownSize.z / 2));
        maxRadius = Mathf.Max(Mathf.Max(caveGroundSize.x, caveGroundSize.z) / 2, Mathf.Sqrt(caveGroundSize.x / 2 * caveGroundSize.x / 2 + caveGroundSize.z / 2 * caveGroundSize.z / 2));
    }
}
