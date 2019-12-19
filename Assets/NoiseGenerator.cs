using UnityEngine;

public class NoiseGenerator : ScriptableObject
{
    // Function for creating different noise patterns. Takes in parameters for generating
    // noise and feeds it to the native Unity Perlin noise function, and outputs a Vector3
    // array of vertices for mesh generation.

    public float GenerateNoise(int x, int z, float base_scale, float frequency, int octaves, float amplitude, float offset, bool floor=false)
    {

        float noiseval = 0f;
        float lacunarity = 2f;                              // Increases the frequency proportional to octaves
        float persistence = 1/lacunarity;                   // Decrease amplitude proportional to octaves  
        

        // Generate noise stack, looping through each octave
        for (int i = 0; i <= octaves; i++)
        {
            float scale = (base_scale * 0.01f) * frequency;            // Noise scale

            noiseval += (Mathf.PerlinNoise(x * scale, z * scale) - 0.4f) * amplitude;

            frequency *= lacunarity;                                   // Increases frequency per octave
            amplitude *= persistence;                                  // Decrease amplitude per octave

        }

        if (floor) {
            return Mathf.Max(0f, offset + noiseval);
        }
        else
            return noiseval;
    }
}
