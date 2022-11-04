using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ColorPlayableAsset : PlayableAsset
{
    public ExposedReference<MeshRenderer> targetMr;
    public Color color;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        ColorPlayableBehaviour playable = new ColorPlayableBehaviour();
        //场景资源引用，所以playable.targetText==Asset.TimeLine上面选择的targetText
        playable.targetMr = targetMr.Resolve(graph.GetResolver());
        //数据类型直接赋值
        playable.color = color;

        return ScriptPlayable<ColorPlayableBehaviour>.Create(graph, playable);
        // return Playable.Create(graph);
    }
}
