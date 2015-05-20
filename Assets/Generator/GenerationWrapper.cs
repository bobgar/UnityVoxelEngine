using System;
using UnityEngine;
using System;
using CoherentNoise;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Displacement;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Generation.Modification;
using CoherentNoise.Generation.Patterns;
using CoherentNoise.Texturing;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class GenerationWrapper
{
	public Generator generator;

	public GenerationWrapper (Generator generator)
	{
		this.generator = generator;

	}

	public float GetValue(Vector3 v)
	{
		return generator.GetValue (v);
	}
}