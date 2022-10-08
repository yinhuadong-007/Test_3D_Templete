using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Src_PaintMask : MonoBehaviour
{
    Texture2D m_paintTexture;//shader的纹理贴图

    public int R = 10;//着色笔半径

    [Tooltip("初始色")] public Color m_initColor = Color.blue;//初始色
    [Tooltip("目标色")] public Color m_tarColor = new Color(0, 0, 0, 0);//目标色
    [Tooltip("是不是mask, 如果是mask，则目标色只会显示触摸区域， 否则触摸过的区域会一直显示目标色")] public bool m_mask;//是不是mask， 使用此方法做mask，R越大性能越差


    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
        Material paintMat = mr.materials[1];

        //涂色的纹理，我们要涂颜色，就是修改这个纹理对象的数据
        paintMat.mainTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
        m_paintTexture = paintMat.mainTexture as Texture2D;
        //end

        //初始化我们纹理的颜色
        Color[] colors = new Color[m_paintTexture.height * m_paintTexture.width];
        for (int i = 0, length = colors.Length; i < length; i++)
        {
            colors[i] = m_initColor;
        }
        m_paintTexture.SetPixels(colors);
        //end

        //要应用一下才能让修改生效
        m_paintTexture.Apply();

    }

    bool m_draw;
    Vector2 lastPoint;

    //mask有效
    List<Vector2> lastUVPoints;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_draw = true;
            this.lastPoint = Vector2.zero;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_draw = false;
            this.lastPoint = Vector2.zero;
        }

        if (m_draw)
        {
            if (m_mask)
            {
                if (lastUVPoints == null) lastUVPoints = new List<Vector2>();
                foreach (var item in lastUVPoints)
                {
                    DrawColor((int)item.x, (int)item.y, m_initColor);
                }
            }
            //使用物理射线检测来获取当前触摸点在地面上的位置信息，进而得到当前点在纹理上的uv坐标
            //获取从摄像机到屏幕点的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))//做射线检测
            {
                //获取碰撞点信息
                Vector2 uv = hit.textureCoord;
                // Debug.Log("uv: " + uv.x + " : " + uv.y);

                //获取在纹理上的当前着色点
                int x = ((int)(uv.x * m_paintTexture.width)) % m_paintTexture.width;
                int y = ((int)(uv.y * m_paintTexture.height)) % m_paintTexture.height;

                //不是mask
                if (!m_mask)
                {
                    //插值上色
                    //初始化上一次的着色点
                    if (this.lastPoint == Vector2.zero)
                    {
                        this.lastPoint = new Vector2(x, y);
                    }

                    //获取插值总系数t
                    float t = Mathf.Abs((float)(y - this.lastPoint.y));
                    float t2 = Mathf.Abs((float)(x - this.lastPoint.x));
                    t = t > t2 ? t : t2;
                    t = t / R;
                    // Debug.Log("t = " + t);

                    //定义当前插值系数i
                    int i = 0;
                    while (i < t)
                    {
                        //插值获取纹理的位置
                        Vector2 curPoint = Vector2.Lerp(this.lastPoint, new Vector2(x, y), i / t);
                        // Debug.Log("curPoint = " + curPoint);
                        //着色插值位置圆形范围的纹理颜色
                        this.DrawCircleAt((int)curPoint.x, (int)curPoint.y);
                        //递增当前插值系数i
                        i++;
                    }
                    //记录当前的位置，用于下一次插值的起始位置
                    this.lastPoint = new Vector2(x, y);
                }

                //着色当前目标位置圆形范围的纹理颜色
                this.DrawCircleAt(x, y);
            }
            //应用纹理上所有的修改
            m_paintTexture.Apply();
        }
    }

    void DrawColor(int x, int y, Color c)
    {
        //在纹理坐标（x + i, y + j）的位置设置m_tarColor颜色
        m_paintTexture.SetPixel(x, y, c);
    }

    void DrawCircleAt(int x, int y)
    {
        for (int i = -R; i < R; i++)
        {
            for (int j = -R; j < R; j++)
            {
                if (i * i + j * j > R * R)
                {
                    continue;
                }
                DrawColor(x + i, y + j, m_tarColor);
                //mask
                if (m_mask) lastUVPoints.Add(new Vector2(x + i, y + j));
            }
        }

    }
}
