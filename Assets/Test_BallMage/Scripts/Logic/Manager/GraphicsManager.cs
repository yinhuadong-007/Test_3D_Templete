using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager instance;

    [SerializeField, Tooltip("引导线点的数量")]
    public int TotalPointCount = 30;

    [SerializeField, Tooltip("引导线")]
    private GameObject m_preDrawLine;

    private LineRenderer m_drawFireGuide;

    [SerializeField, Tooltip("引导节点")]
    private GameObject m_preDrawPoint;

    [SerializeField, Tooltip("引导节点的材质")]
    private Material m_matGuide;

    private int m_countDrawPointAni = -1;//播放第几个点的动画

    private List<GameObject> m_drawPointNodeList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        if (m_drawPointNodeList != null)
        {
            foreach (var item in m_drawPointNodeList)
            {
                Destroy(item);
            }
            m_drawPointNodeList.Clear();
        }

        if (m_preDrawLine)
        {
            m_drawFireGuide = m_preDrawLine.GetComponent<LineRenderer>();
            m_drawFireGuide.numCapVertices = 0;//设置端点圆滑度
            m_drawFireGuide.numCornerVertices = 0;//设置拐角圆滑度，顶点越多越圆滑
        }
        else
        {
            m_preDrawPoint = ChapterData.instance.guidePointPrefab != null ? ChapterData.instance.guidePointPrefab : m_preDrawPoint;
            m_matGuide = ChapterData.instance.matGuide != null ? ChapterData.instance.matGuide : m_matGuide;
            int i = 0;
            while (i < TotalPointCount)
            {
                GameObject obj = Instantiate(m_preDrawPoint, LayerManager.instance.GuideLayer);
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (m_matGuide != null) mr.sharedMaterial = m_matGuide;
                MaterialPropertyBlock prop = new MaterialPropertyBlock();
                mr.SetPropertyBlock(prop);

                m_drawPointNodeList.Add(obj);
                obj.SetActive(false);
                i++;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DrawFireGuide(Vector3[] points)
    {
        ClearFireGuide();
        Print.Log("*******************> DrawFireGuide points.length = " + points.Length);
        if (m_drawFireGuide)
        {
            m_drawFireGuide.positionCount = points.Length;
            m_drawFireGuide.SetPositions(points);
        }
        else
        {
            DrawFireGuideByNode(points);
        }
    }

    public void ClearFireGuide()
    {
        if (m_drawFireGuide)
        {
            // Print.Log("*******************> ClearFireGuide");
            m_drawFireGuide.positionCount = 0;
            // m_drawFireGuide.SetPositions(new Vector3[0]);
        }
        else
        {
            ClearFireGuideByNode();
        }
    }

    public void DrawFireGuideByNode(Vector3[] points)
    {
        // Print.Log("*******************> DrawFireGuideByNode");
        this.CancelInvoke("PlayDrawPointAni2");
        for (int i = 0, len = points.Length; i < TotalPointCount && i < len; i++)
        {
            GameObject obj = m_drawPointNodeList[i];
            obj.SetActive(true);
            obj.transform.position = points[i];
            Vector3 subVec = points[i];
            if (i + 1 < TotalPointCount && i + 1 < len)
            {
                subVec = points[i + 1] - points[i];
            }
            else
            {
                subVec = points[i] - points[i - 1];
            }
            float angle = Mathf.Atan2(subVec.x, subVec.z) * Mathf.Rad2Deg;
            Vector3 eulerAngles = obj.transform.eulerAngles;
            eulerAngles.y = angle;
            obj.transform.eulerAngles = eulerAngles;
            // obj.GetComponent<ActionBlink>().StopAction();
        }

        // m_countDrawPointAni = 0;
        // PlayDrawPointAni();
    }

    public void ClearFireGuideByNode()
    {
        // Print.Log("*******************> ClearFireGuideByNode");
        // m_drawFireGuide.positionCount = 0;
        // m_drawFireGuide.SetPositions(new Vector3[0]);
        this.CancelInvoke("PlayDrawPointAni2");
        for (int i = 0; i < TotalPointCount; i++)
        {
            GameObject obj = m_drawPointNodeList[i];
            obj.SetActive(false);
        }
        m_countDrawPointAni = -1;
    }
}
