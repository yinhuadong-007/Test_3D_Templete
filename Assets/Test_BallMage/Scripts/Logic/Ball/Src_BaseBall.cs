using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_BaseBall : MonoBehaviour
{
    public delegate void BulletPoolEventHandler(GameObject sender);
    public event BulletPoolEventHandler Addbullet;

    [SerializeField, Tooltip("攻击力")]
    public int attackPower = 1;

    [SerializeField, Tooltip("所有具有可见性节点的根节点")]
    public GameObject rootDisplayNode;


    [SerializeField, Tooltip("skin")]
    public GameObject skinNode;

    [SerializeField, Tooltip("拖尾节点")]
    public TrailRenderer trailRenderer;


    private Rigidbody m_rb;
    private SphereCollider m_collider;

    protected Vector3 m_curRigidSpeed = Vector3.zero;
    private Vector3 m_shootDirector;
    protected float m_moveSpeed = 0;

    private bool m_isCreate = true;

    private Vector3 m_initScale;

    public Vector3 velocity
    {
        get { return m_rb.velocity; }
    }

    private double m_ballToOtherSoundTime;
    public double ballToOtherSoundTime
    {
        set { m_ballToOtherSoundTime = value; }
        get { return m_ballToOtherSoundTime; }
    }
    /// <summary>是否模拟球 </summary>
    public bool isSimulator = false;


    /// <summary>小球回收 </summary>
    private Transform m_backTargetTrans;
    private Action m_ActMoveEnd;

    private int _colliderCount = 1;

    private void Awake()
    {

    }

    protected virtual void OnCreate()
    {
        m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<SphereCollider>();
        m_initScale = transform.localScale;
        SetBallColor();
    }

    /// <summary>
    /// 初始化子弹
    /// </summary>
    /// <param name="position">初始位置</param>
    /// <param name="isGuide">是否是引导流程创建的</param>
    public virtual void Init(Vector3 position, Vector3 shootDirector, float moveSpeed, bool isAdvance)
    {
        if (m_isCreate)
        {
            m_isCreate = false;
            this.OnCreate();
        }

        m_collider.enabled = true;

        this.transform.position = position;
        m_shootDirector = shootDirector;
        m_shootDirector.y = 0;
        m_moveSpeed = moveSpeed;

        if (GameManager.instance.stopGuide)
        {
            rootDisplayNode.SetActive(true);
        }
        else
        {
            rootDisplayNode.SetActive(false);
        }

        m_collider.material.bounciness = 1f;
        gameObject.layer = 7;//"Ball"
        transform.localScale = m_initScale;

        if (!GameManager.instance.isFired)
        {
            m_rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            m_rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        ShootBall();

        Print.Log("position= " + position + ", m_shootDirector= " + m_shootDirector + ", moveSpeed= " + moveSpeed);

    }

    private void SetBallColor()
    {
        // Color[] colors = ChapterData.instance.ballColorList;
        // if (colors == null || colors.Length == 0) return;
        // int index = UnityEngine.Random.Range(0, colors.Length);
        // Color color = colors[index];
        // Print.Log("SetBallColor color = " + color);

        // MeshRenderer mr = skinNode.GetComponent<MeshRenderer>();
        // MaterialPropertyBlock prop = new MaterialPropertyBlock();
        // prop.SetColor("_Color", color);
        // mr.SetPropertyBlock(prop);

        // MaterialPropertyBlock prop2 = new MaterialPropertyBlock();
        // if (trailRenderer != null)
        // {
        //     Color color2 = colors[index];
        //     color2.a *= 0.1f;
        //     prop2.SetColor("_Color", color2);
        //     trailRenderer.SetPropertyBlock(prop2);
        // }

        Material[] mats = ChapterData.instance.ballColorMaterialList;
        if (mats == null || mats.Length == 0) return;
        int index = UnityEngine.Random.Range(0, mats.Length);
        Material mat = mats[index];

        MeshRenderer mr = skinNode.GetComponent<MeshRenderer>();
        mr.material = mat;

        MaterialPropertyBlock prop = new MaterialPropertyBlock();
        mr.SetPropertyBlock(prop);

        if (trailRenderer != null)
        {
            Material[] t_mats = ChapterData.instance.trailColorMaterialList;
            trailRenderer.material = t_mats[index];

            trailRenderer.SetPropertyBlock(prop);

        }


    }

    public void EnableGravity()
    {
        m_rb.isKinematic = false;
        m_rb.useGravity = true;
    }

    public void EnableKinematic()
    {
        m_rb.isKinematic = true;
        m_rb.useGravity = false;
    }

    /// <summary> 发射小球 </summary>
    private void ShootBall()
    {
        m_rb.velocity = m_shootDirector * m_moveSpeed;

        // Vector3 force = new Vector3(speedX, speedY, 0);
        // m_rb.AddForce(force, ForceMode.VelocityChange);

        EnableGravity();

        if (trailRenderer != null) trailRenderer.Clear();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (m_rb.useGravity)
        {
            OnRecordRigidData();
            // this.hitBlockPlaySound(other.gameObject.tag);
        }
    }

    public void hitBlockPlaySound()
    {
        // if (tag == "Block")
        // {

        this._colliderCount++;
        int soundNo = this._colliderCount / 2;
        soundNo = soundNo > 24 ? 24 : soundNo;
        AudioManager.Instance.PlayEffect(ESoundName.bullet_hit + soundNo);

        // }
    }

    public void OnRecordRigidData()
    {
        //修復速度
        Rigidbody rg = m_rb;
        Vector3 speedNor = rg.velocity.normalized;

        // if (Mathf.Abs(speedNor.x) < 0.2f)
        // {
        //     speedNor.x = Mathf.Sign(speedNor.x) * 0.2f;//(UnityEngine.Random.Range(0f, 0.3f));
        // }
        if (Mathf.Abs(speedNor.z) < 0.2f)
        {
            speedNor.z = Mathf.Sign(speedNor.z) * 0.2f;//(UnityEngine.Random.Range(0f, 0.3f));
        }
        rg.velocity = speedNor.normalized * m_moveSpeed;
    }


    private void Update()
    {
        if (m_collider.enabled)
        {
            //纠正速度为匀速
            // Vector3 dir = m_rb.velocity.normalized;
            // m_rb.velocity = dir * m_moveSpeed;
        }
        else
        {
            Vector3 dir = (m_backTargetTrans.position - this.transform.position).normalized;
            this.transform.Translate(dir * GameData.instance.ballRecoverSpeed * Time.deltaTime, Space.World);
            Vector3 dir2 = (m_backTargetTrans.position - this.transform.position).normalized;
            // Print.Log("dir= " + dir + "dir2= " + dir2);
            if (Mathf.Sign(dir.x) * Mathf.Sign(dir2.x) == -1 || dir.x == 0)
            {
                if (m_ActMoveEnd != null) m_ActMoveEnd();
            }

        }

    }

    public void MoveToTarget(Transform target, Action cbk = null)
    {
        m_rb.velocity = Vector3.zero;
        m_collider.enabled = false;

        m_backTargetTrans = target;
        m_ActMoveEnd = cbk;

        EnableKinematic();
    }

    public void MoveToTargetPos(Vector3 targetPos, Action cbk = null)
    {
        m_collider.enabled = false;

        float t = (targetPos.x - this.transform.position.x) / GameData.instance.ballRecoverSpeed;
        Tween.target(this.transform)
        .toPosition(targetPos, t, () =>
        {
            if (cbk != null) cbk();
        })
        .start();
    }

    public virtual void SelfDestroy()
    {
        Print.Log("---->BulletLogic SelfDestroy---");

        if (trailRenderer != null) trailRenderer.Clear();

        if (Addbullet != null)
        {
            this.gameObject.SetActive(false);
            m_rb.isKinematic = true;
            m_rb.useGravity = false;
            this.isSimulator = false;
            this._colliderCount = 1;

            Addbullet(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    // private void OnDrawGizmos()
    // {
    //     //绘制上一个点到当前点的位移
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * 1.5f));
    // }
}
