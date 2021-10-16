using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _poolSize = 5;
    [SerializeField] float _checkPoolablesTime = 1;

    private List<GameObject> _inactive = new List<GameObject>();
    private List<GameObject> _toBeReleased = new List<GameObject>();
    private List<GameObject> _active = new List<GameObject>();

    private float _timeSinceLastCheck = 0;

  

    public virtual void Init() {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject poolable = Instantiate(_prefab, WorldRoot.instance.transform);
            poolable.SetActive(false);
            _inactive.Add(poolable);
        }
    }

  
    public GameObject GetNextPoolable() {
        if (_inactive.Count > 0)
        {
            return _inactive[0];
        }
        return null;
    }

    public void Push()
    {
        if (_inactive.Count > 0)
        {
            GameObject poolable = _inactive[0];
            _active.Add(poolable);
            _inactive.Remove(poolable);
            poolable.transform.position = transform.position;
            poolable.SetActive(true);
            
        }
    }

    void Update() {
        if (_timeSinceLastCheck > _checkPoolablesTime)
        {
            CheckPoolables();
        }
        _timeSinceLastCheck += Time.deltaTime;
    }

    void CheckPoolables()
    {
        _toBeReleased.Clear();
        foreach (GameObject poolable in _active)
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
        if (_active.Contains(poolable))
        {
            _active.Remove(poolable);
        }

        if (!_active.Contains(poolable))
        {
            _inactive.Add(poolable);
        }
    }

    public void SetPrefab(GameObject prefab) {
        _prefab = prefab;
    }
    public void SetPoolSize(int poolSize) {
        _poolSize = poolSize;
    }

    public List<GameObject> GetInactive() {
        return _inactive;
    }
}
