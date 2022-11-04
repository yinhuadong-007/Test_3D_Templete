using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class ColorPlayableBehaviour : PlayableBehaviour
{
    public MeshRenderer targetMr;
    public Color color;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("OnGraphStart: 一开始运行就调用");
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        Debug.Log("OnGraphStop: 停止时调用");
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log("OnBehaviourPlay: 触发到Assets时调用");
        // targetMr.material.SetColor("_EmissionColor", color);
        targetMr.material.SetColor("_Color", color);
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        Debug.Log("OnBehaviourPause: Assets中暂停");
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        Debug.Log("PrepareFrame: Assets中每帧调用");
    }
}
