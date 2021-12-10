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

    [SerializeField]
    private Text m_CountdownText = null;

    [SerializeField]
    private GameObject m_CountdownContainer = null;

    private float m_CurrentTime;

    private bool m_IsTimerRunning = false;

    void Start()
    {
        m_CurrentTime = m_MaxTime;
        StartCoroutine(Countdown());

        IEnumerator Countdown()
        {
            m_CountdownContainer.SetActive(true);

            m_CountdownText.text = "READY?";
            yield return new WaitForSeconds(1f);
            m_CountdownText.text = "YEET!";
            yield return new WaitForSeconds(1f);

            m_IsTimerRunning = true;
            m_CountdownContainer.SetActive(false);
        }
    }

    void Update()
    {
        if (!m_IsTimerRunning) return;

        m_CurrentTime -= Time.deltaTime;
        if (m_CurrentTime <= 0)
        {
            m_CurrentTime = 0;
            m_IsTimerRunning = false;
            enabled = false;
        }

        int seconds = (int) m_CurrentTime % 60;
        int minutes = (int) m_CurrentTime / 60;
        m_DisplayText.text = $"{minutes}:{seconds.ToString("00")}";
    }
}
