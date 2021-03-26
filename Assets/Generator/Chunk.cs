﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk {
	public bool isReadyToGenerate = false;
	public bool isGenerating = false;
	public bool isInitializingArrays = false;
	public bool initializingView = false;

	public bool isReadyToView = false;

	public int chunkX;
	public int chunkZ;
	public int chunkSize;
	public int arrayDim;

	public int[] blocks;

	//Neighboring chunks
	public Chunk posXChunk;
	public Chunk negXChunk;
	public Chunk posZChunk;
	public Chunk negZChunk;

	public GameObject gameObject;

	List<int> indices;
	List<Vector2> uvs;
	List<Vector3> verts;
	//List<Color> colors = new List<Color>();

	int[] indeciesArray;
	Vector2[] uvArray;
	Vector3[] vertArray;

	public int GetIndex(int x, int y, int z)
    {
		return (z+1) * arrayDim * arrayDim + (y+1) * arrayDim + (x+1);
    }

	public void SetBlock(int x, int y, int z, int val)
    {		
		blocks[GetIndex(x, y, z)] = val;	
    }

	public Chunk(int chunkX, int chunkY, int chunkSize)
	{
		this.chunkSize = chunkSize;
		this.arrayDim = chunkSize + 2;
		this.chunkX = chunkX;
		this.chunkZ = chunkY;
		blocks = new int[arrayDim * arrayDim * arrayDim];

		isReadyToGenerate = true;
	}


	//public IEnumerator InitializeView()
	public void InitializeArrays ()
	{
		indices = new List<int>();
		uvs = new List<Vector2>();
		verts = new List<Vector3>();
		//new List<Color>();
		
		isInitializingArrays = true;
		
		int curVert = 0;
		int density = 0;
		int[] faceIDs;

		posXChunk = GenerationController.chunkMap [(chunkX + 1) + "," + chunkZ];
		negXChunk = GenerationController.chunkMap [(chunkX - 1) + "," + chunkZ];
		posZChunk = GenerationController.chunkMap [chunkX + "," + (chunkZ + 1)];
		negZChunk = GenerationController.chunkMap [chunkX + "," + (chunkZ - 1)];

		for (int x = 0; x < chunkSize; x++) {
			//yield return null;
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++) {
					Block bp = BlockLibrary.BlockDictionary [blocks [GetIndex( x, y, z)]];
					if (bp != null && ! bp.IsTransparent) {
						if (negXTransparent (x, y, z)) {
							verts.Add (new Vector3 (x, y, z + 1));
							verts.Add (new Vector3 (x, y + 1, z));
							verts.Add (new Vector3 (x, y, z));
							verts.Add (new Vector3 (x, y + 1, z + 1));

							uvs.Add (bp.negXUV.lowerLeft);
							uvs.Add (bp.negXUV.upperRight);
							uvs.Add (bp.negXUV.lowerRight);
							uvs.Add (bp.negXUV.upperLeft);
							
							indices.Add (curVert);
							indices.Add (curVert + 3);
							indices.Add (curVert + 1);
							
							indices.Add (curVert);
							indices.Add (curVert + 1);
							indices.Add (curVert + 2);
							
							curVert += 4;
						}
						if (posXTransparent (x, y, z)) {
							verts.Add (new Vector3 (x + 1, y, z + 1));
							verts.Add (new Vector3 (x + 1, y + 1, z));
							verts.Add (new Vector3 (x + 1, y, z));
							verts.Add (new Vector3 (x + 1, y + 1, z + 1));

							uvs.Add (bp.posXUV.lowerLeft);
							uvs.Add (bp.posXUV.upperRight);
							uvs.Add (bp.posXUV.lowerRight);
							uvs.Add (bp.posXUV.upperLeft);
							
							indices.Add (curVert);
							indices.Add (curVert + 1);
							indices.Add (curVert + 3);
							
							indices.Add (curVert);
							indices.Add (curVert + 2);
							indices.Add (curVert + 1);
							
							curVert += 4;
						}

						if (negYTransparent (x, y, z)) {
							verts.Add (new Vector3 (x, y, z));
							verts.Add (new Vector3 (x + 1, y, z + 1));
							verts.Add (new Vector3 (x + 1, y, z));
							verts.Add (new Vector3 (x, y, z + 1));
							
							uvs.Add (bp.negYUV.lowerLeft);
							uvs.Add (bp.negYUV.upperRight);
							uvs.Add (bp.negYUV.lowerRight);
							uvs.Add (bp.negYUV.upperLeft);

							
							indices.Add (curVert);
							indices.Add (curVert + 2);
							indices.Add (curVert + 1);
							
							indices.Add (curVert);
							indices.Add (curVert + 1);
							indices.Add (curVert + 3);
							
							curVert += 4;
						}
						if (posYTransparent (x, y, z)) {
							verts.Add (new Vector3 (x, y + 1, z));
							verts.Add (new Vector3 (x + 1, y + 1, z + 1));
							verts.Add (new Vector3 (x + 1, y + 1, z));
							verts.Add (new Vector3 (x, y + 1, z + 1));

							uvs.Add (bp.posYUV.lowerLeft);
							uvs.Add (bp.posYUV.upperRight);
							uvs.Add (bp.posYUV.lowerRight);
							uvs.Add (bp.posYUV.upperLeft);
							
							indices.Add (curVert);
							indices.Add (curVert + 1);
							indices.Add (curVert + 2);
							
							indices.Add (curVert);
							indices.Add (curVert + 3);
							indices.Add (curVert + 1);
							
							curVert += 4;
						}

						if (negZTransparent (x, y, z)) {
							verts.Add (new Vector3 (x, y, z));
							verts.Add (new Vector3 (x + 1, y + 1, z));
							verts.Add (new Vector3 (x + 1, y, z));
							verts.Add (new Vector3 (x, y + 1, z));
							
							uvs.Add (bp.negZUV.lowerLeft);
							uvs.Add (bp.negZUV.upperRight);
							uvs.Add (bp.negZUV.lowerRight);
							uvs.Add (bp.negZUV.upperLeft);
							
							indices.Add (curVert);
							indices.Add (curVert + 1);
							indices.Add (curVert + 2);
							
							indices.Add (curVert);
							indices.Add (curVert + 3);
							indices.Add (curVert + 1);
							
							curVert += 4;
						}
						if (posZTransparent (x, y, z)) {
							verts.Add (new Vector3 (x, y, z + 1));
							verts.Add (new Vector3 (x + 1, y + 1, z + 1));
							verts.Add (new Vector3 (x + 1, y, z + 1));
							verts.Add (new Vector3 (x, y + 1, z + 1));
							
							uvs.Add (bp.posZUV.lowerLeft);
							uvs.Add (bp.posZUV.upperRight);
							uvs.Add (bp.posZUV.lowerRight);
							uvs.Add (bp.posZUV.upperLeft);
							
							indices.Add (curVert);
							indices.Add (curVert + 2);
							indices.Add (curVert + 1);
							
							indices.Add (curVert);
							indices.Add (curVert + 1);
							indices.Add (curVert + 3);
							
							curVert += 4;
						}
						//GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load(blockSpec[x,y,z].prefabKey ));
						//instance.transform.position = new Vector3(x,y,z);
					}
				}
			}
		}

		isInitializingArrays = false;
		isReadyToView = true;

		indeciesArray = indices.ToArray();
		uvArray = uvs.ToArray();
		vertArray = verts.ToArray();
	}

	public IEnumerator InitializeView()
	{
		initializingView = true;
		gameObject = new GameObject ("Chunk " + chunkX + "  ,  " + chunkZ);
		gameObject.transform.position = new Vector3( chunkX * chunkSize, 0 , chunkZ * chunkSize);
		//yield return null;
		gameObject.AddComponent <MeshFilter>();
		//yield return null;
		gameObject.AddComponent <MeshCollider>();
		//yield return null;
		MeshRenderer mr = gameObject.AddComponent <MeshRenderer>();
		//yield return null;
		mr.material = GenerationController.material;
		//InitializeMesh ();
		//yield return null;
		Mesh subMesh = gameObject.GetComponent<MeshFilter>().mesh;
		yield return null;
		MeshCollider mc = gameObject.GetComponent<MeshCollider>();
		//yield return null;
		subMesh.Clear();
		//yield return null;
		subMesh.vertices = vertArray;
		//yield return null;
		subMesh.triangles = indeciesArray;
		//yield return null;
		subMesh.uv = uvArray;
		//subMesh.colors = colors.ToArray();
		//yield return null;
		//subMesh.Optimize();
		subMesh.RecalculateNormals();
		//yield return null;
		//if (mc.sharedMesh == null) mc.sharedMesh = subMesh;
		mc.sharedMesh = new Mesh();
		mc.sharedMesh = subMesh;
		initializingView = false;
	}

	public void UpdateView()
	{
		Mesh subMesh = gameObject.GetComponent<MeshFilter>().mesh;
		MeshCollider mc = gameObject.GetComponent<MeshCollider>();
		subMesh.Clear();
		subMesh.vertices = vertArray;
		subMesh.triangles = indeciesArray;
		subMesh.uv = uvArray;
		subMesh.RecalculateNormals();
		mc.sharedMesh = new Mesh();
		mc.sharedMesh = subMesh;
		initializingView = false;
	}

	public void DestroyView()
	{
		if (initializingView == false) {
			GameObject.Destroy (gameObject);
			gameObject = null;
		}
	}

	public void Destroy()
	{
		DestroyView ();
		blocks = null;
		posXChunk = null;
		negXChunk = null;
		posZChunk = null;
		negZChunk = null;
		indices = null;
		uvs = null;
		verts = null;
		indeciesArray = null;
		uvArray = null;
		vertArray = null;
	}

	//For transparency checks, first line is neightbor Chunk test, second is IN chunk test.
	public bool negXTransparent(int x, int y, int z)
	{

		return ((x == 0 && (BlockLibrary.BlockDictionary[negXChunk.blocks[GetIndex(chunkSize-1,y, z)]] == null || BlockLibrary.BlockDictionary[negXChunk.blocks[GetIndex(chunkSize-1,y, z)]].IsTransparent))
			|| (x > 0 && (BlockLibrary.BlockDictionary[blocks[GetIndex(x - 1, y, z)]] == null || BlockLibrary.BlockDictionary[blocks[GetIndex(x - 1, y, z)]].IsTransparent)));
	}

	public bool posXTransparent(int x, int y, int z)
	{
		return ((x == chunkSize-1 && (BlockLibrary.BlockDictionary[posXChunk.blocks[GetIndex(0,y, z)]] == null || BlockLibrary.BlockDictionary[posXChunk.blocks[GetIndex(0,y, z)]].IsTransparent))
		        || (x < chunkSize-1 && (BlockLibrary.BlockDictionary[blocks[GetIndex(x + 1, y, z)]] == null || BlockLibrary.BlockDictionary[blocks[GetIndex(x + 1, y, z)]].IsTransparent)) );
	}

	public bool negYTransparent(int x, int y, int z)
	{
		return ((y <= 0 || BlockLibrary.BlockDictionary[blocks[GetIndex(x, y- 1, z)]] == null || BlockLibrary.BlockDictionary[blocks[GetIndex(x , y- 1, z)]].IsTransparent) && y > 0);
	}
	
	public bool posYTransparent(int x, int y, int z)
	{
		return ((y >= chunkSize-1 || BlockLibrary.BlockDictionary[blocks[GetIndex(x , y+ 1, z)]] == null || BlockLibrary.BlockDictionary[blocks[GetIndex(x , y+ 1, z)]].IsTransparent));
	}

	public bool negZTransparent(int x, int y, int z)
	{
		return ( (z == 0 && (BlockLibrary.BlockDictionary[negZChunk.blocks[GetIndex(x,y, chunkSize - 1)]] == null || BlockLibrary.BlockDictionary[negZChunk.blocks[GetIndex(x,y, chunkSize-1)]].IsTransparent))
			|| (z > 0 && (BlockLibrary.BlockDictionary[blocks[GetIndex(x, y, z - 1)]] == null || BlockLibrary.BlockDictionary[blocks[GetIndex(x , y, z- 1)]].IsTransparent)));
	}
	
	public bool posZTransparent(int x, int y, int z)
	{
		return ((z == chunkSize-1 && (BlockLibrary.BlockDictionary[posZChunk.blocks[GetIndex(x ,y, 0)]] == null || BlockLibrary.BlockDictionary[posZChunk.blocks[GetIndex(x ,y, 0)]].IsTransparent))
			|| (z < chunkSize-1 && (BlockLibrary.BlockDictionary[blocks[GetIndex(x , y, z+ 1)]] == null || BlockLibrary.BlockDictionary[blocks[GetIndex(x , y, z+ 1)]].IsTransparent)));
	}
}
