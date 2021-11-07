using System.Collections;
using UnityEngine;

/**
 * Trees can grow from tiles when miracle water is cast over it
 * When an enemy collides with a tree it slows the enemy down and animates recoil
 * Blackens when damaged by fire 
 *@ author Manny Kwong 
 */

public class Tree : Destroyable, IGrowable, ISelectable, IHoverable
{
    [SerializeField] float growth = 0;
    [SerializeField] Color color = new Color(80,160,80,255);
    [SerializeField] MeshRenderer meshRenderer;

    private float _swayRandom;
    private Tile _tile;

    protected override void Init()
    {
        base.Init();
        _view.transform.localScale = Vector3.one*growth/100;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!_isSwaying)
        {
            //Recoil animation
            RecoilFrom(other.transform.position, 0.1f);
        }
        if (other.GetComponent<Agent>()) {
            other.GetComponent<Agent>().Slow(0.1f,growth/100*0.5f);
        }
    }

    void RecoilFrom(Vector3 hitPosition, float strength)
    {
        hitPosition.y = transform.position.y;
        swayDirection = -(hitPosition-transform.position).normalized;
        swayTime = 2;

        if (!_isSwaying)
        {
            StartCoroutine(Sway(2, strength, 10));
        }
    }

    bool _isSwaying = false;
    float swayTime = 0;
    IEnumerator Sway(float duration, float amplitude, float frequency)
    {
        _isSwaying = true;

        swayTime = duration;

        //Dampened periodic rotation from base of tree
        while(swayTime > 0)
        {
            Vector3 sway = swayTime / duration * amplitude * Mathf.Cos(frequency * (duration - swayTime)) * swayDirection;
   
            _view.up = sway+Vector3.up;
            swayTime -= 0.01f;
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
        Destroy(gameObject);
    }

    public void Select()
    {
        GetComponent<Collider>().enabled = false;
    }

    public void UpdateSelected()
    {
        Vector3 cursorPosition = MyCursor.Instance.GetCursorHit().point + Vector3.up / 20;
        RaycastHit hit;
        if (Physics.Raycast(cursorPosition, Vector3.down, out hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile)
            { 
                if (tile.GetState() >= 100)
                {
                    transform.position = cursorPosition + Vector3.up / 20;
                }
            }
        }
    }

    public void Deselect()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = hit.point;
               
        }
        RecoilFrom(Random.onUnitSphere, 0.1f);
        GetComponent<Collider>().enabled = true;
    }

    public void Destroy()
    {
        TriggerDeath();
    }
    public bool Use()
    {
        return true;
    }


    void OnHover()
    {
        Vector3 hitPositon = MyCursor.Instance.GetCursorHit().point;
        //_view.up = hitPositon - transform.position;
         RecoilFrom(hitPositon,0.1f);
        
    }

    public void OnHoverEnter()
    {
        OnHover();
    }

    public void OnHoverStay()
    {
        OnHover();
    }

    public void OnHoverLeave()
    {
        OnHover();
    }

    public ISelectable GetSelectable()
    {
        return this;
    }
}
