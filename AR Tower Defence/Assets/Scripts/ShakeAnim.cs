using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnim : MonoBehaviour
{

    float amplitude = 0.1f;
    float duration = 1f;
    Vector3 originalPosition;

    public void StartShake(float amplitude, float duration)
    {
        this.amplitude = amplitude;
        this.duration = duration;
        originalPosition = transform.localPosition;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * amplitude * transform.localScale.x;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localPosition = originalPosition;
    }


}