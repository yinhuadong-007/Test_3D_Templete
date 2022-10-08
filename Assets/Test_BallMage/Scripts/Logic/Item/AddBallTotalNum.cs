using UnityEngine;
namespace Item
{
    /// <summary>道具-增加小球总数量</summary>
    public class AddBallTotalNum : Src_BaseBlock
    {
        /// <summary>单次增加小球总数量</summary>
        public int addNum;
        private void Awake() {
            this.addNum=GameData.instance.totalBallNum;
        }
        /// <summary>被消灭时候，增加当前小球数量</summary>
        private void OnDestroy()
        {
            if(this.m_blood>0){
                return;
            }
            GameManager.instance.AddBallCount(this.addNum);
        }
    }
}