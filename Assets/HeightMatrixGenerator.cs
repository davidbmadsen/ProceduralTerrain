using System.Collections;
using UnityEngine;
using static TerrainNode;

public class HeightMatrix : ScriptableObject
{
    // General class for generating the heigh matrix, which describes the height of all points on the mesh
    // for a given feature.

    public TerrainNode[,] GenerateZeroMap(int xSize, int zSize)
    {   /* 
        This function generates the flat heighmap, which is the base for all feature maps (mountains, rivers, lakes, etc.)
        Input: mesh size (xSize, zSize)
        Output: zeroMap 2D array of floats (for now)
        */
        int[] coords = new int[2];

        // Initialize the object matrix
        TerrainNode[,] zeroMap = new TerrainNode[xSize + 1, zSize + 1];
        TerrainNode newNode;

        // Initialize matrix with null nodes
        for (int x = 0; x < xSize + 1; x++ ){
            for (int z = 0; z < zSize + 1; z++){
                newNode = ScriptableObject.CreateInstance<TerrainNode>();
                newNode.Node(x, z, 0f, "null");
                zeroMap[x,z] = newNode;
            }
        }

        return zeroMap;

    }

    public TerrainNode[,] GenerateNoisyMountains(TerrainNode[,] heightMap){
        
        // Declare variables
        int xSize, zSize;
        float height;

        // Get map sizes from zeroMap
        xSize = heightMap.GetLength(0);
        zSize = heightMap.GetLength(1);

        NoiseGenerator noiseGen;
        noiseGen = ScriptableObject.CreateInstance<NoiseGenerator>();

        for (int x = 0; x < xSize; x++){
            for (int z = 0; z < zSize; z++){
                height = noiseGen.GenerateNoise(x, z, 2f, 0.5f, 3, 50f, 5f, true);
                heightMap[x,z].height = height;
                heightMap[x,z].type = "mountain";
            }
        }

        return heightMap;

    }


    public TerrainNode[,] GeneratePyramidMountains(TerrainNode[,] heightMap, bool randomMountains=true){

        // Declare variables
        int xSize, zSize, size, min, max, mtnsLength;
        int[] centerpoint, mtnPos;
        int[,] mountainsPos;
        float height;

        // Declare objects
        System.Random rnd = new System.Random();

        // Get map sizes from zeroMap
        xSize = heightMap.GetLength(0);
        zSize = heightMap.GetLength(1);

        // Mountain creation boundaries
        min = 15;
        max = xSize - min;

        centerpoint = new int[2];

        // Array for storing mountain positions
        mountainsPos = new int[2,200] {   
            { 57, 77, 129, 26, 48, 112, 90, 51, 205, 130, 96, 196, 234, 31, 147, 66, 129, 151, 35, 72, 27, 221, 127, 82, 80, 111, 202, 226, 24, 24, 51, 55, 104, 166, 143, 203, 182, 40, 159, 154, 188, 208, 133, 180, 231, 117, 174, 171, 222, 59, 29, 109, 54, 185, 145, 220, 123, 147, 208, 93, 147, 43, 86, 110, 41, 199, 22, 59, 145, 122, 147, 197, 30, 198, 148, 191, 117, 156, 44, 126, 24, 91, 48, 181, 185, 139, 41, 110, 184, 43, 221, 155, 233, 231, 82, 150, 219, 139, 49, 65, 228, 215, 210, 142, 172, 198, 223, 176, 179, 198, 94, 195, 84, 70, 70, 165, 168, 199, 202, 196, 133, 23, 188, 158, 145, 88, 96, 188, 93, 178, 49, 77, 165, 138, 201, 69, 149, 126, 79, 131, 119, 39, 148, 41, 192, 46, 108, 70, 171, 172, 110, 159, 141, 84, 193, 205, 212, 112, 50, 198, 61, 51, 145, 157, 30, 49, 174, 193, 102, 201, 30, 87, 98, 229, 229, 144, 48, 235, 235, 48, 31, 229, 56, 58, 42, 150, 202, 161, 232, 231, 187, 194, 174, 58, 150, 70, 73, 37, 47, 87 },
            { 82, 209, 148, 209, 150, 188, 109, 95, 97, 162, 168, 85, 235, 98, 185, 44, 33, 221, 88, 100, 189, 75, 86, 221, 199, 212, 152, 208, 229, 42, 26, 212, 91, 136, 178, 94, 83, 68, 232, 27, 89, 234, 20, 229, 32, 29, 113, 84, 210, 226, 173, 29, 153, 148, 65, 22, 67, 123, 156, 119, 72, 112, 215, 229, 143, 209, 121, 173, 194, 221, 153, 142, 140, 64, 230, 234, 90, 92, 24, 138, 29, 61, 161, 99, 32, 81, 37, 88, 194, 207, 120, 136, 101, 31, 226, 182, 45, 43, 91, 126, 173, 50, 164, 70, 189, 42, 77, 68, 221, 153, 222, 113, 138, 126, 25, 170, 74, 183, 30, 214, 223, 214, 110, 145, 210, 52, 198, 186, 25, 36, 139, 223, 221, 37, 230, 188, 82, 190, 54, 200, 47, 99, 33, 87, 165, 176, 168, 193, 73, 33, 170, 58, 36, 48, 26, 20, 23, 73, 58, 232, 199, 33, 145, 59, 52, 84, 84, 231, 85, 59, 95, 98, 63, 179, 89, 233, 97, 136, 232, 44, 50, 219, 66, 186, 155, 196, 63, 76, 41, 54, 56, 95, 169, 88, 158, 183, 147, 222, 127, 194}
            };
        
        mtnPos = new int[2];
        mtnsLength = mountainsPos.GetLength(1);

        //mountainsPos[0] = mountainsPos[1] = {   96, 116, 33, 183, 161, 104, 205, 55, 151, 122, 120, 143, 217, 44, 52, 181, 132, 199, 103, 200, 224, 74, 76, 203, 162, 70, 108, 177, 149, 212, 226, 148, 184, 154, 131, 173, 121, 220, 209, 154, 70, 145, 162, 101, 181, 56, 44, 240, 59, 100, 119, 176, 207, 206, 58, 86, 71, 37, 240, 154, 113, 146, 75, 145, 217, 117, 97, 122, 227, 135, 209, 51, 220, 144, 160, 198, 236, 136, 139, 108, 160, 104, 225, 196, 19, 88, 112, 91, 72, 231, 240, 69, 131, 62, 16, 114, 61, 55, 180, 16, 38, 54, 146, 230, 139, 82, 52, 231, 51, 236, 16, 127, 140, 189, 151, 127, 79, 114, 119, 31, 172, 17, 129, 200, 150, 236, 158, 105, 210, 88, 231, 93, 19, 214, 208, 187, 23, 25, 238, 30, 32, 60, 225, 15, 25, 66, 190, 195, 174, 150, 70, 230, 43, 216, 206, 221, 188, 65, 234, 219, 236, 138, 39, 79, 149, 113, 216, 169, 21, 76, 84, 235, 208, 194, 44, 39, 89, 193, 236, 225, 169, 66, 111, 91, 216, 183, 187, 37, 126, 57, 68, 56, 102, 174, 60, 122, 169, 95, 240, 37   };
        
        if (randomMountains){
            
            // Assign random positions to mountains (10 mountains in this case)
             for (int i = 0; i < mtnsLength; i++){
                mountainsPos[0,i] = rnd.Next(min, max);
                mountainsPos[1,i] = rnd.Next(min, max);
            }
        }

        
        // Generate the mountains
        for (int i = 0; i < mtnsLength; i++){
            size = rnd.Next(5, 20);
            mtnPos[0] = mountainsPos[0,i];
            mtnPos[1] = mountainsPos[1,i];
            heightMap = PyramidGen(heightMap, mtnPos, size);
        }

        

        return heightMap;
    }

    public TerrainNode[,] PyramidGen(TerrainNode[,] heightMap, int[] centerpoint, int size){

        // Declare variables
        int edge;

        // Layers in the pyramid, one i per layer
        // side length
        edge = 0;
        while (edge != size){
            for(int x = edge; x <= (size - edge); x++){
                for(int z = edge; z <= (size - edge); z++){
                    // Raise terrain here by one
                    heightMap[x + centerpoint[0], z + centerpoint[1]].height += 1f;
                    heightMap[x + centerpoint[0], z + centerpoint[1]].type = "mountain";
                    
                }
            }

            // Decrease edge counter
            edge++;
        }

        return heightMap;

    }

    public TerrainNode[,] GenerateNaturalRivers(TerrainNode[,] heightMap){

        // Declare variables
        int[] startPoint, endPoint, middlePoint;
        float[] depth;
        int xSize, zSize;
        
        // Get map sizes from zeroMap
        xSize = heightMap.GetLength(0);
        zSize = heightMap.GetLength(1);

        // Declare objects
        System.Random rnd = new System.Random();

        // Pick random start- and endpoint 
        startPoint = new int[2] { rnd.Next(15,xSize -16), rnd.Next(15,zSize -16) };
        endPoint = new int[2] { rnd.Next(15,xSize -16), rnd.Next(15,zSize -16)};
        depth = new float[2] { 0, -5 };
        
        // Start recursion
        heightMap = RecursiveRiverGen(heightMap, startPoint, endPoint, depth);

        return heightMap;

    }

    public TerrainNode[,] RecursiveRiverGen(TerrainNode[,] heightMap, int[] startPoint, int[] endPoint, float[] depth){
        
        // Declare variables
        int[] middlePoint;
        float[] depthOne, depthTwo;
        float middleDepth, length, scale, zDiff, xDiff, angle, middleVal, branchDeviation;
        
        middlePoint = new int[2];

        // Calculate distance between start- and endpoints (pythagorean formula) plus the angle of the line segment
        xDiff = Mathf.Abs(endPoint[0] - startPoint[0]);
        zDiff = Mathf.Abs(endPoint[1] - startPoint[1]);
        length = Mathf.Sqrt(Mathf.Pow(xDiff, 2) + Mathf.Pow(zDiff, 2));
        angle = Mathf.Atan(xDiff/zDiff);
        
        // Calculate the middle depth between s and e
        middleDepth = (depth[0] + depth[1])/2;

        // Find middle value
        middleVal = Mathf.Round(length/2);
    
        // Pick corner point out from middle value (linear for now). 
        // Using UnityEngine.Random as the Range function overrides the System.Random class.
        // This will be how far out (orthogonally) from the original river line the corner point will be
        branchDeviation = UnityEngine.Random.Range(-length/3, length/3);

        // Calculate the middle point between the start- and end point, as well as the corner point
        // x-values
        if (startPoint[0] < endPoint[0]){
            middlePoint[0] = startPoint[0] + Mathf.RoundToInt(xDiff/2);
            middlePoint[0] += Mathf.RoundToInt(branchDeviation * Mathf.Cos(angle));
        }
        else{
            middlePoint[0] = startPoint[0] - Mathf.RoundToInt(xDiff/2);
            middlePoint[0] -= Mathf.RoundToInt(branchDeviation * Mathf.Cos(angle));
        }

        // z-values
        if (startPoint[1] < endPoint[1]){
            middlePoint[1] = startPoint[1] + Mathf.RoundToInt(zDiff/2);
            middlePoint[1] -= Mathf.RoundToInt(branchDeviation * Mathf.Sin(angle));
        }
        else{
            middlePoint[1] = startPoint[1] - Mathf.RoundToInt(zDiff/2);
            middlePoint[1] += Mathf.RoundToInt(branchDeviation * Mathf.Sin(angle));
        }

        // Create the river bed
        RiverBed(heightMap, startPoint, depth[0]);
        RiverBed(heightMap, middlePoint, middleDepth);
        RiverBed(heightMap, endPoint, depth[1]);

        // New depth-arrays for the recursive calls
        depthOne = new float[2] { depth[0], middleDepth };
        depthTwo = new float[2] { middleDepth, depth[1] };

        // Recurse as long as the length isn't 1
        if (Mathf.RoundToInt(length) > 1){
            // Update depth for each recursive call
            heightMap = RecursiveRiverGen(heightMap, startPoint, middlePoint, depthOne);
            heightMap = RecursiveRiverGen(heightMap, middlePoint, endPoint, depthTwo);
        }
        
        return heightMap;
        
    }

    public TerrainNode[,] RiverBed(TerrainNode[,] heightMap, int[] point, float depth){
        /*
        Carves out a 5x5 pseudo-circular surface at point with depth
        */

        // Declare variables
        int[,] profile;
        int dims;

        // Center the river bed
        point = new int[2] { point[0] - 3, point[1] - 3};

        profile = new int[5,5] {
            {0, 1, 1, 1, 0},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {0, 1, 1, 1, 0} };

        dims = profile.GetLength(0);
        for (int x = 0; x < dims; x++){
            for (int z = 0; z < dims; z++){
                // We only want to modify the river depth where the profile is "1"
                if (profile[x,z] == 1){
                    heightMap[x + point[0], z + point[1]].height = depth * profile[x,z];
                    heightMap[x + point[0], z + point[1]].type = "river";
                }
                
            }
        }

        return heightMap;
    }

    public TerrainNode[,] MergeTerrainMaps(TerrainNode[,] heightMapOne, TerrainNode[,] heightMapTwo){
        /*
        Function for merging terrain heightMaps
        Input: Featuremaps to be merged
        Output: The merged feature map 2D array of height values
        
        Temp: one = river, two = mountains
        */

        // Declare variables
        int xSize, zSize;
        float a, b;
        float[,] mergedHeightMap;

        
        // Get map sizes from zeroMap
        xSize = heightMapOne.GetLength(0);
        zSize = heightMapOne.GetLength(1);

        // Initialize final height map array
        // mergedHeightMap = new float[xSize + 1, zSize + 1];


        // Compare each element
        for (int x = 0; x < xSize; x++){
            for (int z = 0; z < zSize; z++){
                // Some logic that merges terrain features
                // heightMap[x,z] = 0 means no feature is present
                // heightMap[x,z] > 0 means the point is terrain
                // heightMap[x,z] < 0 means the point is riverbed

                a = heightMapOne[x,z];
                b = heightMapTwo[x,z];

                // Check for river
                if (a < 0){
                    mergedHeightMap[x,z] = a;
                }
                // Else it's mountain (aka b)
                else{
                    mergedHeightMap[x,z] = b;
                }
                
            }
        }



        return mergedHeightMap;
    }
}
