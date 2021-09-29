using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class TileLabeller : MonoBehaviour
{
    Vector3 scaledPosition;
    
    [SerializeField] TextMeshPro coordinates;
    private void Start()
    {

    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            string direction = "T";
            float d = transform.localEulerAngles.y;
            if (transform.localEulerAngles.x !=0)
            {
                switch (d)
                {
                    case 0: direction = "S"; break;
                    case 90: direction = "W"; break;
                    case 180: direction = "N"; break;
                    case 270: direction = "E"; break;
                }
            }
            
            scaledPosition = transform.position / transform.localScale.x;
            string s = (int)scaledPosition.x + "," + (int)scaledPosition.z + "," + (int)scaledPosition.y+","+direction;
            coordinates.text = s;
            name = "Tile " + s;
            
        }
    }
}