using System;
using UnityEngine;

public class SBGUIExplanationDialog : SBGUIModalDialog
{
	public void Setup(string message)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("dialog_label");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("dialog_label_boundary");
		Debug.LogError("dialogBoundary " + sBGUIAtlasImage);
		sBGUILabel.SetText(message);
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("character_icon");
		sBGUIAtlasImage2.SetTextureFromAtlas("MrKrabsPortrait_Whaddayamean", true, false, true);
		try
		{
			if (null != sBGUIAtlasImage)
			{
				sBGUILabel.AdjustText(sBGUIAtlasImage);
			}
		}
		catch (Exception)
		{
			TFUtils.DebugLog("-------dialogLabel.AdjustText(dialogBoundary) Exception-------------");
		}
	}
}
