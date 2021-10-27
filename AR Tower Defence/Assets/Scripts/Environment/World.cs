using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavController))]

public class World : MonoBehaviour
{
    static public World Instance;
    public int size = 40;
    [SerializeField] WorldView _view;
    [SerializeField] NavController navController;
    [SerializeField] Tile tilePrefab;
    [SerializeField] Transform _tileGroup;

    [SerializeField] Tile[] _tiles;

    private void Awake()
    {


        Instance = this;

        print(_tiles == null);
    }

    void Start()
    {
        foreach (Tile tile in _tiles)
        {
            if (tile.GetHeight() < 1)
            {
                tile.gameObject.SetActive(false);
            }
        }
        Refresh();
    }

    Enemy[] enemies;
    public void Refresh()
    {
        navController.Bake();
    }

    public void Pause()
    {
        enemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    public void Create()
    {
        if (_tiles != null)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile)
                {
                    DestroyImmediate(tile.gameObject);
                }

            }
        }


        _tiles = new Tile[size * size];
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Tile tile = Instantiate(tilePrefab, _tileGroup);
                tile.transform.localPosition = new Vector3(x - size / 2, -tile.transform.localScale.y, z - size / 2);
                tile.SetCoordinates(x, 0, z);
                tile.SetHeight(0);
                tile.gameObject.name = "tile (" + x + "," + z + ")";
                _tiles[x * size + z] = tile;
            }
        }
        if (_view != null)
        {
            _view.Create();
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
        _view.UpdateView();
    }


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
        _view.UpdateView();
    }

    public void Draw()
    {
        _view.Draw();
    }

    public Tile GetTile(int x, int z)
    {
        if (_tiles != null)
        {
            if (x >= 0 && x < size && z >= 0 && z < size)
            {
                return _tiles[x * size + z];
            }
        }
        return null;
    }


}


