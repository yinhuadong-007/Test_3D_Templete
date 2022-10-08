using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        angle = Random.Range(0.5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.up, angle);
    }
}
