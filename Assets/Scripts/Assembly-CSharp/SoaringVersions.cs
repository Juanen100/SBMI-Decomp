public class SoaringVersions : SoaringDelegate
{
	public class SoaringFileVersion : SoaringObjectBase
	{
		public string fileID;

		public string filePath;

		public string hash;

		public int localVersion;

		public SoaringFileVersion()
			: base(default(IsType))
		{
		}

		public override string ToJsonString()
		{
			return null;
		}
	}

	private class SoaringPendingUpdates
	{
		public SoaringDictionary PendingFiles;

		public SoaringDictionary DownloadingFiles;

		public string Source;

		public string Commit;

		public long Version;
	}

	public const int Error_UpToData = 33;

	public const int Error_VersionReset = 34;

	private const int kVersion = 2;

	private SoaringDictionary mFileDictionary;

	private SoaringPendingUpdates mPendingUpdate;

	private bool mIsRawSave;

	private string mServerURL;

	private string mServerRepoURL;

	private string mLocalFileRepo;

	private int mRetries;

	private bool mServerRandomAppend;

	public bool mVersionsFileExists;

	private string mVersionFileName;

	private string mVersioningSource;

	private string mVersioningCommit;

	private long mVersioningVersion;

	private bool mShouldUpdateFiles;

	private float mCurrentProgress;

	private int mInitialFileCount;

	public int MaxActiveConnections;

	private SoaringArray mSubContentCategories;

	private string platformInitial;

	private static string[] IntToHexArr;

	public bool VersionsFileExists
	{
		get
		{
			return false;
		}
	}

	public SoaringArray SubContentCategories
	{
		get
		{
			return null;
		}
	}

	public SoaringVersions(string serverAddress)
	{
	}

	public float CurrentUpdateProgress()
	{
		return 0f;
	}

	public void SetVersionServer(string versioning, string webrepo, string filerepo)
	{
	}

	public void SetVersionServer(string versioning, string webrepo, string filerepo, string versionFileName)
	{
	}

	public string GetFilePath(string fileID)
	{
		return null;
	}

	public string GetFileHash(string name)
	{
		return null;
	}

	public SoaringFileVersion GetVersionInfo(string name)
	{
		return null;
	}

	private bool LoadVersionData()
	{
		return false;
	}

	private bool SaveSessionData()
	{
		return false;
	}

	private string PostAppendUrlString()
	{
		return null;
	}

	internal void CheckFilesForUpdates(bool updateFiles)
	{
	}

	public bool CheckValidFileData(string id)
	{
		return false;
	}

	internal void AddFileVersions(SoaringArray versions, SoaringArray diffs, long newVersion, string source, string commit)
	{
	}

	private bool NextDownload()
	{
		return false;
	}

	private void SCDownloadCallback(string id, bool success, string path)
	{
	}

	public void HandleSuccess()
	{
	}

	public void ResetVersionDownloads()
	{
	}

	public void ClearAllContent()
	{
	}

	public void RemoveVersionFile(string fileID)
	{
	}

	public override void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
	}

	public bool ValidateHash(string filePath, string hash)
	{
		return false;
	}

	public static string CalculateMD5Hash(byte[] allBytes)
	{
		return null;
	}

	public static bool CheckAndCalculateMD5Hash(byte[] allBytes, string file_hash)
	{
		return false;
	}

	private void CreateHexTable()
	{
	}

	private void DestroyHexTable()
	{
	}
}
