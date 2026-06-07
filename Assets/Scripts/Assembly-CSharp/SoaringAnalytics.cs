using System.IO;

public class SoaringAnalytics
{
	private class Buffer
	{
		private SoaringArray mData;

		private FileStream mFilestream;

		private StreamWriter mStreamWriter;

		public SoaringArray GetData()
		{
			return null;
		}

		public void Open(string filepath)
		{
		}

		public void Append(SoaringArray mAppendData)
		{
		}

		public void Clear()
		{
		}

		public void Close()
		{
		}
	}

	private class SoaringAnalyticsDelegate : SoaringDelegate
	{
		private SoaringAnalytics mAnalytics;

		public SoaringAnalyticsDelegate(SoaringAnalytics analytics)
		{
		}

		public override void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
		{
		}
	}

	private class BufferContainer
	{
		public Buffer mBuffer;

		public Buffer mBufferTemp;

		public bool mWaitingForResponse;

		public float mUpdateTime;

		public float mUpdateInterval;
	}

	public enum EmbededGUIDType
	{
		None = 0,
		AllEntries = 1,
		AllValues = 2
	}

	private static bool _bERROR_LOG;

	private const int kStandardLog = 0;

	private const int kAnonymousLog = 1;

	private static string mDeviceGUID;

	private static ulong mGUIDSequenceID;

	private static SoaringDictionary sMetaData;

	public bool mInitialized;

	private BufferContainer[] mBuffersData;

	private static uint mRandVal;

	private static uint mRandSeed;

	private EmbededGUIDType mEmbededGUIDType;

	public static ulong DeviceSequenceID
	{
		get
		{
			return 0uL;
		}
	}

	public static string DeviceGUID
	{
		get
		{
			return null;
		}
	}

	public static SoaringDictionary StampDeviceMetadata()
	{
		return null;
	}

	private static void LoadSoaringAnalytics()
	{
	}

	public void Initialize()
	{
	}

	private static void SaveSoaringAnalyticFile()
	{
	}

	public void Shutdown()
	{
	}

	public void LogAnonymousEvent(string key, SoaringObjectBase value)
	{
	}

	public void LogAnonymousEvents(SoaringArray entries)
	{
	}

	public void LogEvent(string key, SoaringObjectBase value, int logIndex = 0)
	{
	}

	public void LogEvents(SoaringArray entries, int logIndex = 0)
	{
	}

	public void Update(float deltaTime)
	{
	}

	public void _OnSaveStat(bool success, int nLogIndex, SoaringError error, SoaringContext context)
	{
	}

	public static ulong AnalyticTime()
	{
		return 0uL;
	}

	public static string GenerateGUID()
	{
		return null;
	}

	public static uint Fast_Rand()
	{
		return 0u;
	}
}
