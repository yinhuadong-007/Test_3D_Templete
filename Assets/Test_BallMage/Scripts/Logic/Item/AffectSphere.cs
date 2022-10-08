using UnityEngine;
/// <summary>触发销毁半径</summary>
public class AffectSphere : MonoBehaviour
{
    /// <summary>触发半径</summary>
    public float radius;
    /// <summary>伤害</summary>
    public int damage;
    /// <summary>帧计数</summary>
    private int _timeNum = 0;
    public void init(float radius, int damage)
    {
        this.radius = radius;
        this.damage = damage;

        var sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = this.radius;
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //销毁碰撞到的砖块
        if (other.tag == "Block")
        {
            Print.Log("subBlood OnTriggerEnter");
            other.GetComponent<Src_BaseBlock>().subBlood(this.damage);
        }
    }

    private void Update()
    {
        this._timeNum++;
        if (this._timeNum > 5)
        {
            Destroy(this.gameObject);
        }
    }
}