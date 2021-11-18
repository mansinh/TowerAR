
using UnityEngine;

[RequireComponent(typeof(SpawnEnemy))]
public class EnemySource : Destroyable, IHoverable
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
    [SerializeField] SpriteRenderer sprite;
    int _currentTypeIndex = 0;
    
    protected override void Init()
    {
        base.Init();
        _spawnEnemy = GetComponent<SpawnEnemy>();
        sprite = _view.GetComponent<SpriteRenderer>();
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
            _view.transform.RotateAroundLocal(Vector3.up, Time.deltaTime);

        }
    }

    public void StartWave(int dayCount)
    {
        _isSpawning = true;
        _spawnEnemy.cooldown = _waveCooldownPerDay.Evaluate(dayCount);
        _currentWaveSize = (int)_waveSizePerDay.Evaluate(dayCount);
        _compositionIndex = dayCount % _typeCompositions.Length;
        sprite.color = Color.white;
    }

    public void StopWave()
    {
        _isSpawning = false;
        sprite.color = new Color(0,0,0,0f);
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


    protected override void Remove()
    {
        base.Remove();
        gameObject.SetActive(false);

        EnemySource[] sources = FindObjectsOfType<EnemySource>();
        bool gameWon = true;
        foreach (EnemySource source in sources)
        {
            if (source.isActiveAndEnabled)
            {
                gameWon = false;
                break;
            }
        }

        if (gameWon)
        {
            GameController.Instance.GameWon();
        }
    }


    public void OnHoverEnter(){}

    public void OnHoverStay()
    {
        GameInfo.Instance.SetHoverText("MONSTER PORTAL: A cataclysm turned the island into a toxic wasteland and opened up these portals. Spawns monsters at night. Destroy all of these to win the game.");
    }

    public void OnHoverLeave()
    {
        GameInfo.Instance.SetHoverText("");
    }

    public ISelectable GetSelectable()
    {
        return null;
    }
}

[System.Serializable]
public class WaveTypeComposition
{
    public static int enemy = 0, fast = 1, flying = 2, tanky = 3;
    public int[] Compositon;
}