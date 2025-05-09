using UnityEngine;

public class SoaringAdData : SoaringObjectBase
{
	public enum SoaringAdType
	{
		Web = 0,
		Market = 1,
		Local = 2,
		Other = 3
	}

	private Texture2D mTexture;

	private string mAdID;

	private string mAdPath;

	private long mAdExpires;

	private long mAdStarts;

	private short mAdDisplays = -1;

	private short mTimesShown;

	private short mTimesClicked;

	private SoaringDictionary mUserData;

	private SoaringDictionary mLocalizations;

	private SoaringAdType mAdType = SoaringAdType.Local;

	public Texture2D AdTexture
	{
		get
		{
			return mTexture;
		}
	}

	public string AdID
	{
		get
		{
			return mAdID;
		}
	}

	public string Path
	{
		get
		{
			return mAdPath;
		}
	}

	public long AdExpires
	{
		get
		{
			return mAdExpires;
		}
	}

	public long AdStarts
	{
		get
		{
			return mAdStarts;
		}
	}

	public SoaringAdType AdType
	{
		get
		{
			return mAdType;
		}
	}

	public short TimesWillBeDisplayed
	{
		get
		{
			return mAdDisplays;
		}
	}

	public short TimesDisplayed
	{
		get
		{
			return mTimesShown;
		}
	}

	public short TimesClicked
	{
		get
		{
			return mTimesClicked;
		}
	}

	public SoaringDictionary UserData
	{
		get
		{
			return mUserData;
		}
	}

	public SoaringDictionary AdLocalizations
	{
		get
		{
			return mLocalizations;
		}
	}

	public SoaringAdData()
		: base(IsType.Object)
	{
	}

	internal void SetData(Texture2D texture, string addID, string path, long starts, long expires, int mAdDisplays, SoaringAdType adType, SoaringDictionary localizations)
	{
		mAdType = adType;
		mTexture = texture;
		mAdID = addID;
		mAdPath = path;
		mAdExpires = expires;
		mAdStarts = starts;
		mLocalizations = localizations;
	}

	internal void SetUserData(SoaringDictionary userData)
	{
		mUserData = userData;
	}

	internal void SetCachedData(short shown, short clicks)
	{
		mTimesClicked = clicks;
		mTimesShown = shown;
	}

	internal void SetAdShown()
	{
		mTimesShown++;
	}

	public bool OpenAdPage()
	{
		bool result = false;
		if ((mAdType == SoaringAdType.Market || mAdType == SoaringAdType.Web) && !string.IsNullOrEmpty(mAdPath))
		{
			Application.OpenURL(mAdPath);
			result = true;
		}
		mTimesClicked++;
		return result;
	}

	public void Invalidate()
	{
		if (mTexture != null)
		{
			Object.Destroy(mTexture);
		}
		mTexture = null;
	}
}
