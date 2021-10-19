using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : VillageBuilding
{
    [SerializeField] Transform _wheat;
    [SerializeField] float _growSpeed = 0.1f;
    [SerializeField] float _maxWheatHeight = 0.1f;
    [SerializeField] SpriteRenderer[] _wheatSprites;
    [SerializeField] SpriteRenderer _wheatSpriteBase;
    [SerializeField] Color initialColour;
    [SerializeField] Color halfwayColour;
    [SerializeField] Color fullColour;
    [SerializeField] float growth = 0;

    // Update is called once per frame
    void Update()
    {
        Vector3 wheatPosition = _wheat.transform.localPosition;
        wheatPosition.y = growth / 100 * _maxWheatHeight;
        _wheat.transform.localPosition = wheatPosition;
        if (growth < 100)
        {
            growth += Time.deltaTime * _growSpeed;
            SetColours();
        }
 
  
    }

    void SetColours()
    {
        foreach (SpriteRenderer sprite in _wheatSprites)
        {

            sprite.color = GetColour();
        }
        Color baseColor = GetColour()*0.8f;
        baseColor.a = 1;
        _wheatSpriteBase.color = baseColor;
    }

    Color GetColour()
    {
        if (growth < 50)
        {
            return Color.Lerp(initialColour, halfwayColour, growth / 50);
        }
        return Color.Lerp(halfwayColour, fullColour, (growth - 50) / 50);
    }

    public void AddGrowth(float growthAmount)
    {
        growth += growthAmount;
    }

    public void Harvest()
    {
        growth = 0;
        //Points
    }
}
