using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingleTemplate<AudioManager>
{
    private const string RES_PATH = "Audios/";

    private GameObject m_audioRoot;

    private AudioSource m_player;

    private Dictionary<string, GameObject> m_effectPlayerNode = new Dictionary<string, GameObject>();//循环音效列表


    public AudioManager()
    {
        m_audioRoot = new GameObject("AudioRoot");
        m_player = m_audioRoot.AddComponent<AudioSource>();
        m_player.loop = true;

    }

    /// <summary>
    /// 背景音乐
    /// </summary>
    public void PlaySound(string audioName, float volume = 1.0f)
    {
        string oldName;
        if (m_player.clip == null)
        {
            oldName = "";
        }
        else
        {
            oldName = m_player.clip.name;
        }
        //判断是否正在播放
        if (oldName != audioName)
        {
            AudioClip clip = LoadClip(audioName);
            m_player.clip = clip;
            m_player.volume = volume;
            m_player.Play();
        }
    }

    public void PlayEffect(string audioName, float volume)
    {
        AudioClip clip = LoadClip(audioName);
        m_player.PlayOneShot(clip, volume);
    }

    public void PlayEffect(string audioName)
    {
        AudioClip clip = LoadClip(audioName);
        m_player.outputAudioMixerGroup = this.getAudioMix();
        m_player.PlayOneShot(clip);
    }

    public void PlayLoopEffect(string audioName, float volume = 1.0f)
    {
        GameObject playerNode;
        bool exist = m_effectPlayerNode.TryGetValue(audioName, out playerNode);
        AudioSource player = null;
        if (!exist)
        {
            GameObject soundsLayer = GameObject.Find("SoundsLoopLayer"); //脚本工具根节点
            if (!soundsLayer)
            {
                soundsLayer = new GameObject("SoundsLoopLayer"); //将脚本工具根节点添加到场景
                soundsLayer.transform.parent = m_audioRoot.transform;
            }

            playerNode = new GameObject(audioName);
            player = playerNode.AddComponent<AudioSource>();
            playerNode.transform.parent = soundsLayer.transform;
            m_effectPlayerNode.Add(audioName, playerNode);

        }
        else
        {
            player = playerNode.GetComponent<AudioSource>();
        }
        AudioClip clip = LoadClip(audioName);
        player.clip = clip;
        player.volume = volume;
        player.loop = true;
        player.Play();
    }

    public void StopLoopEffect(string audioName)
    {
        GameObject playerNode;
        bool exist = m_effectPlayerNode.TryGetValue(audioName, out playerNode);
        if (exist)
        {
            AudioSource player = playerNode.GetComponent<AudioSource>();
            player.Stop();
        }
    }

    public void ClearAllLoopEffect()
    {
        foreach (var node in m_effectPlayerNode)
        {
            AudioSource player = node.Value.GetComponent<AudioSource>();
            player.Stop();
            GameObject.Destroy(node.Value);
        }
        m_effectPlayerNode.Clear();
    }

    public void Clear()
    {
        ClearAllLoopEffect();
    }

    private AudioClip LoadClip(string audioName)
    {
        string path = RES_PATH + audioName;
        AudioClip clip = Resources.Load<AudioClip>(path);
        return clip;
    }



    private AudioMixerGroup getAudioMix()
    {
        string path = RES_PATH + "AM";
        AudioMixer am = Resources.Load<AudioMixer>(path);
        return am.FindMatchingGroups("Master")[0];
    }
}
