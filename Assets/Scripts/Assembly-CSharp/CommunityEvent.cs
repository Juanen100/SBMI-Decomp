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
				return null;
			}
			private set
			{
			}
		}

		public Reward(Dictionary<string, object> pData)
		{
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
	}

	public void SetActive(bool bActive)
	{
	}

	public Reward GetReward(int nID)
	{
		return null;
	}

	public Reward GetReward(string sID)
	{
		return null;
	}
}
