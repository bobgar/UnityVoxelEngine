using UnityEngine;
using System;
using CoherentNoise;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;

using System.Reflection;

[SerializeField]
public class ChunkGenerator
{
	public byte[] blockTypes;
	public Generator perlin;
	private static MethodInfo generateMethod;
	private static object classInstance;
	private System.Random rand;

	public ChunkGenerator (GeneratorSpec gs)
	{
		rand = new System.Random ();
		perlin = new PinkNoise(new GradientNoise(gs.seed)).Scale(.01f, .01f, .01f);		
		blockTypes = gs.blockTypes;
	}

	public void Generate(Chunk c, String generationCode)
	{
		int sizeX = c.chunkSizeX;
		int sizeY = c.chunkSizeY;
		int sizeZ = c.chunkSizeZ;

		int offsetX = c.chunkX * sizeX;
		int offsetZ = c.chunkZ * sizeZ;

		for(int localX = 0; localX < sizeX; localX++)
		{
			for(int localZ = 0; localZ < sizeZ; localZ++)
			{
				int x = offsetX + localX;
				int z = offsetZ + localZ; 
				//******  IF YOU NEED TO GENERATE 2D VALUES, THAT SHOULD HAPPEN HERE! ****************************************************

				float perlin2D = perlin.GetValue( x * .25f , 0, z * .25f ) ;
				float percentPerlin2D = (perlin2D + 1f) / 2.0f;

				if(percentPerlin2D < 0)
					percentPerlin2D = 0;
				else if(percentPerlin2D > 1)
					percentPerlin2D = 1;

				float r = Rand();
				int highestPoint = 0;
				for(int y = 0; y < sizeY; y++)
				{
					//******  THIS IS WHERE CODE WILL GO TO HELP DECIDE WHICH BLOCK SHOULD BE GENERATED, IF ANY ****************************************************
					float percentY = ((float)y  ) / ((float)c.blocks.GetLength(1));
					//Debug.Log("percent Y = " + percentY);
					//******  THIS IS WHERE CODE WILL GO FOR DECIDING WHAT BLOCK TO SHOW AT THIS x,y,z location ****************************************************

					if(y == 0)
					{
						c.blocks[localX,y,localZ] = blockTypes[2];
					}
					else
					{
						float perlin3D = perlin.GetValue( x *.5f , y * .25f  , z * .5f );
						float percentPerlin = (perlin3D + 1f) * .75f;

						if(percentPerlin < 0)
							percentPerlin = 0;

						if(percentY - (percentPerlin2D / 2f) > 0f )
						{
							//if(percentPerlin > .4)
							if( (percentY * 1.2f) - percentPerlin <= 0f)
							{
								highestPoint = y; 
								if(percentY > percentPerlin2D)
									c.blocks[localX,y,localZ] = blockTypes[1];
								else
									c.blocks[localX,y,localZ] = blockTypes[2];
							}
						}
						else if(percentPerlin > .1f)
						{
							highestPoint = y; 
							c.blocks[localX,y,localZ] = blockTypes[3];
						}
						else  if(percentPerlin > .05f)
						{
							highestPoint = y; 
							c.blocks[localX,y,localZ] = blockTypes[2];
						}
					}
				}


				//check for tree
				float perlin2DTrees = perlin.GetValue(x * .25f, 16f, z * .25f);//perlin.GetValue( x * .25f , 16, z * .25f );
				if(Rand() + .3f < perlin2DTrees)
				{
					generateTree(c,localX,localZ, highestPoint);
				}
			}
		}
	}

	private void generateTree(Chunk c, int x, int z, int y)
	{
		//Only place a tree if on dirt
		if (c.blocks [x, y, z] != blockTypes [1])
			return;

		y++;

		//Only place a tree if it will fit in the world.
		int treeHeight = (int)( Rand () * 3) + 4;
		if (y + treeHeight >= c.chunkSizeY - 1)
			return;

		// Generate Body
		for (int ty = y; ty < y+treeHeight; ty ++) {
			c.blocks[x,ty,z] = blockTypes[4];
		}

		// Generate Leaves
		for (int ty = y+treeHeight - 2; ty < y+treeHeight; ty ++) {
			if(x+1 < c.chunkSizeX)
				c.blocks[x+1,ty,z] = blockTypes[5];
			else if(GenerationController.chunkMap.ContainsKey((c.chunkX+1)+","+ c.chunkZ))
				GenerationController.chunkMap[(c.chunkX+1)+","+ c.chunkZ].blocks[0, ty, z] = blockTypes[5];

			if(x-1 >= 0)
				c.blocks[x-1,ty,z] = blockTypes[5];
			else if(GenerationController.chunkMap.ContainsKey((c.chunkX-1)+","+ c.chunkZ))
				GenerationController.chunkMap[(c.chunkX-1)+","+ c.chunkZ].blocks[c.chunkSizeX-1, ty, z] = blockTypes[5];

			if(z+1 < c.chunkSizeZ)
				c.blocks[x,ty,z+1] = blockTypes[5];
			else if(GenerationController.chunkMap.ContainsKey(c.chunkX +","+ (c.chunkZ+1)))
				GenerationController.chunkMap[c.chunkX +","+ (c.chunkZ+1)].blocks[x, ty, 0] = blockTypes[5];

			if(z-1 >= 0)
				c.blocks[x,ty,z-1] = blockTypes[5];
			else if(GenerationController.chunkMap.ContainsKey(c.chunkX +","+ (c.chunkZ-1)))
				GenerationController.chunkMap[c.chunkX +","+ (c.chunkZ-1)].blocks[x, ty, c.chunkSizeZ-1] = blockTypes[5];
		}
		//put a top on the tree
		c.blocks[x, y+treeHeight, z] = blockTypes[5];
	}

	private float Rand()
	{
		return (float) rand.NextDouble();
	}

	private float Sin(float x)
	{
		return (float)Math.Sin ((double)x);
	}

	private float Cos(float x)
	{
		return (float)Math.Cos ((double)x);
	}

	private float Tan(float x)
	{
		return (float)Math.Tan ((double)x);
	}
}


/*float val = noiseGenerator.GetValue( (x + c.chunkSizeX*c.chunkX) / 16.0f, y / 16.0f , (z  + c.chunkSizeZ*c.chunkZ)  / 16.0f );

					float f = ((float)y / (float)c.blocks.GetLength(1));

					if(y == 0)
					{
						c.blocks[x,y,z] = blockTypes[1];
					}
					else if(y < c.blocks.GetLength(1) / 2 && val < -.4)
					{
						//air in this case
					}
					else if(y < (c.blocks.GetLength(1) / 2 ) * (val))
					{
						c.blocks[x,y,z] = blockTypes[1];
					}
					else if((val+ .5 + f*6) / 7.0 < .58)
					{
						c.blocks[x,y,z] = blockTypes[1];
					}
					else if((val+ .5 + f*6) / 7.0 < .6)
					{
						c.blocks[x,y,z] = blockTypes[0];
					}*/
