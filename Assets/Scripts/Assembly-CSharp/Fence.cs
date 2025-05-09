public class Fence : Enclosure
{
	private const float BOX_OFFSET = 5f;

	public Fence(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard)
		: base(box, 5f, mgr, billboard)
	{
	}

	protected override string GetMaterialName(EnclosureManager.PieceType piece)
	{
		switch (piece)
		{
		case EnclosureManager.PieceType.BACK_CORNER:
			return "Fence_Back_Corner.png";
		case EnclosureManager.PieceType.BACK_LCORNER:
			return "Fence_Back_LCorner.png";
		case EnclosureManager.PieceType.BACK_LEFT:
			return "Fence_Back_Left.png";
		case EnclosureManager.PieceType.BACK_RCORNER:
			return "Fence_Back_RCorner.png";
		case EnclosureManager.PieceType.BACK_RIGHT:
			return "Fence_Back_Right.png";
		case EnclosureManager.PieceType.FRONT_CORNER:
			return "Fence_Front_Corner.png";
		case EnclosureManager.PieceType.FRONT_LCORNER:
			return "Fence_Front_LCorner.png";
		case EnclosureManager.PieceType.FRONT_LEFT:
			return "Fence_Front_Left.png";
		case EnclosureManager.PieceType.FRONT_RCORNER:
			return "Fence_Front_RCorner.png";
		case EnclosureManager.PieceType.FRONT_RIGHT:
			return "Fence_Front_Right.png";
		default:
			return null;
		}
	}

	protected override EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name)
	{
		return mgr.fenceDefs[name];
	}

	protected override void AddLayer(EnclosureManager mgr, int layer, BillboardDelegate billboard)
	{
		base.AddLayer(mgr, layer, billboard);
	}

	public override void OnUpdate(Simulation simulation, EnclosureManager mgr)
	{
		if (base.IsValid)
		{
			base.OnUpdate(simulation, mgr);
		}
	}

	public override void Destroy()
	{
		if (base.IsValid)
		{
			base.Destroy();
		}
	}
}
