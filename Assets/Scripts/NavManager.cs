using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    NavMeshSurface[] navSurfaces;
    // Start is called before the first frame update
    void Awake()
    {
        navSurfaces = FindObjectsOfType<NavMeshSurface>();
    }


    public void Bake() {
        foreach (NavMeshSurface navSurface in navSurfaces) {
            navSurface.BuildNavMesh();
        }
    }
}
