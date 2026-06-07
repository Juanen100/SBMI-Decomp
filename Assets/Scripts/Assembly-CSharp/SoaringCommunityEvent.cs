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
		}

		public void SetData(SoaringDictionary pData)
		{
		}

		public void _SetAquired(bool bAquired)
		{
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
			return null;
		}
		private set
		{
		}
	}

	public Reward[] IndividualRewards
	{
		get
		{
			return null;
		}
		private set
		{
		}
	}

	public SoaringCommunityEvent(string sEventID, SoaringDictionary pData)
	{
	}

	public Reward GetReward(int nID)
	{
		return null;
	}

	public void SetData(string sEventID, SoaringDictionary pData)
	{
	}
}
