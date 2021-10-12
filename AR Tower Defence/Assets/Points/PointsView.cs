using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Points))]
public class PointsView : MonoBehaviour
{
    [SerializeField] Text _pointsText;
    [SerializeField] float _deltaTime = 0.1f;
    int _currentPoints;
    int _targetPoints;
    bool isUpdatingText = false;

    public void UpdatePoints(int totalPoints) {
        _targetPoints = totalPoints;

        if (!isUpdatingText)
        {
            StartCoroutine(UpdatePointsText());
        }
    }

    IEnumerator UpdatePointsText() {
        isUpdatingText = true;
        while (_currentPoints < _targetPoints) {
            _currentPoints++;
            _pointsText.text = "" + _currentPoints + "p";
            yield return new WaitForSeconds(_deltaTime);
        }
        _currentPoints = _targetPoints;
        _pointsText.text = "" + _targetPoints + "p";
        isUpdatingText = false;
    }

}