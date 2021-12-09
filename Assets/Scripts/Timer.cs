using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float m_MaxTime = 180f;

    [SerializeField]
    private Text m_DisplayText = null;

    private float m_CurrentTime;

    private bool m_IsTimerRunning = false;

    void Start()
    {
        m_CurrentTime = m_MaxTime;
        StartCoroutine(Countdown());

        IEnumerator Countdown()
        {
            for (int timer = 3; timer > -1; timer--)
            {
                yield return new WaitForSeconds(1f);
                m_DisplayText.text = timer.ToString();
            }
            m_IsTimerRunning = true;
        }
    }

    void Update()
    {
        if (!m_IsTimerRunning) return;

        m_CurrentTime -= Time.deltaTime;
        if (m_CurrentTime <= 0)
        {
            m_CurrentTime = 0;
            enabled = false;
        }

        m_DisplayText.text = m_CurrentTime.ToString("0#.000");
    }
}
