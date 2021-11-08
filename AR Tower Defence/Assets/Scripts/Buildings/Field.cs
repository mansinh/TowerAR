using UnityEngine;

/**
 * Grows over time
 * Water miracle speeds up growth
 * TODO harvest for points
 *@ author Manny Kwong 
 */

public class Field : VillageBuilding, IGrowable
{
    [SerializeField] Transform _wheat;
    
    [SerializeField] float maxWheatHeight = 0.1f;
    [SerializeField] SpriteRenderer[] wheatSprites;
    [SerializeField] SpriteRenderer wheatSpriteBase;
    [SerializeField] Color initialColour;
    [SerializeField] Color halfwayColour;
    [SerializeField] Color fullColour;

    [SerializeField] float growSpeed = 0.6f;
    [SerializeField] float growth = 0;

    // Update is called once per frame
    void Update()
    {
        Grow(growSpeed*Time.deltaTime);
    }

    void SetColours()
    {
        Color color = GetColour();
        foreach (SpriteRenderer sprite in wheatSprites)
        {
            sprite.color = color;
        }
        wheatSpriteBase.color = color;
    }

    //Change colour from brown to green to yellow as the wheat field ripens/grows
    Color GetColour()
    {
        if (growth < 50)
        {
            return Color.Lerp(initialColour, halfwayColour, growth / 50);
        }
        return Color.Lerp(halfwayColour, fullColour, (growth - 50) / 50);
    }

    public void Grow(float growAmount) {
        if (growth < 100)
        {
            growth += growAmount;
            SetColours();
        }
        UpdateView();
    }

    protected override void UpdateView() {
        Vector3 wheatPosition = _wheat.transform.localPosition;
        wheatPosition.y = growth / 100 * maxWheatHeight;
        _wheat.transform.localPosition = wheatPosition;
        SetColours();
    }

    public void Harvest()
    {
        growth = 0;
        //Points
    }
}
