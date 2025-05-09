public class Scaffolding : Enclosure
{
	private const float BOX_OFFSET = 5f;

	public Scaffolding(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard)
		: base(box, 5f, mgr, billboard)
	{
	}

	protected override string GetMaterialName(EnclosureManager.PieceType piece)
	{
		switch (piece)
		{
		case EnclosureManager.PieceType.BACK_CORNER:
			return "Scaffold_Back_Corner.png";
		case EnclosureManager.PieceType.BACK_LCORNER:
			return "Scaffold_Back_LCorner.png";
		case EnclosureManager.PieceType.BACK_LEFT:
			return "Scaffold_Back_Left.png";
		case EnclosureManager.PieceType.BACK_RCORNER:
			return "Scaffold_Back_RCorner.png";
		case EnclosureManager.PieceType.BACK_RIGHT:
			return "Scaffold_Back_Right.png";
		case EnclosureManager.PieceType.FRONT_CORNER:
			return "Scaffold_Front_Corner.png";
		case EnclosureManager.PieceType.FRONT_LCORNER:
			return "Scaffold_Front_LCorner.png";
		case EnclosureManager.PieceType.FRONT_LEFT:
			return "Scaffold_Front_Left.png";
		case EnclosureManager.PieceType.FRONT_RCORNER:
			return "Scaffold_Front_RCorner.png";
		case EnclosureManager.PieceType.FRONT_RIGHT:
			return "Scaffold_Front_Right.png";
		default:
			return null;
		}
	}

	protected override EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name)
	{
		return mgr.scaffoldingDefs[name];
	}
}
