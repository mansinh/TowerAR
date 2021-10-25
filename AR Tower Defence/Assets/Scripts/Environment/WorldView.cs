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

                    Mesh mesh = GetMesh(x, y, z, _voxels[index].transform);
                    if (mesh != null)
                    {
                        _voxels[index].SetActive(true);
                        _voxels[index].GetComponent<MeshFilter>().mesh = mesh;
                        _voxels[index].GetComponent<MeshRenderer>().material = GetMaterial(x,y, z, _voxels[index].transform);

                        
                    }
                    else
                    {
                        _voxels[index].SetActive(false);
                    }
                }
            }
        }
    }

    Mesh GetMesh(int x, int y, int z, Transform t)
    {
        int a = GetHeightState(x - 1, y, z - 1);
        int b = GetHeightState(x, y, z - 1);
        int c = GetHeightState(x, y, z);
        int d = GetHeightState(x - 1, y, z);
        int state = a + b * 2 + c * 4 + d * 8;

        Mesh mesh = _meshStates[state];
        t.gameObject.name = "voxel (" + x + "," + z + ") height: " + y + " mesh: " + state;

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

    Material GetMaterial(int x,int y, int z, Transform t)
    {
       

        int a = GetMaterialState(x - 1, z - 1);
        int b = GetMaterialState(x, z - 1);
        int c = GetMaterialState(x, z);
        int d = GetMaterialState(x - 1, z);
        int state = a + b * 2 + c * 4 + d * 8;
        t.gameObject.name = t.gameObject.name + " mat: " + state;

       
        Material material = _materialStates[state];
        if (y%3 == 0)
        {
            material = _materialStates1[state];
        }
        else if (y%2==0)
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

