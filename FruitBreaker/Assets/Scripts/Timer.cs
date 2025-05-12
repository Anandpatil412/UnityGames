using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTime = 120f;

    private float timeValue;
    TextMeshProUGUI textTimerUI;
    private bool startTimeDown;

    void Start()
    {
        textTimerUI = GetComponentInChildren<TextMeshProUGUI>();
        BoxManager.instance.updateTimer += setTimer;
    }

    public void setTimer()
    {
        timeValue = maxTime;
    }

    void Update()
    {
        if((BoxManager.instance != null && BoxManager.instance.gameOver) || textTimerUI == null) return;

        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0f;
        }

        if(timeValue <= 10 && !startTimeDown)
        {
            startTimeDown = true;
            SoundManager.PlayLoopSound(SoundManager.Sound.Timeout);
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;

            BoxManager.instance.GameFinished(1);
            timeValue = maxTime;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        string time = string.Format("{0:00}:{1:00}",minutes,seconds);
        textTimerUI.SetText(time);
    }


    void OnDisable()
    {
        BoxManager.instance.updateTimer -= setTimer;
    }
}
