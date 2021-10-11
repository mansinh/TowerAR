using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldRoot : MonoBehaviour
{
    public static WorldRoot instance;

    NavController navController;
  
  
    public int size = 40;  
    [SerializeField] Tile tilePrefab;
    [SerializeField] Transform world;

    private void Awake()
    {
        instance = this;
       
    }

    public void Init()
    {
        navController = FindObjectOfType<NavController>();
 
    }
    public void Refresh() {
        navController.Bake();
        NavMeshAgent[] agents = FindObjectsOfType<UnityEngine.AI.NavMeshAgent>();
        foreach (NavMeshAgent agent in agents)
        {
            agent.ResetPath();            
        }
    }

    [SerializeField] List<Tile> tiles = new List<Tile>();
    public void Generate()
    {
        foreach (Tile block in tiles)
        {
            if (block != null)
            {
                DestroyImmediate(block.gameObject);
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
