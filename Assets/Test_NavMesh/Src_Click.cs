using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Src_Click : MonoBehaviour
{
    NavMeshAgent _agent;
    RaycastHit _HitInfo = new RaycastHit();
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePoint();
    }

    void GetMousePoint()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out _HitInfo))
            {
                _agent.destination = _HitInfo.point;
            }

        }
    }
}
