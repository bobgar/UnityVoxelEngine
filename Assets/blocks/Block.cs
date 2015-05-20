using UnityEngine;
using System.Collections;

public class Block {

	public UVMapObject negXUV;
	public UVMapObject posXUV;
	public UVMapObject negYUV;
	public UVMapObject posYUV;
	public UVMapObject negZUV;
	public UVMapObject posZUV;

	public bool IsTransparent { get ; set;}

	public Block()
	{
		IsTransparent = false;

		negXUV = BlockUVLibrary.UVMapLibrary["grassdirt.jpg"];
		posXUV = BlockUVLibrary.UVMapLibrary["grassdirt.jpg"];
		negZUV = BlockUVLibrary.UVMapLibrary["grassdirt.jpg"];
		posZUV = BlockUVLibrary.UVMapLibrary["grassdirt.jpg"];

		negYUV = BlockUVLibrary.UVMapLibrary["dirt.jpg"];
		posYUV = BlockUVLibrary.UVMapLibrary["grass.jpg"];
	}
}
