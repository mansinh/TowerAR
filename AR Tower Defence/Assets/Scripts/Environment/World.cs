using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavController))]

public class World : MonoBehaviour
{
    static public World Instance;
    public int size = 40;
    [SerializeField] private WorldView view;
    [SerializeField] private NavController navController;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform tileGroup;
    [SerializeField] private Tile[] tiles;

    private void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.GetHeight() < 1)
            {
                tile.gameObject.SetActive(false);
            }
        }
        Refresh();
    }

    private Enemy[] _enemies;
    public void Refresh()
    {
        navController.Bake();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _enemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    public void Resume()
    {

    }

    public void Create()
    {
        if (tiles != null)
        {
            foreach (Tile tile in tiles)
            {
                if (tile)
                {
                    DestroyImmediate(tile.gameObject);
                }

            }
        }


        tiles = new Tile[size * size];
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Tile tile = Instantiate(tilePrefab, tileGroup);
                tile.transform.localPosition = new Vector3(x - size / 2, -tile.transform.localScale.y, z - size / 2);
                tile.SetCoordinates(x, 0, z);
                tile.SetHeight(0);
                tile.gameObject.name = "tile (" + x + "," + z + ")";
                tiles[x * size + z] = tile;
            }
        }
        if (view != null)
        {
            view.Create();
        }
    }




    public void Generate()
    {
       
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                GetTile(x, z).SetHeight(Mathf.RoundToInt(10 * CalculateHeight(x, z)));
            }
        }
        view.UpdateView();
    }

    [Space(10)]
    [Header("PROCEDURAL GENERATION (Work in Progress)")]
    [Header("______________________________________________________________________")]
    public bool IsGenerating;
    [SerializeField] bool Gaussian;
    [SerializeField] float _scale = 1;
    [SerializeField] float _deviation = 10;
    [SerializeField] float _heightMultiplier = 1;
    [SerializeField] float _baseHeight = 0;
    [SerializeField] float _perlinHeight = 5;
    [SerializeField] Vector2 _perlinOffset = Vector3.zero;
    float CalculateHeight(float x, float z)
    {
        float xCoord = x / size * _scale + _perlinOffset.x;
        float zCoord = z / size * _scale + _perlinOffset.y;
        float perlin = Mathf.PerlinNoise(xCoord, zCoord);

        float xDisp = x - size / 2;
        float zDisp = z - size / 2;
        float gaussian = Mathf.Exp(-(xDisp * xDisp + zDisp * zDisp) / (2 * _deviation * _deviation));
        if (!Gaussian)
        {
            gaussian = 1;
        }
        

        return _heightMultiplier *_perlinHeight * perlin * gaussian + _baseHeight;
    }





    public void UpdateView()
    {
        view.UpdateView();
    }

    public void Draw()
    {
        view.Draw();
    }

    public Tile GetTile(Vector3 position)
    {
        Vector3 p = transform.InverseTransformPoint(position);
        int tileX = Mathf.RoundToInt(p.x + World.Instance.size / 2);
        int tileZ = Mathf.RoundToInt(p.z + World.Instance.size / 2);
        print("tile " + tileX + " " + tileZ);

        return GetTile(tileX,tileZ);
    }

    public Tile GetTile(int x, int z)
    {
        if (tiles != null)
        {
            if (x >= 0 && x < size && z >= 0 && z < size)
            {
                return tiles[x * size + z];
            }
        }
        return null;
    }


}


