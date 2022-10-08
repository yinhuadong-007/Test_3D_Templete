
using System;
using System.Collections;
using UnityEngine;
namespace Item
{
    public enum ItemType
    {
        正常,
        球总数,
        爆炸,
        反射,

    }
    [Serializable]
    public struct MatSelect
    {
        public Material mat;
        public bool use;
    }

    [ExecuteAlways]
    public class SelectItemType : MonoBehaviour
    {
        public ItemType type = ItemType.正常;
        public int idx = 0;
        public int matIdx = -1;
        public Material[] MatList = new Material[] { };



        private void Awake()
        {
            if (Application.isPlaying)
            {
                this.enabled = false;
            }
            else
            {
                this.enabled = true;
            }
        }

        private void Update()
        {
            StartCoroutine(DoReplace());
        }

        IEnumerator DoReplace()
        {
            ReUpdate();
            yield return null;
            ReUpdate();
        }

        private void ReUpdate()
        {
            //道具选择
            for (int z = 0; z < 4; z++)
            {
                var obj = this.transform.GetChild(z).gameObject;
                if (z == this.idx)
                {
                    this.transform.GetChild(z).gameObject.SetActive(true);
                }
                else
                {
                    this.transform.GetChild(z).gameObject.SetActive(false);
                }
            }

            switch (type)
            {
                case ItemType.正常: idx = 0; break;
                case ItemType.球总数: idx = 1; break;
                case ItemType.爆炸: idx = 2; break;
                case ItemType.反射: idx = 3; break;
            }
            //材质选择
            if (this.matIdx != -1 && this.MatList.Length > this.matIdx && this.MatList[this.matIdx] != null)
            {
                this.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material = this.MatList[this.matIdx];
            }
        }



    }
}