using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyCursor : MonoBehaviour
{
    Image cursorImage;
   
    public static MyCursor instance;
   
    void Awake()
    {
        cursorImage = GetComponent<Image>();
        instance = this;
       
    }

    RaycastHit cursorHit;
    bool isCursorHitting;

    public void SetScreenPosition(Vector3 position) {
        cursorImage.rectTransform.position = position;
    }

    public void Cast(Vector3 castFrom)
    {
        cursorImage.color = new Color(1, 1, 1, 0.5f);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(castFrom);
            isCursorHitting = Physics.Raycast(ray, out cursorHit);
            if (isCursorHitting)
            {
                string hitTag = cursorHit.collider.tag;
                switch (hitTag)
                {
                    case "Enemy":
                        {
                            cursorImage.color = Color.magenta;
                            break;
                        }
                    case "Placeable":
                        {
                            cursorImage.color = Color.white;
                            break;
                        }
                    default:
                        {
                            cursorImage.color = Color.gray;
                            break;
                        }
                }
            }

        }
        else {
            isCursorHitting = false;
        }
    }

 

    public RaycastHit GetCursorHit()
    {
        return cursorHit;
    }
    public bool GetCursorHitting()
    {
        return isCursorHitting;
    }
}
