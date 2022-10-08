using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// 功能: 缓动
/// </summary>
public class Tween : MonoBehaviour
{
    private static Tween instance;
    /// <summary> 缓动 </summary>
    public static TweenTransformItem target(Transform target)
    {
        if (instance == null) initToScene();
        TweenTransformItem item = new TweenTransformItem(target);
        return item;
    }
    /// <summary> 移除对象身上的tween </summary>
    public static void remove(Transform target)
    {
        if (instance == null) return;
        instance.removeItem(target);
    }
    // 初始化到场景
    private static void initToScene()
    {
        GameObject srcTools = GameObject.Find("ScriptTools"); //脚本工具根节点
        if (!srcTools) srcTools = new GameObject("ScriptTools"); //将脚本工具根节点添加到场景
        instance = srcTools.AddComponent<Tween>(); //添加缓动组件
    }

    /// <summary> 延时调用 </summary>
    public static TweenTransformItem delay(float delayTime, Action action)
    {
        if (instance == null) initToScene();
        TweenTransformItem item = new TweenTransformItem(null);
        item.delay(delayTime, action);
        return item;
    }

    /// <summary> 取消延时调用 </summary>
    public static void cancelDelay(Action action)
    {
        if (instance == null) return;
        instance.removeItem(action);
    }

    /// <summary> 循环类型 </summary>
    public enum LoopType
    {
        /// <summary> 闭环循环 </summary>
        ABC_ABC,
        /// <summary> 来回循环 </summary>
        ABCBA,
    }

    /// <summary> 缓动项 </summary>
    [Serializable]
    public class TweenTransformItem
    {
        private static Vector3 nullV3 = new Vector3(-9999.987f, 9999.798f, 3.1000090000123456f);
        public Transform transform;
        public List<float> tm_total;
        public float tm_pass;
        public Vector3 _fromPos;
        public List<Vector3> _toPos;
        public Vector3 _fromScale;
        public List<Vector3> _toScale;
        public Vector3 _fromRotation;
        public List<Vector3> _toRotation;
        public List<Action> _callBack;

        private bool rotationIsGlobal = true;//旋转是否是全局
        private bool positonIsGlobal = true;//位置是否是全局
        private bool isLoop = false;//循环
        private bool loopABCA = false;//是否是循环ABCBABCB...这种循环方式
        private int loopCount = -1;//循环次数，默认/小于0表示无限循环

        public TweenTransformItem(Transform target)
        {
            this.transform = target;
            this.tm_total = new List<float>();
            this._toPos = new List<Vector3>();
            this._toScale = new List<Vector3>();
            this._toRotation = new List<Vector3>();
            this._callBack = new List<Action>();
            this.tm_pass = 0f;
            this._fromPos = Vector3.zero;
            this._fromScale = Vector3.zero;
            this._fromRotation = Vector3.zero;
        }

        /// <summary> 设置坐标、角度 为global或者local </summary>
        public TweenTransformItem setLocalGlobal(bool rotationIsGlobal = false, bool positonIsGlobal = false)
        {
            this.rotationIsGlobal = rotationIsGlobal;
            this.positonIsGlobal = positonIsGlobal;
            return this;
        }

        /// <summary> 修正角度旋转不超过180度 </summary>
        private void fixRotationIn180()
        {
            bool isFix = false;
            float fixX = 0f;
            float fixY = 0f;
            float fixZ = 0f;
            for (int i = 0; i < this._toRotation.Count; i++)
            {
                if (this._toRotation[i] != nullV3)
                {
                    Vector3 toRotation = this._toRotation[i];
                    isFix = false;
                    fixX = toRotation.x;
                    fixY = toRotation.y;
                    fixZ = toRotation.z;
                    if (Mathf.Abs(toRotation.x - this._fromRotation.x) > 180f)
                    {
                        fixX = toRotation.x + (toRotation.x > this._fromRotation.x ? -360f : 360f);
                        isFix = true;
                    }
                    if (Mathf.Abs(toRotation.y - this._fromRotation.y) > 180f)
                    {
                        fixY = toRotation.y + (toRotation.y > this._fromRotation.y ? -360f : 360f);
                        isFix = true;
                    }
                    if (Mathf.Abs(toRotation.z - this._fromRotation.z) > 180f)
                    {
                        fixZ = toRotation.z + (toRotation.z > this._fromRotation.z ? -360f : 360f);
                        isFix = true;
                    }

                    if (isFix)
                    {
                        this._toRotation[i] = new Vector3(fixX, fixY, fixZ);
                    }
                }
            }
        }

        /// <summary> 延迟执行-存在帧间隔时间(Time.deltaTime)内的误差 </summary>
        public TweenTransformItem delay(float tm, Action callBack = null)
        {
            if (tm >= 0f)
            {
                this._toPos.Add(nullV3);
                this._toScale.Add(nullV3);
                this._toRotation.Add(nullV3);
                this.tm_total.Add(tm);
                this._callBack.Add(callBack);
            }
            return this;
        }

        /// <summary> 位置缓动 </summary>
        public TweenTransformItem toPosition(Vector3 toPosition, float tm, Action callBack = null)
        {
            this._toPos.Add(toPosition);
            this._toScale.Add(nullV3);
            this._toRotation.Add(nullV3);
            this.tm_total.Add(tm);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary> 缩放缓动 </summary>
        public TweenTransformItem toScale(Vector3 fromScale, float tm, Action callBack = null)
        {
            this._toPos.Add(nullV3);
            this._toScale.Add(fromScale);
            this._toRotation.Add(nullV3);
            this.tm_total.Add(tm);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary> 旋转缓动 </summary>
        public TweenTransformItem toRotation(Vector3 toRotation, float tm, Action callBack = null)
        {
            this._toPos.Add(nullV3);
            this._toScale.Add(nullV3);
            this._toRotation.Add(toRotation);
            this.tm_total.Add(tm);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary> 位置加缩放 </summary>
        public TweenTransformItem toPositionScale(Vector3 toPosition, Vector3 toScale, float tm, Action callBack = null)
        {
            this._toPos.Add(toPosition);
            this._toScale.Add(toScale);
            this._toRotation.Add(nullV3);
            this.tm_total.Add(tm);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary> 位置加旋转 </summary>
        public TweenTransformItem toPositionRotation(Vector3 toPosition, Vector3 toRotation, float tm, Action callBack = null)
        {
            this._toPos.Add(toPosition);
            this._toScale.Add(nullV3);
            this._toRotation.Add(toRotation);
            this.tm_total.Add(tm);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary> 缩放加选择 </summary>
        public TweenTransformItem toScaleRotation(Vector3 toScale, Vector3 toRotation, float tm, Action callBack = null)
        {
            this._toPos.Add(nullV3);
            this._toScale.Add(toScale);
            this._toRotation.Add(toRotation);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary> 位置加缩放加旋转 </summary>
        public TweenTransformItem to(Vector3 toPosition, Vector3 toScale, Vector3 toRotation, float tm, Action callBack = null)
        {
            this._toPos.Add(toPosition);
            this._toScale.Add(toScale);
            this._toRotation.Add(toRotation);
            this.tm_total.Add(tm);
            this._callBack.Add(callBack);
            return this;
        }

        /// <summary>
        /// 设置为循环模式，默认循环方式为[123123123..]，如果参数为true则循环方式变为[123212321...]
        /// <param name="loopType">循环模式</param>
        /// <param name="loopCount">循环次数， exp配置为1, 会执行2次 </param>
        /// <returns></returns>
        /// </summary>
        public TweenTransformItem loop(LoopType loopType = LoopType.ABC_ABC, int loopCount = -1)
        {
            this.isLoop = true;
            this.loopABCA = loopType == LoopType.ABC_ABC ? true : false;
            this.loopCount = loopCount;
            return this;
        }

        /// <summary> 开始缓动 </summary>
        public void start(bool needFixRotationIn180 = false)
        {
            if (this.transform != null)
            {
                if (this.positonIsGlobal)
                {
                    this._fromPos = this.transform.position;
                }
                else
                {
                    this._fromPos = this.transform.localPosition;
                }
                if (this.rotationIsGlobal)
                {
                    this._fromRotation = this.transform.eulerAngles;
                }
                else
                {
                    this._fromRotation = this.transform.localEulerAngles;
                }
                this._fromScale = transform.localScale;
            }
            this.tm_pass = 0;
            if (needFixRotationIn180)
            {
                this.fixRotationIn180();
            }
            if (this.isLoop && this.loopABCA)
            {//如果循环为ABCBABCB...模式，则需要额外添加一些项;
                for (int i = this.tm_total.Count - 2; i > 0; i--)
                {
                    this.tm_total.Add(this.tm_total[i]);
                    this._toPos.Add(this._toPos[i]);
                    this._toScale.Add(this._toScale[i]);
                    this._toRotation.Add(this._toRotation[i]);
                    this._callBack.Add(this._callBack[i]);
                }
            }
            Tween.instance.addItem(this);
        }


        /// <summary> 返回值表示是否完成一步 </summary>
        public bool step(float dt)
        {
            this.tm_pass = this.tm_pass + dt;
            if (this.tm_total.Count == 0) return false;//调动一个已完成的tween，或者没有tween项的tween
            //到达一个目标节点
            if (this.tm_pass >= this.tm_total[0])
            {
                if (this.transform != null)
                {
                    if (this._toPos[0] != nullV3)
                    {
                        if (this.positonIsGlobal)
                        {
                            this.transform.position = this._toPos[0];
                            this._fromPos = this._toPos[0];
                        }
                        else
                        {
                            this.transform.localPosition = this._toPos[0];
                            this._fromPos = this._toPos[0];
                        }
                    }
                    if (this._toRotation[0] != nullV3)
                    {
                        if (this.rotationIsGlobal)
                        {
                            this.transform.eulerAngles = this._toRotation[0];
                            this._fromRotation = this._toRotation[0];
                        }
                        else
                        {
                            this.transform.localEulerAngles = this._toRotation[0];
                            this._fromRotation = this._toRotation[0];
                        }
                    }
                    if (this._toScale[0] != nullV3)
                    {
                        this.transform.localScale = this._toScale[0];
                        this._fromScale = this.transform.localScale;
                    }
                }
                this.tm_pass -= this.tm_total[0];

                if (this.isLoop && this.loopCount != 0)
                {//如果是循环
                    this.tm_total.Add(this.tm_total[0]);
                    this._toPos.Add(this._toPos[0]);
                    this._toScale.Add(this._toScale[0]);
                    this._toRotation.Add(this._toRotation[0]);
                    this._callBack.Add(this._callBack[0]);

                    if (this.loopCount > 0)
                    {
                        this.loopCount--;
                    }
                }
                this.tm_total.RemoveAt(0);
                this._toPos.RemoveAt(0);
                this._toScale.RemoveAt(0);
                this._toRotation.RemoveAt(0);
                return true;
            }
            //进度更新
            float progress = this.tm_pass / this.tm_total[0];
            if (this.transform != null)
            {
                if (this._toPos[0] != nullV3)
                {
                    if (this.positonIsGlobal)
                    {
                        this.transform.position = Vector3.Lerp(this._fromPos, this._toPos[0], progress);
                    }
                    else
                    {
                        this.transform.localPosition = Vector3.Lerp(this._fromPos, this._toPos[0], progress);
                    }
                }
                if (this._toRotation[0] != nullV3)
                {
                    if (this.rotationIsGlobal)
                    {
                        Vector3 lp = Vector3.Lerp(this._fromRotation, this._toRotation[0], progress);
                        this.transform.eulerAngles = Vector3.Lerp(this._fromRotation, this._toRotation[0], progress);
                    }
                    else
                    {
                        this.transform.localEulerAngles = Vector3.Lerp(this._fromRotation, this._toRotation[0], progress);
                    }
                }
                if (this._toScale[0] != nullV3) this.transform.localScale = Vector3.Lerp(this._fromScale, this._toScale[0], progress);
            }
            return false;
        }

        /// <summary> 销毁缓动，置空内部引用 </summary>
        public void dispose()
        {
            this.transform = null;
            this.tm_total = null;
            this._toPos.Clear();
            this._toScale.Clear();
            this._toRotation.Clear();
            this._callBack.Clear();
            this._toPos = null;
            this._toScale = null;
            this._toRotation = null;
            this._callBack = null;
        }
    }

    /// <summary> ================ </summary>
    /// <summary>    缓动运行     </summary>
    /// <summary> ================ </summary>
    /// <summary> 正在运行的缓动数量 </summary>
    [Tooltip("正在运行的缓动数量")]
    public int runCount = 0;

    public List<TweenTransformItem> tweenList = new List<TweenTransformItem>();//待缓动项
    public List<TweenTransformItem> completeCallBackList = new List<TweenTransformItem>();//完成回调列表,用于排序
    /// <summary> 增加一项目 </summary>
    public void addItem(TweenTransformItem item)
    {
        tweenList.Add(item);
    }
    /// <summary> 移除以一项，指定缓动目标 </summary>
    public void removeItem(Transform item)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].transform == item)
            {
                tweenList.RemoveAt(i);
                break;
            }
        }
    }
    /// <summary> 移除一项,指定一个回调 </summary>
    public void removeItem(Action action)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i]._callBack.IndexOf(action) > -1)
            {
                tweenList.RemoveAt(i);
                break;
            }
        }
    }

    void QuickSort(int[] a, int left, int right)
    {
        if (left >= right) return;
        int l = left;
        int r = right;
        int x = a[left];
        while (r > l)
        {
            if (r >= l && a[r] >= x)
                r--;
            a[l] = a[r];
            if (r >= l && a[l] <= x)
                l++;
            a[r] = a[l];
        }
        a[r] = x;
    }

    void QuickSort(List<TweenTransformItem> a, int left, int right)
    {
        if (left >= right) return;
        int l = left;
        int r = right;
        TweenTransformItem x = a[left];
        while (r > l)
        {
            if (r >= l && a[r].tm_pass >= x.tm_pass)
                r--;
            a[l] = a[r];
            if (r >= l && a[l].tm_pass <= x.tm_pass)
                l++;
            a[r] = a[l];
        }
        a[r] = x;
        QuickSort(a, left, r - 1);
        QuickSort(a, l + 1, right);
    }

    void Update()
    {
        completeCallBackList.Clear();
        //缓动一步
        for (int i = 0; i < tweenList.Count; i++)
        {
            TweenTransformItem item = tweenList[i];
            if (item.step(Time.deltaTime))
            {//如果缓动阶段完成
                if (item._callBack[0] != null)
                {//需要执行回调，执行后再移除
                    completeCallBackList.Add(item);
                }
                else
                {
                    item._callBack.RemoveAt(0);//无需执行回调，直接移除
                }
            }
        }
        //回调排序-确保时间先后
        // if (completeCallBackList.Count > 1)
        // {
        // completeCallBackList.Sort((a, b) =>
        // {
        //     return a.tm_pass < b.tm_pass ? 1 : -1;
        // });
        // }

        //冒泡排序
        // for (int i = 0; i < completeCallBackList.Count; i++)
        // {
        //     for (int j = 0; j < completeCallBackList.Count - i - 1; j++)
        //     {
        //         if (completeCallBackList[j].tm_pass > completeCallBackList[j + 1].tm_pass)
        //         {
        //             TweenTransformItem temp = completeCallBackList[j];
        //             completeCallBackList[j] = completeCallBackList[j + 1];
        //             completeCallBackList[j + 1] = temp;
        //         }
        //     }
        // }

        QuickSort(completeCallBackList, 0, completeCallBackList.Count - 1);

        //执行回调
        if (completeCallBackList.Count > 0)
        {
            for (int i = 0; i < completeCallBackList.Count; i++)
            {
                completeCallBackList[i]._callBack[0]();//执行回调 , 在这个里面可能会清理所有的Tween

                if (completeCallBackList.Count == 0)
                {
                    return;
                }
                completeCallBackList[i]._callBack.RemoveAt(0);//执行后移除
            }
        }
        //移除已完成
        for (int i = tweenList.Count - 1; i > -1; i--)
        {
            TweenTransformItem item = tweenList[i];
            if (item.tm_total.Count == 0)
            {
                item.dispose();
                tweenList.RemoveAt(i);
            }
        }
        this.runCount = tweenList.Count;
    }

    //切换场景后
    private void OnDestroy()
    {
        Tween.instance = null;
        this.completeCallBackList.Clear();
        this.completeCallBackList = null;
        for (int i = 0; i < this.tweenList.Count; i++)
        {
            TweenTransformItem item = this.tweenList[i];
            item.dispose();
        }
        this.tweenList.Clear();
        this.tweenList = null;
    }

    public static void Clear()
    {
        if (instance != null) instance._Clear();
    }

    private void _Clear()
    {
        this.completeCallBackList.Clear();
        for (int i = 0; i < this.tweenList.Count; i++)
        {
            TweenTransformItem item = this.tweenList[i];
            item.dispose();
        }
        this.tweenList.Clear();
    }


    // static float linear(float start, float end, float value){
    //  return Mathf.Lerp(start, end, value);
    // }
    // static float easeInCubic(float start, float end, float value){
    //  end -= start;
    //  return end * value * value * value + start;
    // }
    // static float easeOutCubic(float start, float end, float value){
    //  value--;
    //  end -= start;
    //  return end * (value * value * value + 1) + start;
    // }
    // static float easeInOutCubic(float start, float end, float value){
    //  value /= .5f;
    //  end -= start;
    //  if (value < 1) return end * 0.5f * value * value * value + start;
    //  value -= 2;
    //  return end * 0.5f * (value * value * value + 2) + start;
    // }
    // static float easeInSine(float start, float end, float value){
    //  end -= start;
    //  return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
    // }

    // static float easeOutSine(float start, float end, float value){
    //  end -= start;
    //  return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
    // }
    // static float easeInOutSine(float start, float end, float value){
    //  end -= start;
    //  return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
    // }
    // static float easeInBack(float start, float end, float value){
    //  end -= start;
    //  value /= 1;
    //  float s = 1.70158f;
    //  return end * (value) * value * ((s + 1) * value - s) + start;
    // }
    // static float easeOutBack(float start, float end, float value){
    //  float s = 1.70158f;
    //  end -= start;
    //  value = (value) - 1;
    //  return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
    // }
    // static float easeInOutBack(float start, float end, float value){
    //  float s = 1.70158f;
    //  end -= start;
    //  value /= .5f;
    //  if ((value) < 1){
    //      s *= (1.525f);
    //      return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
    //  }
    //  value -= 2;
    //  s *= (1.525f);
    //  return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
    // }

}



