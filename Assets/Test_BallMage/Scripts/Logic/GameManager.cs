using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U14;
using System;
using UnityEngine.UI;

/// <summary>
/// 游戏主逻辑
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("对象实例")]
    [SerializeField, Tooltip("发射器")] private GameObject m_shotMachine;
    [SerializeField, Tooltip("环境")] private GameObject m_environment;

    [Header("脚本实例")]
    [SerializeField, Tooltip("虚拟相机节点")] private Src_CinemachineNode m_cinemachineNode;

    [Header("测试")]
    [Tooltip("启用测试发射角度")] public bool IsTestShot = false;
    [Tooltip("测试发射角度")] public Vector3 TestShotDirection = Vector3.zero;

    [Header("Debug")]
    public Text uiText;

    /// <summary> 镜头是否移动结束 </summary>
    [HideInInspector] public bool cameraMoveEnd = false;

    /// <summary> 是否已经发射 </summary>
    [HideInInspector] public bool isFired = false;

    /// <summary> 是否在引导中 </summary>
    [HideInInspector] public bool stopGuide = true;

    /// <summary> 游戏是否开始 </summary>
    [HideInInspector] public bool gameStart = false;

    /// <summary> 游戏是否结束 </summary>
    [HideInInspector] public bool gameEnd = false;

    /// <summary> 游戏是否胜利 </summary>
    [HideInInspector] public bool gameWin = false;

    /// <summary> 发射方向 </summary>
    [HideInInspector] public Vector3 shootDirector;

    // private int m_curLevel;
    /// <summary> 发射器 </summary>
    private Src_BaseShotMachine m_BaseShotMachine;

    /// <summary> 输入控制器 </summary>
    private InputController m_inputController;

    /// <summary> 游戏UI </summary>
    private Src_GameUI m_UIGame;

    /// <summary> 小球预制 </summary>
    private GameObject m_ballPrefab;

    /// <summary> 初始球数量 </summary>
    private int m_ballCount = 1;
    public void AddBallCount(int value)
    {
        m_ballCount += value;
    }

    /// <summary> 小球初始发射数量 </summary>
    private int m_initBallCount = 0;
    public int initBallCount
    {
        get { return m_initBallCount; }
    }


    /// <summary> 当前回合次数 </summary>
    public int roundCount = 0;


    private float physicsStep = 0.02f;
    private float m_timeScale;
    public float timeScale
    {
        get
        {
            return m_timeScale;
        }
    }

    /// <summary>加速触发器</summary>
    private float m_runTime;

    private bool m_isFirstBack = true;//是不是第一个返回的球

    private Vector3 m_shotMachineMoveTargetPos;//发射器移动到的目标位置

    private Vector3 m_shotMachineInitPos;



    private void Awake()
    {
        Print.Log("----> GameManager Awake <-------");
        instance = this;
#if UNITY_EDITOR
        Application.targetFrameRate = 60;//设置帧率
#endif
        Physics.gravity = GameData.instance.gravityValue;//设置重力值
        m_inputController = this.GetComponent<InputController>();
        m_UIGame = this.GetComponent<Src_GameUI>();

        m_shotMachineInitPos = m_shotMachine.transform.position;

        gameStart = false;
        m_cinemachineNode.Init();
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
    }

    /// <summary>
    /// 章节初始化
    /// </summary>
    public void InitChapter()
    {
        Print.Log("----> GameManager InitChapter <-------");
        Print.Log("Time.timeScale = " + Time.timeScale);
        m_timeScale = GameData.instance.gameTimeScale;
        Time.timeScale = m_timeScale;
        Physics.gravity = GameData.instance.gravityValue;//设置重力值
        // #if UNITY_EDITOR
        //         currentGameLevel = ChapterData.instance.currentGameLevel;
        // #endif
        m_ballCount = ChapterData.instance.curBallCount;
        roundCount = 0;

        InitChapterObject();

        if (m_BaseShotMachine.shotType == EShotMachineType.Touch_Shot)
        {
            shootDirector = Vector3.zero;
        }
        else
        {
            shootDirector = Vector3.forward;
        }

        cameraMoveEnd = false;
        m_UIGame.uiBallNumBg.SetActive(false);

        gameStart = true;

        if (gameStart)
        {
            //镜头移动
            InitUI();
        }
        else
        {
            m_UIGame.SetLevel();
        }

        StartCoroutine(StartLV());
    }

    /// <summary>
    /// 初始化关卡对象
    /// </summary>
    private void InitChapterObject()
    {
        //这environment初始化必须在 Src_BaseShotMachine这个之前
        GameObject environment = Instantiate(ChapterData.instance.environmentPrefab, m_environment.transform.parent);
        environment.transform.position = m_environment.transform.position;
        Destroy(m_environment);
        m_environment = environment;
        m_environment.GetComponent<Src_Environment>().Init();


        // RenderSettings.skybox = ChapterData.instance.matSky;

        GameObject shotMachine = Instantiate(ChapterData.instance.shotMachinePrefab, m_shotMachine.transform.parent);
        shotMachine.transform.position = m_shotMachineInitPos;
        m_BaseShotMachine = shotMachine.GetComponent<Src_BaseShotMachine>();
        Destroy(m_shotMachine);
        m_shotMachine = shotMachine;

        m_BaseShotMachine.Init();

        m_ballPrefab = ChapterData.instance.ballPrefab;
    }

    //关卡
    IEnumerator StartLV()
    {
        // m_curLevel = currentGameLevel - 1;
        Print.Log("game start");
        yield return null;
        InitRound();
    }

    public void Clear()
    {
        InitData();
    }

    public void MoveCamera()
    {
        m_cinemachineNode.CmOnCameraDollyTrack(m_environment.transform, () =>
        {
            Tween.delay(1, () =>
            {
                InitUI();
            }).start();
        });
    }

    private void InitUI()
    {
        m_inputController.Init(m_BaseShotMachine);
        cameraMoveEnd = true;
        // m_UIGame.uiBallNumBg.SetActive(true);
        // m_UIGame.BallUIPosUpdate(m_BaseShotMachine.rotateNode.gameObject, Vector3.zero, true);
        // m_UIGame.ShowGuideHand(true);
        m_cinemachineNode.MoveEnd();

        // EndGame(true);//测试结算界面
    }

    /// <summary>
    /// 初始化下一个回合
    /// </summary>
    public void InitRound()
    {
        Print.Log("----InitRound----");
        roundCount++;
        InitData();
        BallManager.ins.ClearUsingBullet();
        m_UIGame.Init();

        m_initBallCount = m_ballCount;
        m_ballCount = 0;

        if (m_BaseShotMachine.shotType == EShotMachineType.Touch_Shot)
        {
            shootDirector = Vector3.zero;
        }

        if (gameStart)
        {
            m_inputController.Init(m_BaseShotMachine);
        }

        UpdateTextUI();
    }

    private void InitData()
    {
        InitAction();
        isFired = false;
        stopGuide = true;
        gameEnd = false;
        m_isFirstBack = true;
        m_initBallCount = 0;
        m_advanceRatio = -1;
        m_shotMachineMoveTargetPos = Vector3.zero;
    }

    private void InitAction()
    {
        if (m_actShotCbk != null)
        {
            Tween.cancelDelay(m_actShotCbk);
            m_actShotCbk = null;
        }
    }

    public void UpdateTextUI()
    {
        m_UIGame.uiBallNum.text = "" + m_initBallCount;

        if (GameGlobal.isDebug)
        {
            uiText.text = "";
        }
    }

    public void Fire()
    {
        // FireGuide();
        // return;

        StopGuide();

        isFired = true;
        if (m_BaseShotMachine.shotType == EShotMachineType.Touch_Shot)
        {
            CreateBullets();
        }
        else
        {
            CreateBulletToTap();
        }

    }

    private int m_advanceRatio = -1;
    private void CreateBulletToTap()
    {
        if (m_advanceRatio < 0)
        {
            m_advanceRatio = BallManager.ins.bulletContainer.Count != 0 ? m_initBallCount / BallManager.ins.bulletContainer.Count : 1;
        }

        if (m_initBallCount > 0)
        {
            bool fromAdvance = (BallManager.ins.curUsingBullets.Count + 1) % m_advanceRatio == 0 ? true : false;
            AudioManager.Instance.PlayEffect(ESoundName.tower_shoot);
            CreateOneBullet(m_BaseShotMachine.lifePoint.position, shootDirector, GameData.instance.ballSpeed, false, false);
            m_initBallCount--;
            UpdateTextUI();
        }
    }

    private Action m_actShotCbk;
    private void CreateBullets()
    {
        m_BaseShotMachine.PlayShotAni();
        m_advanceRatio = BallManager.ins.bulletContainer.Count != 0 ? m_initBallCount / BallManager.ins.bulletContainer.Count : 1;
        m_actShotCbk = () =>
            {
                AudioManager.Instance.PlayEffect(ESoundName.tower_shoot);
                bool fromAdvance = (BallManager.ins.curUsingBullets.Count + 1) % m_advanceRatio == 0 ? true : false;
                CreateOneBullet(m_BaseShotMachine.lifePoint.position, shootDirector, GameData.instance.ballSpeed, false, fromAdvance);
                m_initBallCount--;
                if (m_initBallCount <= 0)
                {
                    Tween.cancelDelay(m_actShotCbk);
                    m_BaseShotMachine.StopShotAni();

                    Src_TouchShotMachine touchSM = m_BaseShotMachine as Src_TouchShotMachine;
                    //已经有球返回则移动炮塔
                    if (m_isFirstBack == false)
                    {
                        touchSM.MoveToTargetPos(m_shotMachineMoveTargetPos, () =>
                        {
                            TryEndGame();
                        });
                    }
                }
                UpdateTextUI();
            };
        Tween.delay(GameData.instance.ballShotDeltaTime, m_actShotCbk).loop(Tween.LoopType.ABC_ABC).start();
        m_actShotCbk();
    }

    /// <summary>
    /// 创建一个小球
    /// </summary>
    /// <param name="pos">初始位置</param>
    /// <param name="dir">初始运动方向</param>
    /// <param name="moveSpeed">初始速度</param>
    /// <param name="isAdvance">是否是创建预用子弹</param>
    /// <param name="fromAdvance">创建正式游戏子弹时是否优先使用预用子弹创建</param>
    /// <returns></returns>
    public GameObject CreateOneBullet(Vector3 pos, Vector3 dir, float moveSpeed, bool isAdvance = false, bool fromAdvance = false)
    {
        GameObject node = null;
        if (isAdvance)
        {
            node = BallManager.ins.CreateAdvanceBullet(this.m_ballPrefab);
        }
        else
        {
            node = BallManager.ins.CreateBullet(this.m_ballPrefab, isFired, fromAdvance);
        }

        Src_BaseBall srcBall = node.GetComponent<Src_BaseBall>();
        srcBall.Init(pos, dir, moveSpeed, isAdvance);

        if (isFired)
        {
            // AudioManager.Instance.PlayEffect("shoot");
            //播放发射动画
        }
        return node;
    }

    public void StopGuide()
    {
        // Print.Log("---------> StopGuide");
        if (m_BaseShotMachine.shotType == EShotMachineType.Touch_Shot)
        {
            StopCoroutine("IEFireGuide");
            if (m_guideBallCom)
            {
                m_guideBallCom.SelfDestroy();
                m_guideBallCom = null;
            }
            Physics.autoSimulation = true;

            stopGuide = true;
            GraphicsManager.instance.ClearFireGuide();
        }
        // else
        // {
        //     // (m_BaseShotMachine as Src_TapShotMachine).ShowGuide(false);
        // }
    }

    public void FireGuide()
    {
        if (m_BaseShotMachine.shotType == EShotMachineType.Touch_Shot)
        {
            StopGuide();

            int len = BallManager.ins.bulletContainer.Count;
            Src_BaseBall[] balls = new Src_BaseBall[len];
            for (int i = 0; i < len; i++)
            {
                Src_BaseBall ball = BallManager.ins.bulletContainer[i].GetComponent<Src_BaseBall>();
                ball.EnableKinematic();
                balls[i] = ball;
            }

            StartCoroutine("IEFireGuide");

            for (int i = 0; i < len; i++)
            {
                balls[i].EnableGravity();
            }
        }
        // else
        // {
        //     // (m_BaseShotMachine as Src_TapShotMachine).ShowGuide(true);
        // }

        m_UIGame.ShowGuideHand(false);
    }

    private Src_BaseBall m_guideBallCom;

    IEnumerator IEFireGuide()
    {
        // Print.Log("---------> FireGuide");
        stopGuide = false;
        // GraphicsManager.instance.ClearFireGuide();
        GameObject node = CreateOneBullet(m_BaseShotMachine.lifePoint.position, shootDirector, GameData.instance.ballSpeed);
        m_guideBallCom = node.GetComponent<Src_BaseBall>();
        m_guideBallCom.isSimulator = true;
        int totalCount = GraphicsManager.instance.TotalPointCount;
        int indexCount = 0;
        // Print.Log("FireGuide point = " + node.position);
        List<Vector3> posArray = new List<Vector3>();
        posArray.Add(node.transform.position);
        Physics.autoSimulation = false;
        while (posArray.Count < totalCount && stopGuide == false)
        {
            Physics.Simulate(physicsStep);
            Vector3 pos = node.transform.position;
            Print.Log("-----> FireGuide  pos = " + pos);
            if (stopGuide)
            {
                break;
            }
            if (indexCount % 2 == 0)
            {
                posArray.Add(pos);
            }
            // if (indexCount == totalCount)
            // {
            //     yield return null;
            // }
            indexCount++;
        }

        if (m_guideBallCom)
        {
            m_guideBallCom.SelfDestroy();
            m_guideBallCom = null;
        }

        Physics.autoSimulation = true;
        GraphicsManager.instance.DrawFireGuide(posArray.ToArray());
        stopGuide = true;
        yield return null;
    }

    private void FixedUpdate()
    {
        if (Physics.autoSimulation == false)
        {
            Physics.Simulate(physicsStep);
        }
    }

    private void Update()
    {
        m_cinemachineNode.CmUpdate();
        m_UIGame.SelfUpdate();

        m_UIGame.BallUIPosUpdate(m_BaseShotMachine.rotateNode.gameObject, Vector3.zero);
    }

    //////////////////////////////碰撞反馈/////////////////////////////
    public void OnCollideBlock(GameObject ballObj, GameObject blockObj, Vector3 hitPoint)
    {
        Src_BaseBall ball = ballObj.GetComponent<Src_BaseBall>();
        Src_BaseBlock block = blockObj.GetComponent<Src_BaseBlock>();
        block.subBlood(ball.attackPower);
        ball.hitBlockPlaySound();

        //碰撞特效
        var color = blockObj.GetComponentInChildren<MeshRenderer>().material.GetColor("_BaseColor");
        ParticleManager.Instance.PlayParticle(EParticleType.Cube, CommonPrefabData.instance.particleCube, blockObj.transform.position, color);
        Print.Log("subBlood OnCollideBlock！！");
        TryEndGame();
    }

    public void OnCollideBack(GameObject ballObj)
    {

        Src_BaseBall ball = ballObj.GetComponent<Src_BaseBall>();

        //点击发射方式 - 小球回到底部直接消失
        if (m_BaseShotMachine.shotType == EShotMachineType.Tap_Shot)
        {
            Print.Log("OnCollideBack --- 1");
            ball.SelfDestroy();
            m_ballCount++;
            TryEndGame();
            UpdateTextUI();
        }
        //触摸发射方式 - 发射器会移动到首个返回的小球位置
        else if (m_isFirstBack && m_BaseShotMachine.shotType == EShotMachineType.Touch_Shot)
        {
            Vector3 pos = ball.transform.position;

            Print.Log("OnCollideBack --- 2");
            m_isFirstBack = false;
            Src_TouchShotMachine touchSM = m_BaseShotMachine as Src_TouchShotMachine;

            ball.SelfDestroy();
            m_ballCount++;

            //球已发射完则移动炮塔
            if (m_initBallCount <= 0)
            {
                touchSM.MoveToTargetPos(pos, () =>
                {
                    TryEndGame();
                });
            }
            else
            {
                m_shotMachineMoveTargetPos = pos;
            }

            UpdateTextUI();
        }
        //触摸发射方式 - 非首个小球回到底部后会移动到发射器的位置
        else
        {
            Print.Log("OnCollideBack --- 3");
            Src_TouchShotMachine touchSM = m_BaseShotMachine as Src_TouchShotMachine;
            ball.MoveToTarget(
                touchSM.moveNode,
                () =>
                {
                    Print.Log("OnCollideBack --- 3 end");
                    ball.SelfDestroy();
                    m_ballCount++;
                    TryEndGame();
                    UpdateTextUI();
                });
        }
    }

    private void TryEndGame()
    {
        //小方块的行数超过最大行数


    }

    /////////////////////////////游戏结束///////////////////////////////
    public void EndGame(bool success)
    {
        // ReportUtil.OnLevelEnd(success, roundCount);
        gameEnd = true;
        gameWin = success;
        // m_UIGame.HideSpeedBtn();
        Tween.delay(0.5f, () =>
        {
            ShowEndUI(success);
        }).start();
    }

    public void ShowEndUI(bool success)
    {
        Physics.gravity = GameData.instance.gravityValueResult;//设置重力值
        if (success)
        {
            GameObject ui = Instantiate(CommonPrefabData.instance.uiGameSuccess, LayerManager.instance.UIParticleRootLayer);
            // MainGameScene.instance.EnterNextChapter();
        }
        else
        {
            // GameObject ui = Instantiate(CommonPrefabData.instance.uiGameFail, LayerManager.instance.UIPopLayer);
            MainGameScene.instance.ResetCurrentChapter();
        }
    }



    ///////////////////////////////事件监听/////////////////////////////









}
