using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Forest : MonoBehaviour, IGrowable
{
    Pool _treePool;
    [SerializeField] GameObject _treePrefab;
    [SerializeField] int _maxTreeCount = 6;
    [SerializeField] Tile _tile;
    // Start is called before the first frame update
    void Start()
    {
        _tile = GetComponent<Tile>();
        _treePool = gameObject.AddComponent<Pool>();
        _treePool.SetPrefab(_treePrefab);
        _treePool.SetPoolSize(_maxTreeCount);
        _treePool.Init();
    }

    public void Grow(float growAmount)
    {
        if (_tile.GetCorrupt())
        {
            return;
        }
        if (Random.value < 1f / 100)
        {
            Vector2 randomCircle =Random.insideUnitCircle/3;
            Vector3 randomPos = new Vector3(randomCircle.x, 0,randomCircle.y);
            GameObject newTree = _treePool.Push();
            if (newTree)
            {
                if (newTree.GetComponent<Tree>())
                {
                    newTree.transform.position = _tile.GetTop() + randomPos;
                    newTree.transform.localEulerAngles = new Vector3(0, 360 * Random.value, 0);
                    newTree.GetComponent<Tree>().SetForest(this);
                }
            }
        }
        foreach (GameObject tree in _treePool.Active)
        {
            if (tree.GetComponent<Tree>())
            {
                tree.GetComponent<Tree>().Grow(growAmount);
            }
        }

    }

}
