using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{
    public Transform parent;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        int length = 100;
        for (int i = 0; i < length; i++)
        {
            GameObject b = Instantiate(ball, parent);

            b.transform.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-6f, 6f), Random.Range(-5f, 5f));

            MaterialPropertyBlock props = new MaterialPropertyBlock();
            MeshRenderer renderer = b.GetComponent<MeshRenderer>();
            Color tarColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            props.SetColor("_Color", tarColor);
            renderer.SetPropertyBlock(props);
        }
    }

}
