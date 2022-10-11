using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Src_PhysicsCube : MonoBehaviour
{
    public static Dictionary<string, List<Src_PhysicsCube>> blockDictionary = new Dictionary<string, List<Src_PhysicsCube>>();
    public static Vector3 blockSize = Vector3.zero;
    public static Vector3 blockScale = Vector3.zero;
    public static Vector3 blockScaleSize = Vector3.zero;

    // string m_sign;
    Collider m_collider;
    Rigidbody m_rb;
    protected bool m_isDestroy;

    void Start()
    {
        m_collider = GetComponent<Collider>();
        m_rb = GetComponent<Rigidbody>();
    }

    float changeTime = 0f;

    void Update()
    {
        float dis = Vector3.Distance(Src_PhysicsCaveEntry.instance.circleCave.transform.position, this.transform.position);
        float planeY = Src_PhysicsCaveEntry.instance.plane.position.y;
        if (this.transform.position.y < planeY - blockScaleSize.y / 2)
        {
            SelfDestroy();
        }
        else if (!m_isDestroy && m_rb != null)
        {
            if (dis > Src_PhysicsCaveEntry.instance.circleCave.minRadius && this.gameObject.layer != TagLayer.layer_Default)
            {
                changeTime = 0;
                this.gameObject.layer = TagLayer.layer_Default;
            }
            else if (dis < Src_PhysicsCaveEntry.instance.circleCave.minRadius && this.gameObject.layer == TagLayer.layer_Default)
            {
                EnableGravity();
                this.gameObject.layer = TagLayer.layer_Falling;
            }
            else if (this.gameObject.layer == TagLayer.layer_Default)
            {
                //3秒后移除刚体
                changeTime += Time.deltaTime;
                if (changeTime > 3)
                {
                    changeTime = 0;
                    Destroy(m_rb);
                    m_rb = null;
                }
            }
        }
        else if (m_rb == null)
        {
            if (dis <= Src_PhysicsCaveEntry.instance.circleCave.minRadius)
            {
                EnableGravity();
                this.gameObject.layer = TagLayer.layer_Falling;
            }
            else if (dis <= Src_PhysicsCaveEntry.instance.circleCave.maxRadius)
            {
                EnableGravity();
                this.gameObject.layer = TagLayer.layer_Default;
            }
        }

    }


    public virtual void Init()
    {
        m_isDestroy = false;
        Destroy(m_rb);
        m_rb = null;
    }

    public void SelfDestroy()
    {
        if (m_isDestroy) return;
        m_isDestroy = true;
        Destroy(this.gameObject);
    }

    public void EnableGravity()
    {
        if (m_rb == null)
        {
            m_rb = this.gameObject.AddComponent<Rigidbody>();
        }
        m_rb.isKinematic = false;
        m_rb.useGravity = true;
        m_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    // public void ChangeManyBlock()
    // {
    //     string key = m_sign;

    //     if (!blockDictionary.ContainsKey(key)) return;
    //     for (int i = 0, len = blockDictionary[key].Count; i < len; i++)
    //     {
    //         Src_PhysicsCube block = blockDictionary[key][i];
    //         Tween.delay(0.1f * i, () =>
    //         {
    //             if (block.m_isDestroy) return;
    //             block.EnableGravity();
    //         }).start();
    //     }
    // }


    public static void Clear()
    {
        foreach (var item in blockDictionary)
        {
            List<Src_PhysicsCube> list = item.Value;
            foreach (var block in list)
            {
                block.SelfDestroy();
            }
        }
        blockDictionary.Clear();
    }

    public static void Init(Src_PhysicsCube[] blocks)
    {
        Print.Log("blocks : " + blocks.Length);
        foreach (var block in blocks)
        {
            if (block.gameObject.activeSelf)
            {
                block.Init();
                //统计所有的方块
                // Vector2 sign = new Vector2(block.transform.position.x, block.transform.position.z);
                // string key = $"{Mathf.FloorToInt(sign.x * 100)}_{Mathf.FloorToInt(sign.y * 100)}";
                // block.m_sign = key;
                // if (!blockDictionary.ContainsKey(key))
                // {
                //     blockDictionary.Add(key, new List<Src_PhysicsCube>());
                // }
                // blockDictionary[key].Add(block);
            }
        }
        // SortBlockDictionary();
        Vector3 s = blocks[0].gameObject.GetComponent<BoxCollider>().size;
        Vector3 tmpBlockSize;
        tmpBlockSize.x = blocks[0].transform.lossyScale.x * s.x;
        tmpBlockSize.y = blocks[0].transform.lossyScale.y * s.y;
        tmpBlockSize.z = blocks[0].transform.lossyScale.z * s.z;
        blockSize = s;
        blockScale = blocks[0].transform.lossyScale;
        blockScaleSize = tmpBlockSize;

        // foreach (var item in blockDictionary)
        // {
        //     item.Value[0].EnableGravity();
        // }
    }

    // private static void SortBlockDictionary()
    // {
    //     foreach (var item in blockDictionary)
    //     {
    //         item.Value.Sort((a, b) =>
    //         {
    //             Print.Log("---------------->sort a.name : " + a.name + ", b.name: " + b.name);
    //             return (a.transform.position.y - b.transform.position.y) >= 0 ? 1 : -1;
    //         });
    //     }
    //     foreach (var item in blockDictionary)
    //     {
    //         Print.Log("---------------->sort key : " + item.Key);
    //         for (int i = 0; i < item.Value.Count; i++)
    //         {
    //             Print.Log("sort value: " + item.Value[i].transform.position);
    //         }
    //         // return;
    //     }
    // }

}
