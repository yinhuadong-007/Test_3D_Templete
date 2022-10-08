using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    /// <summary>
    /// 获得animator下某个动画片段的时长方法
    /// </summary>
    /// <param animator="animator">Animator组件</param> 
    /// <param name="name">要获得的动画片段名字</param>
    /// <returns></returns>
    public static float GetAnimatorLength(Animator animator, string name)
    {
        //动画片段时间长度
        float length = 0;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            Print.Log(clip.name);
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        return length;
    }

    // 清除所有的激活中的trigger缓存
    public static void ResetAllTriggers(Animator animator)
    {
        AnimatorControllerParameter[] aps = animator.parameters;
        for (int i = 0; i < aps.Length; i++)
        {
            AnimatorControllerParameter paramItem = aps[i];
            if (paramItem.type == AnimatorControllerParameterType.Trigger)
            {
                string triggerName = paramItem.name;
                bool isActive = animator.GetBool(triggerName);
                if (isActive)
                {
                    animator.ResetTrigger(triggerName);
                }
            }
        }
    }

    /// <summary>
    /// 清除父物体下面的所有子物体
    /// </summary>
    /// <param name="parent"></param>
    public static void ClearChildren(Transform parent)
    {
        if (parent.childCount > 0)
        {
            for (int i = 0, len = parent.childCount; i < len; i++)
            {
                GameObject.Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 判断触摸点是否有点击事件遮挡
    /// </summary>
    /// <param name="screenPosition">触摸点</param>
    /// <returns>是否被遮挡</returns>
    public static bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        UnityEngine.EventSystems.PointerEventData eventDataCurrentPosition = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<UnityEngine.EventSystems.RaycastResult> results = new List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static void AddButtonEvent(GameObject UIObject, UnityEngine.Events.UnityAction cbk)
    {
        if (UIObject == null) return;
        UnityEngine.UI.Button btn = UIObject.GetComponent<UnityEngine.UI.Button>();
        if (btn == null)
        {
            Debug.LogError("未添加 UnityEngine.UI.Button 脚本");
            return;
        }
        UnityEngine.UI.Button.ButtonClickedEvent e_t = new UnityEngine.UI.Button.ButtonClickedEvent();
        e_t.AddListener(cbk);
        btn.onClick = e_t;
    }

}


/// <summary>
/// 作者：Foldcc
/// </summary>
public class BezierMath
{
    /// <summary>
    /// 二次贝塞尔
    /// </summary>
    public static Vector3 Bezier_2(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
    }
    public static void Bezier_2ref(ref Vector3 outValue, Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        outValue = (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
    }

    /// <summary>
    /// 三次贝塞尔
    /// </summary>
    public static Vector3 Bezier_3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) + t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
    }
    public static void Bezier_3ref(ref Vector3 outValue, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        outValue = (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) + t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
    }
}


