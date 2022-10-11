using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_PhysicsCaveEntry : MonoBehaviour
{
    public static Src_PhysicsCaveEntry instance;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    public Transform plane;
    public GameObject model;
    public Src_PhysicsCircleCave circleCave;

    private void Awake()
    {
        instance = this;
        Physics.gravity = gravity;
    }
    void Start()
    {
        Src_PhysicsCube[] blocks = this.model.GetComponentsInChildren<Src_PhysicsCube>();
        Src_PhysicsCube.Init(blocks);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
