using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_PhysicsCaveClick : MonoBehaviour
{
    RaycastHit _HitInfo = new RaycastHit();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetMousePoint();
    }

    void GetMousePoint()
    {

        if (Input.GetMouseButton(0))
        {
            int layerMask = 1 << TagLayer.layer_Plane;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out _HitInfo, 1000, layerMask))
            {
                transform.position = _HitInfo.point;
            }

        }
    }
}
