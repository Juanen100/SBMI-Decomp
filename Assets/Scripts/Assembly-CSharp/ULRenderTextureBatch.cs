using System.Collections;
using UnityEngine;

public class ULRenderTextureBatch
{
	private ArrayList batchList;

	private ULRenderTextureCameraRig renderTextureCameraRig;

	public ULRenderTextureCameraRig CameraRig
	{
		get
		{
			return null;
		}
	}

	public ArrayList BatchList
	{
		get
		{
			return null;
		}
	}

	public ULRenderTextureBatch(int workingLayer)
	{
	}

	public ULRenderTextureBatchEntry AddEntry(GameObject subject, int squareSize, string shaderIdentifier, ULRenderTextureCameraRig.RelativeCamDelegate camDelegate)
	{
		return null;
	}

	public ULRenderTextureBatchEntry AddEntry(GameObject subject, ULRenderTexture target, ULRenderTextureCameraRig.RelativeCamDelegate camDelegate)
	{
		return null;
	}

	public void BatchUpdate(bool useCamDelegate)
	{
	}
}
