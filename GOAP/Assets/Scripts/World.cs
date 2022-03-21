using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private static readonly World instance = new World();
    private static WorldStates worldStates;

    static World()
    {
        worldStates = new WorldStates();
    }

    private World()
    {

    }

    public static World Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
