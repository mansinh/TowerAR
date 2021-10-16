using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    //A comfortable way to save assets in one place and the pull them out when needed.
    private static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if(_i == null)
            {
                _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _i;
        }
    }

    public Transform damageIndicatorPopup;
}
