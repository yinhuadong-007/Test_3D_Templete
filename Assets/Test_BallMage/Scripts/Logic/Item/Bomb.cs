using UnityEngine;
namespace Item
{
    /// <summary>
    /// 道具-炸弹
    /// </summary>
    public class Bomb : Src_BaseBlock
    {
        /// <summary>爆炸半径</summary>
        public float radius;
        /// <summary>爆炸伤害</summary>
        public int damage;

        private void Awake()
        {
            //初始化数据
            radius = GameData.instance.bombRadius;
            damage = GameData.instance.bombDamage;
        }
        /// <summary>销毁时触发爆炸</summary>
        private void OnDestroy()
        {
            if (this.m_blood > 0)
            {
                return;
            }


            var affectObj = new GameObject();
            affectObj.transform.position = this.transform.parent.position;
            affectObj.AddComponent<AffectSphere>().init(this.radius, this.damage);
            // var ownerPos = this.transform.parent.position;
            // var ts = this.transform.parent.parent;
            // for (int z = 0; z < ts.childCount; z++)
            // {
            //     var cube = ts.GetChild(z);
            //     var distance = Vector3.Distance(ownerPos, cube.transform.position);
            //     var comList_block = cube.GetComponentsInChildren<Src_BaseBlock>();
            //     if (distance < this.radius)
            //     {
            //         for (int y = 0; y < comList_block.Length; y++)
            //         {
            //             comList_block[y].subBlood(this.damage);
            //         }
            //     }
            // }

            //爆炸特效
            var color = this.GetComponentInChildren<MeshRenderer>().material.GetColor("_BaseColor");
            ParticleManager.Instance.PlayParticle(EParticleType.Bomb, CommonPrefabData.instance.particleBomb, this.transform.position);
        }
    }
}