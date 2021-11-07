using UnityEngine;

/**
 * Adds to points rewarded per enemy slain / per time during the day
 * TODO: Villagers 
 *@ author Manny Kwong 
 */

public class VillageBuilding : Destroyable, ISelectable, IHoverable
{
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
        //ShakeAnim.StartShake(0.01f, 0.3f, new Vector3(0, -(MaxHealth - Health) / MaxHealth, 0));
        ShakeAnim.StartShake(0.01f, 0.3f, new Vector3(0, 0, 0));
    }
    protected override void Remove()
    {
        base.Remove();
        gameObject.SetActive(false);
    }



    public virtual void Select()
    {
       
    }

    public virtual void Deselect()
    {
      
    }

    public virtual void UpdateSelected()
    {
       
    }

    public virtual void Destroy()
    {
        TriggerDeath();
    }

    public virtual void OnHoverEnter()
    {
       
    }

    public virtual void OnHoverStay()
    {
       
    }

    public virtual void OnHoverLeave()
    {
        
    }

    public virtual ISelectable GetSelectable()
    {
        return this;
    }

    public bool Use()
    {
        return true;
    }
}
