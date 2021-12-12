using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969
public class LightFlicker : MonoBehaviour
{
    private Light m_Light;

    [SerializeField]
    private float m_MinIntensity = 0f;

    [SerializeField]
    private float m_MaxIntensity = 1f;

    [SerializeField, Range(1, 50)]
    private int m_Smoothing = 5;

    private Queue<float> m_SmoothQueue;
    private float m_LastSum = 0f;

    void Awake()
    {
        m_Light = GetComponent<Light>();
    }

    void Reset()
    {
        m_SmoothQueue.Clear();
        m_LastSum = 0f;
    }

    void Start()
    {
        m_SmoothQueue = new Queue<float>(m_Smoothing);
        
    }

    void Update()
    {
        while (m_SmoothQueue.Count >= m_Smoothing)
            m_LastSum -= m_SmoothQueue.Dequeue();
        
        float newVal = Random.Range(m_MinIntensity, m_MaxIntensity);
        m_SmoothQueue.Enqueue(newVal);
        m_LastSum += newVal;

        m_Light.intensity = m_LastSum / (float) m_SmoothQueue.Count;
    }
}
