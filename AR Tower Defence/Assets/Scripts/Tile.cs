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
    public int height = 0;


    // Start is called before the first frame update
    void Awake()
    {
        tileCollider = GetComponent<BoxCollider>();
        transform.eulerAngles = new Vector3(0,(int)(Random.value*4)*90,0);
    }

    public void MouseOver()
    {
       
        
    }

    public void MouseExit()
    {
        
       
    }

    public void Raise()
    {
        height++;
        UpdateBlock();
    }
    public void Lower()
    {
        height--;
        UpdateBlock();
    }
    public void SetHeight(int height)
    {
        this.height = height;
        UpdateBlock();
    }
    public void UpdateBlock()
    {
        for (int i = 0; i < levels.Length; i++) {
            levels[i].SetActive(i < height);  
        }
        tileCollider.center = new Vector3(0, (height+1f) / 2,0);
        tileCollider.size = new Vector3(1, height+1, 1);
    }
}
