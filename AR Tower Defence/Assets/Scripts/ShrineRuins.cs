using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineRuins : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] MiracleController miracleController;
    [SerializeField] HealEffect activationEffect;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] Material mat_selected;
    [SerializeField] Material mat_normal;
    public void Deselect()
    {
        SetMaterial(mat_normal);
    }

    public ISelectable GetSelectable()
    {
        return this;
    }

    public void OnHoverEnter(){}
    public void OnHoverLeave(){}
    public void OnHoverStay(){}
    public void Destroy(){}

    public bool Select()
    {
        SetMaterial(mat_selected);
        return false;
    }

    public void UpdateSelected(){}

    //Return deselect when used
    public bool Use()
    {
        if (World.Instance.GetTile(transform.position).GetState() >= Tile.RESTORED)
        {
            if (!miracleController.gameObject.active)
            {
                activationEffect.PlayEffects();
                if (miracleController)
                {
                    miracleController.gameObject.SetActive(true);
                }
            }
        }
        return true;
    }

    void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = material;
        }
    }
}
