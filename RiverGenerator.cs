using UnityEngine;

public class RiverGenerator : ScriptableObject
{
    /* 
    Experimental river generating function 
    */


    private Vector3[] vertices;                 // Generated vertices are stored here locally

    public Vector3[] GenerateRiver(int xSize, int zSize)
    {
        // Initialize vertices array
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        // First generate flat mesh with y = 0
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, 0 ,z);
                i++;
            }
        }

        // Pick starting point
        int xStartPoint;
        int riverOffset;
        float riverDepth;
        
        System.Random rnd = new System.Random();

        xStartPoint = 128;
        riverOffset = 0;
        riverDepth = -1f;

        // Create valley along x-axis from given z starting point
        for (int j = 0; j < xSize; j++)
        {   
            
            // Update current vertex' y-value
            vertices[xStartPoint + (j * (zSize + 1)) + 1][1] = riverDepth;
            vertices[xStartPoint + (j * (zSize + 1))][1]     = riverDepth;
            vertices[xStartPoint + (j * (zSize + 1)) - 1][1] = riverDepth;
            

            riverOffset = rnd.Next(-1, 2); 
            xStartPoint += riverOffset;

            // riverDepth -= j * 0.0001f;
        }   

        // Update its height

        //

        return vertices;

    }
    
}
