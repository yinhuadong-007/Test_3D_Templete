using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager
{

    public static Sprite LoadSprite(string name)
    {
        // Print.Log("-----> ResManager  Load Image = " + name);
        string path = "Textures/" + name;
        Sprite spr = Resources.Load<Sprite>(path);

        return spr;
    }

    public static GameObject LoadPrefab(string name)
    {
        // Print.Log("-----> ResManager  Load LoadPrefab = " + name);
        string path = "Prefabs/" + name;
        GameObject prefab = Resources.Load<GameObject>(path);

        return prefab;
    }

}
