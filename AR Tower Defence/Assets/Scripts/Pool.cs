using System.Collections.Generic;
using UnityEngine;

/**
 * Handles pooling of gameobjects 
 * @author Manny Kwong
 */

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 5;
    [SerializeField] float checkPoolablesTime = 1;

    public List<GameObject> Inactive = new List<GameObject>();
    public List<GameObject> Active = new List<GameObject>();

    private List<GameObject> _toBeReleased = new List<GameObject>();
    private float _timeSinceLastCheck = 0;

    public virtual void Init() {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject poolable = Instantiate(prefab, World.Instance.transform);
            poolable.SetActive(false);
            Inactive.Add(poolable);
        }
    }

    public GameObject GetNextPoolable() {
        if (Inactive.Count > 0)
        {
            return Inactive[0];
        }
        return null;
    }

    //Push and activate object into game world
    public GameObject Push()
    {
        if (Inactive.Count > 0)
        {
            GameObject poolable = Inactive[0];
            Active.Add(poolable);
            Inactive.Remove(poolable);
            poolable.transform.position = transform.position;
            poolable.SetActive(true);
            return poolable;
        }
        return null;
    }

    void Update() {
        if (_timeSinceLastCheck > checkPoolablesTime)
        {
            CheckPoolables();
        }
        _timeSinceLastCheck += Time.deltaTime;
    }

    //Check if an pushed object is ready to return to the pool (is gameObject is not active)
    void CheckPoolables()
    {
        _toBeReleased.Clear();
        foreach (GameObject poolable in Active)
        {
            if (!poolable.activeSelf)
            {
                _toBeReleased.Add(poolable);
            }
        }
        foreach (GameObject poolable in _toBeReleased) {
            Release(poolable);
        }
        _timeSinceLastCheck = 0;
    }

    //Return to pool
    public void Release(GameObject poolable)
    {
        if (Active.Contains(poolable))
        {
            Active.Remove(poolable);
        }

        if (!Active.Contains(poolable))
        {
            Inactive.Add(poolable);
        }
    }

    public void SetPrefab(GameObject prefab) {
        this.prefab = prefab;
    }
    public void SetPoolSize(int poolSize) {
        this.poolSize = poolSize;
    }

    public List<GameObject> GetInactive() {
        return Inactive;
    }
}
