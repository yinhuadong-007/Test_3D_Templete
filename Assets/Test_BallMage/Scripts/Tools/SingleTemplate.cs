using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//where 约束T class表示是一个引用类型，new()表示带有公共的无参构造函数
public class SingleTemplate<T> where T : class, new()
{
    // private static readonly object syslock = new object();

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // lock (syslock)
                // {
                if (instance == null)
                {
                    instance = new T();
                }
                // }
            }
            return instance;
        }
    }
}

