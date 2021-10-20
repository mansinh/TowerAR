using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : VillageBuilding, IGrowable
{
    [SerializeField] Transform _wheat;
    
    [SerializeField] float _maxWheatHeight = 0.1f;
    [SerializeField] SpriteRenderer[] _wheatSprites;
    [SerializeField] SpriteRenderer _wheatSpriteBase;
    [SerializeField] Color initialColour;
    [SerializeField] Color halfwayColour;
    [SerializeField] Color fullColour;

    [SerializeField] float _growSpeed = 0.1f;
    [SerializeField] float _growthBonus = 0;
    [SerializeField] float _growth = 0;

    // Update is called once per frame
    void Update()
    {
        Grow();
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
        if (_growth < 50)
        {
            return Color.Lerp(initialColour, halfwayColour, _growth / 50);
        }
        return Color.Lerp(halfwayColour, fullColour, (_growth - 50) / 50);
    }

    public void Grow() {
        if (_growth < 100)
        {
            _growth += Time.deltaTime * (_growSpeed + _growthBonus);
            _growthBonus *= 0.9f;
            SetColours();
        }
        UpdateView();
    }

  

    private void UpdateView() {
        Vector3 wheatPosition = _wheat.transform.localPosition;
        wheatPosition.y = _growth / 100 * _maxWheatHeight;
        _wheat.transform.localPosition = wheatPosition;
        SetColours();
    }

    public void Harvest()
    {
        _growth = 0;
        //Points
    }

    public void SetGrowthBonus(float growthBonus)
    {
        _growthBonus = growthBonus;
    }
}
