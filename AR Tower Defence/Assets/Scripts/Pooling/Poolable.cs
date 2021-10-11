using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    protected Pool Source;

    public virtual void Init(Pool pool) {
        this.Source = pool;     
        gameObject.SetActive(false);
    }

    public virtual void OnPush() {
        gameObject.SetActive(true);
    }
    public virtual void OnRelease() {
        Source.Release(this);
        gameObject.SetActive(false);
    }
}
