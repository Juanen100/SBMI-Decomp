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

		public PendingTextures()
		{
			uvs = default(Vector2);
		}

		public void Clear()
		{
			texture = null;
			uvs.x = (uvs.y = 0f);
		}
	}

	private const RenderTextureFormat INVALID_FORMAT = RenderTextureFormat.Depth;

	private RenderTextureFormat mTextureFormat = RenderTextureFormat.Depth;

	private RenderTexture mRenderBuffer;

	private UVMapTree mMapTree;

	private List<PendingTextures> mPendingWarehouse = new List<PendingTextures>(32);

	private List<PendingTextures> mTexturesToAdd;

	public Texture Texture
	{
		get
		{
			return mRenderBuffer;
		}
	}

	private PendingTextures CreatePending()
	{
		if (mPendingWarehouse.Count == 0)
		{
			return new PendingTextures();
		}
		PendingTextures result = mPendingWarehouse[mPendingWarehouse.Count - 1];
		mPendingWarehouse.RemoveAt(mPendingWarehouse.Count - 1);
		return result;
	}

	private void ReturnPending(PendingTextures p)
	{
		p.Clear();
		if (mPendingWarehouse.Count != mPendingWarehouse.Capacity)
		{
			mPendingWarehouse.Add(p);
		}
	}

	public bool FindBestSupportedFormatsWithAlpha(QualityMode q)
	{
		if (q > QualityMode.VeryHigh)
		{
			return false;
		}
		RenderTextureFormat[] array = null;
		switch (q)
		{
		case QualityMode.VeryLow:
		case QualityMode.Low:
			array = new RenderTextureFormat[2]
			{
				RenderTextureFormat.ARGB1555,
				RenderTextureFormat.ARGB4444
			};
			break;
		case QualityMode.Medium:
			array = new RenderTextureFormat[3]
			{
				RenderTextureFormat.ARGB4444,
				RenderTextureFormat.ARGB32,
				RenderTextureFormat.ARGBHalf
			};
			break;
		default:
			array = new RenderTextureFormat[2]
			{
				RenderTextureFormat.ARGB32,
				RenderTextureFormat.ARGBFloat
			};
			break;
		}
		mTextureFormat = FindFirstSupported(array);
		if (mTextureFormat == RenderTextureFormat.Depth)
		{
			Debug.LogError("No Valid Render Format Found For Quality: " + q);
			QualityMode q2 = q + 1;
			return FindBestSupportedFormatsWithAlpha(q2);
		}
		return true;
	}

	private RenderTextureFormat FindFirstSupported(RenderTextureFormat[] tests)
	{
		RenderTextureFormat result = RenderTextureFormat.Depth;
		for (int i = 0; i < tests.Length; i++)
		{
			if (SystemInfo.SupportsRenderTextureFormat(tests[i]))
			{
				result = tests[i];
				break;
			}
		}
		return result;
	}

	private bool CheckValidFormatFound()
	{
		return mTextureFormat != RenderTextureFormat.Depth;
	}

	public QualityModeSettings SettingsForMode(QualityMode mode)
	{
		QualityModeSettings qualityModeSettings = new QualityModeSettings();
		switch (mode)
		{
		case QualityMode.VeryLow:
			qualityModeSettings.Height = (qualityModeSettings.Width = 1024);
			qualityModeSettings.Depth = 0;
			break;
		case QualityMode.Low:
			qualityModeSettings.Height = (qualityModeSettings.Width = 1024);
			qualityModeSettings.Depth = 0;
			break;
		case QualityMode.Medium:
			qualityModeSettings.Height = (qualityModeSettings.Width = 2048);
			qualityModeSettings.Depth = 0;
			break;
		case QualityMode.High:
			qualityModeSettings.Height = (qualityModeSettings.Width = 4096);
			qualityModeSettings.Depth = 0;
			break;
		case QualityMode.VeryHigh:
			qualityModeSettings.Height = (qualityModeSettings.Width = 4096);
			qualityModeSettings.Depth = 0;
			break;
		}
		return qualityModeSettings;
	}

	public bool Create(QualityMode mode, bool clearBuffer = false)
	{
		FindBestSupportedFormatsWithAlpha(mode);
		QualityModeSettings qualityModeSettings = SettingsForMode(mode);
		mRenderBuffer = new RenderTexture(qualityModeSettings.Width, qualityModeSettings.Height, qualityModeSettings.Depth, mTextureFormat, RenderTextureReadWrite.Linear);
		mRenderBuffer.useMipMap = false;
		if (clearBuffer)
		{
			RenderTexture.active = mRenderBuffer;
			GL.Clear(true, true, Color.white);
			RenderTexture.active = null;
		}
		if (!mRenderBuffer.Create())
		{
			Debug.LogError("RenderTexture failed to be created");
			return false;
		}
		mMapTree = new UVMapTree(new Vector2(qualityModeSettings.Width, qualityModeSettings.Height));
		return true;
	}

	public bool AddTexture(Texture tx, bool destroyAfterLoad = false, bool processImmidiatly = true)
	{
		if (tx == null)
		{
			return false;
		}
		if (tx.width == 0)
		{
			return false;
		}
		if (mTexturesToAdd == null)
		{
			mTexturesToAdd = new List<PendingTextures>(2);
		}
		PendingTextures pendingTextures = CreatePending();
		pendingTextures.processImmidiatly = processImmidiatly;
		pendingTextures.texture = tx;
		pendingTextures.destroyOnLoad = destroyAfterLoad;
		bool flag = false;
		if (processImmidiatly)
		{
			pendingTextures.uvs.x = pendingTextures.texture.width;
			pendingTextures.uvs.y = pendingTextures.texture.height;
			flag = mMapTree.AddTexture(pendingTextures.uvs, ref pendingTextures.uvs);
		}
		else
		{
			flag = true;
		}
		mTexturesToAdd.Add(pendingTextures);
		return flag;
	}

	public void UpdateRenderTexture()
	{
		if (mTexturesToAdd == null || mTexturesToAdd.Count == 0)
		{
			return;
		}
		Rect screenRect = default(Rect);
		RenderTexture.active = mRenderBuffer;
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, mRenderBuffer.width, mRenderBuffer.height, 0f);
		Vector2 vector = new Vector2(0f, 0f);
		for (int i = 0; i < mTexturesToAdd.Count; i++)
		{
			PendingTextures pendingTextures = mTexturesToAdd[i];
			Texture texture = pendingTextures.texture;
			vector.x = texture.width;
			vector.y = texture.height;
			if (!pendingTextures.processImmidiatly)
			{
				mMapTree.AddTexture(vector, ref pendingTextures.uvs);
			}
			screenRect.position = pendingTextures.uvs;
			screenRect.size = vector;
			Debug.Log(screenRect.ToString());
			Graphics.DrawTexture(screenRect, texture);
			if (pendingTextures.destroyOnLoad)
			{
				Resources.UnloadAsset(texture);
			}
		}
		GL.PopMatrix();
		RenderTexture.active = null;
		mTexturesToAdd.Clear();
	}
}
