using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;


public class Src_CinemachineNode : MonoBehaviour
{
    [SerializeField, Tooltip("主相机")] public Transform MainCamera;
    [SerializeField, Tooltip("轨道相机")] public Transform cm_vcamDollyTrack;

    private Cinemachine.CinemachineTrackedDolly cm_track;
    private Cinemachine.CinemachineVirtualCamera cm_dollyCam;

    private float cm_distance;
    private Action m_actCmDollyMoveEnd;


    public void Init()
    {
        if (cm_track == null)
        {
            cm_track = cm_vcamDollyTrack.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>();
            cm_dollyCam = cm_vcamDollyTrack.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }

        cm_track.m_PathPosition = 0;
    }

    public void MoveEnd()
    {
        cm_dollyCam.LookAt = null;
    }

    /// <summary>
    /// 启用轨道相机
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetEnemy"></param>
    public void CmOnCameraDollyTrack(Transform lookNode, Action cbk)
    {
        CinemachineBlendDefinition a = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, GameData.instance.cameraDollyRunTime);
        MainCamera.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend = a;

        cm_dollyCam.LookAt = lookNode;
        cm_track.m_PathPosition = 0;

        cm_distance = 1 / GameData.instance.cameraDollyRunTime;
        cm_vcamDollyTrack.gameObject.SetActive(true);

        m_actCmDollyMoveEnd = cbk;
    }

    /// <summary>
    /// 每帧刷新
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetEnemy"></param>
    /// <param name="m_attackCameraEnd"></param>
    /// <param name="playerFlay"></param>
    public void CmUpdate()
    {
        if (m_actCmDollyMoveEnd != null && cm_vcamDollyTrack.gameObject.activeSelf)
        {
            cm_track.m_PathPosition += Time.deltaTime * cm_distance;

            if (cm_track.m_PathPosition >= 1)
            {
                cm_track.m_PathPosition = 1;

                m_actCmDollyMoveEnd();
                m_actCmDollyMoveEnd = null;
            }
        }
    }
}
