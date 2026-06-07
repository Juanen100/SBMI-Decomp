using UnityEngine;

public class TerrainPathing
{
	private GridPosition goal;

	private Terrain terrain;

	private PathFinder2 pathFinder2;

	public GridPosition Goal
	{
		get
		{
			return null;
		}
	}

	public TerrainPathing(Terrain terrain, Vector2 start, Vector2 goal)
	{
	}

	public PathFinder2.PROGRESS Seek(int budget)
	{
		return default(PathFinder2.PROGRESS);
	}

	public void BuildPath(out Path<GridPosition> path)
	{
		path = null;
	}
}
