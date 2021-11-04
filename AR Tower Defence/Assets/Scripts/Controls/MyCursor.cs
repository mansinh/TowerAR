using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/**
 * Finds out what is in front of the cursor
 * The cursor is at the center of the screen on mobile and follows the mouse when testing on pc
 * The colour of the cursor changes depending on what is in front of it
 *@ author Manny Kwong 
 */

public class MyCursor : MonoBehaviour
{
    Image _cursorImage;
    public static MyCursor Instance;
    private RaycastHit _cursorHit;
    private bool _isCursorHitting;

    void Awake()
    {
        _cursorImage = GetComponent<Image>();
        Instance = this;

    }

    public void SetScreenPosition(Vector3 position)
    {
        _cursorImage.rectTransform.position = position;
    }

    //Returns whether the player can perform an action at the cursor location
    //The player can only act when inside their own territory (cursor is over the green "restored" tiles)
    public float GetTileState()
    {
        if (_isCursorHitting)
        {
            try
            {
                Tile tile = World.Instance.GetTile(_cursorHit.point);
                if (tile != null)
                {
                    return tile.GetState();
                }
                return -1;
            }
            catch (Exception e)
            {
            }
        }
        return -1;
    }

    //Raycasts into the game world to find what is infront of the cursor
    //If the player can act at cursor location, the cursor will change colour
    //Magenta if the cursor is over an enemy and green otherwise
    public void Cast(Vector3 castFrom)
    {
        _cursorImage.color = new Color(1, 1, 1, 0.5f);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(castFrom);
            _isCursorHitting = Physics.Raycast(ray, out _cursorHit);
            if (GetTileState() >= 0)
            {
                string hitTag = _cursorHit.collider.tag;
                switch (hitTag)
                {
                    case "Enemy":
                        {
                            _cursorImage.color = Color.magenta;
                            break;
                        }

                    default:
                        {
                            _cursorImage.color = Color.green;
                            break;
                        }
                }
            }
        }
        else
        {
            _isCursorHitting = false;
        }
    }

    public RaycastHit GetCursorHit()
    {
        return _cursorHit;
    }
    public bool GetCursorHitting()
    {
        return _isCursorHitting;
    }
}
