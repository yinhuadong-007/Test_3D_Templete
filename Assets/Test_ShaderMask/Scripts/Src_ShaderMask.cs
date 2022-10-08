using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_ShaderMask : MonoBehaviour
{
    public float R = 10;//半径
    bool m_draw;
    MeshRenderer mr;
    Material mat;

    void Start()
    {
        mr = this.gameObject.GetComponent<MeshRenderer>();
        mat = mr.material;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_draw = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_draw = false;
        }
        if (m_draw)
        {
            //使用物理射线检测来获取当前触摸点在地面上的位置信息，进而得到当前点在纹理上的uv坐标
            //获取从摄像机到屏幕点的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))//做射线检测
            {
                //获取碰撞点信息
                Vector2 uv = hit.textureCoord;
                mat.SetVector("_TouchPosition", uv);
                mat.SetFloat("_Radius", R);
                mr.material = mat;
            }
        }
    }
}
