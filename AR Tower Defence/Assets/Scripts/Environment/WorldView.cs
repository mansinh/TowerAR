using UnityEngine;

/**
 * Uses voxels and marching squares to render tiles 
 *@ author Manny Kwong 
 */

public class WorldView : MonoBehaviour
{
    [SerializeField] GameObject _voxelPrefab;
    [SerializeField] GameObject[] _voxels;
    [SerializeField] int _height;
    [SerializeField] Transform _voxelGroup;
    [SerializeField] Mesh[] _meshStates;
    [SerializeField] Mesh[] _meshStatesEmpty;

    [SerializeField] Material[] _materialStates;
    [SerializeField] Material[] _materialStatesDesert;

    [SerializeField] Material m_corrupt, m_restored;

    [SerializeField] int _size;
    public World world;

    public bool UseGameobjects = false;

    [SerializeField] Vector3[] _positions;
    [SerializeField] Mesh[] _meshes;
    [SerializeField] Material[] _materials;



    public void Create()
    {
        //Destroy old voxels if any
        if (_voxels != null)
        {
            foreach (GameObject voxel in _voxels)
            {
                DestroyImmediate(voxel);
            }
        }

        //Create volume of voxels with dimensiton (world size+1)x(world size+1)xheight
        _size = world.size;


        //Have the voxels as individual game objects
        _voxels = new GameObject[_size * _height * _size];


        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    GameObject voxel = Instantiate(_voxelPrefab, _voxelGroup);
                    //Voxel positions
                    float tileHeight = world.GetTile(x, z).transform.localScale.y;
                    voxel.transform.localPosition = new Vector3(x - world.size / 2, y * tileHeight, z - world.size / 2);

                    //Voxel scale
                    voxel.transform.localScale = new Vector3(1, tileHeight*4, 1);


                    //Tile name
                    voxel.gameObject.name = "voxel (" + x + "," + z + ") height " + y;
                    //3d coordinates to 1d array index
                    int index = x * _size * _height + y * _size + z;
                    _voxels[index] = voxel;
                }
            }
        }


        UpdateView();
    }

    public void UpdateView()
    {
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    int index = x * _size * _height + y * _size + z;

                    Mesh mesh = GetMesh(x, y, z);
                    if (mesh != null)
                    {
                        _voxels[index].SetActive(true);
                        //Assign mesh to voxel based on tile position 
                        _voxels[index].GetComponent<MeshFilter>().mesh = mesh;
                        //Assign material to voxel based on tile state
                        _voxels[index].GetComponent<MeshRenderer>().material = GetMaterial(x, y, z);
                    }
                    else
                    {
                        _voxels[index].SetActive(false);
                    }
                }
            }
        }

    }
    //Draw function for when using mesh instances instead of game objects
    public void Draw()
    {
        if (!UseGameobjects)
        {
            for (int i = 0; i < _meshes.Length; i++)
            {
                Graphics.DrawMesh(_meshes[i], _positions[i] + transform.position, transform.rotation, _materials[i], 0);
            }
        }
    }

    private void Update()
    {
        Draw();
    }

    //Get mesh based on coordinates, height and the height of adjacent tiles using Marching Squares algorithm
    Mesh GetMesh(int x, int y, int z)
    {
        
        int a = GetHeightState(x, y, z+1);
        int b = GetHeightState(x+1, y, z);
        int c = GetHeightState(x, y, z-1);
        int d = GetHeightState(x-1, y, z);
        int state = a + b * 2 + c * 4 + d * 8;

        Mesh mesh = null;
        if (GetHeightState(x, y, z) == 0)
        {
            mesh = _meshStatesEmpty[state];
        }
        else
        {
            mesh = _meshStates[state];
        }

       

        return mesh;
    }

    int GetHeightState(int x, int y, int z)
    {
        Tile tile = world.GetTile(x, z);
        if (tile == null)
        {
            return 0;
        }

        if (tile.GetHeight() > y)
        {
            return 1;
        }
        return 0;
    }

    //Get material based on coordinates, state of tile and the state of adjacent tiles using Marching Squares Algorithm
    Material GetMaterial(int x, int y, int z)
    {
        int state = 16;
        int height = world.GetTile(x, z).GetHeight();
        
        int a = GetMaterialState(x,  z + 1, height);
        int b = GetMaterialState(x + 1,  z, height);
        int c = GetMaterialState(x,  z - 1, height);
        int d = GetMaterialState(x - 1,  z, height);

        state = a + b * 2 + c * 4 + d * 8;

        Material material = null;
        if (world.GetTile(x, z).GetState()<Tile.RESTORED)
        {
            material = _materialStatesDesert[state];
        }
        else
        {
            material = _materialStates[state];
        }

        return material;
    }

    int GetMaterialState(int x,int z,int height)
    {
        Tile tile = world.GetTile(x, z);
        if (tile == null || tile.GetHeight() == 0)
        {
            return 0;
        }
     
        if (tile.GetState() == Tile.RESTORED)// && tile.GetHeight() == height)
        {
            return 1;
        }
        return 0;
    }


    /*
  public void Create()
  {
      //Destroy old voxels if any
      if (_voxels != null)
      {
          foreach (GameObject voxel in _voxels)
          {
              DestroyImmediate(voxel);
          }
      }

      //Create volume of voxels with dimensiton (world size+1)x(world size+1)xheight
      _size = world.size + 1;

      if (UseGameobjects)
      {          
          //Have the voxels as individual game objects
          _voxels = new GameObject[_size * _height * _size];


          for (int x = 0; x < _size; x++)
          {
              for (int y = 0; y < _height; y++)
              {
                  for (int z = 0; z < _size; z++)
                  {
                      GameObject voxel = Instantiate(_voxelPrefab, _voxelGroup);
                      //Voxel positions
                      voxel.transform.localPosition = new Vector3(x - world.size / 2 - 0.5f, (float)y / 4, z - world.size / 2 - 0.5f);
                      //Tile name
                      voxel.gameObject.name = "voxel (" + x + "," + z + ") height " + y;
                      //3d coordinates to 1d array index
                      int index = x * _size * _height + y * _size + z;
                      _voxels[index] = voxel;
                  }
              }
          }
      }
      else
      {
          //Have the voxels as mesh instances
          _positions = new Vector3[_size * _height * _size];
          _meshes = new Mesh[_size * _height * _size];
          _materials = new Material[_size * _height * _size];
          for (int x = 0; x < _size; x++)
          {
              for (int y = 0; y < _height; y++)
              {
                  for (int z = 0; z < _size; z++)
                  {
                      //3d coordinates to 1d array index
                      int index = x * _size * _height + y * _size + z;
                      //Voxel positions
                      _positions[index] = new Vector3(x - world.size / 2 - 0.5f, (float)y / 4, z - world.size / 2 - 0.5f);
                  }
              }
          }

      }
      UpdateView();
  }

  public void UpdateView()
  {
      if (UseGameobjects)
      {
          for (int x = 0; x < _size; x++)
          {
              for (int y = 0; y < _height; y++)
              {
                  for (int z = 0; z < _size; z++)
                  {
                      int index = x * _size * _height + y * _size + z;

                      Mesh mesh = GetMesh(x, y, z);
                      if (mesh != null)
                      {             
                          _voxels[index].SetActive(true);
                          //Assign mesh to voxel based on tile position 
                          _voxels[index].GetComponent<MeshFilter>().mesh = mesh;
                          //Assign material to voxel based on tile state
                          _voxels[index].GetComponent<MeshRenderer>().material = GetMaterial(x, y, z);
                      }
                      else
                      {
                          _voxels[index].SetActive(false);
                      }
                  }
              }
          }
      }
      else {
          for (int x = 0; x < _size; x++)
          {
              for (int y = 0; y < _height; y++)
              {
                  for (int z = 0; z < _size; z++)
                  {
                      int index = x * _size * _height + y * _size + z;
                      //Assign mesh to voxel based on tile position 
                      _meshes[index] = GetMesh(x, y, z);

                      if (_meshes[index] != null)
                      {
                          //Assign material to voxel based on tile state
                          _materials[index] = GetMaterial(x,y,z);
                      }
                  }
              }
          }
      }

  }
  */
}

