using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IGrowable
{
    [SerializeField] float _growSpeed = 0.1f;
    [SerializeField] float _growthBonus = 0;
    [SerializeField] float _growth = 0;
    [SerializeField] float _swayFrequency = 0.3f;
    [SerializeField] float _seedProbability = 1f;
    [SerializeField] int _maxSeedCount = 1;
    [SerializeField] Tree treePrefab;
    int _seedCount = 0;

    float _swayRandom;
    Tile tile;

    private void Awake()
    {
        UpdateView();
        _swayRandom = Random.value * Mathf.PI * 2;
      

    }

    void Update()
    {
        Grow();
    }

    public void Grow()
    {
        if (_growth < 100)
        {
            _growth += Time.deltaTime * (_growSpeed + _growthBonus);
            _growthBonus *= 0.9f;
        }
        else if (Random.value < _seedProbability && _seedCount < _maxSeedCount)
        {
            Seed();
        }
        UpdateView();
    }

    void Seed() {
        _seedCount++;
        float randomDirection = Mathf.PI * 2 * Random.value;
        Vector3 seedPosition = new Vector3(Mathf.Cos(randomDirection), 0, Mathf.Sin(randomDirection))/2;

        seedPosition += transform.position;
        Tree seed = Instantiate(treePrefab);
        seed.transform.position = seedPosition;
        seed.SetGrowth(0);
        seed.UpdateView();
        seed.gameObject.SetActive(true);
    }

    public void SetGrowthBonus(float growthBonus)
    {
        _growthBonus = growthBonus;
    }

    Vector3 swayDirection = new Vector3(1,0,0);

    private void UpdateView()
    {
        transform.localScale = Vector3.one * _growth / 100;
        transform.localEulerAngles = 3 * Mathf.Sin((_growthBonus + _swayFrequency) * Time.time + _swayRandom) * swayDirection;
    }

    public void SetGrowth(float growth)
    {
        _growth = growth;
    }
}
