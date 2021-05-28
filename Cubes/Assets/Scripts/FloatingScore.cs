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

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        //StartCoroutine(SelfDestruct());

        //start = GetComponent<Transform>();
        //end = GetComponent<Transform>();

        //end.position = new Vector3(start.position.x, start.position.y, start.position.z);

        StartCoroutine(Fade());
    }

    public void UpdateFloatingScore(int newPoints)
    {
        text.SetText(""+ newPoints);
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
        //transform.position = Vector3.Lerp(transform.position, end.position + new Vector3(0,0.2f,0), lerptime * Time.deltaTime);
        //transform.localScale -= new Vector3(0.02f, 0.02f, 0.2f);
        this.transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

}
