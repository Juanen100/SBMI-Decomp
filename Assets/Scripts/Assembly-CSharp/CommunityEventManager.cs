using System;
using System.Collections.Generic;
using MTools;
using UnityEngine;

public class CommunityEventManager
{
	private const int m_nUpdateEventValueTimeLimit = 300;

	private const int m_nUpdateEventMinWaitTime = 10;

	public static Dictionary<string, object> _pEventStatusDialogData;

	public static string _sSpongyGamesEventID = "1";

	public static string _sSpongyGamesLastDayEventID = "2";

	public static string _sChrismas14EventID = "3";

	public static int _nColiseumDID = 20403;

	public static string _sValentines15EventID = "4";

	private volatile Action dialogNeededCallback;

	private MDictionary m_pCommunityEventDefinitions;

	private float m_fUpdateEventValueTimer;

	private float m_fPreviousValueUpdateTime;

	private float m_fPreviousRewardUpdateTime;

	private float m_fPreviousBannerPingTime;

	private Session m_pSession;

	public Action DialogNeededCallback
	{
		set
		{
			dialogNeededCallback = value;
		}
	}

	public CommunityEventManager(Session pSession)
	{
		m_pSession = pSession;
		LoadCommunityEventsFromSpreadsheets();
		SoaringCommunityEventManager.GetValueFinished += HandleSoaringCallFinished;
		SoaringCommunityEventManager.SetValueFinished += HandleSoaringCallFinished;
		SoaringCommunityEventManager.AquireGiftFinished += HandleSoaringCallFinished;
		SoaringCommunityEventManager.AquireGiftFinished += HandleSoaringAquireGiftFinished;
	}

	public Session GetSession()
	{
		return m_pSession;
	}

	public void DialogNeeded()
	{
		if (dialogNeededCallback != null)
		{
			dialogNeededCallback();
		}
	}

	public void QuestComplete(uint nQuestID)
	{
		CommunityEvent activeEvent = GetActiveEvent();
		if (activeEvent != null && (activeEvent.m_sID == _sSpongyGamesEventID || activeEvent.m_sID == _sSpongyGamesLastDayEventID) && activeEvent.m_nQuestPrereqID == nQuestID && (m_pSession.TheGame.simulation.FindSimulated(_nColiseumDID) != null || m_pSession.TheGame.inventory.HasItem(_nColiseumDID)))
		{
			Soaring.FireEvent("spongy_games_banner", null);
		}
	}

	private void HandleSoaringAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		if (!bSuccess || m_pSession == null)
		{
			return;
		}
		string text = pContext.soaringValue("eventDid").ToString();
		int num = pContext.soaringValue("giftDid");
		CommunityEvent communityEvent = (CommunityEvent)m_pCommunityEventDefinitions.objectWithKey(text);
		if (communityEvent == null)
		{
			return;
		}
		SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(text);
		if (soaringCommunityEvent == null || (bool)pData.soaringValue("already_acquired"))
		{
			return;
		}
		bool flag = pContext.soaringValue("purchased");
		int num2 = 0;
		Cost cost = null;
		Dictionary<int, int> dictionary = null;
		if (flag)
		{
			int amount = m_pSession.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount;
			int num3 = soaringCommunityEvent.GetReward(num).m_nValue - amount;
			if (num3 > 0)
			{
				dictionary = new Dictionary<int, int>();
				dictionary.Add(communityEvent.m_nValueID, num3);
				dictionary.Add(ResourceManager.HARD_CURRENCY, -(int)pContext.soaringValue("purchaseCost"));
			}
			num2 = pContext.soaringValue("purchaseCost");
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			dictionary2.Add(ResourceManager.HARD_CURRENCY, num2);
			cost = new Cost(dictionary2);
		}
		CommunityEvent.Reward reward = communityEvent.GetReward(num);
		string empty = string.Empty;
		Reward reward2;
		if (reward.m_sType == "recipe")
		{
			CraftingRecipe recipeById = m_pSession.TheGame.craftManager.GetRecipeById(num);
			empty = recipeById.recipeName;
			reward2 = new Reward(dictionary, null, null, new List<int> { num }, null, null, null, null, false, null);
			FoundItemDialogInputData item = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get(recipeById.recipeName)), reward.m_sTexture, "Beat_FoundRecipe");
			m_pSession.TheGame.dialogPackageManager.AddDialogInputBatch(m_pSession.TheGame, new List<DialogInputData> { item });
			DialogNeeded();
			if (flag)
			{
				m_pSession.analytics.LogPurchaseEventReward(recipeById.recipeName, cost, m_pSession.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			}
		}
		else
		{
			List<int> list = null;
			Dictionary<int, Vector2> buildingPositions = null;
			int[] landIDs = reward.LandIDs;
			int num4 = landIDs.Length;
			if (num4 > 0)
			{
				list = new List<int>(num4);
				for (int i = 0; i < num4; i++)
				{
					list.Add(landIDs[i]);
				}
				Dictionary<int, Vector2> dictionary3 = new Dictionary<int, Vector2>();
				dictionary3.Add(num, new Vector2(reward.m_nAutoPlaceX, reward.m_nAutoPlaceY));
				buildingPositions = dictionary3;
			}
			reward2 = new Reward(dictionary, new Dictionary<int, int> { { num, 1 } }, buildingPositions, null, null, null, list, null, false, null);
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, num, true);
			empty = (string)blueprint.Invariable["name"];
			FoundItemDialogInputData item2 = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get(empty)), reward.m_sTexture, "Beat_FoundRecipe");
			m_pSession.TheGame.dialogPackageManager.AddDialogInputBatch(m_pSession.TheGame, new List<DialogInputData> { item2 });
			if (reward.m_nDialogSequenceID >= 0)
			{
				m_pSession.TheGame.questManager.AddDialogSequences(m_pSession.TheGame, 1u, (uint)reward.m_nDialogSequenceID, new List<Reward>(), 0u, false);
			}
			DialogNeeded();
			if (flag)
			{
				m_pSession.analytics.LogPurchaseEventReward((string)blueprint.Invariable["name"], cost, m_pSession.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			}
		}
		m_pSession.TheGame.ApplyReward(reward2, TFUtils.EpochTime(), false);
		m_pSession.TheGame.ModifyGameState(new ReceiveRewardAction(reward2, num.ToString()));
		AnalyticsWrapper.LogRecievedEventItem(m_pSession.TheGame, num, empty);
		if (num == _nColiseumDID)
		{
			m_fPreviousBannerPingTime = 0f;
			CheckEventBannerPing();
		}
	}

	private void HandleSoaringCallFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string text = pContext.soaringValue("eventDid").ToString();
		CommunityEvent communityEvent = (CommunityEvent)m_pCommunityEventDefinitions.objectWithKey(text);
		if (communityEvent != null)
		{
			if (Soaring.CommunityEventManager.GetEvent(text) == null)
			{
				communityEvent.SetActive(false);
			}
			else
			{
				communityEvent.SetActive(true);
			}
		}
	}

	public CommunityEvent GetActiveEvent()
	{
		bool isOnline = Soaring.IsOnline;
		int num = m_pCommunityEventDefinitions.count();
		CommunityEvent communityEvent = null;
		for (int i = 0; i < num; i++)
		{
			communityEvent = (CommunityEvent)m_pCommunityEventDefinitions.objectAtIndex(i);
			if (isOnline)
			{
				if (communityEvent.m_bActive)
				{
					return communityEvent;
				}
				continue;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow > communityEvent.m_pStartDate && utcNow < communityEvent.m_pEndDate)
			{
				return communityEvent;
			}
		}
		return null;
	}

	public CommunityEvent[] GetEvents()
	{
		int num = m_pCommunityEventDefinitions.count();
		CommunityEvent[] array = new CommunityEvent[num];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			array[num2++] = (CommunityEvent)m_pCommunityEventDefinitions.objectAtIndex(i);
		}
		return array;
	}

	public void OnUpdate(Session pSession)
	{
		if (m_pSession == null || pSession != m_pSession)
		{
			m_pSession = pSession;
		}
		CheckValueUpdate();
		CheckClaimRewardUpdate();
		CheckEventBannerPing();
	}

	private void CheckValueUpdate()
	{
		m_fUpdateEventValueTimer += Time.deltaTime;
		if (Time.time - m_fPreviousValueUpdateTime <= 10f)
		{
			return;
		}
		if (m_fUpdateEventValueTimer >= 300f)
		{
			UpdateValuesToSoaring();
			m_fUpdateEventValueTimer = 0f;
			return;
		}
		int num = m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)m_pCommunityEventDefinitions.objectAtIndex(i);
			if (!communityEvent.m_bActive)
			{
				continue;
			}
			SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(communityEvent.m_sID);
			if (soaringCommunityEvent == null)
			{
				continue;
			}
			bool flag = false;
			int amount = m_pSession.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount;
			int num2 = soaringCommunityEvent.CommunityRewards.Length;
			for (int j = 0; j < num2; j++)
			{
				SoaringCommunityEvent.Reward reward = soaringCommunityEvent.CommunityRewards[j];
				if (!reward.m_bUnlocked && amount >= reward.m_nValue)
				{
					UpdateValueToSoaring(communityEvent);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			num2 = soaringCommunityEvent.IndividualRewards.Length;
			for (int k = 0; k < num2; k++)
			{
				SoaringCommunityEvent.Reward reward = soaringCommunityEvent.IndividualRewards[k];
				if (!reward.m_bUnlocked && amount >= reward.m_nValue)
				{
					UpdateValueToSoaring(communityEvent);
					flag = true;
					break;
				}
			}
		}
	}

	private void CheckClaimRewardUpdate()
	{
		if (Time.time - m_fPreviousRewardUpdateTime <= 10f)
		{
			return;
		}
		int num = m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)m_pCommunityEventDefinitions.objectAtIndex(i);
			if (!communityEvent.m_bActive || (communityEvent.m_nQuestPrereqID >= 0 && !m_pSession.TheGame.questManager.IsQuestCompleted((uint)communityEvent.m_nQuestPrereqID)))
			{
				continue;
			}
			SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(communityEvent.m_sID);
			if (soaringCommunityEvent == null)
			{
				continue;
			}
			int num2 = soaringCommunityEvent.CommunityRewards.Length;
			for (int j = 0; j < num2; j++)
			{
				SoaringCommunityEvent.Reward reward = soaringCommunityEvent.CommunityRewards[j];
				if (!reward.m_bAcquired && reward.m_bUnlocked)
				{
					AquireEventGift(communityEvent, reward);
				}
			}
			num2 = soaringCommunityEvent.IndividualRewards.Length;
			for (int k = 0; k < num2; k++)
			{
				SoaringCommunityEvent.Reward reward = soaringCommunityEvent.IndividualRewards[k];
				if (!reward.m_bAcquired && reward.m_bUnlocked)
				{
					AquireEventGift(communityEvent, reward);
				}
			}
		}
	}

	private void CheckEventBannerPing()
	{
		if (SBSettings.CommunityEventBannerPing <= 0f || Time.time - m_fPreviousBannerPingTime <= SBSettings.CommunityEventBannerPing)
		{
			return;
		}
		CommunityEvent activeEvent = GetActiveEvent();
		if (activeEvent != null)
		{
			if ((activeEvent.m_sID == _sSpongyGamesEventID || activeEvent.m_sID == _sSpongyGamesLastDayEventID) && m_pSession.TheGame.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID) && (m_pSession.TheGame.simulation.FindSimulated(_nColiseumDID) != null || m_pSession.TheGame.inventory.HasItem(_nColiseumDID)))
			{
				Soaring.FireEvent("spongy_games_banner", null);
			}
			m_fPreviousBannerPingTime = Time.time;
		}
	}

	private void UpdateValuesToSoaring()
	{
		int num = m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)m_pCommunityEventDefinitions.objectAtIndex(i);
			if (communityEvent.m_bActive)
			{
				UpdateValueToSoaring(communityEvent);
			}
		}
	}

	private void UpdateValueToSoaring(CommunityEvent pEvent)
	{
		if (pEvent != null)
		{
			SBMISoaring.SetEventValue(m_pSession, int.Parse(pEvent.m_sID), m_pSession.TheGame.resourceManager.Resources[pEvent.m_nValueID].Amount);
			m_fPreviousValueUpdateTime = Time.time;
		}
	}

	private void AquireEventGift(CommunityEvent pEvent, SoaringCommunityEvent.Reward pSoaringReward)
	{
		SBMISoaring.AquireEventGift(m_pSession, int.Parse(pEvent.m_sID), pSoaringReward.m_nID, 0);
		m_fPreviousRewardUpdateTime = Time.time;
	}

	private void LoadCommunityEventsFromSpreadsheets()
	{
		m_pCommunityEventDefinitions = new MDictionary();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "CommunityEvents";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		Dictionary<int, Dictionary<string, object>> dictionary = new Dictionary<int, Dictionary<string, object>>();
		List<int> list = new List<int>();
		string text = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
			if (dictionary.ContainsKey(intCell) || list.Contains(intCell))
			{
				TFUtils.ErrorLog("Community Event Collision! DID: " + intCell);
				continue;
			}
			if (instance.GetIntCell(sheetIndex, rowIndex, "enabled") != 1)
			{
				list.Add(intCell);
				continue;
			}
			dictionary2.Add("did", intCell);
			dictionary2.Add("value_did", instance.GetIntCell(sheetIndex, rowIndex, "currency type"));
			dictionary2.Add("lifetime_contribute_cap", instance.GetIntCell(sheetIndex, rowIndex, "lifetime contribution cap"));
			dictionary2.Add("prerequisite_quest", instance.GetIntCell(sheetIndex, rowIndex, "prerequisite quest"));
			dictionary2.Add("hide_ui", instance.GetIntCell(sheetIndex, rowIndex, "hide ui") == 1);
			dictionary2.Add("name", instance.GetStringCell(sheetName, rowName, "name"));
			dictionary2.Add("start_date", instance.GetStringCell(sheetName, rowName, "start date"));
			dictionary2.Add("end_date", instance.GetStringCell(sheetName, rowName, "end date"));
			dictionary2.Add("event_button_texture", instance.GetStringCell(sheetName, rowName, "event button texture"));
			dictionary2.Add("tab_one_texture", instance.GetStringCell(sheetName, rowName, "tab texture 1"));
			dictionary2.Add("tab_two_texture", instance.GetStringCell(sheetName, rowName, "tab texture 2"));
			dictionary2.Add("left_banner_texture", instance.GetStringCell(sheetName, rowName, "header texture"));
			dictionary2.Add("right_banner_texture", instance.GetStringCell(sheetName, rowName, "title backing texture"));
			dictionary2.Add("right_banner_title", instance.GetStringCell(sheetName, rowName, "title"));
			dictionary2.Add("right_banner_description", instance.GetStringCell(sheetName, rowName, "individual tab header"));
			dictionary2.Add("individual_footer_text", instance.GetStringCell(sheetName, rowName, "individual tab footer"));
			dictionary2.Add("community_header_text", instance.GetStringCell(sheetName, rowName, "community tab header"));
			dictionary2.Add("community_footer_text", instance.GetStringCell(sheetName, rowName, "community tab footer"));
			dictionary2.Add("community_footer_all_unlocks_text", instance.GetStringCell(sheetName, rowName, "community tab footer all unlocks"));
			dictionary2.Add("community_footer_texture", instance.GetStringCell(sheetName, rowName, "community tab footer texture"));
			dictionary2.Add("quest_icon", instance.GetStringCell(sheetName, rowName, "quest list event reminder icon"));
			dictionary.Add(intCell, dictionary2);
		}
		sheetName = "CommunityEventItems";
		sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		int num2 = -1;
		for (int j = 0; j < num; j++)
		{
			string rowName = j.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
			if (num2 == -1)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "max unlocked land slots");
			}
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "event did");
			if (!dictionary.ContainsKey(intCell))
			{
				if (!list.Contains(intCell))
				{
					TFUtils.ErrorLog("Community Event can not be found for event item! Event DID: " + intCell);
				}
				continue;
			}
			Dictionary<string, object> dictionary2 = dictionary[intCell];
			List<object> list2 = ((!dictionary2.ContainsKey("rewards")) ? new List<object>() : ((List<object>)dictionary2["rewards"]));
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			dictionary3.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			dictionary3.Add("threshold", instance.GetIntCell(sheetIndex, rowIndex, "threshold"));
			dictionary3.Add("width", instance.GetIntCell(sheetIndex, rowIndex, "width"));
			dictionary3.Add("height", instance.GetIntCell(sheetIndex, rowIndex, "height"));
			dictionary3.Add("dialog_sequence", instance.GetIntCell(sheetIndex, rowIndex, "dialog sequence"));
			dictionary3.Add("auto_place_y", instance.GetIntCell(sheetIndex, rowIndex, "auto place y"));
			dictionary3.Add("auto_place_x", instance.GetIntCell(sheetIndex, rowIndex, "auto place x"));
			dictionary3.Add("hide_name_when_locked", instance.GetIntCell(sheetIndex, rowIndex, "hide name when locked") == 1);
			dictionary3.Add("item_name", instance.GetStringCell(sheetName, rowName, "item name"));
			dictionary3.Add("type", instance.GetStringCell(sheetName, rowName, "type"));
			dictionary3.Add("entity_type", instance.GetStringCell(sheetName, rowName, "reward type"));
			dictionary3.Add("texture", instance.GetStringCell(sheetName, rowName, "texture"));
			string stringCell = instance.GetStringCell(sheetName, rowName, "Locked Texture");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary3.Add("locked_texture", stringCell);
			}
			else
			{
				dictionary3.Add("locked_texture", null);
			}
			List<int> list3 = new List<int>();
			for (int k = 1; k <= num2; k++)
			{
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "unlocked land did " + k);
				if (intCell2 >= 0)
				{
					list3.Add(intCell2);
				}
			}
			dictionary3.Add("land_dids", list3);
			list2.Add(dictionary3);
			dictionary2["rewards"] = list2;
			dictionary[intCell] = dictionary2;
		}
		foreach (KeyValuePair<int, Dictionary<string, object>> item in dictionary)
		{
			CommunityEvent communityEvent = new CommunityEvent(item.Value);
			m_pCommunityEventDefinitions.addValue(communityEvent, communityEvent.m_sID);
		}
	}
}
