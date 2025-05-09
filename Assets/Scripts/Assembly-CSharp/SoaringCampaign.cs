public class SoaringCampaign : SoaringDictionary
{
	public string CampaignId
	{
		get
		{
			return soaringValue("campaignId");
		}
	}

	public string Description
	{
		get
		{
			return soaringValue("description");
		}
	}

	public string Group
	{
		get
		{
			return soaringValue("group");
		}
	}

	public string Custom
	{
		get
		{
			return soaringValue("custom");
		}
	}

	public string CampaignType
	{
		get
		{
			return soaringValue("type");
		}
	}

	public SoaringCampaign(SoaringDictionary dictionary)
	{
		CopyExisting(dictionary);
	}
}
