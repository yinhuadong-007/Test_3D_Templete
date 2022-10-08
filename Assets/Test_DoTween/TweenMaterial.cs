using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenMaterial : MonoBehaviour
{
    public Gradient _gradient;
    // Start is called before the first frame update
    void Start()
    {
        //materail 
        Material material = GetComponent<MeshRenderer>().material;
        // material.DOColor(Color.red, 2);//shader必须有_Color属性 否则 DoColor(Color.red,"_OtherColor",2)
        // material.DOFade(0, 2); //material.DOColor(Color.clear, 2)//变透明 两种,  需要材质支持透明才行，即RenderingMode不是opaque
        // material.DOGradientColor(_gradient, 2);  //颜色渐变
        // material.DOOffset(new Vector2(10, 20), 2); //贴图偏移
        // material.DOVector(new Vector4(1, 1, 1, 0), "_Color", 2); // 修改shader属性值

        //颜色混合
        // material.DOBlendableColor(Color.red, 2);
        // material.DOBlendableColor(Color.blue, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
