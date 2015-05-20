
using System;

public class GeneratorSpec
{
	public int seed;
	public byte[] blockTypes;

	public GeneratorSpec (int seed, byte[] blockTypes)
	{
		this.seed = seed;
		this.blockTypes = blockTypes;
	}
}