// using UnityEngine;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// public class ReportManager
// {
//     /// <summary> 埋点初始化 </summary>
//     public static void Init()
//     {

// #if UNITY_ANDROID && !UNITY_EDITOR
//             var instance = Tenjin.getInstance("$TenjinKey");
//             instance.SetAppStoreType(AppStoreType.googleplay);
//             instance.Connect();
//             GameAnalyticsSDK.GameAnalytics.Initialize();
// #endif

//     }

//     /// <summary> 开始关卡 </summary>
//     public static void reportBegin(string lv)
//     {
//         Debug.LogWarning($"ReportManager Begin lv= {lv}");
//         TjMissionApi.Begin(lv);

//     }

//     /// <summary> 关卡胜利 </summary>
//     public static void reportSuccess(string lv)
//     {
//         Debug.LogWarning($"ReportManager Success lv= {lv}");
//         TjMissionApi.End(lv);

//     }

//     // /// <summary> 章节胜利 </summary>
//     // public static reportChapterSuccess()
//     // {

//     // }

//     /// <summary> 失败 </summary>
//     public static void reportFail(string lv)
//     {
//         Debug.LogWarning($"ReportManager Fail lv= {lv}");
//         TjMissionApi.Fail(lv);
//     }



// }