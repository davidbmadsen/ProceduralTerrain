using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{   
    NoiseGenerator noiseGen;
    Mesh mesh;

    public Vector3[] vertices;
    public int[] triangles;

    // Mesh dimensions (max size 255x255)
    public int xSize = 255;
    public int zSize = 255;

    void Start()
    {
        // Instantiate objects
        mesh = new Mesh();
        noiseGen = ScriptableObject.CreateInstance<NoiseGenerator>();

        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();       // Generate the terrain mesh
        UpdateMesh();       // Updates mesh with vertices
    }


    // Method for creating 
    void CreateMesh()
    {

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = noiseGen.GenerateNoise(x, z, 1f, 0.5f, 3, 60f, true);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        
        int vert = 0; // current vertex
        int tris = 0; // 

        triangles = new int[xSize * zSize * 6]; // Array that stores triangle points

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

    /* // Draw gizmos to represent vertices  
    private void OnDrawGizmos()
    {

        if (vertices == null)
            return;
            
         
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
        
    }
    */ 
}
