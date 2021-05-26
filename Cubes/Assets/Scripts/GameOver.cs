using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    TextMeshProUGUI textUI;

    public void InitGameOver(int gameFinish,int points)
    {
        if(textUI == null)
            textUI = GetComponentInChildren<TextMeshProUGUI>();

        if(gameFinish == 1)
            textUI.SetText("Time Up! \n Your Score: " + points);
        else
            textUI.SetText("Your Score: " + points);

    }
}
