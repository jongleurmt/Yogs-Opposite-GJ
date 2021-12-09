using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private Color m_DeselectColor = Color.white;

    [SerializeField]
    private Color m_SelectColor = Color.black;

    private Text m_Text = null;

    void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
    }

    public void Select()
        => m_Text.color = m_SelectColor;

    public void Deselect()
        => m_Text.color = m_DeselectColor;
}
