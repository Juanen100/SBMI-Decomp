using System.Collections.Generic;

public abstract class Enclosure
{
	private class Piece
	{
		public BasicSprite sprite;

		public string defName;

		public int layer;

		public float sequence;

		public Piece(BasicSprite s, string d, int layer, float sequence)
		{
		}
	}

	public const float LAYER_HEIGHT = 10f;

	protected const float CORNER_LENGTH = 10f;

	protected const float SIDE_LENGTH = 20f;

	protected AlignedBox box;

	protected float height;

	protected bool needsUpdate;

	protected float boxOffset;

	private List<Piece> pieces;

	public bool IsValid
	{
		get
		{
			return false;
		}
	}

	public AlignedBox Box
	{
		get
		{
			return null;
		}
	}

	public Enclosure(AlignedBox box, float boxOffset, EnclosureManager mgr, BillboardDelegate billboard)
	{
	}

	public Enclosure(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard)
	{
	}

	protected abstract string GetMaterialName(EnclosureManager.PieceType piece);

	protected abstract EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name);

	public bool IsInitialized()
	{
		return false;
	}

	public void SetEnclosureBox(AlignedBox box)
	{
	}

	protected virtual void AddLayer(EnclosureManager mgr, int layer, BillboardDelegate billboard)
	{
	}

	public void SetHeight(EnclosureManager mgr, float newHeight, BillboardDelegate billboard)
	{
	}

	protected void AddPiece(EnclosureManager mgr, int layer, float sequence, string defName, string spriteName, BillboardDelegate billboard)
	{
	}

	public virtual void OnUpdate(Simulation simulation, EnclosureManager mgr)
	{
	}

	public virtual void Destroy()
	{
	}
}
