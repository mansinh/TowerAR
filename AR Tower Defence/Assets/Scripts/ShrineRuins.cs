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
    [SerializeField] SpriteRenderer symbol;
    [SerializeField] string deckType;

    void Awake()
    {
        miracleController = GameObject.Find(deckType).GetComponent<MiracleController>();
        miracleController.gameObject.SetActive(false);
    }

    public void Deselect()
    {
        SetMaterial(mat_normal);
    }

    public ISelectable GetSelectable()
    {
        if (!miracleController.gameObject.active)
        {
            return this;
        }
        return null;
    }

    public void OnHoverEnter()
    {
        SetGameInfo();
    }
    public void OnHoverLeave()
    {
        GameInfo.Instance.SetHoverText("");
    }
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
                SoundManager.Instance.Play(SoundManager.SoundType.Restored);
                if (miracleController)
                {
                    SetGameInfo();
                    miracleController.gameObject.SetActive(true);
                    symbol.color = Color.white;
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

    public bool UseImmediately()
    {
        return true;
    }

    void SetGameInfo()
    {
        string description = "SHRINE RUIN: Shrine of a fallen god.";
        if (!miracleController.gameObject.active)
        {
            description += "\nTap to unlock " + miracleController.GetInfo() ;
        }
        else
        {
            description += miracleController.GetInfo();
        }
        GameInfo.Instance.SetHoverText(description);
    }
}
