using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Test_Feel : MonoBehaviour
{
    MMF_Player player;
    private void Start()
    {
        player = GetComponent<MMF_Player>();
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    // void OnMouseUp()
    // {
    //     player.PlayFeedbacks();
    // }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            player.PlayFeedbacks();
        }
    }
}
