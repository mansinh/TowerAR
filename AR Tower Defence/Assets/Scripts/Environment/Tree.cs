using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IGrowable
{
    [SerializeField] float _growth = 0;
    [SerializeField] float _swayFrequency = 10f;
    [SerializeField] Transform _view;
    private Forest _forest;
    float _growSpeed = 0;

    float _swayRandom;
    Tile tile;

    private void Awake()
    {
        _view.transform.localScale = Vector3.zero;
        _swayRandom = Random.value * Mathf.PI * 2;
        changeSwayDirection();
    }

    public void Grow(float growSpeed)
    {
        _growSpeed = growSpeed;
        if (_growth < 100)
        {
            _growth += Time.deltaTime * growSpeed;
        }
        UpdateView();
        _growSpeed = 0;
    }

    Vector3 swayDirection = new Vector3(1,0,0);

    private void UpdateView()
    {
        _view.transform.localScale = Vector3.one * _growth / 100;
        if (!_isSwaying)
        {
            _view.transform.localEulerAngles = 0.5f * Mathf.Sin(20 * Time.time) * swayDirection;
        }

    }

    public void SetForest(Forest forest) {
        _forest = forest;
    }
    public Forest GetForest()
    {
        return _forest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isSwaying)
        {
            
            StartCoroutine(Sway(2, 5, 10));
        }
        if (other.GetComponent<Agent>()) {
            other.GetComponent<Agent>().Slow(0.1f,_growth/100*0.5f);
        }
    }


    bool _isSwaying = false;
    IEnumerator Sway(float duration, float amplitude, float frequency)
    {
        _isSwaying = true;
        changeSwayDirection();
        for (float i = duration; i > 0;  i -= 0.01f)
        {
            _view.transform.localEulerAngles = i/duration* amplitude * Mathf.Sin(frequency * Time.time) * swayDirection;
            yield return new WaitForSeconds(0.01f);
        }
        _isSwaying = false;
    }

    void changeSwayDirection()
    {
        swayDirection = Random.onUnitSphere;
        swayDirection.y = 0;
        swayDirection.Normalize();
    }
}
