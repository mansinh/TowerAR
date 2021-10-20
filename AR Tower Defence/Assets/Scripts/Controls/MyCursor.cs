using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MyCursor : MonoBehaviour
{
    Image cursorImage;
   
    public static MyCursor Instance;
   
    void Awake()
    {
        cursorImage = GetComponent<Image>();
        Instance = this;
       
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
            if (GetIsActionable())
            {
                string hitTag = cursorHit.collider.tag;
                switch (hitTag)
                {
                    case "Enemy":
                        {
                            cursorImage.color = Color.magenta;
                            break;
                        }
                   
                    default:
                        {
                            cursorImage.color = Color.green;
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
    public bool GetIsActionable() {
        if (isCursorHitting)
        {
            try
            {
                Vector3 p = cursorHit.point - WorldRoot.Instance.transform.position;
                int tileX = Mathf.RoundToInt(p.x + WorldRoot.Instance.size / 2);
                int tileZ = Mathf.RoundToInt(p.z + WorldRoot.Instance.size / 2);
                int tileIndex = tileZ + tileX * WorldRoot.Instance.size;

                return !WorldRoot.Instance.tiles[tileIndex].GetCorrupt();
            }
            catch (Exception e)
            {
            }
        }
        return false;
    }
}
