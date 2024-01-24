using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class MenuItem_Test
{

    [MenuItem("自定义工具/层级收纳/全部展开")]
    static void foldSelection()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        var hierarchyWindow = EditorWindow.focusedWindow;
        var expandMethodInfo = hierarchyWindow.GetType().GetMethod("SetExpandedRecursive");
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            expandMethodInfo.Invoke(hierarchyWindow, new object[] { root.GetInstanceID(), true });
        }
    }

    [MenuItem("自定义工具/层级收纳/全部折叠")]
    static void UnfoldSelection()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        var hierarchyWindow = EditorWindow.focusedWindow;
        var expandMethodInfo = hierarchyWindow.GetType().GetMethod("SetExpandedRecursive");
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            expandMethodInfo.Invoke(hierarchyWindow, new object[] { root.GetInstanceID(), false });
        }
    }

}

public class FileWindow : EditorWindow
{
    // 精确匹配
    bool isMatchExactly = false;
    // 临时的计数器
    int counter = 0;
    // 记录需要检查的节点名称
    string findNodeName = "";
    // 记录需要检查的组件名称
    string findComponentName = "";

    [MenuItem("自定义工具/资源检查")]
    static void Open()
    {
        FileWindow window = GetWindow<FileWindow>();
        window.titleContent = new GUIContent("资源检查");
        window.Show();
    }

    private void OnGUI()
    {
        findNodeName = EditorGUILayout.TextField("查询节点名称", findNodeName);

        if(GUILayout.Button("模糊匹配查询"))
        {
            isMatchExactly = false;
            ConsoleCleaner();
            CheckfindNode();
        }
        if(GUILayout.Button("精确匹配查询"))
        {
            isMatchExactly = true;
            ConsoleCleaner();
            CheckfindNode();
        }

        findComponentName = EditorGUILayout.TextField("查询组件名称", findComponentName);
        if(GUILayout.Button("组件查询"))
        {
            ConsoleCleaner();
            CheckfindComponent();
        }

        EditorGUILayout.Space();
        GUILayout.Label("----------------------------------------");
        GUILayout.Label(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        GUILayout.Label("----------------------------------------");

    }
    /// <summary>
    /// 检查节点
    /// </summary>
    void CheckfindNode()
    {
        // 异常处理
        if (findNodeName == "")
        {
            Debug.LogError("Error1: 没有输入查询节点名称!!!");
            return;
        }
        if (Selection.count == 0)
        {
            Debug.LogError("Error2: 没有选中查询节点的物体!!!");
            return;
        }

        counter = 0;
        for (int i = 0 ; i < Selection.count ; i++)
        {
            Transform rootTs = Selection.gameObjects[i].transform;
            nameJudge(rootTs , findNodeName);
            foreach (Transform childTs in rootTs)
            {
                nameJudge(childTs , findNodeName);
                foreach (Transform item in childTs)
                {
                    nameJudge(item , findNodeName);
                }
            }
        }

        if (counter <= 0)
        {
            Debug.LogError("Error3: 没有找到对应节点!!!");
        }
        else
        {
            Debug.Log("找到 " + counter + " 个匹配节点");
        }

    }
    // 根据不同的匹配方式进行判断
    void nameJudge(Transform ts , string findNodeName)
    {
        // 精确匹配
        if (isMatchExactly && ts.name == findNodeName)
        {
            counter++;
            Debug.Log(ts.name , ts.gameObject);
        }
        // 模糊匹配
        if (!isMatchExactly && ts.name.Contains(findNodeName))
        {
            counter++;
            Debug.Log(ts.name , ts.gameObject);
        }
    }

    /// <summary>
    /// 检查组件
    /// </summary>
    void CheckfindComponent()
    {
        // 异常处理
        if (findComponentName == "")
        {
            Debug.LogError("Error1: 没有输入查询组件名称!!!");
            return;
        }
        if (Selection.count == 0)
        {
            Debug.LogError("Error2: 没有选中查询组件的物体!!!");
            return;
        }

        // // 未实现System.String向UnityEngine.Component的转换
        // var t = (Component)Convert.ChangeType(findComponentName , typeof(Component));

        counter = 0;
        for (int i = 0 ; i < Selection.count ; i++)
        {
            Transform rootTs = Selection.gameObjects[i].transform;
            componentJudge(rootTs);
            foreach (Transform childTs in rootTs)
            {
                componentJudge(childTs);
                foreach (Transform item in childTs)
                {
                    componentJudge(item);
                }
            }
        }

        if (counter <= 0)
        {
            Debug.LogError("Error3: 没有找到对应组件!!!");
        }
        else
        {
            Debug.Log("找到 " + counter + " 个匹配组件");
        }

    }
    // GetComponent内置字符串匹配方法
    void componentJudge(Transform ts)
    {
        var component = ts.GetComponent(findComponentName);
        if (component != null)
        {
            counter++;
            Debug.Log(component.transform.name , component.transform.gameObject);
        }
    }
    // // 转换类型
    // T ChangeType<T>(object value) where T : Component
    // {
    //     return (T)Convert.ChangeType(value , typeof(T));
    // }



    /// <summary>
    /// 清空控制台
    /// </summary>
    void ConsoleCleaner()
    {
        #if UNITY_EDITOR
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
        System.Reflection.MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);
        #endif
    }


}

