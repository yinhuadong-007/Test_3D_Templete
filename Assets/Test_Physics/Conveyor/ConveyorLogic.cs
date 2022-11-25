using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConveyorLogic : MonoBehaviour
{
    public float speed;
    public Vector3 direction = Vector3.back;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        Vector3 pos = rb.position;
        rb.position += direction * speed * Time.fixedDeltaTime;
        //猜测：在这一次物理步长计算中，物理内部会计算一个瞬时力将刚体移动到指定位置
        //通过这个瞬时力移动刚体时与表面其他物体的摩擦来让表面物体朝着相同的方向移动。
        rb.MovePosition(pos);
    }
}
