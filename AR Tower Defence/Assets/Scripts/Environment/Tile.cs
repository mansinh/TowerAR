using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    BoxCollider _tileCollider;
    MeshRenderer _renderer;
    [SerializeField] int _state = 0;

    public Vector3Int Coordinates = Vector3Int.zero;
    public const int CORRUPT = 0, RESTORED = 1;

    // Start is called before the first frame update
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

    public Vector3 GetTop()
    {
        return transform.position + Vector3.up * ((Coordinates.y + 1) * transform.localScale.y);
    }

    public void SetState(int state)
    {
        _state = state;

    }

    public void Heal()
    {
        _state++;

    }

    void UpdateCollider()
    {
        _tileCollider.center = new Vector3(0, (Coordinates.y + 1f) / 2, 0);
        _tileCollider.size = new Vector3(1, Coordinates.y + 1, 1);
    }



    public bool GetCorrupt()
    {
        return _state == CORRUPT;
    }
}
