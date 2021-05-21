using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeValue = 120f;
    TextMeshProUGUI textTimerUI;

    void Start()
    {
        textTimerUI = GetComponentInChildren<TextMeshProUGUI>();
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

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
            BoxManager.instance.GameFinished(1);
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        string time = string.Format("{0:00}:{1:00}",minutes,seconds);
        textTimerUI.SetText("Timer:"+time);
    }
}
