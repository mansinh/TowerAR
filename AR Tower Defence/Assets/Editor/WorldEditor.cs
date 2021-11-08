
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * Editor for creating world/environment/levels
 *@ author Manny Kwong 
 */

[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{
    World world;
    
    private int _brushSize;
    private bool _rounded = false;
    int basePaintHeight = 0;
    int paintHeight = -1;
    float paintState = -1;

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        if (world == null)
        {
            world = (World)target;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Create"))
        {
            world.Create();
        }
        /*
        if (GUILayout.Button("Generate"))
        {
            world.Generate();
        }*/
        GUILayout.Space(10);
        GUILayout.Label("______________________________________________________________________");
        GUILayout.Space(10);
        GUILayout.Label("EDITOR CONTROLS", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("To edit world, hover mouse over the world and press F");
        GUILayout.Label("(Make sure debug mode is off)");
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Size (PgUp/PgDown):");
        GUILayout.Label(""+_brushSize, EditorStyles.miniButtonRight, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Shape (Insert):");
        GUILayout.Label(ShowBrushShape(), EditorStyles.miniButtonRight, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Base Paint Height:" );
        GUILayout.Label("" + basePaintHeight, EditorStyles.miniButtonRight, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Paint Height (hold numpad):" );
        GUILayout.Label(ShowHeight(), EditorStyles.miniButtonRight, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Paint State (hold End/Home):" );
        GUILayout.Label(ShowState(), EditorStyles.miniButtonRight, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (world.IsGenerating)
        {
            world.Generate();
          
        }
    }

    string ShowState()
    {
        switch (paintState)
        {
            case 1: return "Restored";
            case 0: return "Desert";
        }
        return "Off";
    }

    string ShowHeight()
    {
        if (paintHeight >= 0)
        {
            return "" + paintHeight;
        }
        return "Off";
    }

    string ShowBrushShape()
    {
        if (_rounded) {
            return "Round";
        }
        return "Square";
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

    //Get the tile the mouse is over and modify it
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
   
        if (e.type == EventType.KeyDown)
        {
            //Add 10 to paint height if period held down
            KeyCode key = e.keyCode;
            if (key == KeyCode.KeypadPeriod)
            {
                basePaintHeight = 10;
            }
            //Paint tile height to Numpad number
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
                    paintState = Tile.DESERT;
                    e.Use();
                    break;
                case KeyCode.Home:
                    paintState = Tile.RESTORED;
                    e.Use();
                    break;
                case KeyCode.PageUp:   //Increase brush size
                    _brushSize++;      
                    e.Use();
                    break;
                case KeyCode.PageDown: //Decrease brush size
                    if (_brushSize>0)
                    _brushSize--;
                    e.Use();
                    break;
                case KeyCode.Insert: //Toggle brush shape between square and round 
                    _rounded = !_rounded;
                    e.Use();
                    break;
            }           
        }

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

        Repaint();
    }

    void PaintHeight(int height)
    {
        foreach (Tile tile in tilesEditing)
        {
            tile.SetHeight(height);
        }
    }

    void PaintState(float state)
    {
        foreach (Tile tile in tilesEditing)
        {
            tile.SetState(state);
        }
    }

    //Get a set of tiles to edit
    List<Tile> GetTilesEditing(Vector3Int center)
    {
        try
        {
            foreach (Tile tile in tilesEditing)
            {
                tile.ResetDecorator();
            }
        }
        catch (Exception e)
        {

        }
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
                    Tile tile = world.GetTile(tileX, tileZ);
                    tilesEditing.Add(tile);
                    tile.Select();
                }
            }
        }
        return tilesEditing;
    }
}
