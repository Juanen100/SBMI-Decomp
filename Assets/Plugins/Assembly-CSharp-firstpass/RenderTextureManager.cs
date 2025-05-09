using System.Collections.Generic;
using UnityEngine;

public class RenderTextureManager
{
	private const int MaxAtlases = 4;

	private List<RenderTextureBuffer> mBufferList = new List<RenderTextureBuffer>(4);

	private static RenderTextureManager sActive;

	private RenderTextureBuffer.QualityMode mQuality;

	public static RenderTextureManager Active
	{
		get
		{
			return sActive;
		}
	}

	public static void CreateActive(RenderTextureBuffer.QualityMode quality)
	{
		if (sActive == null)
		{
			sActive = new RenderTextureManager();
			sActive.mQuality = quality;
		}
	}

	public bool AddTexture(Texture texture, bool destroyOnLoad, bool processInstantly = false)
	{
		bool flag = false;
		for (int i = 0; i < mBufferList.Count; i++)
		{
			if (mBufferList[i].AddTexture(texture, destroyOnLoad))
			{
				if (processInstantly)
				{
					mBufferList[i].UpdateRenderTexture();
				}
				flag = true;
				break;
			}
		}
		if (!flag && mBufferList.Count != mBufferList.Capacity)
		{
			RenderTextureBuffer renderTextureBuffer = new RenderTextureBuffer();
			mBufferList.Add(renderTextureBuffer);
			if (renderTextureBuffer.Create(mQuality))
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
				gameObject.name = "Buffer_" + mBufferList.Count;
				gameObject.GetComponent<Renderer>().material.mainTexture = renderTextureBuffer.Texture;
				if (renderTextureBuffer.AddTexture(texture, destroyOnLoad))
				{
					if (processInstantly)
					{
						renderTextureBuffer.UpdateRenderTexture();
					}
					flag = true;
				}
			}
		}
		return flag;
	}

	public void UpdateRenderBuffers()
	{
		for (int i = 0; i < mBufferList.Count; i++)
		{
			mBufferList[i].UpdateRenderTexture();
		}
	}

	public static void DestroyActive()
	{
		sActive = null;
	}
}
