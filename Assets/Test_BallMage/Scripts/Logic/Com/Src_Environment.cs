using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_Environment : MonoBehaviour
{
    public static float WallLeftValue;
    public static float WallRightValue;

    [Tooltip("左边的墙")]
    public Transform m_wallLeft;

    [Tooltip("右边的墙")]
    public Transform m_wallRight;

    // Start is called before the first frame update
    public void Init()
    {
        WallLeftValue = m_wallLeft.position.x + (m_wallLeft.lossyScale.x / 2) * m_wallLeft.GetComponent<BoxCollider>().size.x;
        WallRightValue = m_wallRight.position.x - m_wallRight.lossyScale.x / 2 * m_wallRight.GetComponent<BoxCollider>().size.x;

        // StaticMeshFun();
    }

    private Dictionary<Material, List<GameObject>> combineList;
    private void Awake()
    {

    }
    private void Start()
    {
        // StaticMeshFun();
    }

    private void StaticMeshFun()
    {
        MeshRenderer[] mrs = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        combineList = new Dictionary<Material, List<GameObject>>();
        for (int i = 0; i < mrs.Length; i++)
        {
            Add2CombineList(mrs[i]);
        }

        int index = 0;
        foreach (KeyValuePair<Material, List<GameObject>> item in combineList)
        {
            index++;
            CreateStaticMesh(item.Value, index);
        }
    }

    private GameObject meshRoot;
    private void CreateStaticMesh(List<GameObject> objs, int index)
    {
        if (meshRoot == null)
        {
            meshRoot = new GameObject("CombineMesh");
        }
        GameObject[] gos = new GameObject[objs.Count];
        for (int i = 0; i < objs.Count; i++)
        {
            gos[i] = objs[i];
        }
        StaticBatchingUtility.Combine(gos, meshRoot);
    }

    private void Add2CombineList(MeshRenderer mr)
    {
        if (combineList.ContainsKey(mr.sharedMaterial) == false)
        {
            combineList.Add(mr.sharedMaterial, new List<GameObject>());
        }
        combineList[mr.sharedMaterial].Add(mr.gameObject);


    }
}
