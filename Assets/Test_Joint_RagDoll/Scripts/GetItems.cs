using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItems : MonoBehaviour
{
    bool m_isGet;
    Rigidbody m_rb;
    List<GameObject> itemTriggerList;
    List<GameObject> itemGetList;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.transform.tag == TagLayer.tag_Goods)
    //     {
    //         FixedJoint fixedJoint = other.gameObject.GetComponent<FixedJoint>();
    //         fixedJoint.connectedBody = this.transform.GetComponent<Rigidbody>();
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == TagLayer.tag_Goods)
        {
            if (itemTriggerList == null) itemTriggerList = new List<GameObject>();
            itemTriggerList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == TagLayer.tag_Goods)
        {
            if (itemTriggerList != null && itemTriggerList.Count > 0)
            {
                itemTriggerList.Remove(other.gameObject);
            }

        }
    }

    private void Update()
    {
        //拾起
        if (itemTriggerList != null && itemTriggerList.Count > 0 && Input.GetKeyUp(KeyCode.E))
        {
            ItemBase item = itemTriggerList[0].GetComponent<ItemBase>();
            item.SetConnectedBody(m_rb);
            itemTriggerList.Remove(item.gameObject);

            if (itemGetList == null) itemGetList = new List<GameObject>();
            itemGetList.Add(item.gameObject);
        }
        //丢弃
        if (itemGetList != null && itemGetList.Count > 0 && Input.GetKeyUp(KeyCode.F))
        {
            ItemBase item = itemGetList[0].GetComponent<ItemBase>();
            item.SetConnectedBody(null);
            itemGetList.Remove(item.gameObject);
        }
    }
}
