using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager
{
    public static BallManager ins = new BallManager();

    private BallManager()
    {
        // m_txtRemainCnt = MainLogic.instance.InfoLayer.Find("icon").Find("remainCnt").GetComponent<TextMesh>();
    }

    //子弹对象池
    private List<GameObject> bulletPool = new List<GameObject>();

    //子弹容器盒(预创建子弹)
    public List<GameObject> bulletContainer = new List<GameObject>();

    private int bulletPoolCapacity = 800;

    /// <summary>
    /// 当前使用的所有子弹对象
    /// </summary>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    public List<GameObject> curUsingBullets = new List<GameObject>();

    //--------------------private function-------------------//

    /// <summary>
    /// 返回一个没有使用的子弹，如果没有则返回null
    /// </summary>
    /// <returns></returns>
    private GameObject PopBulletFromPool(List<GameObject> pool)
    {
        Print.Log("--->对象池 当前数量： " + pool.Count);
        GameObject node = null;
        if (pool.Count > 0)
        {
            int idx = pool.Count - 1;
            node = pool[idx];
            pool.RemoveAt(idx);
        }
        return node;
    }

    private void ClearBulletPool()
    {
        // Print.Log("------------->ResetBulletPool");
        if (bulletPool.Count > 0)
        {
            bulletPool.Clear();
        }
    }

    public void ClearUsingBullet()
    {
        for (int i = curUsingBullets.Count - 1; i >= 0; i--)
        {
            GameObject bulletObj = curUsingBullets[i];
            if (bulletObj == null)
            {
                curUsingBullets.RemoveAt(i);
                continue;
            }
            // Print.Log("---->Reset 移除子弹 序号 " + i);
            curUsingBullets.RemoveAt(i);
            var bulletCom = bulletObj.GetComponent<Src_BaseBall>();
            bulletCom.SelfDestroy();
        }
    }

    //--------------------------------public-------------------------------------//

    public void Clear()
    {
        bulletPool.Clear();
        bulletContainer.Clear();
        curUsingBullets.Clear();
    }

    /// <summary>
    /// 创建一个子弹
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="isGuide"></param>
    /// <returns></returns>
    public GameObject CreateBullet(GameObject bulletPrefab, bool isFired = true, bool fromBulletContainer = false)
    {
        GameObject node = null;
        if (isFired && fromBulletContainer)
        {
            node = PopBulletFromPool(bulletContainer);
        }
        if (node == null)
        {
            node = PopBulletFromPool(bulletPool);
        }

        Src_BaseBall bullet;

        if (node == null)
        {
            node = GameObject.Instantiate(bulletPrefab, LayerManager.instance.BallLayer);
            node.name = "Ball_" + curUsingBullets.Count;
            bullet = node.GetComponent<Src_BaseBall>();
            bullet.Addbullet += Addbullet;
        }
        else
        {
            bullet = node.GetComponent<Src_BaseBall>();
            node.name = "Ball_" + curUsingBullets.Count;
        }
        if (isFired)
        {
            curUsingBullets.Add(node);
        }
        node.SetActive(true);
        return node;
    }

    /// <summary>
    /// 创建一个预用子弹
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <returns></returns>
    public GameObject CreateAdvanceBullet(GameObject bulletPrefab)
    {
        GameObject node = PopBulletFromPool(bulletPool);

        Src_BaseBall bullet;

        if (node == null)
        {
            node = GameObject.Instantiate(bulletPrefab, LayerManager.instance.BallLayer);
            node.name = "Ball_Advance_" + bulletContainer.Count;
            bullet = node.GetComponent<Src_BaseBall>();
            bullet.Addbullet += Addbullet;
        }
        else
        {
            bullet = node.GetComponent<Src_BaseBall>();
            node.name = "Ball_Advance_" + curUsingBullets.Count;
        }
        bulletContainer.Add(node);

        node.SetActive(true);
        return node;
    }

    /// <summary>
    /// 回收子弹
    /// </summary>
    /// <param name="sender"></param>
    private void Addbullet(GameObject sender)
    {
        curUsingBullets.Remove(sender);
        if (bulletPool.Count >= bulletPoolCapacity)
        {
            GameObject.Destroy(sender);
        }
        else
        {
            bulletPool.Add(sender);
        }
        Print.Log("curUsingBullets.Count = " + curUsingBullets.Count);
        //没有在运动的小球了
        // if (curUsingBullets.Count == 0 && GameManager.instance.isFired)
        // {
        //     GameManager.instance.TryEndGame(false);
        // }
    }

}
