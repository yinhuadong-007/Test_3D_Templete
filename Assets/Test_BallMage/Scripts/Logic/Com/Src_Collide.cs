using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_Collide : MonoBehaviour
{

    private Collider m_collider;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
        SetEnableCollider(true);
    }

    public void SetEnableCollider(bool enable)
    {
        m_collider.enabled = enable;
    }

    public float GetColliderWidth(string type)
    {
        float width = 0f;
        if (type == "box")
        {
            width = ((BoxCollider)m_collider).size.x * transform.lossyScale.x;
        }
        return width;
    }

    private void OnCollisionEnter(Collision other)
    {
        OnWorkCollider(other.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Print.Log("---->>>>>Src_Collide Trigger Enter name =  " + other.name + ", tag:" + transform.tag);
        OnWorkCollider(other);
    }

    private void OnWorkCollider(Collider other)
    {
        var myTag = this.transform.tag;
        if (!GameManager.instance.isFired)
        {
            if (myTag == "Ball" && other.tag == "Back")
            {
                GameManager.instance.stopGuide = true;
            }
            return;
        };

        bool valid = true;
        // if (other.tag == "Ball")
        // {
        //     switch (myTag)
        //     {
        //         // case "Prop":
        //         //     {//道具效果
        //         //         var pos = m_collider.bounds.ClosestPoint(other.transform.position);
        //         //         // Print.Log("Src_Collide pos = " + pos);
        //         //         pos.z -= 3;
        //         //         GameManager.instance.OnDoProp(other.gameObject, this.gameObject, pos);
        //         //         break;
        //         //     }

        //     }
        // }
        if (myTag == "Ball" && other.tag == "Block")//球和小方块碰撞
        {
            var pos = other.bounds.ClosestPoint(this.transform.position);
            GameManager.instance.OnCollideBlock(this.gameObject, other.gameObject, pos);
        }
        else if (myTag == "Ball" && other.tag == "Back")//小球回到发射区域
        {
            GameManager.instance.OnCollideBack(this.gameObject);
        }
        else if (myTag == "Ball" && (other.tag == "Untagged"))//球和其他中性物碰撞
        {

        }
        else
        {
            valid = false;
        }

        // if (valid)
        // {
        //     Print.Log("---->>>>>Src_Collide Enter name =  " + other.name + ", name:" + transform.name + "myTag = " + myTag);
        // }
    }

}
