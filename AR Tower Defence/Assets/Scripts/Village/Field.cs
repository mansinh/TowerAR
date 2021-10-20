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
    [SerializeField] float _growth = 0;

    // Update is called once per frame
    void Update()
    {
        Grow(_growSpeed);
    }

    void SetColours()
    {
        Color color = GetColour();
        foreach (SpriteRenderer sprite in _wheatSprites)
        {

            sprite.color = color;
        }
       
        _wheatSpriteBase.color = color;
    }

    Color GetColour()
    {
        if (_growth < 50)
        {
            return Color.Lerp(initialColour, halfwayColour, _growth / 50);
        }
        return Color.Lerp(halfwayColour, fullColour, (_growth - 50) / 50);
    }

    public void Grow(float growSpeed) {
        if (_growth < 100)
        {
            _growth += Time.deltaTime *growSpeed;
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
}
