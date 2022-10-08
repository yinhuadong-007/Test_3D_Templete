using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_BaseBlock : MonoBehaviour
{
    public static int CurrentCount = 0;
    public static Dictionary<Vector2, List<Src_BaseBlock>> BlockDictionary = new Dictionary<Vector2, List<Src_BaseBlock>>();//x,z位置 + this

    [SerializeField, Tooltip("血量")] protected int m_blood = 1;

    protected bool m_isDestroy;

    protected Rigidbody m_rb;
    protected BoxCollider m_boxCollider;

    private Vector3 m_initPosition;

    public bool isDestroy
    {
        get { return m_isDestroy; }
    }

    protected virtual void Awake()
    {
        MaterialPropertyBlock prop = new MaterialPropertyBlock();
        MeshRenderer mr = this.GetComponentInChildren<MeshRenderer>();
        mr.SetPropertyBlock(prop);
    }

    public static void Clear()
    {
        Src_BaseBlock.CurrentCount = 0;
        Src_BaseBlock.BlockDictionary.Clear();
    }

    public virtual void Init()
    {
        m_isDestroy = false;
        CurrentCount++;

        //刚体初始
        m_rb = GetComponent<Rigidbody>();
        m_boxCollider = GetComponent<BoxCollider>();
        m_rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        EnableKinematic();

        //缩小碰撞范围，防止贴合过紧
        Vector3 size = m_boxCollider.size;
        size.x = size.x * 0.99f;
        size.z = size.z * 0.99f;
        m_boxCollider.size = size;

        //初始数据
        m_initPosition = this.transform.position;

        //统计所有的方块
        Vector2 sign = new Vector2(this.transform.position.x, this.transform.position.z);
        if (!BlockDictionary.ContainsKey(sign))
        {
            BlockDictionary.Add(sign, new List<Src_BaseBlock>());
        }
        BlockDictionary[sign].Add(this);

        //物理材质
        m_boxCollider.material = CommonPrefabData.instance.blockPhysicsMat;

        // if (m_blood <= 0)
        // {
        //     SelfDestroy();
        //     return;
        // }
    }


    public void EnableGravity()
    {
        m_rb.isKinematic = false;
        m_rb.useGravity = true;
        m_initPosition = transform.position;
    }

    public void EnableKinematic()
    {
        m_rb.isKinematic = true;
        m_rb.useGravity = false;
        m_initPosition = transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (m_isDestroy) return;
        if (!m_rb.useGravity) return;

        if (m_rb.velocity == Vector3.zero && other.collider.tag != "Ball" && m_initPosition.y - transform.position.y > 0.1f)
        {
            EnableKinematic();
        }
    }

    public static void SortBlockDictionary()
    {
        foreach (var item in BlockDictionary)
        {
            item.Value.Sort((a, b) =>
            {
                return (a.transform.position.y - b.transform.position.y) > 0 ? 1 : -1;
            });
        }
        foreach (var item in BlockDictionary)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                Print.Log("sort " + item.Value[i].transform.position);
            }
            return;
        }

    }

    public void subBlood(int sub)
    {
        m_blood -= sub;
        if (m_blood <= 0)
        {
            SelfDestroy();
        }
    }

    public virtual void SelfDestroy()
    {
        if (m_isDestroy) return;
        Vector2 sign = new Vector2(this.transform.position.x, this.transform.position.z);
        BlockDictionary[sign].Remove(this);
        this.m_boxCollider.enabled = false;

        for (int i = 0, len = BlockDictionary[sign].Count; i < len; i++)
        {
            Src_BaseBlock block = BlockDictionary[sign][i];
            Tween.delay(0.1f * i, () =>
            {
                if (block.m_isDestroy) return;
                block.EnableGravity();
            }).start();
        }

        CurrentCount--;

        m_isDestroy = true;
        Destroy(this.m_rb);
        Destroy(this.m_boxCollider);
        Destroy(gameObject);

    }

}
