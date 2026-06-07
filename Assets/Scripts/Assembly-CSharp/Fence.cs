public class Fence : Enclosure
{
	private const float BOX_OFFSET = 5f;

	public Fence(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard)
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

	protected override void AddLayer(EnclosureManager mgr, int layer, BillboardDelegate billboard)
	{
	}

	public override void OnUpdate(Simulation simulation, EnclosureManager mgr)
	{
	}

	public override void Destroy()
	{
	}
}
