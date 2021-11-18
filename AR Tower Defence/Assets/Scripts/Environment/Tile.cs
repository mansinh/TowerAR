using UnityEngine;
using TMPro;
/**
 * What the environment is made up of
 * Shows the player territory by its state
 * Can be in corrupt or restored state (player territory)
 * Player can only do actions over restored tiles
 * Can be edited in editor mode to create environment/world/level
 *@ author Manny Kwong 
 */

[ExecuteInEditMode]
public class Tile : MonoBehaviour, IHoverable
{
    private BoxCollider _tileCollider;
    private MeshRenderer _renderer;
    [SerializeField] float state = 0;
    public Tile[] neighbours;
    public Vector3Int Coordinates = Vector3Int.zero;
    public static float DESERT = 0, HEALABLE = 10, RESTORED = 100;
    [SerializeField] private HealEffect healedEffect;
    [SerializeField] private HealEffect healingEffect;

    [SerializeField] private MeshRenderer decorator;
    [SerializeField] private TMP_Text showHeight;



    void Awake()
    {
        _tileCollider = GetComponent<BoxCollider>();
        
       
    }

    private void Start()
    {
        ResetDecorator();
        if (Application.isPlaying)
        {
            showHeight.gameObject.SetActive(false);
        }
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
        healedEffect.transform.position = GetTop();
        healingEffect.transform.position = GetTop();
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
        }
        showHeight.SetText("" + GetHeight() + " " + state);
    }

    public void OnMiracleRain(float healAmount)
    {

        if (state < RESTORED)
        {

           
            state += healAmount;
            if (state >= RESTORED)
            {
                if (healedEffect)
                {
                    SoundManager.Instance.Play(SoundManager.SoundType.Restored);
                    healedEffect.PlayEffects();
                }

                state = RESTORED;
                World.Instance.UpdateView();
                ResetDecorator();
            }
            else
            {
                if (healingEffect)
                { 
                    healingEffect.PlayEffects();
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
        decorator.transform.position = GetTop();
        healedEffect.transform.position = GetTop();
        healingEffect.transform.position = GetTop();
        showHeight.transform.position = GetTop();
        showHeight.SetText(""+ GetHeight() + " "+ state);
    }

    public void Select()
    {
        decorator.gameObject.SetActive(true);
    }

    public void ResetDecorator()
    {
        decorator.gameObject.SetActive(false);   
    }



    public void OnHoverEnter()
    {
        if (state == RESTORED)
        {
            GameInfo.Instance.SetHoverText("RESTORED GRASSLAND: Can build and cast miracles on. Cast miracle RAIN to grow trees for wood.");
        }
        else
        {
            GameInfo.Instance.SetHoverText("TOXIC WASTELAND: Can't build or cast miracles on. Cast miracle RAIN near the edges to RESTORE.");
        }
        
    }

    public void OnHoverStay()
    {
       
    }

    public void OnHoverLeave()
    {
        GameInfo.Instance.SetHoverText("");
    }

    public ISelectable GetSelectable()
    {
        return null;
    }
}
