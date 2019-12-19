using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    // Objects used to generate terrain features
    NoiseGenerator noiseGen;
    HeightMatrix heightMatrix;
    TerrainNode[,] heightMap, zeroMapOne, zeroMapTwo, zeroMapThree, riverMap, riverMap2, mountainMap;
    List<TerrainNode[,]> TerrainFeatures;
    Mesh mesh;

    // Mesh variables
    public Vector3[] vertices;
    public int[] triangles;

    // Mesh dimensions (max size 255x255)
    public int xSize = 255;
    public int zSize = 255;

    void Start()
    {
        // Instantiate objects used to draw the mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        CreateMesh();       // Generate the terrain mesh
        UpdateMesh();       // Updates mesh with vertices
    }


    // Method for creating mesh
    void CreateMesh()
    {

        // vertices = riverGen.GenerateRiver(xSize, zSize);
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        // Instantiate height matrix
        heightMatrix = ScriptableObject.CreateInstance<HeightMatrix>();

        // Generate the empty terrain height maps zeroMap
        zeroMapOne = heightMatrix.GenerateZeroMap(xSize, zSize);
        zeroMapTwo = heightMatrix.GenerateZeroMap(xSize, zSize);

        riverMap = heightMatrix.GenerateNaturalRivers(zeroMapOne, -10, 0);
        mountainMap = heightMatrix.GeneratePyramidMountains(zeroMapTwo);


        List<TerrainNode[,]> TerrainFeatures = new List<TerrainNode[,]> { riverMap, mountainMap };


        // Use merge function to merge mountains and river 
        heightMap = heightMatrix.MergeTerrainMaps(TerrainFeatures, true);

        // --- Vector creation part (note to self: dont touch) ---

        // Generate vectors for terrain from heightmap matrix
        float height;
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Extract each point from the matrix and create a Vector3 for vertices
                height = heightMap[x, z].height;
                vertices[i] = new Vector3(x, height, z);
                i++;
            }
        }

        // Code courtesy of Brackeys (https://www.youtube.com/watch?v=eJEpeUH1EMg)
        // Indices to keep track of triangles and vertices
        int vert = 0;
        int tris = 0;

        triangles = new int[xSize * zSize * 6]; // Array that stores triangle points

        // Generate triangles
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

            }

            vert++;
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;   // Sets the mesh vertices to be the points in "vertices" array
        mesh.triangles = triangles; // Sets the triangles correspondingly

        mesh.RecalculateNormals();  // Fix lighting
    }
}
