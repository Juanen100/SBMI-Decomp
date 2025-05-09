public class SoaringCommunityEvent
{
	public class Reward
	{
		public int m_nID { get; private set; }

		public int m_nValue { get; private set; }

		public bool m_bUnlocked { get; private set; }

		public bool m_bAcquired { get; private set; }

		public Reward(SoaringDictionary pData)
		{
			SetData(pData);
		}

		public void SetData(SoaringDictionary pData)
		{
			m_nID = int.Parse(pData.soaringValue("giftDid"));
			m_nValue = pData.soaringValue("valueNeeded");
			m_bUnlocked = pData.soaringValue("unlocked");
			m_bAcquired = pData.soaringValue("acquired");
		}

		public void _SetAquired(bool bAquired)
		{
			m_bAcquired = bAquired;
		}
	}

	private Reward[] m_pCommunityRewards;

	private Reward[] m_pIndividualRewards;

	public string m_sID { get; private set; }

	public int m_nValue { get; private set; }

	public int m_nCommunityValue { get; private set; }

	public Reward[] CommunityRewards
	{
		get
		{
			return (Reward[])m_pCommunityRewards.Clone();
		}
		private set
		{
			m_pCommunityRewards = value;
		}
	}

	public Reward[] IndividualRewards
	{
		get
		{
			return (Reward[])m_pIndividualRewards.Clone();
		}
		private set
		{
			m_pIndividualRewards = value;
		}
	}

	public SoaringCommunityEvent(string sEventID, SoaringDictionary pData)
	{
		m_pCommunityRewards = new Reward[0];
		m_pIndividualRewards = new Reward[0];
		SetData(sEventID, pData);
	}

	public Reward GetReward(int nID)
	{
		Reward[] pCommunityRewards = m_pCommunityRewards;
		int num = pCommunityRewards.Length;
		for (int i = 0; i < num; i++)
		{
			if (pCommunityRewards[i].m_nID == nID)
			{
				return pCommunityRewards[i];
			}
		}
		pCommunityRewards = m_pIndividualRewards;
		num = pCommunityRewards.Length;
		for (int j = 0; j < num; j++)
		{
			if (pCommunityRewards[j].m_nID == nID)
			{
				return pCommunityRewards[j];
			}
		}
		return null;
	}

	public void SetData(string sEventID, SoaringDictionary pData)
	{
		if (pData != null)
		{
			m_sID = sEventID;
			m_nValue = pData.soaringValue("value");
			m_nCommunityValue = pData.soaringValue("communityValue");
			SoaringArray soaringArray = (SoaringArray)pData.objectWithKey("communityGifts");
			int num = soaringArray.count();
			Reward[] array = new Reward[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new Reward((SoaringDictionary)soaringArray.objectAtIndex(i));
			}
			m_pCommunityRewards = array;
			soaringArray = (SoaringArray)pData.objectWithKey("individualGifts");
			num = soaringArray.count();
			array = new Reward[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = new Reward((SoaringDictionary)soaringArray.objectAtIndex(j));
			}
			m_pIndividualRewards = array;
		}
	}
}
