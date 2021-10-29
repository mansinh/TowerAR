using UnityEngine;

/**
 * Pool of trees
 * When a water miracle is cast over it, has a chance to spawn trees and spawned trees will grow
 *@ author Manny Kwong 
 */

public class Forest : MonoBehaviour, IGrowable
{
    Pool _treePool;
    [SerializeField] GameObject treePrefab;
    [SerializeField] int maxTreeCount = 5;
    [SerializeField] Tile tile;
    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<Tile>();
        _treePool = gameObject.AddComponent<Pool>();
        _treePool.SetPrefab(treePrefab);
        _treePool.SetPoolSize(maxTreeCount);
        _treePool.Init();
    }

    public void Grow(float growAmount)
    {
        if (tile.GetCorrupt())
        {
            return;
        }

        //1% chance to spawn a tree on the top of a tile in a random position within a circle radius of 1/3 of tile size
        if (Random.value < 1f / 100)
        {
            Vector2 randomCircle =Random.insideUnitCircle/3*World.Instance.transform.localScale.x;
            Vector3 randomPos = new Vector3(randomCircle.x, 0,randomCircle.y);
            GameObject newTree = _treePool.Push();
            if (newTree)
            {
                if (newTree.GetComponent<Tree>())
                {
                    newTree.transform.position = tile.GetTop() + randomPos;
                    //Random rotation
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
