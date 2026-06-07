public class SoaringAdServer : SoaringObjectBase
{
	private const int kFormatVersionNumber = 0;

	private const string kAnyAdvert = "kS_ANY";

	private const string kAdvertDisplay = "display_advert";

	private const string kAdvertName = "advert_name";

	private const string kAdvertData = "advert_data";

	private string mAdServer;

	private string mAdFilePath;

	private SoaringArray mSoaringAdDataReferences;

	private SoaringDictionary mActiveAdRequests;

	public SoaringAdServer()
		: base(default(IsType))
	{
	}

	public void RequestAd(string adName, bool displayAdOnComplete, SoaringContext context)
	{
	}

	public bool AdAvailable(string adName)
	{
		return false;
	}

	public bool DisplayAd(string adName)
	{
		return false;
	}

	public void SetAdServerURL(string url)
	{
	}

	private void CleanupAds()
	{
	}

	private SoaringAdData CheckAdExists(string adID)
	{
		return null;
	}

	private void SetAdReference(SoaringAdData data)
	{
	}

	public void MarkAdAsShown(SoaringAdData data)
	{
	}

	internal void HandleAdRequestReturn(SoaringDictionary returnData, SoaringContext context)
	{
	}

	private void AdCallback(string id, bool success, string path)
	{
	}

	private void HandleAdDownload(SoaringAdData adData, SoaringContext context)
	{
	}

	private void LoadAdReferences()
	{
	}

	private void SaveAdReferences()
	{
	}
}
