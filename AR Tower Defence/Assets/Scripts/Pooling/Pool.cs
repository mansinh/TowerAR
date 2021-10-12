using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] Poolable _prefab;
    [SerializeField] int _poolSize = 5;

    private List<Poolable> _inactive = new List<Poolable>();
    private List<Poolable> _active = new List<Poolable>();

    private void Start()
    {
        Init();
    }

    public virtual void Init() {
        for (int i = 0; i < _poolSize; i++)
        {
            Poolable poolable = Instantiate(_prefab, WorldRoot.instance.transform).GetComponent<Poolable>();
            poolable.Init(this);
            _inactive.Add(poolable);
        }
    }

    public Poolable GetNextPoolable() {
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
            Poolable poolable = _inactive[0];
            _active.Add(poolable);
            _inactive.Remove(poolable);
            poolable.OnPush();
        }
    }

    public void Release(Poolable poolable)
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

    public void SetPrefab(Poolable prefab) {
        _prefab = prefab;
    }
    public void SetPoolSize(int poolSize) {
        _poolSize = poolSize;
    }

    public List<Poolable> GetInactive() {
        return _inactive;
    }
}
