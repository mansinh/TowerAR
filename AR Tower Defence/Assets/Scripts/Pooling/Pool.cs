using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _poolSize = 5;
    [SerializeField] float _checkPoolablesTime = 1;

    public List<GameObject> Inactive = new List<GameObject>();
    public List<GameObject> Active = new List<GameObject>();

    private float _timeSinceLastCheck = 0;

  

    public virtual void Init() {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject poolable = Instantiate(_prefab, World.Instance.transform);
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
        if (_timeSinceLastCheck > _checkPoolablesTime)
        {
            CheckPoolables();
        }
        _timeSinceLastCheck += Time.deltaTime;
    }

    private List<GameObject> _toBeReleased = new List<GameObject>();
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
        _prefab = prefab;
    }
    public void SetPoolSize(int poolSize) {
        _poolSize = poolSize;
    }

    public List<GameObject> GetInactive() {
        return Inactive;
    }
}
