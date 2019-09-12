using UnityEngine;

public class NoiseGenerator : ScriptableObject
{
    // Function for creating different noise patterns. Takes in octaves and base noise scale to generate a detailed noise pattern
    public float GenerateNoise(int x, int z, float base_scale, float frequency, int octaves, float amplitude, bool floor=true)
    {
        float noiseval = 0f;                            // Initialize noise value
        float lacunarity = 2f;                          // Increases the frequency proportional to octaves
        float persistence = 0.5f;                       // Dampens the amplitude proportional to octaves 

        // Generate noise stack, looping through each octave
        for (int i = 0; i <= octaves; i++)
        {
            float scale = (base_scale * 0.01f) * frequency;                     // Calculate new scale value for each iteration

            noiseval += (Mathf.PerlinNoise(x * scale, z * scale) - 0.4f) * amplitude;

            frequency *= lacunarity;                                            // Increases frequency per octave
            amplitude *= persistence;                                           // Decrease amplitude per octave

        }
        if (floor) {
            return Mathf.Max(0f, noiseval);
        }
        else
            return noiseval;
    }

    

}
