namespace TestConfigJointDoll
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GetGoods : MonoBehaviour
    {
        bool m_isGet;
        List<GameObject> goodsList;

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
                if (goodsList == null) goodsList = new List<GameObject>();
                goodsList.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == TagLayer.tag_Goods)
            {
                if (goodsList != null && goodsList.Count > 0)
                {
                    goodsList.Remove(other.gameObject);
                }

            }
        }

        private void Update()
        {
            if (goodsList.Count > 0 && Input.GetKeyDown(KeyCode.E))
            {
                FixedJoint fixedJoint = goodsList[0].gameObject.GetComponent<FixedJoint>();
                fixedJoint.connectedBody = this.transform.GetComponent<Rigidbody>();

            }
        }
    }
}
