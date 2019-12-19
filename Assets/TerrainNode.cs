using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainNode : ScriptableObject
{
    /*
    Terrain node class
    x, z: int - containing the x and z coords of the node
    Height: float - contains the height of the point in the terrain
    Type: string - type of terrain that the node is describing, current types are "river", "mountain", "null"
    */   

    public int x, z;
    public float height;
    public string type;

    // Node constructor - Initializes the node with input data
    public void Create(int x, int z, float height, string type){
        this.x = x;
        this.z = z;
        this.height = height;
        this.type = type;
    }
}
