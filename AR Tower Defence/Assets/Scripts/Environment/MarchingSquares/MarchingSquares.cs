using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    [SerializeField] int _size;
    [SerializeField] int _levels;
    [SerializeField] SpriteRenderer _fieldPoint;
    [SerializeField] SpriteRenderer _view;
    [SerializeField] Sprite[] _states;
    [SerializeField] float _updateTime = 0.1f;
    float[,] _field;
    SpriteRenderer[,] _fieldPoints;
    SpriteRenderer[,,] _viewLevels;



    [SerializeField] float _scale = 1;
    [SerializeField] float _deviation = 1;
    [SerializeField] float _baseHeight = 0;
    [SerializeField] float _perlinHeight = 1;


    float _timeSinceUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        _fieldPoints = new SpriteRenderer[_size, _size];
        _field = new float[_size,_size];
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {

                _fieldPoints[x, z] = Instantiate(_fieldPoint.gameObject, transform).GetComponent<SpriteRenderer>();
                _fieldPoints[x, z].transform.localPosition = new Vector3(x - _size / 2, 0, z - _size / 2);
            }
        }

       _viewLevels = new SpriteRenderer[_size - 1, _size - 1,_levels];
        for (float y = 0; y < _levels; y++)
        {

            print(y);
            for (int x = 0; x < _size - 1; x++)
            {
                for (int z = 0; z < _size - 1; z++)
                {

                    _viewLevels[x, z, (int)y] = Instantiate(_view.gameObject, transform).GetComponent<SpriteRenderer>();
                    _viewLevels[x, z, (int)y].transform.localPosition = new Vector3(x + 0.5f - _size / 2, y, z + 0.5f - _size / 2);
                    _viewLevels[x, z, (int)y].color = Color.Lerp(Color.black, Color.white, y/_levels);
                }
            }
        }
    }

    private void Update()
    {
        if (_timeSinceUpdate < 0)
        {
            for (int x = 0; x < _size; x++)
            {
                for (int z = 0; z < _size; z++)
                {
                   _field[x, z] = GetValue(x, z);
                   _fieldPoints[x, z].color = new Color(1, 1, 1) * _field[x, z];
                }
            }

            for (float i = 0; i < _levels; i++)
            {
                DrawLevel(i);
            }
            _timeSinceUpdate = _updateTime;
        }
        _timeSinceUpdate -= Time.deltaTime;
    }

   
    void DrawLevel(float level) {
        for (int x = 0; x < _size - 1; x++)
        {
            for (int z = 0; z < _size - 1; z++)
            {
                _viewLevels[x, z, (int)level].sprite = _states[GetState(x, z, (level+1)/(_levels+1))];
            }
        }
    }

    int GetState(int x, int z, float level)
    {
        int a = State(_field[x, z], level);
        int b = State(_field[x + 1, z], level);
        int c = State(_field[x + 1, z + 1], level);
        int d = State(_field[x, z + 1], level);
        return a + b * 2 + c * 4 + d *8;
    }

    int State(float level, float value)
    {
        if (value > level)
        {
            return 0;
        }
        return 1;
    }
   
   
    float GetValue(float x, float z)
    {
        float xCoord = x / _size * _scale;
        float zCoord = z / _size * _scale;
        //return Random.value;

        float xDisp = x- _size / 2;
        float zDisp = z - _size / 2;
        float gaussian = Mathf.Exp(-(xDisp*xDisp + zDisp*zDisp)/(2*_deviation* _deviation));
       
        return Mathf.Max(0,Mathf.Min(1,_perlinHeight*Mathf.PerlinNoise(xCoord,zCoord)*gaussian+_baseHeight));
    }
}
