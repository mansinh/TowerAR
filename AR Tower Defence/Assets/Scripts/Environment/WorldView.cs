using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace World
{
    public class WorldView : MonoBehaviour
    {
        [SerializeField] GameObject _voxelPrefab;
        [SerializeField] GameObject[] _voxels;
        [SerializeField] int _height;
        [SerializeField] Transform _voxelGroup;
        [SerializeField] Mesh[] _meshStates;

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


            _size = _world.size+1;

            _voxels = new GameObject[_size*_height*_size];

            
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int z = 0; z < _size; z++)
                    {
                        GameObject voxel = Instantiate(_voxelPrefab, _voxelGroup);
                        voxel.transform.localPosition = new Vector3(x - _world.size / 2-0.5f, (float)y/10, z - _world.size / 2-0.5f);
                        voxel.gameObject.name = "voxel (" + x+","+z+") height "+y;
                        int index = x * _size * _height + y * _size + z;
                        _voxels[index] = voxel;
                    }
                }
            }
            UpdateView();
        }

        public void UpdateView() {
            print("UpdateView");
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
                            _voxels[index].GetComponent<MeshRenderer>().material = GetMaterial(x, z);
                        }
                        else {
                            _voxels[index].SetActive(false);
                        }
                    }
                }
            }
        }

        Mesh GetMesh(int x, int y, int z, Transform t)
        {
            int a = GetHeightState(x-1,y,z-1);
            int b = GetHeightState(x,y,z-1);
            int c = GetHeightState(x,y,z);
            int d = GetHeightState(x-1,y,z);
            int state = a + b * 2 + c * 4 + d * 8;
            
            Mesh mesh = null;
            switch (state)
            {
                case 1: { mesh = _meshStates[0]; break; }
                case 2: { mesh = _meshStates[1]; break; }
                case 3: { mesh = _meshStates[2]; break; }
                case 4: { mesh = _meshStates[3]; break; }

                case 5: { mesh = _meshStates[4]; break; }
                case 6: { mesh = _meshStates[5]; break; }
                case 7: { mesh = _meshStates[6]; break; }
                case 8: { mesh = _meshStates[7]; break; }

                case 9: { mesh = _meshStates[8]; break; }
                case 10: { mesh = _meshStates[9]; break; }

                case 11: { mesh = _meshStates[10]; break; }
                case 12: { mesh = _meshStates[11]; break; }
                case 13: { mesh = _meshStates[12]; break; }
                case 14: { mesh = _meshStates[13]; break; }

                case 15: { mesh = _meshStates[14]; break; }
            }
            t.gameObject.name = "voxel (" + x + "," + z + ") height: " + y + " state: " + state;
       

            return mesh;
        }

        int GetHeightState(int x, int y, int z)
        {
            Tile tile = _world.GetTile(x,z);
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

        Material GetMaterial(int x, int z)
        {
            Tile tile = _world.GetTile(x, z);
            if (tile != null)
            {
                if (!_world.GetTile(x, z).GetCorrupt())
                {
                    return m_restored;
                }
            }
            return m_corrupt;
        }
    }
}
