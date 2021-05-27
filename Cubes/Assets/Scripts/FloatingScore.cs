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
    }

    public void UpdateFloatingScore(int newPoints)
    {
        text.SetText(""+ newPoints);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }


    //float timeElapsed;
    //float lerpDuration = 3;

    //float valueToLerp;

    //void Update()
    //{
    //    if(timeElapsed < lerpDuration)
    //    {
    //         = Mathf.Lerp(transform.position.y, transform.position.y + 1, timeElapsed / lerpDuration);
    //        timeElapsed += Time.deltaTime;
    //    }
    //}

}
