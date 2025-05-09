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
			return goal;
		}
	}

	public TerrainPathing(Terrain terrain, Vector2 start, Vector2 goal)
	{
		this.terrain = terrain;
		this.goal = this.terrain.ComputeGridPosition(goal);
		pathFinder2 = new PathFinder2(this.terrain);
		pathFinder2.Start(this.terrain.ComputeGridPosition(start), this.goal);
	}

	public PathFinder2.PROGRESS Seek(int budget)
	{
		return pathFinder2.Seek(budget);
	}

	public void BuildPath(out Path<GridPosition> path)
	{
		pathFinder2.BuildPath(out path);
	}
}
