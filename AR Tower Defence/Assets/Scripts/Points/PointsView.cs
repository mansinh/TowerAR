using System.Collections;
using UnityEngine;
using TMPro;

/**
 * Shows the points on the screen and maybe some animation when points change
 *  //TODO sounds for changing points
 *@ author Manny Kwong 
 */

[RequireComponent(typeof(Points))]
public class PointsView : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private float deltaTime = 0.1f; //time between frames for animation
    private int _currentPoints;
    private int _targetPoints;
    private int _maxPoints;
    private bool _isUpdatingText = false;

    public void SetPoints(int points, int maxPoints) {
        _currentPoints = points;
        _targetPoints = points;
        _maxPoints = maxPoints;
        SetText();
    }

    public void UpdatePoints(int points, int maxPoints) {
        _targetPoints = points;
        _maxPoints = maxPoints;

        //If not already animating, start the points animation
        if (!_isUpdatingText)
        {
            StartCoroutine(PointsAnimation());
        }
    }

    //When points are increased, increase points from current to target value over time
    //TODO more animation effects, maybe changing colour or glowing
    IEnumerator PointsAnimation() {
        _isUpdatingText = true;
        while (_currentPoints < _targetPoints) {
            _currentPoints++;
            SetText();
            yield return new WaitForSeconds(deltaTime);
        }
        _currentPoints = _targetPoints;
        SetText();
        _isUpdatingText = false;
    }

    void SetText() {
        pointsText.text = "" + _currentPoints + "/"+ _maxPoints + " p";
    }
}
