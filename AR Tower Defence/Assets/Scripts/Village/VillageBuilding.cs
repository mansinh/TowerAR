using UnityEngine;

/**
 * Adds to points rewarded per enemy slain / per time during the day
 * TODO: Villagers 
 *@ author Manny Kwong 
 */

public class VillageBuilding : Destroyable, ISelectable, IHoverable
{
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] Material mat_selected;
    [SerializeField] Material mat_normal;
    [SerializeField] float destroyedHeight = 0.1f;
    [SerializeField] HealEffect finishedEffect;
    [SerializeField] bool canSacrifice = true;

    public bool IsBuilt = true;

    protected override void Init()
    {
        base.Init();
        if (Points.Instance)
        {
            Points.Instance.UpdateVillagePoints();
        }
    }

    protected override void DamageEffects(Damage damage)
    {
        base.DamageEffects(damage);
        ShakeAnim.StartShake(0.01f, 0.3f, Vector3.zero);
        _view.transform.localScale = new Vector3(1, Health / MaxHealth,1);
        if (!IsBuilt)
        {
            if (Health >= MaxHealth)
            {
                FinishedBuilding(); 
            }
        }
        SetGameInfo();
    }

    public void SetHealthPercentage(float percentage)
    {
        Health = percentage * MaxHealth;
        ShakeAnim.StartShake(0.01f, 0.1f, Vector3.zero);
        _view.transform.localScale = new Vector3(1, Health / MaxHealth, 1);
    }

    protected override void Remove()
    {
        base.Remove();
        gameObject.SetActive(false);
        Points.Instance.UpdateVillagePoints();
    }

    public virtual void FinishedBuilding()
    {
        IsBuilt = true;
        finishedEffect.PlayEffects();
    }

    public virtual void OnUpgrade()
    {
        finishedEffect.PlayEffects();
    }


    public virtual bool Select()
    {
        SetMaterial(mat_selected);
        return canSacrifice;
    }

    public virtual void Deselect()
    {
        SetMaterial(mat_normal);
    }

    public virtual void UpdateSelected()
    {
       
    }

    public virtual void Destroy()
    {
        if (canSacrifice)
        {
            TriggerDeath();
        }
    }

    public virtual void OnHoverEnter()
    {
        SetGameInfo();
    }

    public virtual void OnHoverStay()
    {
        SetGameInfo();
    }

    public virtual void OnHoverLeave()
    {
        
    }

    public virtual ISelectable GetSelectable()
    {
        return this;
    }

    public virtual bool Use()
    {
        return true;
    }

    public virtual bool UseImmediately()
    {
        return false;
    }

    void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = material;
        }
    }

    public virtual void SetGameInfo()
    {
        GameInfo.Instance.SetHoverText(GetGameInfo(true));
    }

    public virtual string GetGameInfo(bool showState)
    {
        string s = "";

        if (showState)
        {
            s += " " + Health + "/" + MaxHealth + " HP";
        }
        
        return s;
    }
}
