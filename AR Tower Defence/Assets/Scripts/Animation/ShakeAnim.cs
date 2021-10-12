using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnim : MonoBehaviour
{

    float amplitude = 0.1f;
    float duration = 1f;
    Vector3 _originalPosition;

    public void StartShake(float amplitude, float duration, Vector3 originalPosition)
    {
        this.amplitude = amplitude;
        this.duration = duration;
        _originalPosition = originalPosition;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            transform.localPosition = _originalPosition + Random.insideUnitSphere * amplitude * transform.localScale.x;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localPosition = _originalPosition;
    }


}
