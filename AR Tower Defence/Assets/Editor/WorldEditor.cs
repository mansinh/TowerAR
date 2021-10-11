using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldRoot))]
public class WorldEditor : Editor
{
    WorldRoot world;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        world = (WorldRoot)target;
        if (GUILayout.Button("Generate"))
        {
            world.Generate();
        }
        GUILayout.Label("Press F to edit world");

 

    }

    

    private void OnSceneGUI()
    {
        OnSceneMouseOver();
        if (blockEditing)
            OnEditBlock();

    }

    Tile blockEditing = null;
    void OnSceneMouseOver()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        //And add switch Event.current.type for checking Mouse click and switch tiles
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.tag == "Placeable")
            {
                Tile block = hit.transform.GetComponent<Tile>();
                if (!block && hit.transform.parent)
                {
                    block = hit.transform.parent.GetComponent<Tile>();
                }
                if (block)
                {
                    if (block != blockEditing)
                    {
                        if (blockEditing)
                            blockEditing.MouseExit();
                    }
                    blockEditing = block;
                    blockEditing.MouseOver();
                    if (paintHeight > -1)
                    {
                        blockEditing.SetHeight(paintHeight);
                    }
                }
            }
        }
        else
        {
            if (blockEditing)
                blockEditing.MouseExit();
            blockEditing = null;
        }
    }


    int paintHeight = -1;

    void OnEditBlock()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyUp)
        {
            paintHeight = -1;
            KeyCode key = e.keyCode;
            switch (key)
            {
                case KeyCode.KeypadPlus:
                    blockEditing.Raise();
                    Debug.Log("up");
                    e.Use();
                    break;
                case KeyCode.KeypadMinus:
                    blockEditing.Lower();
                    Debug.Log("down");
                    e.Use();
                    break;
                case KeyCode.KeypadMultiply:
                    blockEditing.transform.localEulerAngles += Vector3.up*90;
                    Debug.Log("turn left");
                    e.Use();
                    break;
                case KeyCode.KeypadDivide:
                    blockEditing.transform.localEulerAngles -= Vector3.up * 90;
                    Debug.Log("turn right");
                    e.Use();
                    break;
             
            }

        }
        if (e.type == EventType.KeyDown)
        {
            Debug.Log("editblock");

            KeyCode key = e.keyCode;
            switch (key)
            {
               
                case KeyCode.Keypad0:
                    paintHeight = 0;
                    break;
                case KeyCode.Keypad1:
                    paintHeight = 1;
                    break;
                case KeyCode.Keypad2:
                    paintHeight = 2;
                    break;
                case KeyCode.Keypad3:
                    paintHeight = 3;
                    break;
                case KeyCode.Keypad4:
                    paintHeight = 4;
                    break;
                case KeyCode.Keypad5:
                    paintHeight = 5;
                    break;
                case KeyCode.Keypad6:
                    paintHeight = 6;
                    break;
                case KeyCode.Keypad7:
                    paintHeight = 7;
                    break;
                case KeyCode.Keypad8:
                    paintHeight = 8;
                    break;
                case KeyCode.Keypad9:
                    paintHeight = 9;
                    break;
            }
        }
    }


}
