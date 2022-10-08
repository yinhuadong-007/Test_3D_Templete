using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 功能:配置文件
/// 更新：2022-19-08 17:41:33
/// <summary>
[Serializable]
public class Config : MonoBehaviour{
    /// <summary> 配置数据 </summary>
    public static ConfigRoot data;
    /// <summary> json文件 </summary>
    [Tooltip("json文件")]
    public TextAsset json;
    public ConfigRoot root;
    void Awake(){
        JsonUtility.FromJsonOverwrite(json.text,this.root);
        data = this.root;
    }
    /// <summary> 配置数据接口：描述xxx </summary>
    [Serializable]
    public class B{
        private static Dictionary<int,int> iidic = new Dictionary<int, int>();
        ///<summary> 根据id获取 </summary>
        public static B get(int id){
            if(iidic.ContainsKey(id)){return data.B[iidic[id]];}
            for(int i = 0;i < data.B.Length;i++){ if(data.B[i].id == id){iidic.Add(id,i);return data.B[i];}}
            return null;
        }
        /// <summary> 例int </summary>
        public int id;
        /// <summary> 例float </summary>
        public float scale;
        /// <summary> 例string </summary>
        public string name;
        /// <summary> 例int[] </summary>
        public int[] intList;
        /// <summary> 例float[] </summary>
        public float[] floatList;
        /// <summary> 例string[] </summary>
        public string[] nameList;
    }
    /// <summary> 配置数据接口：方块预设表 </summary>
    [Serializable]
    public class PreBlockList{
        private static Dictionary<int,int> iidic = new Dictionary<int, int>();
        ///<summary> 根据id获取 </summary>
        public static PreBlockList get(int id){
            if(iidic.ContainsKey(id)){return data.PreBlockList[iidic[id]];}
            for(int i = 0;i < data.PreBlockList.Length;i++){ if(data.PreBlockList[i].id == id){iidic.Add(id,i);return data.PreBlockList[i];}}
            return null;
        }
        /// <summary> 方块编号 </summary>
        public int id;
        /// <summary> 血量 </summary>
        public int blood;
        /// <summary> 样式 </summary>
        public int style;
        /// <summary> 类型 </summary>
        public int type;
    }
    /// <summary> 配置数据接口：描述xxx </summary>
    [Serializable]
    public class A{
        /// <summary> 数据 </summary>
        public static A data{ get{ return Config.data.A; } }

        /// <summary> 例1-整数 </summary>
        public int intVal;
        /// <summary> 例2-浮点数 </summary>
        public float floatVal;
        /// <summary> 例3-字符串 </summary>
        public string strVal;
        /// <summary> 例4-整数列表 </summary>
        public int[] intList;
        /// <summary> 例5-浮点数列表 </summary>
        public float[] floatList;
        /// <summary> 例6-字符串列表 </summary>
        public string[] strList;
    }
    /// <summary> 数据类型-配置根节点 </summary>
    [Serializable]
    public class ConfigRoot{
        /// <summary> 描述xxx </summary>
        public B[] B;
        /// <summary> 方块预设表 </summary>
        public PreBlockList[] PreBlockList;
        /// <summary> 描述xxx </summary>
        public A A;
    }
}