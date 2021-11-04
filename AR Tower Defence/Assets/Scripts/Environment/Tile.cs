using UnityEngine;

/**
 * What the environment is made up of
 * Shows the player territory by its state
 * Can be in corrupt or restored state (player territory)
 * Player can only do actions over restored tiles
 * Can be edited in editor mode to create environment/world/level
 *@ author Manny Kwong 
 */

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    private BoxCollider _tileCollider;
    private MeshRenderer _renderer;
    [SerializeField] float state = 0;
    public Tile[] neighbours;
    public Vector3Int Coordinates = Vector3Int.zero;
    public static float DESERT = 0, HEALABLE = 10, RESTORED = 100;
    [SerializeField] private HealEffect healEffect;


    void Awake()
    {
        _tileCollider = GetComponent<BoxCollider>();
       
    }
    public void Raise()
    {
        Coordinates.y++;
        UpdateCollider();
    }
    public void Lower()
    {
        Coordinates.y--;
        UpdateCollider();
    }
    public void SetHeight(int height)
    {
        Coordinates.y = height;
        healEffect.transform.position = GetTop();
        UpdateCollider();
    }
    public int GetHeight()
    {
        return Coordinates.y;
    }

    public void SetCoordinates(int x, int height, int z)
    {
        Coordinates.x = x;
        Coordinates.y = height;
        Coordinates.z = z;
    }

    //Get the position of the top of the tile
    public Vector3 GetTop()
    {
        return transform.position + Vector3.up * ((Coordinates.y + 1) * transform.localScale.y);
    }

    public void SetState(float state)
    {
        this.state = state;
        if (this.state >= RESTORED)
        {
            this.state = RESTORED;
            SetNeighboursHealable();
        }

    }

    public void Heal(float healAmount)
    {
       
        if (state < RESTORED)
        {
            
            print("prehealed" + Coordinates + " " + state);
            state += healAmount;
            if (state >= RESTORED)
            {
                if (healEffect)
                {
                    healEffect.PlayEffects();
                }
                print("healed"+Coordinates);
                state = RESTORED;
                SetNeighboursHealable();
                World.Instance.UpdateView();

            }
        }
    }

    void SetNeighboursHealable()
    {
        foreach (Tile neighbour in neighbours)
        {
            if (neighbour)
            {
                if (neighbour.GetState() == DESERT)
                {
                    neighbour.SetState(HEALABLE);
                }
            }
        }
    }

    public float GetState()
    {
        return state;
    }

    //Matches the height of the collider to the height of the tile
    void UpdateCollider()
    {
        _tileCollider.center = new Vector3(0, (Coordinates.y + 1f) / 2, 0);
        _tileCollider.size = new Vector3(1, Coordinates.y + 1, 1);
    }
}
