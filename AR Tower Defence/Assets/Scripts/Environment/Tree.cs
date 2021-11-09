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
        if (growth < 20)
        {
            growth = 20;
        }
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

    public float GetGrowth()
    {
        return growth;
    }
    public bool GetIsFullyGrown()
    {
        return growth >= 100;
    }

    Vector3 swayDirection = new Vector3(1,0,0);
    protected override void UpdateView()
    {
        if (!isGrowing)
        {
            StartCoroutine(GrowTo());
        }
        
        //Swaying side to side animation
        if (!_isSwaying)
        {
            StartCoroutine(Sway(1, 0.02f, 10));
        }
        //Lerp between color at max health and black at 0 health
        meshRenderer.material.color = Color.Lerp(Color.black, color, Health / MaxHealth);
    }

    bool isGrowing = false;
    IEnumerator GrowTo()
    {
        isGrowing = true;
        while (_view.transform.localScale.x < growth / 100)
        {
            _view.transform.localScale += Vector3.one/100;
            yield return new WaitForSeconds(0.05f);
        }
        isGrowing = false;
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
        swayTime = 1;

        if (!_isSwaying)
        {
            StartCoroutine(Sway(swayTime, strength, 20));
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
            Vector3 eulerAngles = _view.eulerAngles;
            eulerAngles.y = 0;
            _view.eulerAngles = eulerAngles;
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
        Lumberyard lumberyard = FindObjectOfType<Lumberyard>();
        if (lumberyard)
        {
            lumberyard.ShowArrow();
        }

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
            Lumberyard lumberyard = hit.collider.GetComponent<Lumberyard>();
            if (lumberyard)
            {
                transform.position = cursorPosition + Vector3.up / 20;
            }
        }
    }

    public void Deselect()
    {
        Lumberyard lumberyard = FindObjectOfType<Lumberyard>();
        if (lumberyard)
        {
            lumberyard.HideArrow();
        }

        float height = World.Instance.GetTile(transform.position).GetTop().y;
        Vector3 position = transform.position;
        position.y = height;
        transform.position = position;
        RecoilFrom(Random.onUnitSphere, 0.1f);
        GetComponent<Collider>().enabled = true;

        Vector3 cursorPosition = MyCursor.Instance.GetCursorHit().point + Vector3.up / 20;
        RaycastHit hit;
        if (Physics.Raycast(cursorPosition, Vector3.down, out hit))
        {
            lumberyard = hit.collider.GetComponent<Lumberyard>();
            if (lumberyard)
            {
                if (lumberyard.TreeToWood(this))
                {
                    TriggerDeath();
                }           
            }
        }      
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
        if (!IsDestroyed)
        {
            Vector3 hitPositon = MyCursor.Instance.GetCursorHit().point;
            //_view.up = hitPositon - transform.position;
            RecoilFrom(hitPositon, 0.1f);
        }
        
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
