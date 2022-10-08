using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : MonoBehaviour
{
    public static bool isDebug = true;
    public static void Log(string str)
    {
        if (isDebug) Debug.Log(str);
    }

    public static void LogError(string str)
    {
        Debug.LogError(str);
    }

    public static void LogWarning(string str)
    {
        Debug.LogWarning(str);
    }

    private void Awake()
    {
        // m_isDebug = isDebug;
    }

}
