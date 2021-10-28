using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UITransitions 
{
    public static IEnumerator AlphaTo(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        for (float i = duration; i > 0; i -= Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, 1-i / duration);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        if (targetAlpha == 0)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
