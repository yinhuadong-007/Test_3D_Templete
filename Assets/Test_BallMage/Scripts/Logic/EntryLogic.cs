using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class EntryLogic : MonoBehaviour
{
    [Tooltip("GM 层")] public GameObject m_GMLayer;
    [Tooltip("loading 层")] public Src_LoadingUI m_loadingUI;

    [Header("Debug")]
    [SerializeField] private bool m_isDebug;
    private void Awake()
    {
        Print.Log("Entry Game");
        GameGlobal.isDebug = m_isDebug;
        FpsText.show = GameGlobal.isDebug;
        Print.isDebug = GameGlobal.isDebug;

        // Application.targetFramm_isDebugeRate = 60;//设置帧率

        // GameGlobal.GetInstance().loadSceneModel = loadSceneModel;
        // GameGlobal.GetInstance().sceneNameArray = sceneNameArray;
    }

    // Start is called before the first frame update
    void Start()
    {
        // TinySauce.OnGameStarted();
        // ReportPage.OnPageShow("启动页");

        // #if UNITY_ANDROID && !UNITY_EDITOR
        //         var instance = Tenjin.getInstance("EYOE4HXDX697SRMYBSZAY3VJI7UYWQRN");
        //         instance.SetAppStoreType(AppStoreType.googleplay);
        //         instance.Connect();
        //         GameAnalyticsSDK.GameAnalytics.Initialize();
        // #endif

        if (GameGlobal.isDebug)
        {
            m_GMLayer.SetActive(true);
            DontDestroyOnLoad(m_GMLayer);
        }
        else
        {
            m_GMLayer.SetActive(false);
        }

        DontDestroyOnLoad(m_loadingUI.gameObject);

        m_loadingUI.Init(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }


}
