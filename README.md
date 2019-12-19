# ProceduralTerrain
 
HeightMatrix.cs contains all the generator functions as well as the merge function.
MeshGenerator.cs renders the mesh in Unity. The files need to be opened as a project file in order to generate terrain,
however all .cs files can be opened in any text editor for inspection.
NoiseGenerator.cs generates Perlin noise values for a point (x,z).
TerrainNode.cs contains the terrain node class used to store height and type data.

How to run:
1. Open in Unity 3D (tested in version 2019.2.4f1 but higher versions should also work)

2. Click the play button in the top of the interface

*The terrain generation may take a few seconds.* 
To generate Perlin noise mountains, change "GeneratePyramidMountains" to "GenerateNoisyMountains" in MeshGenerator.cs (line 51)
More layers of terrain may be added to the TerrainFeatures list, however the merge function will only layer 
rivers on top of mountains as per today. 

Use the true/false flag in the MergeTerrainMaps function to enable/disable kernel filtering (line 58).


Known bugs:
In case of array out of bounds error (check the log), start the generation again until it works.