using UnityEngine;

/**
 * Activates when all portals are destroyed. Fade in when activated.
 * TODO animations, effects? play cutscene?
 *@ author Manny Kwong 
 */

public class GameWon : MonoBehaviour
{
    public void Continue()
    {
        StartCoroutine(UITransitions.AlphaTo(GetComponent<CanvasGroup>(), 0, 0.3f));
    }
}
