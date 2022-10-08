using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using U14;

public class UIText : MonoBehaviour
{
    [SerializeField, Tooltip("ui预制体")] private GameObject m_prefab;//预制体。
    [SerializeField, Tooltip("ui实例")] private GameObject m_obj;//实例。
    [SerializeField, Tooltip("目标对象,不填为当前对象")] private Transform m_target;//目标对象
    [SerializeField, Tooltip("相对于角色的偏移量")] private Vector3 m_offset;



    private Text m_text;
    // private TMPro.TextMeshProUGUI m_text;

    private Vector3 m_initScale;

    // Start is called before the first frame update
    void Start()
    {
        if (m_target == null)
        {
            m_target = this.transform;
        }
        if (m_obj == null && m_prefab != null)
        {
            m_obj = Instantiate(m_prefab, LayerManager.instance.TextCanvas.transform);
        }
        m_initScale = m_obj.transform.localScale;
        m_text = m_obj.GetComponent<Text>();
        // m_text = m_obj.GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_obj != null)
        {
            var screenPos = Camera.main.WorldToScreenPoint(m_target.position + m_offset);
            m_obj.transform.position = screenPos;
        }
    }

    public void ScaleText()
    {
        if (m_text.transform.localScale == m_initScale)
        {
            Tween.target(m_text.transform).toScale(m_initScale * 1.4f, 0.05f).toScale(m_initScale, 0.05f).start();
        }
    }

    public void UpdateText(string str, bool init = false)
    {
        if (m_text.text != str)
        {
            m_text.text = str;
            if (init == false) ScaleText();
        }
        // m_text.text = "";
    }

    public void UpdateText(int val, bool init = false)
    {
        string str = val.ToString();
        UpdateText(str, init);
    }

    public void Hide()
    {
        m_obj.SetActive(false);
    }

    public void Show()
    {
        m_obj.SetActive(true);
    }

    private void OnHideLvUI(U14.EventArgs evt)
    {
        bool isHide = (bool)evt.args[0];
        m_obj.SetActive(!isHide);
    }

    public void SelfDestroy()
    {
        Destroy(this.m_obj.gameObject);
    }
}
