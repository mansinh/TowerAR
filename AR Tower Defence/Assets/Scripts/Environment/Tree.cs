using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Destroyable, IGrowable
{
    [SerializeField] float _growth = 0;
    [SerializeField] Color _color;
    [SerializeField] MeshRenderer _meshRenderer;
    private Forest _forest;


    float _swayRandom;
    Tile tile;

    protected override void Init()
    {
        base.Init();
        _view.transform.localScale = Vector3.zero;
        _swayRandom = Random.value * Mathf.PI * 2;
        changeSwayDirection();
    }

    public void Grow(float growAmount)
    {
       
        if (_growth < 100)
        {
            _growth += growAmount;
        }

        MaxHealth = 1+10 * _growth / 100;
        Health = MaxHealth;

        UpdateView();
       
    }

    Vector3 swayDirection = new Vector3(1,0,0);

    protected override void UpdateView()
    {
        _view.transform.localScale = Vector3.one * _growth / 100;
        if (!_isSwaying)
        {
            _view.transform.localEulerAngles = 0.5f * Mathf.Sin(20 * Time.time) * swayDirection;
        }
        _meshRenderer.material.color = Color.Lerp(Color.black, _color, Health / MaxHealth);
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

    protected override void DamageEffects(Damage damage)
    {
        base.DamageEffects(damage);
       
    }

    protected override void Death()
    {
        base.Death();
    }

    protected override void Remove()
    {
        base.Remove();
        gameObject.SetActive(false);
    }
}
