using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 输入控制器，发射玩家
/// </summary>
public class InputController : MonoBehaviour
{
    [SerializeField, Tooltip("炮塔的转动角度范围, 取值范围 0-360")]
    private float[] m_angleRange = { 5, 175 };

    [SerializeField, Tooltip("触摸失效阈值")]
    private float m_touchInvalid = 60;

    private Vector3 m_toScreenPos;

    private Vector2 m_touchPosition;

    private Src_BaseShotMachine m_baseShotMachine;

    private void Awake()
    {
        m_angleRange = GameData.instance.shotAngleRange;
        m_touchInvalid = GameData.instance.shotInvalidTouch;
    }

    public void Init(Src_BaseShotMachine baseShotMachine)
    {
        m_baseShotMachine = baseShotMachine;
        m_toScreenPos = Camera.main.WorldToScreenPoint(m_baseShotMachine.rotateNode.position);
        Print.Log("InputController m_toScreenPos = " + m_toScreenPos);
    }

    // Update is called once per frame
    void Update()
    {
        this.TryCheckTouch();
    }

    //触摸瞄准，松手发射
    void TryCheckTouch()
    {
        // Print.Log("TryCheckTouch = ");
        Vector2 tempPosition = Vector2.zero;
        bool isFire = false;
        if (!GameManager.instance.gameStart)
        {
            return;
        }
        if (GameManager.instance.gameEnd)
        {
            return;
        }
        if (!GameManager.instance.cameraMoveEnd)
        {
            return;
        }
        if (GameManager.instance.initBallCount <= 0)
        {
            return;
        }
        if (m_baseShotMachine.shotType == EShotMachineType.Touch_Shot && GameManager.instance.isFired)
        {
            return;
        }
        if (m_baseShotMachine.shotType == EShotMachineType.Tap_Shot && GameManager.instance.initBallCount == 0)
        {
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 禁用多点触控
            if (Input.touchCount == 1 && Tools.IsPointerOverUIObject(touch.position))
            {
                GameManager.instance.StopGuide();
                return;
            }

            // if (Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            // {
            //     return;
            // }

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    Print.Log(" Began ");
                    // Record initial touch position.
                    tempPosition = touch.position;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    Print.Log(" Moved ");
                    // Determine direction by comparing the current touch position with the initial one
                    tempPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    Print.Log(" Ended ");
                    // Report that the touch has ended when it ends
                    tempPosition = touch.position;
                    isFire = true;
                    break;
            }
            // Print.Log("----> touchPosition" + tempPosition);
        }
        else
        {
            // if (IsPointerOverUIObject(Input.mousePosition))
            // {
            //     return;
            // }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameManager.instance.StopGuide();
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Print.Log(" UP ");
                tempPosition = Input.mousePosition;
                isFire = true;
            }
            else if (Input.GetMouseButton(0))
            {
                Print.Log(" Move ");
                tempPosition = Input.mousePosition;
            }
        }


        if ((this.m_touchPosition == null) || (tempPosition != Vector2.zero && (tempPosition != this.m_touchPosition || isFire == true)))
        {
            Print.Log("----> tempPosition" + tempPosition);
            this.m_touchPosition = tempPosition;

            bool valid = RotationInputControllerPao(tempPosition);

            if (isFire && valid && GameManager.instance.shootDirector != Vector3.zero)
            {
                Print.Log("----> Fire ");
                if (m_baseShotMachine.shotType == EShotMachineType.Touch_Shot)
                {
                    GameManager.instance.Fire();
                }
            }
        }
    }

    /// <summary>
    /// 左右旋转发射器
    /// </summary>
    /// <param name="target"> 目标位置 </param>
    public bool RotationInputControllerPao(Vector3 target)
    {
        // var pos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 subVec = target - m_toScreenPos;//this.transform.position;
        Print.Log("********InputController TestShot Direction = " + subVec);

        if (GameManager.instance.IsTestShot)
        {
            subVec = GameManager.instance.TestShotDirection;
        }

        float radian = Mathf.Atan2(subVec.y, subVec.x);
        // Print.Log("********InputController radian = " + radian);

        float angle = radian * Mathf.Rad2Deg;
        // Print.Log("********InputController angle = " + angle);

        //34象限
        // angle = angle + 90;

        //12象限
        angle = 90 - angle;

        // Print.Log("********InputController angle2 = " + angle);

        var cAngle = angle + 90;
        Print.Log("cAngle = " + cAngle);

        if (cAngle < m_angleRange[0] + 360 - m_touchInvalid && cAngle > m_angleRange[1] + m_touchInvalid)
        {
            if (m_baseShotMachine.shotType == EShotMachineType.Touch_Shot)
            {
                GameManager.instance.shootDirector = Vector3.zero;
            }
            GameManager.instance.StopGuide();
            return false;
        }
        else
        {
            if (cAngle >= m_angleRange[0] && cAngle <= m_angleRange[1] && m_baseShotMachine.SetRotateBySubAngleY(angle))
            {
                GameManager.instance.shootDirector = new Vector3(subVec.x, 0, subVec.y).normalized;
                GameManager.instance.FireGuide();
            }
            return true;
        }
    }

}
