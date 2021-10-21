using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavController))]

public class WorldRoot : MonoBehaviour
{
    public static WorldRoot Instance;
    [SerializeField] NavController navController;
    public int size = 40;  
    [SerializeField] Tile tilePrefab;
    [SerializeField] Transform world;

    
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        foreach (Tile tile in tiles)
        {
            if (tile.GetHeight() == 0)
            {
                tile.gameObject.SetActive(false);
            }
        }
        
    }
    public void Init()
    {
        
 
    }

 
    Enemy[] enemies;

    public void Refresh() {
        navController.Bake(); 
    }
    public void Pause()
    {
        enemies = FindObjectsOfType<Enemy>();
      
        for(int i = 0; i < enemies.Length;i++)
        {    
            enemies[i].GetComponent<NavMeshAgent>().enabled = false;          
        }
    }


    [SerializeField] public List<Tile> tiles = new List<Tile>();
    public void Generate()
    {
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                DestroyImmediate(tile.gameObject);
            }
        }
        tiles = new List<Tile>();

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Tile tile = Instantiate(tilePrefab, world);
                tile.transform.localPosition = new Vector3(x - size / 2, -tile.transform.localScale.y, z - size / 2);
                tile.SetHeight(0);
                tiles.Add(tile);
            }
        }
    }
}