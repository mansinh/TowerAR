using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{
    World world;
    
    private int _brushSize;
    private bool _rounded = false;
    int basePaintHeight = 0;
    int paintHeight = -1;
    int paintState = -1;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (world == null)
        {
            world = (World)target;
        }
        
        if (GUILayout.Button("Create"))
        {
            world.Create();
        }
        /*
        if (GUILayout.Button("Generate"))
        {
            world.Generate();
        }*/

        GUILayout.Label("Edit world (F):");
        GUILayout.Label("Brush Size (PgUp/PgDown):                 " + _brushSize);
        GUILayout.Label("Rounded Brush (Insert):                    " + _rounded);

        GUILayout.Label("Base Paint Height:                                " + basePaintHeight);
        GUILayout.Label("Paint Height (hold numpad)               " + paintHeight);
        GUILayout.Label("Paint State (hold End/Home)             " + paintState);

        if (world.IsGenerating)
        {
            world.Generate();
        }
    }

    

    private void OnSceneGUI()
    {
        OnSceneMouseOver();
        if (world == null)
        {
            world = (World)target;
        }
        world.Draw();
 
        if (tilesEditing.Count > 0)
            OnEdit();

    }
    Tile tileEditing;
    List<Tile> tilesEditing = new List<Tile>();
    void OnSceneMouseOver()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        //And add switch Event.current.type for checking Mouse click and switch tiles
        if (Physics.Raycast(ray, out hit, 1000,LayerMask.GetMask("Tile")))
        {
            if (hit.transform.tag == "Placeable")
            {
                Tile tile = hit.transform.GetComponent<Tile>();
                if (!tile && hit.transform.parent)
                {
                    tile = hit.transform.parent.GetComponent<Tile>();
                }
                if (tile)
                {
                    tileEditing = tile;
                    tilesEditing = GetTilesEditing(tile.Coordinates);
         
                    if (paintHeight > -1)
                    {
                        PaintHeight(paintHeight);
                        world.UpdateView();
                    }
                    if (paintState > -1)
                    {
                        PaintState(paintState);
                        world.UpdateView();
                    }
                }
            }
        }
        else
        {         
            tileEditing = null;
        }
    }
    

    void OnEdit()
    {
        Event e = Event.current;

        

        if (e.type == EventType.KeyUp)
        {
            paintHeight = -1;
            paintState = -1;
            KeyCode key = e.keyCode;
            switch (key)
            {
                case KeyCode.KeypadPlus:
                    tileEditing.Raise();
                    Debug.Log("up");
                    e.Use();
                    break;
                case KeyCode.KeypadMinus:
                    tileEditing.Lower();
                    Debug.Log("down");
                    e.Use();
                    break;
                case KeyCode.KeypadPeriod:
                    basePaintHeight = 0;           
                    e.Use();
                    break;   
            }
        }
   
        if (e.type == EventType.KeyDown)
        {
            KeyCode key = e.keyCode;
            if (key == KeyCode.KeypadPeriod)
            {
                basePaintHeight = 10;
            }

            switch (key)
            {            
                case KeyCode.Keypad0:
                    paintHeight = 0+basePaintHeight;
                    break;
                case KeyCode.Keypad1:
                    paintHeight = 1 + basePaintHeight;
                    break;
                case KeyCode.Keypad2:
                    paintHeight = 2 + basePaintHeight;
                    break;
                case KeyCode.Keypad3:
                    paintHeight = 3 + basePaintHeight;
                    break;
                case KeyCode.Keypad4:
                    paintHeight = 4 + basePaintHeight;
                    break;
                case KeyCode.Keypad5:
                    paintHeight = 5 + basePaintHeight;
                    break;
                case KeyCode.Keypad6:
                    paintHeight = 6 + basePaintHeight;
                    break;
                case KeyCode.Keypad7:
                    paintHeight = 7 + basePaintHeight;
                    break;
                case KeyCode.Keypad8:
                    paintHeight = 8 + basePaintHeight;
                    break;
                case KeyCode.Keypad9:
                    paintHeight = 9 + basePaintHeight;
                    break;
                case KeyCode.End:
                    paintState = 0;
                    e.Use();
                    break;
                case KeyCode.Home:
                    paintState = 1;
                    e.Use();
                    break;
                case KeyCode.PageUp:                  
                    _brushSize++;      
                    e.Use();
                    break;
                case KeyCode.PageDown:
                    if(_brushSize>0)
                    _brushSize--;
                    e.Use();
                    break;
                case KeyCode.Insert:
                    _rounded = !_rounded;
                    e.Use();
                    break;
            }

            

            Repaint();
        }
    }

    void PaintHeight(int height)
    {
        foreach (Tile tile in tilesEditing)
        {
            tile.SetHeight(height);
        }
    }

    void PaintState(int state)
    {
        foreach (Tile tile in tilesEditing)
        {
            tile.SetState(state);
        }
    }

    List<Tile> GetTilesEditing(Vector3Int center)
    {
        tilesEditing.Clear();
        for (int x = -_brushSize; x <= _brushSize; x++)
        {
            for (int z = -_brushSize; z <= _brushSize; z++)
            {
                if (_rounded)
                {
                    if (x * x + z * z > _brushSize * _brushSize)
                    {
                        continue;
                    }
                }
                int tileX = x + center.x;
                int tileZ = z + center.z;
                if (tileX >= 0 && tileX < world.size && tileZ >= 0 && tileZ < world.size)
                {
                    tilesEditing.Add(world.GetTile(tileX, tileZ));
                }
            }
        }
        return tilesEditing;
    }
}
