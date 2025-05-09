using System.Collections.Generic;
using UnityEngine;

public class SBGUISpongyGamesDialog : SBGUIScreen
{
	public void Setup(SpongyGamesDialogInputData pInputData)
	{
		GameObject gameObject = null;
		int num = 4;
		SBGUIAtlasImage[] array = new SBGUIAtlasImage[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (SBGUIAtlasImage)FindChild("character_portrait_" + i);
			if (i == 0)
			{
				gameObject = array[i].gameObject.transform.parent.gameObject;
			}
		}
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("description_one_boundary");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("description_one_label");
		GameObject gameObject2 = sBGUIAtlasImage.gameObject.transform.parent.gameObject;
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("description_two_boundary");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("description_two_label");
		GameObject gameObject3 = sBGUIAtlasImage2.gameObject.transform.parent.gameObject;
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)FindChild("event_icon");
		SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)FindChild("verticle_bar");
		SBGUILabel sBGUILabel3 = (SBGUILabel)FindChild("event_results_label");
		SBGUILabel sBGUILabel4 = (SBGUILabel)FindChild("event_title_label");
		SBGUILabel sBGUILabel5 = (SBGUILabel)FindChild("todays_event_label");
		SBGUILabel sBGUILabel6 = (SBGUILabel)FindChild("first_day_title_label");
		SBGUIAtlasImage sBGUIAtlasImage5 = (SBGUIAtlasImage)FindChild("first_day_character_portrait");
		GameObject gameObject4 = sBGUIAtlasImage5.gameObject.transform.parent.gameObject;
		SBGUILabel sBGUILabel7 = (SBGUILabel)FindChild("last_day_character_label");
		SBGUIAtlasImage sBGUIAtlasImage6 = (SBGUIAtlasImage)FindChild("last_day_character_portrait");
		SBGUIAtlasImage sBGUIAtlasImage7 = (SBGUIAtlasImage)FindChild("last_day_description_two_boundary");
		SBGUILabel sBGUILabel8 = (SBGUILabel)FindChild("last_day_description_two_label");
		SBGUIAtlasImage sBGUIAtlasImage8 = (SBGUIAtlasImage)FindChild("last_day_reward_portrait");
		SBGUILabel sBGUILabel9 = (SBGUILabel)FindChild("last_day_title_one_label");
		GameObject gameObject5 = sBGUILabel9.gameObject.transform.parent.gameObject;
		Dictionary<string, object> eventData = pInputData.EventData;
		int num2 = TFUtils.LoadInt(eventData, "event_days");
		int num3 = TFUtils.LoadInt(eventData, "day");
		string text = TFUtils.TryLoadString(eventData, "title");
		if (text == null)
		{
			text = string.Empty;
		}
		string text2 = TFUtils.TryLoadString(eventData, "description_one");
		if (text2 == null)
		{
			text2 = string.Empty;
		}
		string text3 = TFUtils.TryLoadString(eventData, "description_two");
		if (text3 == null)
		{
			text3 = string.Empty;
		}
		string text4 = TFUtils.TryLoadString(eventData, "event_portrait");
		if (text4 == null)
		{
			text4 = string.Empty;
		}
		string text5 = TFUtils.TryLoadString(eventData, "event_name");
		if (text5 == null)
		{
			text5 = string.Empty;
		}
		List<int> list = TFUtils.TryLoadList<int>(eventData, "characters");
		int count = list.Count;
		if (num3 == 1)
		{
			gameObject5.SetActive(false);
			gameObject4.SetActive(true);
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			gameObject3.SetActive(true);
			sBGUIAtlasImage3.SetActive(true);
			sBGUIAtlasImage4.SetActive(true);
			sBGUILabel3.SetActive(false);
			sBGUILabel4.SetActive(true);
			sBGUILabel5.SetActive(true);
		}
		else if (num3 == num2)
		{
			gameObject5.SetActive(true);
			gameObject4.SetActive(false);
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			gameObject3.SetActive(false);
			sBGUIAtlasImage3.SetActive(false);
			sBGUIAtlasImage4.SetActive(false);
			sBGUILabel3.SetActive(false);
			sBGUILabel4.SetActive(false);
			sBGUILabel5.SetActive(false);
		}
		else
		{
			gameObject5.SetActive(false);
			gameObject4.SetActive(false);
			gameObject.SetActive(true);
			gameObject2.SetActive(true);
			gameObject3.SetActive(true);
			sBGUIAtlasImage3.SetActive(true);
			sBGUIAtlasImage4.SetActive(true);
			sBGUILabel3.SetActive(true);
			sBGUILabel4.SetActive(true);
			sBGUILabel5.SetActive(true);
		}
		if (gameObject4.activeSelf)
		{
			sBGUIAtlasImage5.SetActive(false);
			if (count > 0)
			{
				Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, list[0], true);
				if (blueprint != null && blueprint.Invariable.ContainsKey("quest_reminder_icon"))
				{
					sBGUIAtlasImage5.SetActive(true);
					sBGUIAtlasImage5.SetTextureFromAtlas((string)blueprint.Invariable["quest_reminder_icon"], true, false, true);
				}
			}
			sBGUILabel6.SetText(Language.Get(text));
		}
		else if (gameObject5.activeSelf)
		{
			sBGUILabel7.SetText(Language.Get(text5));
			sBGUILabel9.SetText(Language.Get(text));
			sBGUILabel8.SetText(Language.Get(text3));
			sBGUIAtlasImage6.SetActive(false);
			if (count > 0)
			{
				Blueprint blueprint2 = EntityManager.GetBlueprint(EntityType.RESIDENT, list[0], true);
				if (blueprint2 != null && blueprint2.Invariable.ContainsKey("quest_reminder_icon"))
				{
					sBGUIAtlasImage6.SetActive(true);
					sBGUIAtlasImage6.SetTextureFromAtlas((string)blueprint2.Invariable["quest_reminder_icon"], true, false, true);
				}
			}
			sBGUIAtlasImage8.SetTextureFromAtlas(text4, true, false, true);
		}
		else
		{
			int num4 = 0;
			float num5 = 0.4f;
			for (int j = 0; j < num; j++)
			{
				SBGUIAtlasImage sBGUIAtlasImage9 = array[j];
				if (j >= count)
				{
					sBGUIAtlasImage9.SetActive(false);
					continue;
				}
				int did = list[j];
				Blueprint blueprint3 = EntityManager.GetBlueprint(EntityType.RESIDENT, did, true);
				if (blueprint3 == null)
				{
					sBGUIAtlasImage9.SetActive(false);
					continue;
				}
				if (!blueprint3.Invariable.ContainsKey("quest_reminder_icon"))
				{
					sBGUIAtlasImage9.SetActive(false);
					continue;
				}
				sBGUIAtlasImage9.SetActive(true);
				sBGUIAtlasImage9.SetTextureFromAtlas((string)blueprint3.Invariable["quest_reminder_icon"], true, false, true);
				Transform transform = sBGUIAtlasImage9.transform;
				if (j <= 1)
				{
					transform.localPosition = new Vector3(transform.localPosition.x, num5, transform.localPosition.z);
				}
				else
				{
					transform.localPosition = new Vector3(transform.localPosition.x, 0f - num5, transform.localPosition.z);
				}
				num4++;
			}
			if (num4 <= 2)
			{
				for (int k = 0; k < num; k++)
				{
					SBGUIAtlasImage sBGUIAtlasImage9 = array[k];
					if (sBGUIAtlasImage9.IsActive())
					{
						Transform transform = sBGUIAtlasImage9.transform;
						transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
					}
				}
			}
			sBGUILabel3.SetText(Language.Get(text));
		}
		if (gameObject2.activeInHierarchy)
		{
			sBGUILabel.SetText(Language.Get(text2));
		}
		if (gameObject3.activeInHierarchy)
		{
			sBGUILabel2.SetText(Language.Get(text3));
		}
		if (sBGUILabel4.gameObject.activeInHierarchy)
		{
			sBGUILabel4.SetText(Language.Get(text5));
		}
		if (sBGUIAtlasImage3.gameObject.activeInHierarchy)
		{
			sBGUIAtlasImage3.SetTextureFromAtlas(text4, true, false, true);
		}
	}
}
