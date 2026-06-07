public class Scaffolding : Enclosure
{
	private const float BOX_OFFSET = 5f;

	public Scaffolding(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard)
		: base(null, 0f, null, null)
	{
	}

	protected override string GetMaterialName(EnclosureManager.PieceType piece)
	{
		return null;
	}

	protected override EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name)
	{
		return null;
	}
}
