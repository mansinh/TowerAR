
using UnityEngine;

[RequireComponent(typeof(SpawnEnemy))]
public class EnemySource : Destroyable
{
    SpawnEnemy _spawnEnemy;
    
    [Space(10)]
    [Header("WAVE COMPOSER")]
    [Header("________________________________________________________________________")]
 
 
    [SerializeField] AnimationCurve _waveSizePerDay;
    [SerializeField] AnimationCurve _waveCooldownPerDay;
    [Space(10)]
    [Header("Base = 0,  Fast = 1,  Flying = 2,  Tanky = 3")]
    [SerializeField] WaveTypeComposition[] _typeCompositions;
  
    [Space(10)]
    [Header("WAVE STATUS")]
    [Header("________________________________________________________________________")]
  
    [SerializeField] bool _isSpawning = false;
    [SerializeField] int _currentWaveSize;
    [SerializeField] int _enemiesSpawned = 0;
    [SerializeField] int _compositionIndex = 0;
    [SerializeField] string _currentType = "";
    int _currentTypeIndex = 0;
    
    protected override void Init()
    {
        base.Init();
        _spawnEnemy = GetComponent<SpawnEnemy>();
    }

    void Update() {
        if (_isSpawning)
        {
            int[] composition = _typeCompositions[_compositionIndex].Compositon;

            _currentTypeIndex = _currentTypeIndex % composition.Length;
            _spawnEnemy.SetEnemyType(composition[_currentTypeIndex]);
            ShowCurrentEnemyType(composition[_currentTypeIndex]);

            if (_spawnEnemy.Activate(transform.position))
            {
                _currentTypeIndex++;
                _enemiesSpawned++;
                if (_enemiesSpawned >= _currentWaveSize)
                {
                    StopWave();
                }
            }
        }
    }

    public void StartWave(int dayCount)
    {
        _isSpawning = true;
        _spawnEnemy.cooldown = _waveCooldownPerDay.Evaluate(dayCount);
        _currentWaveSize = (int)_waveSizePerDay.Evaluate(dayCount);
        _compositionIndex = dayCount % _typeCompositions.Length;
    }

    public void StopWave()
    {
        _isSpawning = false;
       
    }

    public void ResetWave()
    {
        StopWave();
        _enemiesSpawned = 0;
        _currentTypeIndex = 0;
        _spawnEnemy.Reset();
    }

    void ShowCurrentEnemyType(int type)
    {
        switch (type)
        {
            case 0: _currentType = "Base";break;
            case 1: _currentType = "Fast"; break;
            case 2: _currentType = "Flying"; break;
            case 3: _currentType = "Tanky"; break;
        }
    }
}

[System.Serializable]
public class WaveTypeComposition
{
    public static int enemy = 0, fast = 1, flying = 2, tanky = 3;
    public int[] Compositon;
}