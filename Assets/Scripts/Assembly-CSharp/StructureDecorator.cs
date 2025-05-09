public class StructureDecorator : EntityDecorator
{
	public bool IsObstacle
	{
		get
		{
			return true;
		}
	}

	public bool ShouldBlockPlacement
	{
		get
		{
			return true;
		}
	}

	public AlignedBox Footprint
	{
		get
		{
			return (AlignedBox)Invariable["footprint"];
		}
	}

	public bool Immobile
	{
		get
		{
			object value;
			if (Invariable.TryGetValue("immobile", out value))
			{
				return (bool)value;
			}
			return true;
		}
	}

	public bool ShareableSpace
	{
		get
		{
			object value;
			if (Invariable.TryGetValue("shareable_space", out value))
			{
				return (bool)value;
			}
			return false;
		}
	}

	public bool ShareableSpaceSnap
	{
		get
		{
			object value;
			if (Invariable.TryGetValue("shareable_space_snap", out value))
			{
				return (bool)value;
			}
			return false;
		}
	}

	public StructureDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}
}
