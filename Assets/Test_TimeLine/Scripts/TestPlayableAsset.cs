using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class TestPlayableAsset : PlayableAsset
{
    public ExposedReference<Text> targetText;
    public string dialogStr;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        TestPlayableBehaviour playable = new TestPlayableBehaviour();
        //场景资源引用，所以playable.targetText==Asset.TimeLine上面选择的targetText
        playable.targetText = targetText.Resolve(graph.GetResolver());
        //数据类型直接赋值
        playable.dialogStr = dialogStr;

        return ScriptPlayable<TestPlayableBehaviour>.Create(graph, playable);
        // return Playable.Create(graph);
    }
}
