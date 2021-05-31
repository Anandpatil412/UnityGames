using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour
{
    TextMeshPro text;
    public int newPoints;
    public float fadeDuration = 2f;
    public float speed = 2f;

    [SerializeField]
    private Color increaseScoreColor;
    [SerializeField]
    private Color decreaseScoreColor;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        StartCoroutine(Fade());
    }

    public void UpdateFloatingScore(int newPoints,bool isIncrease)
    {
        if(isIncrease)
        {
            text.faceColor = increaseScoreColor;
            text.SetText("+" + newPoints);
        }
        else
        {
            text.faceColor = decreaseScoreColor;
            text.SetText("-" + newPoints);
        }
    }

    IEnumerator Fade()
    {
        float speed = (float)1 / fadeDuration;
        Color c = text.color;

        for(float t = 0.0f; t < 1f; t += Time.deltaTime * speed)
        {
            c.a = Mathf.Lerp(1,0,t);
            text.color = c;
            yield return true;
        }

        Destroy(this.gameObject);

    }

    void Update()
    {
        this.transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

}
