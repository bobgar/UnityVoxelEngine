using UnityEngine;
using System;
using CoherentNoise;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Displacement;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Generation.Modification;
using CoherentNoise.Generation.Patterns;
using CoherentNoise.Texturing;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System;

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

				for(int y = 0; y < sizeY; y++)
				{
					//******  THIS IS WHERE CODE WILL GO TO HELP DECIDE WHICH BLOCK SHOULD BE GENERATED, IF ANY ****************************************************
					float percentY = ((float)y  ) / ((float)c.blocks.GetLength(1));
					//Debug.Log("percent Y = " + percentY);
					//******  THIS IS WHERE CODE WILL GO FOR DECIDING WHAT BLOCK TO SHOW AT THIS x,y,z location ****************************************************
				}
			}
		}
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
