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

	private short mAdDisplays;

	private short mTimesShown;

	private short mTimesClicked;

	private SoaringDictionary mUserData;

	private SoaringDictionary mLocalizations;

	private SoaringAdType mAdType;

	public Texture2D AdTexture
	{
		get
		{
			return null;
		}
	}

	public string AdID
	{
		get
		{
			return null;
		}
	}

	public string Path
	{
		get
		{
			return null;
		}
	}

	public long AdExpires
	{
		get
		{
			return 0L;
		}
	}

	public long AdStarts
	{
		get
		{
			return 0L;
		}
	}

	public SoaringAdType AdType
	{
		get
		{
			return default(SoaringAdType);
		}
	}

	public short TimesWillBeDisplayed
	{
		get
		{
			return 0;
		}
	}

	public short TimesDisplayed
	{
		get
		{
			return 0;
		}
	}

	public short TimesClicked
	{
		get
		{
			return 0;
		}
	}

	public SoaringDictionary UserData
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary AdLocalizations
	{
		get
		{
			return null;
		}
	}

	public SoaringAdData()
		: base(default(IsType))
	{
	}

	internal void SetData(Texture2D texture, string addID, string path, long starts, long expires, int mAdDisplays, SoaringAdType adType, SoaringDictionary localizations)
	{
	}

	internal void SetUserData(SoaringDictionary userData)
	{
	}

	internal void SetCachedData(short shown, short clicks)
	{
	}

	internal void SetAdShown()
	{
	}

	public bool OpenAdPage()
	{
		return false;
	}

	public void Invalidate()
	{
	}
}
