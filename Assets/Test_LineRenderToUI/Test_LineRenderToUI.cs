using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_LineRenderToUI : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer lineRender;
    public Camera uiCamera;
    List<Vector3> pathList;

    void Start()
    {
        pathList = new List<Vector3>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pathList.Clear();
            lineRender.positionCount = 0;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 point = uiCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            pathList.Add(point);
            Draw();
        }
    }

    void Draw()
    {
        lineRender.positionCount = pathList.Count;
        lineRender.SetPositions(pathList.ToArray());
    }
}
