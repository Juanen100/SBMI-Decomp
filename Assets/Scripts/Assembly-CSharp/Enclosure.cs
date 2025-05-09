using System.Collections.Generic;
using UnityEngine;

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
			sprite = s;
			defName = d;
			this.layer = layer;
			this.sequence = sequence;
		}
	}

	public const float LAYER_HEIGHT = 10f;

	protected const float CORNER_LENGTH = 10f;

	protected const float SIDE_LENGTH = 20f;

	protected AlignedBox box;

	protected float height;

	protected bool needsUpdate = true;

	protected float boxOffset;

	private List<Piece> pieces;

	public bool IsValid
	{
		get
		{
			return pieces != null;
		}
	}

	public AlignedBox Box
	{
		get
		{
			return box;
		}
	}

	public Enclosure(AlignedBox box, float boxOffset, EnclosureManager mgr, BillboardDelegate billboard)
	{
		if (box.Width <= 2f * boxOffset || box.Height <= 2f * boxOffset)
		{
			Debug.Log(string.Format("Requested enclosure is too small: {0}x{1}", box.Width, box.Height));
			return;
		}
		this.boxOffset = boxOffset;
		SetEnclosureBox(box);
		pieces = new List<Piece>();
		AddLayer(mgr, 0, billboard);
	}

	public Enclosure(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard)
		: this(box, 0f, mgr, billboard)
	{
	}

	protected abstract string GetMaterialName(EnclosureManager.PieceType piece);

	protected abstract EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name);

	public bool IsInitialized()
	{
		return pieces != null && pieces.Count > 0;
	}

	public void SetEnclosureBox(AlignedBox box)
	{
		if (this.box == null)
		{
			this.box = new AlignedBox(box.xmin + boxOffset, box.xmax - boxOffset, box.ymin + boxOffset, box.ymax - boxOffset);
		}
		else
		{
			this.box.xmin = box.xmin + boxOffset;
			this.box.xmax = box.xmax - boxOffset;
			this.box.ymin = box.ymin + boxOffset;
			this.box.ymax = box.ymax - boxOffset;
		}
		needsUpdate = true;
	}

	protected virtual void AddLayer(EnclosureManager mgr, int layer, BillboardDelegate billboard)
	{
		AddPiece(mgr, layer, 0f, "front_corner", GetMaterialName(EnclosureManager.PieceType.FRONT_CORNER), billboard);
		AddPiece(mgr, layer, 0f, "back_corner", GetMaterialName(EnclosureManager.PieceType.BACK_CORNER), billboard);
		AddPiece(mgr, layer, 0f, "back_lcorner", GetMaterialName(EnclosureManager.PieceType.BACK_LCORNER), billboard);
		AddPiece(mgr, layer, 0f, "front_lcorner", GetMaterialName(EnclosureManager.PieceType.FRONT_LCORNER), billboard);
		AddPiece(mgr, layer, 0f, "back_rcorner", GetMaterialName(EnclosureManager.PieceType.BACK_RCORNER), billboard);
		AddPiece(mgr, layer, 0f, "front_rcorner", GetMaterialName(EnclosureManager.PieceType.FRONT_RCORNER), billboard);
		float num = box.xmax - box.xmin;
		float num2 = box.ymax - box.ymin;
		float num3 = num - 20f;
		int num4 = Mathf.CeilToInt(num3 / 20f);
		float num5 = num3 / ((float)num4 * 20f);
		for (int i = 0; i < num4; i++)
		{
			AddPiece(mgr, layer, (float)i * num5, "front_left", GetMaterialName(EnclosureManager.PieceType.FRONT_LEFT), billboard);
			AddPiece(mgr, layer, (float)i * num5, "back_right", GetMaterialName(EnclosureManager.PieceType.BACK_RIGHT), billboard);
		}
		num3 = num2 - 20f;
		num4 = Mathf.CeilToInt(num3 / 20f);
		num5 = num3 / ((float)num4 * 20f);
		for (int j = 0; j < num4; j++)
		{
			AddPiece(mgr, layer, (float)j * num5, "front_right", GetMaterialName(EnclosureManager.PieceType.FRONT_RIGHT), billboard);
			AddPiece(mgr, layer, (float)j * num5, "back_left", GetMaterialName(EnclosureManager.PieceType.BACK_LEFT), billboard);
		}
	}

	public void SetHeight(EnclosureManager mgr, float newHeight, BillboardDelegate billboard)
	{
		if (!IsValid || !(newHeight > height))
		{
			return;
		}
		int num = (int)(height / 10f);
		int num2 = (int)(newHeight / 10f);
		if (num2 > num)
		{
			for (int i = num + 1; i <= num2; i++)
			{
				AddLayer(mgr, i, billboard);
			}
			needsUpdate = true;
		}
		height = newHeight;
	}

	protected void AddPiece(EnclosureManager mgr, int layer, float sequence, string defName, string spriteName, BillboardDelegate billboard)
	{
		EnclosureManager.PieceDef def = GetDef(mgr, defName);
		BasicSprite basicSprite = new BasicSprite(null, spriteName, new Vector2(0f, -0.5f * def.height), def.width, def.height);
		basicSprite.PublicInitialize();
		basicSprite.Billboard(billboard);
		basicSprite.Visible = false;
		Piece item = new Piece(basicSprite, defName, layer, sequence);
		pieces.Add(item);
	}

	public virtual void OnUpdate(Simulation simulation, EnclosureManager mgr)
	{
		if (!needsUpdate || !IsValid)
		{
			return;
		}
		Vector3 direction = -simulation.TheCamera.transform.forward;
		direction.Scale(new Vector3(2f, 0f, 1f));
		foreach (Piece piece in pieces)
		{
			BasicSprite sprite = piece.sprite;
			if (!sprite.Visible)
			{
				sprite.Visible = true;
			}
			EnclosureManager.PieceDef def = GetDef(mgr, piece.defName);
			sprite.Position = mgr.CalcPosition(def.type, box) + def.placementOffset + piece.sequence * def.sequenceOffset;
			if (def.type == EnclosureManager.PieceType.FRONT_RIGHT)
			{
				sprite.Face(direction, simulation.TheCamera.transform.up);
				if (sprite.BillboardScaling != def.scale)
				{
					sprite.BillboardScaling = def.scale;
				}
			}
			else
			{
				sprite.OnUpdate(simulation.TheCamera, simulation.particleSystemManager);
			}
			sprite.Translate(-def.textureOrigin);
			sprite.Translate(new Vector3(0f, 10f * (float)piece.layer, 0f));
		}
		needsUpdate = false;
	}

	public virtual void Destroy()
	{
		if (!IsValid)
		{
			return;
		}
		foreach (Piece piece in pieces)
		{
			if (piece.sprite != null)
			{
				piece.sprite.Destroy();
			}
		}
	}
}
