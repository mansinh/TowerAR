using System.Collections;
using UnityEngine;

/**
 * Trees can grow from tiles when miracle water is cast over it
 * When an enemy collides with a tree it slows the enemy down and animates recoil
 * Blackens when damaged by fire 
 *@ author Manny Kwong 
 */

public class Tree : Destroyable, IGrowable
{
    [SerializeField] float growth = 0;
    [SerializeField] Color color = new Color(80,160,80,255);
    [SerializeField] MeshRenderer meshRenderer;
    private Forest _forest;
    private float _swayRandom;
    private Tile _tile;

    protected override void Init()
    {
        base.Init();
        _view.transform.localScale = Vector3.zero;
        _swayRandom = Random.value * Mathf.PI * 2;
        changeSwayDirection();
    }

    //Increase max health and size when growing
    public void Grow(float growAmount)
    {       
        if (growth < 100)
        {
            growth += growAmount;
        }

        MaxHealth = 1+10 * growth / 100;
        Health = MaxHealth;

        UpdateView();
    }

    Vector3 swayDirection = new Vector3(1,0,0);
    protected override void UpdateView()
    {
        _view.transform.localScale = Vector3.one * growth / 100;
        
        //Swaying side to side animation
        if (!_isSwaying)
        {
            _view.transform.localEulerAngles = 0.5f * Mathf.Sin(20 * Time.time) * swayDirection;
        }
        //Lerp between color at max health and black at 0 health
        meshRenderer.material.color = Color.Lerp(Color.black, color, Health / MaxHealth);
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
            //Recoil animation
            StartCoroutine(Sway(2, 5, 10));
        }
        if (other.GetComponent<Agent>()) {
            other.GetComponent<Agent>().Slow(0.1f,growth/100*0.5f);
        }
    }

    bool _isSwaying = false;
    IEnumerator Sway(float duration, float amplitude, float frequency)
    {
        _isSwaying = true;
        changeSwayDirection();

        //Dampened periodic rotation from base of tree
        for (float i = duration; i > 0;  i -= 0.01f)
        {
            _view.transform.localEulerAngles = i/duration* amplitude * Mathf.Sin(frequency * Time.time) * swayDirection;
            yield return new WaitForSeconds(0.01f);
        }
        _isSwaying = false;
    }

    //Randomize swaying direction
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
