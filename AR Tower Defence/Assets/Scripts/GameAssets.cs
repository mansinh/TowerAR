using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    //A comfortable way to save assets in one place and the pull them out when needed.
    private static GameAssets _instance;
    public static GameAssets instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _instance;
        }
    }

    public Transform damageIndicatorPopup;
}
