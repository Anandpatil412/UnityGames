using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour
{
    TextMeshPro text;
    public int newPoints;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        StartCoroutine(SelfDestruct());

        start = GetComponent<Transform>();
        end = GetComponent<Transform>();

        end.position = new Vector3(start.position.x, start.position.y, start.position.z);
    }

    public void UpdateFloatingScore(int newPoints)
    {
        text.SetText(""+ newPoints);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


    float timeElapsed;
    float lerpDuration = 10f;
    Transform start;
    Transform end;
    float lerptime = 10f;

    float valueToLerp;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, end.position + new Vector3(0,0.2f,0), lerptime * Time.deltaTime);
        transform.localScale -= new Vector3(0.02f, 0.02f, 0.2f);
    }

}
