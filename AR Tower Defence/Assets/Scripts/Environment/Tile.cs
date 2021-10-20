using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
  

    [SerializeField] GameObject[] levels;
    BoxCollider tileCollider;
    MeshRenderer blockRenderer;
    [SerializeField] int _height = 0;

    public bool IsOccupied = false;
    public bool IsCorrupt = false;

    // Start is called before the first frame update
    void Awake()
    {
        tileCollider = GetComponent<BoxCollider>();
        transform.eulerAngles = new Vector3(0,(int)(Random.value*4)*90,0);
       
    }   

    public void Raise()
    {
        _height++;
        UpdateBlock();
    }
    public void Lower()
    {
        _height--;
        UpdateBlock();
    }
    public void SetHeight(int height)
    {
        _height = height;
        UpdateBlock();
    }
    public int GetHeight() {
        return _height;
    }

    //TODO
    public Vector3 GetTop() {
        return transform.position + Vector3.up * ((_height+1)*transform.localScale.y);
    }
    public void UpdateBlock()
    {
        for (int i = 0; i < levels.Length; i++) {
            levels[i].SetActive(i < _height);  
        }
        tileCollider.center = new Vector3(0, (_height+1f) / 2,0);
        tileCollider.size = new Vector3(1, _height+1, 1);
    }

   
}
