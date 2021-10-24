using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace World
{
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
                foreach(Tile tile in _tiles)
                {
                        if (tile)
                        {
                            DestroyImmediate(tile.gameObject);
                        }
                    
                }
            }


            _tiles = new Tile[size*size];
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    Tile tile = Instantiate(tilePrefab, _tileGroup);
                    tile.transform.localPosition = new Vector3(x - size / 2, -tile.transform.localScale.y, z - size / 2);
                    tile.SetCoordinates(x, 0, z);
                    tile.SetHeight(0);
                    tile.gameObject.name = "tile (" + x + "," + z + ")";
                    _tiles[x*size +z] = tile;
                }
            }
            if (_view != null)
            {
                _view.Create();
            }
        }

        public void UpdateView()
        {
            
                _view.UpdateView();
            
        }

        public Tile GetTile(int x, int z)
        {
            if (_tiles != null)
            {
                if (x>=0 && x<size && z >= 0 && z < size)
                {
                    return _tiles[x * size + z];
                }
            }
            return null;
        }

        /*
        public Tile[] GetNeighbours(int x, int z)
        {
            Tile[] neighbours = new Tile[8];
            //[-1,+1][+0,+1][+1,+1]
            //[-1,+0][+0,+0][+1,+0]
            //[-1,-1][+0,-1][+1,-1]
            neighbours[0] = GetTile(x,z+1);
            neighbours[1] = GetTile(x+1,z+1);
            neighbours[2] = GetTile(x+1,z);
            neighbours[3] = GetTile(x+1, z-1);
            neighbours[4] = GetTile(x,z-1);
            neighbours[5] = GetTile(x-1, z-1);
            neighbours[6] = GetTile(x-1,z);
            neighbours[7] = GetTile(x-1,z+1);

            return neighbours;
        }*/
    }

}
