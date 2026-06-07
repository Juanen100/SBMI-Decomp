public class StructureDecorator : EntityDecorator
{
	public bool IsObstacle
	{
		get
		{
			return false;
		}
	}

	public bool ShouldBlockPlacement
	{
		get
		{
			return false;
		}
	}

	public AlignedBox Footprint
	{
		get
		{
			return null;
		}
	}

	public bool Immobile
	{
		get
		{
			return false;
		}
	}

	public bool ShareableSpace
	{
		get
		{
			return false;
		}
	}

	public bool ShareableSpaceSnap
	{
		get
		{
			return false;
		}
	}

	public StructureDecorator(Entity toDecorate)
		: base(null)
	{
	}
}
