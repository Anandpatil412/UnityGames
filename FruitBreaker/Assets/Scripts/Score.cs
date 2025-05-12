using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TextMeshProUGUI textUI;

    private void Start()
    {
        textUI = GetComponentInChildren<TextMeshProUGUI>();
        BoxManager.instance.addScore += UpdateScore;
    }

    public void UpdateScore(int points)
    {
        textUI.SetText("Score: "+points);
    }

    void OnDisable()
    {
        BoxManager.instance.addScore -= UpdateScore;
    }

}
