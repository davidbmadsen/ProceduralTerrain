using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{   
    NoiseGenerator noiseGen;
    RiverGen2 rivGen2;
    // RiverGenerator riverGen;
    Mesh mesh;

    public Vector3[] vertices;
    public int[] triangles;
    public int[,] arbitraryHeighmap;


    // Mesh dimensions (max size 255x255)
    public int xSize = 50;
    public int zSize = 50;

    void Start()
    {
        // Instantiate objects to draw the mesh
        mesh = new Mesh();
        noiseGen = ScriptableObject.CreateInstance<NoiseGenerator>();
        rivGen2 = ScriptableObject.CreateInstance<RiverGen2>();

        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateMesh();       // Generate the terrain mesh
        UpdateMesh();       // Updates mesh with vertices
    }


    // Method for creating mesh
    void CreateMesh()
    {

        // vertices = riverGen.GenerateRiver(xSize, zSize);
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        

        // Section for generating the finished mesh.

        // Generate height matrix
        arbitraryHeighmap = rivGen2.riverHeightMap(xSize, zSize);


        // Generate vectors for terrain from heightmap matrix
        for (int i= 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = noiseGen.GenerateNoise(x, z, 1f, 2f, 4, 30f, true);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        

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
