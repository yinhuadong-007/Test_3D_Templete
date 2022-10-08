using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EParticleType
{
    Bomb,
    Cube,
}

public class ParticleManager : SingleTemplate<ParticleManager>
{
    private const string RES_PATH = "Particles/";

    private Dictionary<EParticleType, List<GameObject>> m_ParticlePool = new Dictionary<EParticleType, List<GameObject>>();
    private Dictionary<EParticleType, Vector3> m_ParticleScalePool = new Dictionary<EParticleType, Vector3>();
    private Dictionary<EParticleType, GameObject> m_ParticleParentPool = new Dictionary<EParticleType, GameObject>();

    private int m_curCubeParticleCount = 0;
    private int m_curBombParticleCount = 0;

    public ParticleManager()
    {

    }


    public void Clear()
    {
        m_ParticlePool.Clear();
        m_ParticleScalePool.Clear();
        m_ParticleParentPool.Clear();
    }

    /// <summary>
    /// 播放一个对象池中的粒子，如果对象池没有可用的实例，则实例化一个粒子
    /// </summary>
    /// <param name="type">粒子类型</param>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="changeColor"></param>
    public void PlayParticle(EParticleType type, GameObject obj, Vector3 pos, Color changeColor)
    {
        GameObject particle = null;
        if (m_curCubeParticleCount > 60) return;
        if (m_curBombParticleCount > 60) return;

        if (m_ParticlePool.ContainsKey(type) && m_ParticlePool[type].Count > 0)
        {
            particle = m_ParticlePool[type][0];
            m_ParticlePool[type].RemoveAt(0);
        }
        else if (obj == null)
        {
            Print.LogError("PlayParticle obj is null!");
            return;
        }
        else
        {
            GameObject typeRoot = null;
            if (!m_ParticleParentPool.ContainsKey(type))
            {
                typeRoot = new GameObject(type.ToString());
                typeRoot.transform.parent = LayerManager.instance.SkillLayer;
                m_ParticleParentPool.Add(type, typeRoot);
            }
            else
            {
                typeRoot = m_ParticleParentPool[type];
            }

            particle = GameObject.Instantiate(obj, typeRoot.transform);
        }
        particle.SetActive(true);
        particle.transform.position = pos;

        var ani = particle.GetComponent<ParticleSystem>();
        // ani.Simulate(0.0f);

        if (type == EParticleType.Cube)
        {
            m_curCubeParticleCount++;
        }
        else if (type == EParticleType.Bomb)
        {
            m_curBombParticleCount++;
        }

        ani.Play();
        Tween.delay(ani.main.duration / ani.main.simulationSpeed, () =>
        {
            if (type == EParticleType.Cube)
            {
                m_curCubeParticleCount--;
            }
            else if (type == EParticleType.Bomb)
            {
                m_curBombParticleCount--;
            }
            particle.SetActive(false);
            if (!m_ParticlePool.ContainsKey(type))
            {
                m_ParticlePool.Add(type, new List<GameObject>());
            }
            m_ParticlePool[type].Add(particle);
        }).start();

        ParticleSystemRenderer[] p_renders = particle.GetComponentsInChildren<ParticleSystemRenderer>();
        for (int i = 0, len = p_renders.Length; i < len; i++)
        {
            var item = p_renders[i];
            item.material.SetColor("_BaseColor", changeColor);
        }

        return;
    }

    /// <summary>
    /// 播放一个对象池中的粒子，如果对象池没有可用的实例，则实例化一个粒子
    /// </summary>
    /// <param name="type">粒子类型</param>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="destoryTime"></param>
    public GameObject PlayParticle(EParticleType type, GameObject obj, Vector3 pos, float scale = 1)
    {
        GameObject particle = null;

        if (m_ParticlePool.ContainsKey(type) && m_ParticlePool[type].Count > 0)
        {
            particle = m_ParticlePool[type][0];
            m_ParticlePool[type].RemoveAt(0);
        }
        else if (obj == null)
        {
            Print.LogError("PlayParticle obj is null!");
            return particle;
        }
        else
        {
            GameObject typeRoot = null;
            if (!m_ParticleParentPool.ContainsKey(type))
            {
                typeRoot = new GameObject(type.ToString());
                typeRoot.transform.parent = LayerManager.instance.SkillLayer;
                m_ParticleParentPool.Add(type, typeRoot);
            }
            else
            {
                typeRoot = m_ParticleParentPool[type];
            }

            particle = GameObject.Instantiate(obj, typeRoot.transform);

            if (!m_ParticleScalePool.ContainsKey(type))
            {
                m_ParticleScalePool.Add(type, particle.transform.localScale);
            }
        }
        particle.SetActive(true);
        particle.transform.position = pos;
        particle.transform.localScale = m_ParticleScalePool[type] * scale;

        var ani = particle.GetComponent<ParticleSystem>();
        // ani.Simulate(0.0f);
        ani.Play();
        Tween.delay(ani.main.duration, () =>
        {
            particle.SetActive(false);
            if (!m_ParticlePool.ContainsKey(type))
            {
                m_ParticlePool.Add(type, new List<GameObject>());
            }
            m_ParticlePool[type].Add(particle);
        }).start();

        return particle;
    }

    /// <summary>
    /// 播放一个在存在的粒子的复制
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="destroyTime"></param>
    public void PlayParticle(GameObject obj, Vector3 pos, float destroyTime = 2.0f)
    {
        if (obj == null)
        {
            Print.LogError("PlayParticle obj is null!");
            return;
        }
        GameObject particle = GameObject.Instantiate(obj, LayerManager.instance.SkillLayer);
        particle.transform.position = pos;
        var ani = particle.GetComponent<ParticleSystem>();
        ani.Play();
        GameObject.Destroy(particle, destroyTime);
    }

    /// <summary>
    /// 播放一个在存在的粒子的复制
    /// </summary>
    /// <param name="obj">粒子预制</param>
    /// <param name="parent">父节点</param>
    /// <param name="pos">位置</param>
    /// <param name="destroyTime"></param>
    public void PlayParticle(GameObject obj, Transform parent, Vector3 pos, float destroyTime = 2.0f)
    {
        if (obj == null)
        {
            Print.LogError("PlayParticle obj is null!");
            return;
        }
        if (parent == null)
        {
            parent = LayerManager.instance.SkillLayer;
        }
        GameObject particle = GameObject.Instantiate(obj, parent);
        particle.transform.localPosition = pos;
        var ani = particle.GetComponent<ParticleSystem>();
        ani.Play();
        GameObject.Destroy(particle, destroyTime);
    }

    /// <summary>
    /// 播放一个在Resource下的粒子
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="destroyTime"></param>
    public void PlayParticle(string name, Vector3 pos, float destroyTime = 2.0f)
    {
        GameObject obj = LoadParticle(name);
        GameObject particle = GameObject.Instantiate(obj, LayerManager.instance.SkillLayer);
        particle.transform.position = pos;
        var ani = particle.GetComponent<ParticleSystem>();
        ani.Play();
        GameObject.Destroy(particle, destroyTime);
    }

    private GameObject LoadParticle(string name)
    {
        Print.Log("-----> ParticleManager LoadParticle name = " + name);
        string path = RES_PATH + name;
        GameObject obj = Resources.Load<GameObject>(path);
        return obj;
    }




}
