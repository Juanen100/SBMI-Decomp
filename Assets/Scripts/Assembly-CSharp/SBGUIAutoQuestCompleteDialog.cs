using System.Collections.Generic;
using UnityEngine;

public class SBGUIAutoQuestCompleteDialog : SBGUIScreen
{
	private SBGUIPulseButton okayButton;

	private SBGUIAtlasImage window;

	public void SetupDialogInfo(string sDialogHeading, string sDialogBody, string sPortrait, List<Reward> pRewards, QuestDefinition pQuestDef)
	{
		SBGUIShadowedLabel sBGUIShadowedLabel = (SBGUIShadowedLabel)FindChild("dialog_heading");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("dialog_body");
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)FindChild("dialog_body_boundary");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("portrait");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("portrait_shadow");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("reward_label");
		SBGUILabel sBGUILabel3 = (SBGUILabel)FindChild("reward_gold_label");
		SBGUILabel sBGUILabel4 = (SBGUILabel)FindChild("reward_xp_label");
		window = (SBGUIAtlasImage)FindChild("window");
		okayButton = (SBGUIPulseButton)FindChild("okay");
		int num = 0;
		int num2 = 0;
		int count = pRewards.Count;
		for (int i = 0; i < count; i++)
		{
			Reward reward = pRewards[i];
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				num += reward.ResourceAmounts[ResourceManager.SOFT_CURRENCY];
			}
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.XP))
			{
				num2 += reward.ResourceAmounts[ResourceManager.XP];
			}
		}
		string text = Language.Get(sDialogBody);
		if (text.Contains("{0}") && pQuestDef.AutoQuestCharacterID >= 0)
		{
			Simulated simulated = session.TheGame.simulation.FindSimulated(pQuestDef.AutoQuestCharacterID);
			if (simulated != null && simulated.HasEntity<ResidentEntity>())
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				text = string.Format(text, Language.Get(entity.Name));
			}
		}
		sBGUILabel3.SetText(num.ToString());
		sBGUILabel4.SetText(num2.ToString());
		sBGUIShadowedLabel.SetText(Language.Get(sDialogHeading));
		sBGUILabel.SetText(text);
		sBGUILabel.AdjustText(boundary);
		sBGUILabel2.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		sBGUIAtlasImage.SetTextureFromAtlas(sPortrait, true);
		sBGUIAtlasImage2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
	}
}
