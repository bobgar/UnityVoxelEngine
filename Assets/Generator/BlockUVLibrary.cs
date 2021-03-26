
using System;
using UnityEngine;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public class BlockUVLibrary
{
	public static Dictionary<String, UVMapObject> UVMapLibrary;

	public static void Initialize()
	{
		UVMapLibrary = new Dictionary<String, UVMapObject>();
		TextAsset ta = Resources.Load<TextAsset>("textureSheet");

		//TextureJsonObject data = JsonMapper.ToObject<TextureJsonObject>(ta.text);
		JsonData data = JsonMapper.ToObject (ta.text);
		
		float w = (int)data["meta"]["size"]["w"];
		float h = (int)data["meta"]["size"]["h"];

		foreach(String k in (data["frames"] as IDictionary).Keys )
		{
			UVMapObject cur = new UVMapObject();
			cur.upperLeft = new Vector2 ((int)data["frames"][k]["frame"]["x"] / w, 1.0f - (int)data["frames"][k]["frame"]["y"] / h);
			cur.upperRight = new Vector2 (((int)data["frames"][k]["frame"]["x"] + (int)data["frames"][k]["frame"]["w"]) / w, 1.0f - (int)data["frames"][k]["frame"]["y"] / h);
			cur.lowerLeft = new Vector2 ((int)data["frames"][k]["frame"]["x"] / w, 1.0f - ((int)data["frames"][k]["frame"]["y"] + (int)data["frames"][k]["frame"]["h"]) / h);
			cur.lowerRight = new Vector2 (((int)data["frames"][k]["frame"]["x"] + (int)data["frames"][k]["frame"]["w"]) / w, 1.0f - ((int)data["frames"][k]["frame"]["y"] + (int)data["frames"][k]["frame"]["h"]) / h);

			UVMapLibrary.Add(k, cur);
		}
	}
}


public class TextureJsonObject
{
	public SpritesheetMetaData meta;
	
	//public Dictionary<string, TextureInfo> frames;
	public Dictionary<string, TextureInfo> frames;
}

public class SpritesheetMetaData
{
	public string app;
	public string version;
	public string image;
	public string format;
	public string size;
	public Dictionary<string, int> scale;
	public string smartupdate;
}

public class TextureInfo
{
	public Dictionary<string, int> frame;
	public bool rotated;
	public bool trimmed;
	public Dictionary<string, int> spriteSourceSize;
	public Dictionary<string, int> sourceSize;
}

public class UVMapObject
{
	public Vector2 lowerLeft;
	public Vector2 lowerRight;
	public Vector2 upperLeft;
	public Vector2 upperRight;
}