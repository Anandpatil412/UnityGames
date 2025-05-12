using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodText : MonoBehaviour
{

    TextMeshProUGUI textUI;
    private bool isRunning;
    public List<string> goodText = new List<string>();
    private int textIndex = 0;

    private void Start()
    {
        textUI = GetComponentInChildren<TextMeshProUGUI>();
        BoxManager.instance.showGoodText += InitShowText;
        textUI.enabled = false;
    }

    public void InitShowText()
    {
        if (!isRunning)
            StartCoroutine(Showtext());
    }

    IEnumerator Showtext()
    {
        textUI.enabled = true;
        isRunning = true;
        textIndex = UnityEngine.Random.Range(0, goodText.Count);
        textUI.SetText(goodText[textIndex]);

        yield return new WaitForSeconds(1.5f);

        isRunning = false;
        textUI.enabled = false;
    }

    void OnDisable()
    {
        BoxManager.instance.showGoodText -= InitShowText;
    }

}
