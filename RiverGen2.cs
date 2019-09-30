using UnityEngine;

public class RiverGen2 : ScriptableObject
{
    // Function that generates rivers
    // Input: mesh size
    // Output: 2D array of point values, size: xSize + 1 * zSize + 1

    public int[,] riverHeightMap(int xSize, int zSize)
    {   
        // Create map height matrix
        int[,] heightMap;
        heightMap = new int[xSize + 1, zSize + 1];
        for (int x = 0; x < xSize + 1; x++ ){
            for (int z = 0; z < zSize + 1; z++){
             heightMap[x,z] = 0;
            }
        }
        Debug.Log(heightMap);

        // Random starting point
        int[] startingPoint;
        startingPoint = new int[2];
        System.Random rnd = new System.Random();
        startingPoint[0] = xSize;
        startingPoint[1] = zSize; 
        

        return heightMap;

    }

        


}
