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
    }

    private void OnEnable()
    {
        BoxManager.onScoreAdded += UpdateScore;
    }

    private void OnDisable()
    {
        BoxManager.onScoreAdded -= UpdateScore;
    }

    public void UpdateScore()
    {
        textUI.SetText("Score: "+BoxManager.instance.points);
    }
}
