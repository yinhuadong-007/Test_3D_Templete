using System.Collections.Generic;
using UnityEngine;

public class DollToBone : MonoBehaviour
{
    /// <summary> 布娃娃根节点 </summary>
    [Tooltip("布娃娃根节点")]
    public GameObject dollRoot;

    /// <summary> 骨骼根节点 </summary>
    [Tooltip("骨骼根节点")]
    public GameObject boneRoot;

    private Dictionary<Transform, bool> dollKinematicDic = new Dictionary<Transform, bool>();

    private Dictionary<Transform, Transform> dollToBoneDic = new Dictionary<Transform, Transform>();
    private Dictionary<Transform, Vector3> boneDollDicOffsetPos = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> boneDollDicOffsetRotation = new Dictionary<Transform, Quaternion>();

    private Dictionary<Transform, Vector3> dollDicInitScale = new Dictionary<Transform, Vector3>();

    private Dictionary<Transform, Vector3> dollDicConnectedAnchor = new Dictionary<Transform, Vector3>();

    private Dictionary<Transform, Vector3> dollDicAnchor = new Dictionary<Transform, Vector3>();

    private Dictionary<Transform, Vector3> boneDicIdleLocalPosition = new Dictionary<Transform, Vector3>();

    public Dictionary<Transform, Transform> DollToBoneDic
    {
        get
        {
            return dollToBoneDic;
        }
    }

    /// <summary> 当前模式是不是布娃娃 </summary>
    private bool isDollModule = false;

    private Vector3 offsetRootNodePos;

    private Vector3 m_initScale;

    private Vector3 m_initScaleDoll;

    private Animator ani;

    private void Awake()
    {
        ani = transform.GetComponent<Animator>();

    }

    void Start()
    {
        m_initScale = transform.localScale;
        m_initScaleDoll = dollRoot.transform.localScale;
        // Debug.Log("m_initScale = " + m_initScale);

        dollToBoneDic.Clear();
        Transform[] dollTrans = dollRoot.GetComponentsInChildren<Transform>();
        Transform[] boneTransChild = boneRoot.GetComponentsInChildren<Transform>();
        Transform[] boneTrans = new Transform[boneTransChild.Length + 1];
        boneTrans[0] = this.transform;
        for (int i = 0; i < boneTransChild.Length; i++)
        {
            boneTrans[i + 1] = boneTransChild[i];
        }
        for (int i = 0; i < dollTrans.Length; i++)
        {
            for (int j = 0; j < boneTrans.Length; j++)
            {
                if (dollTrans[i].gameObject.name == boneTrans[j].gameObject.name)
                {
                    dollToBoneDic.Add(dollTrans[i], boneTrans[j]);
                    boneDollDicOffsetPos.Add(dollTrans[i], boneTrans[j].position - dollTrans[i].position);
                    boneDollDicOffsetRotation.Add(dollTrans[i], Quaternion.Inverse(dollTrans[i].rotation) * boneTrans[j].rotation);

                    if (j != 0)
                    {
                        boneDicIdleLocalPosition.Add(boneTrans[j], boneTrans[j].localPosition);
                    }

                    dollDicInitScale.Add(dollTrans[i], dollTrans[i].localScale);

                    var joint = dollTrans[i].GetComponent<Joint>();
                    if (joint != null)
                    {
                        dollDicConnectedAnchor.Add(dollTrans[i], dollTrans[i].GetComponent<Joint>().connectedAnchor);
                        dollDicAnchor.Add(dollTrans[i], dollTrans[i].GetComponent<Joint>().anchor);
                    }
                    break;
                }
            }
        }
    }
    void Update()
    {
        if (!isDollModule)
        {
            foreach (var item in dollToBoneDic)
            {
                // if(item.Key.name.Contains("脚") == false && item.Key.name.Contains("腿") == false){
                item.Key.position = item.Value.position - boneDollDicOffsetPos[item.Key];
                item.Key.rotation = item.Value.rotation * Quaternion.Inverse(boneDollDicOffsetRotation[item.Key]);
                // }
            }
        }
        else
        {
            foreach (var item in dollToBoneDic)
            {
                item.Value.position = item.Key.position + boneDollDicOffsetPos[item.Key];
                item.Value.rotation = item.Key.rotation * boneDollDicOffsetRotation[item.Key];
            }
        }
    }


    /// <summary>
    /// 缩放布娃娃
    /// </summary>
    /// <param name="scale"></param>
    public void ScaleBoneDoll(float scale)
    {
        //对骨骼进行缩放
        var curScale = transform.localScale;
        var sign = curScale.x > 0 ? 1 : -1;
        var i_scale = m_initScale;
        i_scale.x = Mathf.Abs(i_scale.x) * sign;

        transform.localScale = i_scale * (1 + scale);

        //对布娃娃进行缩放
        foreach (var item in dollDicInitScale)
        {
            item.Key.localScale = item.Value * (1 + scale);

            var joint = item.Key.GetComponent<Joint>();
            if (joint != null)
            {
                item.Key.GetComponent<Joint>().connectedAnchor = dollDicConnectedAnchor[item.Key];
                item.Key.GetComponent<Joint>().anchor = dollDicAnchor[item.Key];
            }
        }
        // dollRoot.transform.localScale = m_initScaleDoll * (1 + scale);
        Debug.Log("ScaleBoneDoll ---> " + transform.localScale);
    }

    public void InitIdlePosition()
    {
        foreach (var item in boneDicIdleLocalPosition)
        {
            item.Key.localPosition = item.Value;
        }
    }

    /// <summary> 激活动画控制 </summary>
    public void activateAnimator()
    {
        // if(dollBoneRoot){
        //     player.position = dollBoneRoot.position + offsetRootNodePos;
        // transform.eulerAngles = new Vector3(transform.eulerAngles.x, dollBoneRoot.eulerAngles.y+angleY, transform.eulerAngles.z);
        //     Debug.Log("activateAnimator eulerAngles " + transform.eulerAngles);
        // }

        isDollModule = false;
        // dollRoot.SetActive(false);
        ani.enabled = true;

        Debug.Log("isDollModule = " + isDollModule);

        InitIdlePosition();
    }

    /// <summary> 激活布娃娃控制 </summary>
    public void activateDoll()
    {
        isDollModule = true;
        // dollRoot.SetActive(true);
        ani.enabled = false;

        Debug.Log("isDollModule = " + isDollModule);
    }

    /// <summary> 是否启用布娃娃的重力并设置为牛顿力学刚体 </summary>
    public void activateDollGravity(bool active)
    {
        bool first = dollKinematicDic.Count == 0 ? true : false;
        foreach (var item in dollToBoneDic)
        {
            var rb = item.Key.GetComponent<Rigidbody>();
            if (rb == null) continue;
            if (first)
            {
                dollKinematicDic.Add(item.Key, rb.isKinematic);
            }
            rb.useGravity = active;
            rb.isKinematic = dollKinematicDic[item.Key] && !active;
        }
    }

    public void setDollBelongLayer(int layerIdx)
    {
        foreach (var item in dollToBoneDic)
        {
            item.Key.gameObject.layer = layerIdx;
        }
    }

}
