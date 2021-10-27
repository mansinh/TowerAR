using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldView : MonoBehaviour
{
    [SerializeField] GameObject _voxelPrefab;
    [SerializeField] GameObject[] _voxels;
    [SerializeField] int _height;
    [SerializeField] Transform _voxelGroup;
    [SerializeField] Mesh[] _meshStates;

    [SerializeField] Material[] _materialStates;
    [SerializeField] Material[] _materialStates1;
    [SerializeField] Material[] _materialStates2;

    [SerializeField] Material m_corrupt, m_restored;

    [SerializeField] int _size;
    public World _world;

    public bool UseGameobjects = false;

    [SerializeField] Vector3[] _positions;
    [SerializeField] Mesh[] _meshes;
    [SerializeField] Material[] _materials;

    public void Create()
    {
        if (_voxels != null)
        {
            foreach (GameObject voxel in _voxels)
            {
                DestroyImmediate(voxel);
            }
        }
        _size = _world.size + 1;
        if (UseGameobjects)
        {          
            _voxels = new GameObject[_size * _height * _size];

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int z = 0; z < _size; z++)
                    {
                        GameObject voxel = Instantiate(_voxelPrefab, _voxelGroup);
                        voxel.transform.localPosition = new Vector3(x - _world.size / 2 - 0.5f, (float)y / 10, z - _world.size / 2 - 0.5f);
                        voxel.gameObject.name = "voxel (" + x + "," + z + ") height " + y;
                        int index = x * _size * _height + y * _size + z;
                        _voxels[index] = voxel;
                    }
                }
            }
        }
        else
        {
            _positions = new Vector3[_size * _height * _size];
            _meshes = new Mesh[_size * _height * _size];
            _materials = new Material[_size * _height * _size];
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int z = 0; z < _size; z++)
                    {
                        int index = x * _size * _height + y * _size + z;
                        _positions[index] = new Vector3(x - _world.size / 2 - 0.5f, (float)y / 10, z - _world.size / 2 - 0.5f);
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
                            _voxels[index].GetComponent<MeshFilter>().mesh = mesh;
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

                        _meshes[index] = GetMesh(x, y, z);
                      
                        if (_meshes[index] != null)
                        {
                            _materials[index] = GetMaterial(x,y,z);


                        }
                    }
                }
            }
        }
       
    }

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

    Mesh GetMesh(int x, int y, int z)
    {
        int a = GetHeightState(x - 1, y, z - 1);
        int b = GetHeightState(x, y, z - 1);
        int c = GetHeightState(x, y, z);
        int d = GetHeightState(x - 1, y, z);
        int state = a + b * 2 + c * 4 + d * 8;

        Mesh mesh = _meshStates[state];
       
        return mesh;
    }

    int GetHeightState(int x, int y, int z)
    {
        Tile tile = _world.GetTile(x, z);
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

    Material GetMaterial(int x,int y, int z)
    {
        int a = GetMaterialState(x - 1, z - 1);
        int b = GetMaterialState(x, z - 1);
        int c = GetMaterialState(x, z);
        int d = GetMaterialState(x - 1, z);
        int state = a + b * 2 + c * 4 + d * 8;
       
        Material material = _materialStates[state];
        
        if (y%3 == 1)
        {
            material = _materialStates1[state];
        }
        else if (y%3==2)
        {
            material = _materialStates2[state];
        }

        return material;
    }

    int GetMaterialState(int x, int z)
    {
        Tile tile = _world.GetTile(x, z);
        if (tile == null)
        {
            return 0;
        }

        if (!tile.GetCorrupt())
        {
            return 1;
        }
        return 0;
    }


}

