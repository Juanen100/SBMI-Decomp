using System;
using System.Collections.Generic;
using MTools;

public class CommunityEvent
{
	public class Reward
	{
		private int[] m_pLandIDs;

		public int m_nID { get; private set; }

		public string m_sTexture { get; private set; }

		public int m_nWidth { get; private set; }

		public int m_nHeight { get; private set; }

		public int m_nDialogSequenceID { get; private set; }

		public int m_nAutoPlaceX { get; private set; }

		public int m_nAutoPlaceY { get; private set; }

		public string m_sType { get; private set; }

		public string m_sLockedTexture { get; private set; }

		public bool m_bHideNameWhenLocked { get; private set; }

		public int[] LandIDs
		{
			get
			{
				return (int[])m_pLandIDs.Clone();
			}
			private set
			{
				m_pLandIDs = value;
			}
		}

		public Reward(Dictionary<string, object> pData)
		{
			m_nID = TFUtils.LoadInt(pData, "did");
			m_nWidth = TFUtils.LoadInt(pData, "width");
			m_nHeight = TFUtils.LoadInt(pData, "height");
			m_nDialogSequenceID = TFUtils.LoadInt(pData, "dialog_sequence");
			m_nAutoPlaceX = TFUtils.LoadInt(pData, "auto_place_x");
			m_nAutoPlaceY = TFUtils.LoadInt(pData, "auto_place_y");
			m_sTexture = TFUtils.LoadString(pData, "texture");
			m_sType = TFUtils.LoadString(pData, "entity_type");
			m_sLockedTexture = TFUtils.LoadString(pData, "locked_texture");
			m_bHideNameWhenLocked = TFUtils.LoadBool(pData, "hide_name_when_locked");
			List<int> list = TFUtils.LoadList<int>(pData, "land_dids");
			int count = list.Count;
			m_pLandIDs = new int[count];
			for (int i = 0; i < count; i++)
			{
				m_pLandIDs[i] = list[i];
			}
		}
	}

	private MDictionary m_pRewards;

	public string m_sID { get; private set; }

	public string m_sName { get; private set; }

	public bool m_bActive { get; private set; }

	public bool m_bHideUI { get; private set; }

	public int m_nValueID { get; private set; }

	public int m_nQuestPrereqID { get; private set; }

	public DateTime m_pStartDate { get; private set; }

	public DateTime m_pEndDate { get; private set; }

	public string m_sEventButtonTexture { get; private set; }

	public string m_sTabOneTexture { get; private set; }

	public string m_sTabTwoTexture { get; private set; }

	public string m_sLeftBannerTexture { get; private set; }

	public string m_sRightBannerTexture { get; private set; }

	public string m_sRightBannerTitle { get; private set; }

	public string m_sRightBannerDescription { get; private set; }

	public string m_sIndividualFooterText { get; private set; }

	public string m_sCommunityHeaderText { get; private set; }

	public string m_sCommunityFooterText { get; private set; }

	public string m_sCommunityFooterAllUnlocksText { get; private set; }

	public string m_sCommunityFooterTexture { get; private set; }

	public string m_sQuestIcon { get; private set; }

	public CommunityEvent(Dictionary<string, object> pData)
	{
		m_sID = TFUtils.LoadUint(pData, "did").ToString();
		m_sName = TFUtils.LoadString(pData, "name");
		m_nValueID = TFUtils.LoadInt(pData, "value_did");
		m_nQuestPrereqID = TFUtils.LoadInt(pData, "prerequisite_quest");
		m_pStartDate = DateTime.Parse(TFUtils.LoadString(pData, "start_date"));
		m_pEndDate = DateTime.Parse(TFUtils.LoadString(pData, "end_date"));
		m_sEventButtonTexture = TFUtils.LoadString(pData, "event_button_texture");
		m_sTabOneTexture = TFUtils.LoadString(pData, "tab_one_texture");
		m_sTabTwoTexture = TFUtils.LoadString(pData, "tab_two_texture");
		m_sLeftBannerTexture = TFUtils.LoadString(pData, "left_banner_texture");
		m_sRightBannerTexture = TFUtils.LoadString(pData, "right_banner_texture");
		m_sRightBannerTitle = TFUtils.LoadString(pData, "right_banner_title");
		m_sRightBannerDescription = TFUtils.LoadString(pData, "right_banner_description");
		m_sIndividualFooterText = TFUtils.LoadString(pData, "individual_footer_text");
		m_sCommunityHeaderText = TFUtils.LoadString(pData, "community_header_text");
		m_sCommunityFooterText = TFUtils.LoadString(pData, "community_footer_text");
		m_sCommunityFooterAllUnlocksText = TFUtils.LoadString(pData, "community_footer_all_unlocks_text");
		m_sCommunityFooterTexture = TFUtils.LoadString(pData, "community_footer_texture");
		m_sQuestIcon = TFUtils.LoadString(pData, "quest_icon");
		m_bHideUI = TFUtils.LoadBool(pData, "hide_ui");
		m_bActive = false;
		m_pRewards = new MDictionary();
		if (!pData.ContainsKey("rewards"))
		{
			return;
		}
		List<object> list = TFUtils.LoadList<object>(pData, "rewards");
		foreach (Dictionary<string, object> item in list)
		{
			Reward reward = new Reward(item);
			m_pRewards.addValue(reward, reward.m_nID.ToString());
		}
	}

	public void SetActive(bool bActive)
	{
		m_bActive = bActive;
	}

	public Reward GetReward(int nID)
	{
		return GetReward(nID.ToString());
	}

	public Reward GetReward(string sID)
	{
		if (m_pRewards.containsKey(sID))
		{
			return (Reward)m_pRewards.objectWithKey(sID);
		}
		return null;
	}
}
