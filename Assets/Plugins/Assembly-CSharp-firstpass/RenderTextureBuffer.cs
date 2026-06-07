using System.Collections.Generic;
using UnityEngine;

public class RenderTextureBuffer
{
	public enum QualityMode
	{
		VeryLow = 0,
		Low = 1,
		Medium = 2,
		High = 3,
		VeryHigh = 4
	}

	public class QualityModeSettings
	{
		public int Width;

		public int Height;

		public int Depth;
	}

	private class PendingTextures
	{
		public Texture texture;

		public Vector2 uvs;

		public bool destroyOnLoad;

		public bool processImmidiatly;

		public void Clear()
		{
		}
	}

	private const RenderTextureFormat INVALID_FORMAT = RenderTextureFormat.Depth;

	private RenderTextureFormat mTextureFormat;

	private RenderTexture mRenderBuffer;

	private UVMapTree mMapTree;

	private List<PendingTextures> mPendingWarehouse;

	private List<PendingTextures> mTexturesToAdd;

	public Texture Texture
	{
		get
		{
			return null;
		}
	}

	private PendingTextures CreatePending()
	{
		return null;
	}

	private void ReturnPending(PendingTextures p)
	{
	}

	public bool FindBestSupportedFormatsWithAlpha(QualityMode q)
	{
		return false;
	}

	private RenderTextureFormat FindFirstSupported(RenderTextureFormat[] tests)
	{
		return default(RenderTextureFormat);
	}

	private bool CheckValidFormatFound()
	{
		return false;
	}

	public QualityModeSettings SettingsForMode(QualityMode mode)
	{
		return null;
	}

	public bool Create(QualityMode mode, bool clearBuffer = false)
	{
		return false;
	}

	public bool AddTexture(Texture tx, bool destroyAfterLoad = false, bool processImmidiatly = true)
	{
		return false;
	}

	public void UpdateRenderTexture()
	{
	}
}
