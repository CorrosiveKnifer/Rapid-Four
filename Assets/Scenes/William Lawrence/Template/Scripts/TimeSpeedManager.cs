using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedManager : MonoBehaviour
{
    [Header("Current Values")]
    [ReadOnly] public float m_CurrentTimeSpeed = 1.0f;
    public int m_CurrentTimeSpeedIndex = 3;

    [Header("Time Motion")]
    public float[] m_AvailableTimeSpeeds = new float[]
    {
        0.01f,
        0.1f, 
        0.5f,
        1.0f
    };

    public float m_TimeLerpSpeed = 4.0f;

    [Header("UI Elements")]
    public Text m_TimeReadout;

    public Button m_TimeSlowerButton;
    public Button m_TimeFasterButton;

    public void ChangeTimeSpeed(int _dir)
    {
        m_CurrentTimeSpeedIndex += _dir;
    }

    public void SetTimeSpeed(int _index)
    {
        m_CurrentTimeSpeedIndex = _index;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_CurrentTimeSpeedIndex++;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_CurrentTimeSpeedIndex--;
        }
        m_CurrentTimeSpeedIndex = Mathf.Clamp(m_CurrentTimeSpeedIndex, 0, m_AvailableTimeSpeeds.Length - 1);

        m_TimeSlowerButton.interactable = m_CurrentTimeSpeedIndex != 0;
        m_TimeFasterButton.interactable = m_CurrentTimeSpeedIndex != m_AvailableTimeSpeeds.Length - 1;

        m_CurrentTimeSpeed = Mathf.Lerp(m_CurrentTimeSpeed, m_AvailableTimeSpeeds[m_CurrentTimeSpeedIndex], m_TimeLerpSpeed * Time.unscaledDeltaTime);
        Time.timeScale = m_CurrentTimeSpeed;

        m_TimeReadout.text = "- x" + m_CurrentTimeSpeed.ToString("F2") + " -";
    }
}
