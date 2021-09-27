using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    enum SpellType:int
    {
        Lightning = 0,
        Fire = 1,
        Heal = 2,
        Water = 3
    }

    SpellType currentSpell = SpellType.Lightning;
    LightningController lightning;
    int spellLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        lightning = FindObjectOfType<LightningController>();
    }

  
    public void CastLightning(RaycastHit hit) {
        lightning.Cast(hit);
    }
}
