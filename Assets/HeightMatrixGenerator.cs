using System;
using System.Collections.Generic;
using UnityEngine;
using static TerrainNode;

public class HeightMatrix : ScriptableObject
{
    // General class for generating the heigh matrix, which describes the height of all points on the mesh
    // for a given feature. Also includes the merge function.

    // Pseudorandom number generator
    public System.Random rnd = new System.Random();

    public TerrainNode[,] GenerateZeroMap(int xSize, int zSize)
    {   /* 
        This function generates the flat heighmap, which is the base for all feature maps (mountains, rivers, lakes, etc.)
        Input: Terrain map size (xSize, zSize)
        Output: zeroMap: 2D array of TerrainNode-objects
        */

        // Initialize the object matrix
        TerrainNode[,] zeroMap = new TerrainNode[xSize + 1, zSize + 1];
        TerrainNode Node;

        // Initialize matrix with null nodes
        for (int x = 0; x < xSize + 1; x++)
        {
            for (int z = 0; z < zSize + 1; z++)
            {
                Node = ScriptableObject.CreateInstance<TerrainNode>();
                Node.Create(x, z, 0f, "null");
                zeroMap[x, z] = Node;
            }
        }

        return zeroMap;
    }

    public TerrainNode[,] GenerateNoisyMountains(TerrainNode[,] heightMap)
    {

        // Declare variables
        int xSize, zSize;
        float height;

        // Get map sizes from zeroMap
        xSize = heightMap.GetLength(0);
        zSize = heightMap.GetLength(1);

        NoiseGenerator noiseGen;
        noiseGen = ScriptableObject.CreateInstance<NoiseGenerator>();

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                height = noiseGen.GenerateNoise(x, z, 2f, 0.5f, 4, 50f, 10f, true);
                heightMap[x, z].height = height;
                heightMap[x, z].type = "mountain";
            }
        }

        return heightMap;

    }


    public TerrainNode[,] GeneratePyramidMountains(TerrainNode[,] heightMap, bool randomMountains = true)
    {
        /*
        Function generates pyramid mountains
        
        randomMountains: boolean flag to create randomly distributed mountains or from a fixed list
        */


        // Declare variables
        int xSize, zSize, size, min, max, mtnsLength;
        int[] centerpoint, mtnPos;
        int[,] mountainsPos;


        // Get map sizes from zeroMap
        xSize = heightMap.GetLength(0);
        zSize = heightMap.GetLength(1);

        // Mountain creation boundaries
        min = 15;
        max = xSize - min;

        centerpoint = new int[2];
        mtnPos = new int[2];

        // Decide whether or not to make random or fixed mountains based on flag
        if (randomMountains)
        {
            mtnsLength = 200;
            mountainsPos = new int[2, mtnsLength];

            // Assign random positions to mountains
            for (int i = 0; i < mtnsLength; i++)
            {
                mountainsPos[0, i] = rnd.Next(min, max);
                mountainsPos[1, i] = rnd.Next(min, max);
            }
        }
        else
        {
            // Array for storing fixed mountain positions
            mountainsPos = new int[2, 200] {
            { 57, 77, 129, 26, 48, 112, 90, 51, 205, 130, 96, 196, 234, 31, 147, 66, 129, 151, 35, 72, 27, 221, 127, 82, 80, 111, 202, 226, 24, 24, 51, 55, 104, 166, 143, 203, 182, 40, 159, 154, 188, 208, 133, 180, 231, 117, 174, 171, 222, 59, 29, 109, 54, 185, 145, 220, 123, 147, 208, 93, 147, 43, 86, 110, 41, 199, 22, 59, 145, 122, 147, 197, 30, 198, 148, 191, 117, 156, 44, 126, 24, 91, 48, 181, 185, 139, 41, 110, 184, 43, 221, 155, 233, 231, 82, 150, 219, 139, 49, 65, 228, 215, 210, 142, 172, 198, 223, 176, 179, 198, 94, 195, 84, 70, 70, 165, 168, 199, 202, 196, 133, 23, 188, 158, 145, 88, 96, 188, 93, 178, 49, 77, 165, 138, 201, 69, 149, 126, 79, 131, 119, 39, 148, 41, 192, 46, 108, 70, 171, 172, 110, 159, 141, 84, 193, 205, 212, 112, 50, 198, 61, 51, 145, 157, 30, 49, 174, 193, 102, 201, 30, 87, 98, 229, 229, 144, 48, 235, 235, 48, 31, 229, 56, 58, 42, 150, 202, 161, 232, 231, 187, 194, 174, 58, 150, 70, 73, 37, 47, 87 },
            { 82, 209, 148, 209, 150, 188, 109, 95, 97, 162, 168, 85, 235, 98, 185, 44, 33, 221, 88, 100, 189, 75, 86, 221, 199, 212, 152, 208, 229, 42, 26, 212, 91, 136, 178, 94, 83, 68, 232, 27, 89, 234, 20, 229, 32, 29, 113, 84, 210, 226, 173, 29, 153, 148, 65, 22, 67, 123, 156, 119, 72, 112, 215, 229, 143, 209, 121, 173, 194, 221, 153, 142, 140, 64, 230, 234, 90, 92, 24, 138, 29, 61, 161, 99, 32, 81, 37, 88, 194, 207, 120, 136, 101, 31, 226, 182, 45, 43, 91, 126, 173, 50, 164, 70, 189, 42, 77, 68, 221, 153, 222, 113, 138, 126, 25, 170, 74, 183, 30, 214, 223, 214, 110, 145, 210, 52, 198, 186, 25, 36, 139, 223, 221, 37, 230, 188, 82, 190, 54, 200, 47, 99, 33, 87, 165, 176, 168, 193, 73, 33, 170, 58, 36, 48, 26, 20, 23, 73, 58, 232, 199, 33, 145, 59, 52, 84, 84, 231, 85, 59, 95, 98, 63, 179, 89, 233, 97, 136, 232, 44, 50, 219, 66, 186, 155, 196, 63, 76, 41, 54, 56, 95, 169, 88, 158, 183, 147, 222, 127, 194}
            };

            // Get length of array
            mtnsLength = mountainsPos.GetLength(1);
        }

        // Generate the mountains
        for (int i = 0; i < mtnsLength; i++)
        {
            size = rnd.Next(5, 20);
            mtnPos = new int[2] { mountainsPos[0, i], mountainsPos[1, i] };
            heightMap = PyramidGen(heightMap, mtnPos, size);
        }

        return heightMap;
    }

    public TerrainNode[,] PyramidGen(TerrainNode[,] heightMap, int[] centerpoint, int size)
    {

        // Declare variables
        int edge;
        int[] offset;

        // Layers in the pyramid, one i per layer
        offset = new int[2] {
            centerpoint[0] - Mathf.RoundToInt(size/2),
            centerpoint[1] - Mathf.RoundToInt(size/2) };
        edge = 0;
        while (edge != size)
        {
            for (int x = edge; x <= (size - edge); x++)
            {
                for (int z = edge; z <= (size - edge); z++)
                {
                    // Raise terrain here by one
                    heightMap[x + offset[0], z + offset[1]].height += 1f;
                    heightMap[x + offset[0], z + offset[1]].type = "mountain";

                }
            }

            // Increase edge counter
            edge++;
        }

        return heightMap;
    }

    public TerrainNode[,] GenerateNaturalRivers(TerrainNode[,] heightMap, float startDepth, float endDepth)
    {

        // Declare variables
        int[] startPoint, endPoint;
        float[] depth;
        int xSize, zSize;

        // Get map sizes from zeroMap
        xSize = heightMap.GetLength(0);
        zSize = heightMap.GetLength(1);

        // Pick random start- and endpoint 
        startPoint = new int[2] { rnd.Next(15, xSize - 16), rnd.Next(15, zSize - 16) };
        endPoint = new int[2] { rnd.Next(15, xSize - 16), rnd.Next(15, zSize - 16) };
        depth = new float[2] { startDepth, endDepth };

        // Start recursion
        heightMap = RecursiveRiverGen(heightMap, startPoint, endPoint, depth);

        return heightMap;

    }

    public TerrainNode[,] RecursiveRiverGen(TerrainNode[,] heightMap, int[] startPoint, int[] endPoint, float[] depth)
    {
        /*
        Function for recursively generating river patterns
        */

        // Declare variables
        int[] mStar;
        float[] depthFirst, depthSecond;
        float middleDepth, length, deltaZ, deltaX, angle, middleVal, dist;

        mStar = new int[2];

        // Calculate distance between start- and endpoints (pythagorean formula) plus the angle of the line segment
        deltaX = Mathf.Abs(endPoint[0] - startPoint[0]);
        deltaZ = Mathf.Abs(endPoint[1] - startPoint[1]);
        length = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaZ, 2));
        angle = Mathf.Atan(deltaX / deltaZ);

        // Calculate the middle depth between s and e
        middleDepth = (depth[0] + depth[1]) / 2;

        // Find middle value
        middleVal = Mathf.Round(length / 2);

        // Pick corner point distance. 
        //Optional parameter: "cubic" for cubic distribution
        dist = ChooseLength(length);

        // Calculate the middle point between the start- and end point, as well as the corner point m*
        mStar[0] = Mathf.Min(startPoint[0], endPoint[0]) + Mathf.RoundToInt(deltaX / 2);
        mStar[1] = Mathf.Min(startPoint[1], endPoint[1]) + Mathf.RoundToInt(deltaZ / 2);

        // x-values
        if (startPoint[0] > endPoint[0])
        {
            mStar[0] += Mathf.RoundToInt(dist * Mathf.Cos(angle));
        }
        else
        {
            mStar[0] -= Mathf.RoundToInt(dist * Mathf.Cos(angle));
        }

        // z-values
        if (startPoint[1] > endPoint[1])
        {
            mStar[1] -= Mathf.RoundToInt(dist * Mathf.Sin(angle));
        }
        else
        {
            mStar[1] += Mathf.RoundToInt(dist * Mathf.Sin(angle));
        }

        // Create the river bed
        CreateRiverBed(heightMap, startPoint, mStar, endPoint, depth, middleDepth);

        // New depth-arrays for the recursive calls
        depthFirst = new float[2] { depth[0], middleDepth };
        depthSecond = new float[2] { middleDepth, depth[1] };

        // Recurse as long as the length isn't 1
        if (Mathf.RoundToInt(length) > 1)
        {
            // Update depth for each recursive call
            heightMap = RecursiveRiverGen(heightMap, startPoint, mStar, depthFirst);
            heightMap = RecursiveRiverGen(heightMap, mStar, endPoint, depthSecond);
        }

        return heightMap;

    }

    public float ChooseLength(float length, string flag = "")
    {
        float d, r;

        r = UnityEngine.Random.Range(-1, 1);
        if (flag == "zero")
        {
            // For debugging purposes
            d = 0;
        }
        else if (flag == "cubic")
        {
            // Square distribution f(r) = 1/3*length*r^3
            d = (length / 3) * (Mathf.Pow(r, 3));
        }
        else
        {
            // Linear distribution
            d = r * length / 3;
        }

        return d;
    }

    public void CreateRiverBed(TerrainNode[,] heightMap, int[] startPoint, int[] mStar, int[] endPoint, float[] depth, float middleDepth)
    {
        RiverBed(heightMap, startPoint, depth[0]);
        RiverBed(heightMap, mStar, middleDepth);
        RiverBed(heightMap, endPoint, depth[1]);
    }

    public TerrainNode[,] RiverBed(TerrainNode[,] heightMap, int[] point, float depth)
    {
        /*
        Carves out a 5x5 pseudo-circular surface at point
        */

        // Declare variables
        int[,] F;
        int dims;

        // Center the river bed
        point = new int[2] { point[0] - 3, point[1] - 3 };

        F = new int[5, 5] {
            {0, 1, 1, 1, 0},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {0, 1, 1, 1, 0}
            };


        dims = F.GetLength(0);
        for (int x = 0; x < dims; x++)
        {
            for (int z = 0; z < dims; z++)
            {
                // We only want to modify the river depth where the filter F is "1"
                if (F[x, z] == 1)
                {
                    heightMap[x + point[0], z + point[1]].height = depth * F[x, z];
                    heightMap[x + point[0], z + point[1]].type = "river";
                }

            }
        }

        return heightMap;
    }

    public TerrainNode[,] MergeTerrainMaps(List<TerrainNode[,]> TerrainFeatures, bool filter = true)
    {

        /*
        Priority based function for merging terrain heightMaps
        */

        // Declare variables

        int xSize, zSize, xLimit, zLimit, kernelSize, priorityIndex, currentPriorityIndex;
        float[,] kernel;
        float sum;
        float[,] convolutedValues;

        TerrainNode[,] mergedHeightMap;

        // Get map sizes from the first terrain feature (assuming all features are the same size)
        xSize = TerrainFeatures[0].GetLength(0);
        zSize = TerrainFeatures[0].GetLength(1);

        // Initialize merged heightMap
        mergedHeightMap = GenerateZeroMap(xSize, zSize);

        // Order of priority
        var priority = new List<string> { "river", "mountain", "null" };

        // Stack terrain based on priority list
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {

                // Add all current types to types list
                var types = new List<string>();
                foreach (TerrainNode[,] terrainFeature in TerrainFeatures)
                {
                    types.Add(terrainFeature[x, z].type);
                }

                // From types, find highest priority type and retrieve its index 
                // (this is the point we want to write to merged map)

                // Initialize feature priority
                priorityIndex = 0;
                // Set current priority to some high number
                currentPriorityIndex = 100;

                // Run through types list to find point with highest priority
                for (int i = 0; i < types.Count; i++)
                {

                    if (priority.IndexOf(types[i]) < currentPriorityIndex)
                    {

                        // Update priority index and current priority index if found better value
                        currentPriorityIndex = priority.IndexOf(types[i]);
                        priorityIndex = i;
                    }
                }
                // Write this point to the merged height map
                mergedHeightMap[x, z].height = TerrainFeatures[priorityIndex][x, z].height;
                mergedHeightMap[x, z].type = TerrainFeatures[priorityIndex][x, z].type;

            }

        }


        // -- Kernel filtering --

        // Write merged heightmap data to new matrix
        if (filter)
        {
            var filteredHeightMap = new float[xSize, zSize];

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    filteredHeightMap[x, z] = 0;
                }
            }
            // Define kernel data
            // kernel = new float[3, 3] {
            //     {1, 1, 1},
            //     {1, 1, 1},
            //     {1, 1, 1,}
            //     };

            kernel = new float[5, 5] {
                    {1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1}};


            kernelSize = kernel.GetLength(0);
            convolutedValues = new float[kernelSize, kernelSize];

            // Limits to avoid array out of bounds errors
            xLimit = Mathf.FloorToInt(kernel.GetLength(0) / 2);
            zLimit = Mathf.FloorToInt(kernel.GetLength(1) / 2);

            // Run over all the points on the map
            for (int x = xLimit; x < (xSize - xLimit); x++)
            {
                for (int z = zLimit; z < (zSize - zLimit); z++)
                {
                    if (mergedHeightMap[x, z].type == "river")
                    {
                        // Loop through kernel
                        for (int i = 0; i < kernelSize; i++)
                        {
                            for (int j = 0; j < kernelSize; j++)
                            {
                                // Calculate convoluted values
                                convolutedValues[i, j] = mergedHeightMap[x + (i - xLimit), z + (i - zLimit)].height * kernel[i, j];

                            }
                        }
                        // Average the convoluted values
                        sum = 0;
                        for (int i = 0; i < kernelSize; i++)
                        {
                            for (int j = 0; j < kernelSize; j++)
                            {
                                // Calculate convoluted values
                                sum += convolutedValues[i, j];

                            }
                        }
                        sum /= Mathf.Pow(kernelSize, 2);

                        // Apply the avg to the points height
                        filteredHeightMap[x, z] = sum;
                    }
                    else
                    {
                        filteredHeightMap[x, z] = mergedHeightMap[x, z].height;
                    }
                }

            }

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    mergedHeightMap[x, z].height = filteredHeightMap[x, z];
                }
            }
        }

        return mergedHeightMap;

    }

}

