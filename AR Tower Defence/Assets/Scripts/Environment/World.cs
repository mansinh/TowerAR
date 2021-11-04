using UnityEngine;

/**
 * Collection of tiles that make up the environment
 * Can create a set of tiles of given size
 * Can add Perlin noise and Gaussian filter to tiles to generate pseudo-random terrain
 *@ author Manny Kwong 
 */

[RequireComponent(typeof(NavController))]

public class World : MonoBehaviour
{
    static public World Instance;
    public int size = 40;
    [SerializeField] private WorldView view;
    [SerializeField] private NavController _navController;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform tileGroup;
    [SerializeField] private Tile[] tiles;
 

    private void Awake()
    {
        Instance = this;
        _navController = GetComponent<NavController>();
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
        _navController.Bake();
    }

    
    public void Create()
    {
        //Destroy old tiles when creating new set
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

        //Create new set of tiles over a square area of a given size
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

    //Generate a random terrain
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
    public bool IsGenerating; //Generates pseudo-random terrain when on, turn off to make modifications
    [SerializeField] bool Gaussian; //Useful for making islands
    [SerializeField] float radius = 10;
    [SerializeField] float scale = 1;
    [SerializeField] float heightMultiplier = 1;
    [SerializeField] float baseHeight = 0;
    [SerializeField] float perlinHeight = 5; //Height of peaks
    [SerializeField] Vector2 perlinOffset = Vector3.zero;

    //Use Perlin noise and Gaussian filter to generate terrain
    float CalculateHeight(float x, float z)
    {
        float xCoord = x / size * scale + perlinOffset.x;
        float zCoord = z / size * scale + perlinOffset.y;
        float perlin = Mathf.PerlinNoise(xCoord, zCoord);

        float xDisp = x - size / 2;
        float zDisp = z - size / 2;
        float gaussian = Mathf.Exp(-(xDisp * xDisp + zDisp * zDisp) / (2 * radius * radius));
        if (!Gaussian)
        {
            gaussian = 1;
        }    
        return heightMultiplier *perlinHeight * perlin * gaussian + baseHeight;
    }

    public void UpdateView()
    {
        view.UpdateView();
    }

    public void Draw()
    {
        view.Draw();
    }

    //Get tile from world position
    public Tile GetTile(Vector3 position)
    {
        Vector3 p = transform.InverseTransformPoint(position);
        int tileX = Mathf.RoundToInt(p.x + size / 2);
        int tileZ = Mathf.RoundToInt(p.z + size / 2);
        print("tile " + tileX + " " + tileZ);

        return GetTile(tileX,tileZ);
    }

    //Get tile from index
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


