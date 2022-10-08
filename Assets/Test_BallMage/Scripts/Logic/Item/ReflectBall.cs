using UnityEngine;
namespace Item
{
    /// <summary>
    /// 道具-反射球
    /// </summary>
    public class ReflectBall : Src_BaseBlock
    {
        /// <summary>反弹角度间隔设置</summary>
        public float reflectAngle = 30;
        /// <summary>额外生成球数量 </summary>
        public int createNum = 4;
        private void Awake()
        {
            //初始化数据
            reflectAngle=GameData.instance.reflectAngle;
            createNum=GameData.instance.reflectNum;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Ball")
            {
                //模拟发射的球不会触发道具
                if (other.gameObject.GetComponent<Src_BaseBall>().isSimulator == true)
                {
                    
                    return;
                }
                for (int z = 0; z < createNum; z++)
                {
                    var k=(z%2==1)?-1:1;
                    var multi=1+Mathf.Floor(z/2);
                    var newDirection = Quaternion.AngleAxis(this.reflectAngle*multi*k, Vector3.up) * other.gameObject.GetComponent<Rigidbody>().velocity;
                    GameManager.instance.CreateOneBullet(other.transform.position+0.1f*newDirection, newDirection, GameData.instance.ballSpeed);
                }
            }
        }
    }
}