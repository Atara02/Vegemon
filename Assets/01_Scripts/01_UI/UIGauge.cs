using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{

    private CanvasGroup m_group = null;
    private Image m_fill;
    private void Awake()
    {
        m_group = GetComponent<CanvasGroup>();
        m_group.blocksRaycasts = false;
        m_fill = transform.GetChild(0).GetComponent<Image>();
        m_fill.fillAmount = 0f;
    }

    public void SetGauge(float val)
    {
        m_fill.fillAmount = Mathf.Clamp01(val);
    }
    public void SetGauge(bool val)
    {
        m_group.alpha = val ? 1f : 0f;
    }
}
