using UnityEngine;
using System;
using CoherentNoise;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;

using System.Reflection;
using System.Collections.Generic;

[SerializeField]
public class ChunkGenerator
{
	public static ComputeShader blockShader;

	public byte[] blockTypes;
	//public Generator perlin;
	//private static MethodInfo generateMethod;
	//private static object classInstance;
	//private System.Random rand;
	private int seed;

	//**** COMPUTE SHADER STUFF ****//
	const int threadGroupSize = 8;
	public float noiseScale = 1;	
	public float weightMultiplier = 1;
	protected List<ComputeBuffer> buffersToRelease;
	ComputeBuffer pointsBuffer;

	public ChunkGenerator (GeneratorSpec gs)
	{
		//rand = new System.Random ();
		//perlin = new PinkNoise(new GradientNoise(gs.seed)).Scale(.01f, .01f, .01f);		
		blockTypes = gs.blockTypes;
		seed = gs.seed;
	}

	public void Generate(Chunk c)
	{
		int size = c.chunkSize;

		int PointsPerAxis = size + 2;
		int numPoints = PointsPerAxis * PointsPerAxis * PointsPerAxis;		

		pointsBuffer = new ComputeBuffer(numPoints, sizeof(int));

		//**** COMPUTE SHADER STUFF ****//
		buffersToRelease = new List<ComputeBuffer>();

		blockShader.SetFloat("noiseScale", noiseScale);

		int numThreadsPerAxis = Mathf.CeilToInt(PointsPerAxis / (float)threadGroupSize);		
		// Points buffer is populated inside shader with pos (xyz) + density (w).
		// Set paramaters
		blockShader.SetBuffer(0, "points", pointsBuffer);
		blockShader.SetInt("numPointsPerAxis", PointsPerAxis);		
		blockShader.SetVector("offset", new Vector4(c.chunkX, 0, c.chunkZ));

		// Dispatch shader
		blockShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

		if (buffersToRelease != null)
		{
			foreach (var b in buffersToRelease)
			{
				b.Release();
			}
		}

		pointsBuffer.GetData(c.blocks);

		pointsBuffer.Release();
		// Return voxel data buffer so it can be used to generate mesh
		//return pointsBuffer;
	}
}

