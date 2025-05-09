using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MTools;
using UnityEngine;

public class SoaringVersions : SoaringDelegate
{
	public class SoaringFileVersion : SoaringObjectBase
	{
		public string fileID;

		public string filePath;

		public string hash;

		public int localVersion;

		public SoaringFileVersion()
			: base(IsType.Object)
		{
		}

		public override string ToJsonString()
		{
			return "{\n\"n\":\"" + fileID + "\",\n\"d\":\"" + hash + "\",\n\"l\":" + localVersion + "\n}";
		}
	}

	private class SoaringPendingUpdates
	{
		public SoaringDictionary PendingFiles;

		public SoaringDictionary DownloadingFiles;

		public string Source;

		public string Commit;

		public long Version;

		public SoaringPendingUpdates()
		{
			PendingFiles = new SoaringDictionary();
			DownloadingFiles = new SoaringDictionary();
		}
	}

	public const int Error_UpToData = 33;

	public const int Error_VersionReset = 34;

	private const int kVersion = 2;

	private SoaringDictionary mFileDictionary;

	private SoaringPendingUpdates mPendingUpdate;

	private bool mIsRawSave = true;

	private string mServerURL;

	private string mServerRepoURL;

	private string mLocalFileRepo;

	private int mRetries;

	private bool mServerRandomAppend;

	public bool mVersionsFileExists = true;

	private string mVersionFileName;

	private string mVersioningSource;

	private string mVersioningCommit;

	private long mVersioningVersion;

	private bool mShouldUpdateFiles = true;

	private float mCurrentProgress;

	private int mInitialFileCount;

	public int MaxActiveConnections = 6;

	private SoaringArray mSubContentCategories;

	private string platformInitial;

	private static string[] IntToHexArr = new string[256];

	public bool VersionsFileExists
	{
		get
		{
			return mVersionsFileExists;
		}
	}

	public SoaringArray SubContentCategories
	{
		get
		{
			if (mSubContentCategories == null)
			{
				mSubContentCategories = new SoaringArray();
			}
			return mSubContentCategories;
		}
	}

	public SoaringVersions(string serverAddress)
	{
		mFileDictionary = new SoaringDictionary();
		mServerURL = serverAddress;
		mServerRepoURL = serverAddress;
		mVersionFileName = "Soaring/SoaringVR.ver";
		mLocalFileRepo = ResourceUtils.GetFilePath(string.Empty, "Soaring/Content");
		SoaringInternal.instance.RegisterModule(new SoaringVersionSoaringModule());
		LoadVersionData();
	}

	public float CurrentUpdateProgress()
	{
		return mCurrentProgress;
	}

	public void SetVersionServer(string versioning, string webrepo, string filerepo)
	{
		SetVersionServer(versioning, webrepo, filerepo, null);
	}

	public void SetVersionServer(string versioning, string webrepo, string filerepo, string versionFileName)
	{
		if (!string.IsNullOrEmpty(versioning))
		{
			mServerURL = versioning;
			mServerRandomAppend = true;
		}
		if (!string.IsNullOrEmpty(webrepo))
		{
			mServerRepoURL = webrepo;
		}
		if (!string.IsNullOrEmpty(filerepo))
		{
			mLocalFileRepo = filerepo;
			char c = mLocalFileRepo[mLocalFileRepo.Length - 1];
			if (c != '/' && c != '\\')
			{
				mLocalFileRepo += "/";
			}
		}
		if (!string.IsNullOrEmpty(versionFileName) && mVersionFileName != versionFileName)
		{
			mVersionFileName = versionFileName;
			LoadVersionData();
		}
	}

	public string GetFilePath(string fileID)
	{
		string result = null;
		if (string.IsNullOrEmpty(fileID))
		{
			return result;
		}
		SoaringFileVersion soaringFileVersion = (SoaringFileVersion)mFileDictionary.objectWithKey(fileID);
		if (soaringFileVersion != null)
		{
			result = soaringFileVersion.filePath;
		}
		return result;
	}

	public string GetFileHash(string name)
	{
		if (string.IsNullOrEmpty(name) || mFileDictionary == null)
		{
			return null;
		}
		SoaringFileVersion soaringFileVersion = (SoaringFileVersion)mFileDictionary.objectWithKey(name);
		if (soaringFileVersion == null)
		{
			return null;
		}
		return soaringFileVersion.hash;
	}

	public SoaringFileVersion GetVersionInfo(string name)
	{
		if (string.IsNullOrEmpty(name) || mFileDictionary == null)
		{
			return null;
		}
		return (SoaringFileVersion)mFileDictionary.objectWithKey(name);
	}

	private bool LoadVersionData()
	{
		bool result = false;
		MBinaryReader fileStream = ResourceUtils.GetFileStream(mVersionFileName, null, null, 5);
		Debug.Log(mVersionFileName);
		if (fileStream == null)
		{
			mVersionsFileExists = false;
			return result;
		}
		if (!fileStream.IsOpen())
		{
			mVersionsFileExists = false;
			return result;
		}
		try
		{
			if (mIsRawSave)
			{
				string json_data = Encoding.UTF8.GetString(fileStream.ReadAllBytes());
				SoaringDictionary soaringDictionary = new SoaringDictionary(json_data);
				mVersioningVersion = soaringDictionary.soaringValue("version");
				mVersioningCommit = soaringDictionary.soaringValue("commit");
				mVersioningSource = soaringDictionary.soaringValue("source");
				SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("contents");
				int num = soaringArray.count();
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringArray.objectAtIndex(i);
					SoaringFileVersion soaringFileVersion = new SoaringFileVersion();
					soaringFileVersion.filePath = soaringDictionary2.soaringValue("n");
					soaringFileVersion.fileID = soaringFileVersion.filePath;
					soaringFileVersion.hash = soaringDictionary2.soaringValue("d");
					soaringFileVersion.localVersion = soaringDictionary2.soaringValue("l");
					mFileDictionary.setValue(soaringFileVersion, soaringFileVersion.fileID);
				}
			}
			else
			{
				short num2 = fileStream.ReadShort();
				if (num2 != 2)
				{
					SoaringDebug.Log("SoaringVersions: Invalid Version ID: " + num2, LogType.Warning);
				}
				else
				{
					mVersioningSource = fileStream.ReadCharArrayAsString();
					mVersioningCommit = fileStream.ReadCharArrayAsString();
					mVersioningVersion = fileStream.ReadLong();
					int num3 = fileStream.ReadInt();
					for (int j = 0; j < num3; j++)
					{
						SoaringFileVersion soaringFileVersion2 = new SoaringFileVersion();
						soaringFileVersion2.filePath = fileStream.ReadCharArrayAsString();
						soaringFileVersion2.fileID = fileStream.ReadCharArrayAsString();
						soaringFileVersion2.hash = fileStream.ReadCharArrayAsString();
						mFileDictionary.setValue(soaringFileVersion2, soaringFileVersion2.fileID);
					}
				}
			}
		}
		catch (Exception ex)
		{
			mVersionsFileExists = false;
			SoaringDebug.Log(ex.Message, LogType.Error);
			ResetVersionDownloads();
		}
		fileStream.Close();
		fileStream = null;
		return result;
	}

	private bool SaveSessionData()
	{
		bool result = false;
		string writePath = ResourceUtils.GetWritePath(mVersionFileName, string.Empty, 1);
		if (string.IsNullOrEmpty(writePath))
		{
			return result;
		}
		MBinaryWriter mBinaryWriter = new MBinaryWriter();
		if (!mBinaryWriter.Open(writePath, true, true))
		{
			return result;
		}
		if (!mBinaryWriter.IsOpen())
		{
			return result;
		}
		try
		{
			if (mIsRawSave)
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary(4);
				soaringDictionary.addValue(mVersioningVersion, "version");
				soaringDictionary.addValue(mVersioningCommit, "commit");
				soaringDictionary.addValue(mVersioningSource, "source");
				SoaringObjectBase[] array = mFileDictionary.allValues();
				SoaringArray soaringArray = new SoaringArray(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						soaringArray.addObject(array[i]);
					}
				}
				soaringDictionary.addValue(soaringArray, "contents");
				mBinaryWriter.WriteRawString(soaringDictionary.ToJsonString());
				mBinaryWriter.Flush();
				result = true;
			}
			else
			{
				mBinaryWriter.Write((short)2);
				mBinaryWriter.WriteCharArrayAsString(mVersioningSource);
				mBinaryWriter.WriteCharArrayAsString(mVersioningCommit);
				mBinaryWriter.Write(mVersioningVersion);
				SoaringObjectBase[] array2 = mFileDictionary.allValues();
				int num = mFileDictionary.count();
				mBinaryWriter.Write(num);
				for (int j = 0; j < num; j++)
				{
					SoaringFileVersion soaringFileVersion = (SoaringFileVersion)array2[j];
					mBinaryWriter.WriteCharArrayAsString(soaringFileVersion.filePath);
					mBinaryWriter.WriteCharArrayAsString(soaringFileVersion.fileID);
					mBinaryWriter.WriteCharArrayAsString(soaringFileVersion.hash);
				}
				mBinaryWriter.Flush();
				result = true;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
		mBinaryWriter.Close();
		mBinaryWriter = null;
		return result;
	}

	private string PostAppendUrlString()
	{
		if (string.IsNullOrEmpty(platformInitial))
		{
			try
			{
				platformInitial = Application.platform.ToString();
				platformInitial = "?" + char.ToLower(platformInitial[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log(ex.Message, LogType.Error);
				platformInitial = "?";
			}
		}
		return platformInitial + UnityEngine.Random.Range(1000, int.MaxValue);
	}

	internal void CheckFilesForUpdates(bool updateFiles)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		mShouldUpdateFiles = updateFiles;
		string text = mServerURL;
		if (mServerRandomAppend)
		{
			text += PostAppendUrlString();
		}
		soaringDictionary.addValue(text, "turl");
		soaringDictionary.addValue(SoaringInternal.GameID, "gameId");
		if (!SoaringInternal.instance.CallModule("retrieveVersions", soaringDictionary, null))
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, "Invalid Call", null);
		}
	}

	public bool CheckValidFileData(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return false;
		}
		bool flag = false;
		try
		{
			SoaringFileVersion soaringFileVersion = (SoaringFileVersion)mFileDictionary.objectWithKey(id);
			return ValidateHash(soaringFileVersion.filePath, soaringFileVersion.hash);
		}
		catch
		{
			return false;
		}
	}

	internal void AddFileVersions(SoaringArray versions, SoaringArray diffs, long newVersion, string source, string commit)
	{
		if (versions == null)
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, new SoaringError("No Version Data Found", -1), null);
			return;
		}
		if (newVersion == mVersioningVersion && mVersioningCommit == commit)
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, new SoaringError("Version Up To Date", 33), null);
			return;
		}
		if (!mShouldUpdateFiles)
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Update, null, null);
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Success, null, null);
			return;
		}
		if (source != mVersioningSource)
		{
			//ClearAllContent();
		}
		mPendingUpdate = new SoaringPendingUpdates();
		mPendingUpdate.Commit = commit;
		mPendingUpdate.Version = newVersion;
		mPendingUpdate.Source = source;
		SoaringArray soaringArray = null;
		int num = versions.count();
		if (diffs != null)
		{
			soaringArray = new SoaringArray(num);
			for (int i = 0; i < num; i++)
			{
				SoaringFileVersion soaringFileVersion = new SoaringFileVersion();
				SoaringDictionary soaringDictionary = (SoaringDictionary)versions.objectAtIndex(i);
				soaringFileVersion.hash = soaringDictionary.soaringValue("d");
				soaringFileVersion.filePath = soaringDictionary.soaringValue("n");
				soaringFileVersion.fileID = soaringFileVersion.filePath;
				soaringArray.addObject(soaringFileVersion);
			}
			int num2 = diffs.count();
			for (int j = 0; j < num2; j++)
			{
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)diffs.objectAtIndex(j);
				string text = soaringDictionary2.soaringValue("d");
				string text2 = soaringDictionary2.soaringValue("n");
				bool flag = false;
				for (int k = 0; k < num; k++)
				{
					SoaringFileVersion soaringFileVersion2 = (SoaringFileVersion)soaringArray.objectAtIndex(k);
					if (soaringFileVersion2.fileID == text2)
					{
						if (soaringFileVersion2.hash != text)
						{
							soaringFileVersion2.hash = text;
							SoaringDebug.Log("Diff File: " + soaringFileVersion2.fileID);
						}
						flag = true;
					}
				}
				if (!flag)
				{
					SoaringFileVersion soaringFileVersion3 = new SoaringFileVersion();
					soaringFileVersion3.hash = text;
					soaringFileVersion3.filePath = text2;
					soaringFileVersion3.fileID = soaringFileVersion3.filePath;
					soaringArray.addObject(soaringFileVersion3);
					SoaringDebug.Log("Diff File Added: " + soaringFileVersion3.fileID);
				}
			}
			num = soaringArray.count();
		}
		for (int l = 0; l < num; l++)
		{
			SoaringFileVersion soaringFileVersion4 = null;
			if (soaringArray == null)
			{
				soaringFileVersion4 = new SoaringFileVersion();
				SoaringDictionary soaringDictionary3 = (SoaringDictionary)versions.objectAtIndex(l);
				soaringFileVersion4.hash = soaringDictionary3.soaringValue("d");
				soaringFileVersion4.filePath = soaringDictionary3.soaringValue("n");
				soaringFileVersion4.fileID = soaringFileVersion4.filePath;
			}
			else
			{
				soaringFileVersion4 = (SoaringFileVersion)soaringArray.objectAtIndex(l);
			}
			SoaringFileVersion soaringFileVersion5 = (SoaringFileVersion)mFileDictionary.objectWithKey(soaringFileVersion4.fileID);
			if (soaringFileVersion5 == null)
			{
				mPendingUpdate.PendingFiles.addValue(soaringFileVersion4, soaringFileVersion4.fileID);
			}
			else if (soaringFileVersion4.hash != soaringFileVersion5.hash)
			{
				mPendingUpdate.PendingFiles.addValue(soaringFileVersion4, soaringFileVersion4.fileID);
			}
		}
		mInitialFileCount = mPendingUpdate.PendingFiles.count();
		SoaringDebug.Log("Soaring Updating: " + mInitialFileCount + " files", LogType.Log);
		if (!NextDownload())
		{
			HandleSuccess();
		}
		else
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Update, null, null);
		}
	}

	private bool NextDownload()
	{
		if (mPendingUpdate.PendingFiles.count() == 0 && mPendingUpdate.DownloadingFiles.count() == 0)
		{
			return false;
		}
		while (mPendingUpdate.PendingFiles.count() != 0 && mPendingUpdate.DownloadingFiles.count() < MaxActiveConnections)
		{
			SoaringFileVersion soaringFileVersion = (SoaringFileVersion)mPendingUpdate.PendingFiles.objectAtIndex(0);
			mPendingUpdate.PendingFiles.removeObjectAtIndex(0);
			SoaringDebug.Log(mLocalFileRepo + soaringFileVersion.filePath, LogType.Log);
			string text = mServerRepoURL + soaringFileVersion.hash;
			if (mServerRandomAppend)
			{
				text += PostAppendUrlString();
			}
			mPendingUpdate.DownloadingFiles.addValue(soaringFileVersion, soaringFileVersion.fileID);
			SoaringInternal.instance.DownloadFileWithSoaring(soaringFileVersion.fileID, text, mLocalFileRepo + soaringFileVersion.filePath, SCDownloadCallback, this);
		}
		return true;
	}

	private void SCDownloadCallback(string id, bool success, string path)
	{
		SoaringFileVersion soaringFileVersion = null;
		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(path))
		{
			success = false;
		}
		else
		{
			if (mPendingUpdate == null)
			{
				SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, new SoaringError("Failed to download file " + id, 34), null);
				return;
			}
			if (mPendingUpdate.PendingFiles == null || mPendingUpdate.DownloadingFiles == null)
			{
				SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, "Failed to download file " + id, null);
				return;
			}
			soaringFileVersion = (SoaringFileVersion)mPendingUpdate.DownloadingFiles.objectWithKey(id);
			mPendingUpdate.DownloadingFiles.removeObjectWithKey(id);
			soaringFileVersion.localVersion++;
			if (success && soaringFileVersion != null)
			{
				if (!string.IsNullOrEmpty(soaringFileVersion.hash))
				{
					success = ValidateHash(path, soaringFileVersion.hash);
				}
			}
			else
			{
				success = false;
			}
		}
		if (success)
		{
			SoaringObjectBase soaringObjectBase = mPendingUpdate.PendingFiles.objectWithKey(id);
			mPendingUpdate.PendingFiles.removeObjectWithKey(id);
			if (soaringObjectBase != null)
			{
				mFileDictionary.setValue(soaringObjectBase, id);
			}
		}
		else
		{
			if (mRetries >= 3)
			{
				mRetries = 0;
				mPendingUpdate.PendingFiles.clear();
				mPendingUpdate.DownloadingFiles.clear();
				SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, "Failed to download file " + id, null);
				return;
			}
			mRetries++;
			if (soaringFileVersion != null)
			{
				mPendingUpdate.PendingFiles.addValue(soaringFileVersion, soaringFileVersion.fileID);
			}
		}
		if (mPendingUpdate.PendingFiles.count() == 0 && mPendingUpdate.DownloadingFiles.count() == 0)
		{
			HandleSuccess();
			return;
		}
		SaveSessionData();
		NextDownload();
	}

	public void HandleSuccess()
	{
		if (mPendingUpdate != null)
		{
			mPendingUpdate.PendingFiles.clear();
			mPendingUpdate.DownloadingFiles.clear();
			mVersioningVersion = mPendingUpdate.Version;
			mVersioningCommit = mPendingUpdate.Commit;
			mVersioningSource = mPendingUpdate.Source;
		}
		SaveSessionData();
		SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Success, null, null);
	}

	public void ResetVersionDownloads()
	{
		if (mFileDictionary != null)
		{
			mFileDictionary.clear();
		}
		if (mPendingUpdate != null)
		{
			mPendingUpdate.PendingFiles.clear();
			mPendingUpdate.DownloadingFiles.clear();
		}
		mPendingUpdate = null;
	}

	public void ClearAllContent()
	{
		try
		{
			ResetVersionDownloads();
			if (Directory.Exists(mLocalFileRepo))
			{
				Directory.Delete(mLocalFileRepo, true);
			}
			string filePath = ResourceUtils.GetFilePath(mVersionFileName);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			else
			{
				SoaringDebug.Log("Invalid Manifiest: " + filePath, LogType.Error);
			}
			LoadVersionData();
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
	}

	public void RemoveVersionFile(string fileID)
	{
		if (string.IsNullOrEmpty(fileID))
		{
			return;
		}
		SoaringFileVersion soaringFileVersion = (SoaringFileVersion)mFileDictionary.objectWithKey(fileID);
		if (soaringFileVersion == null)
		{
			string extension = Path.GetExtension(fileID);
			if (!string.IsNullOrEmpty(extension))
			{
				int num = mFileDictionary.count();
				for (int i = 0; i < num; i++)
				{
					soaringFileVersion = (SoaringFileVersion)mFileDictionary.objectAtIndex(i);
					if (soaringFileVersion.filePath.Contains(fileID))
					{
						mFileDictionary.removeObjectWithKey(mFileDictionary.allKeys()[i]);
						break;
					}
					soaringFileVersion = null;
				}
			}
			if (soaringFileVersion == null)
			{
				return;
			}
		}
		else
		{
			mFileDictionary.removeObjectWithKey(fileID);
		}
		SaveSessionData();
	}

	public override void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
		if (state == SoaringState.Update && error == null && mPendingUpdate != null && mPendingUpdate.PendingFiles != null)
		{
			float num = 0f;
			if (mInitialFileCount == 0)
			{
				num = 1f;
			}
			num = (float)(mInitialFileCount - (mPendingUpdate.PendingFiles.count() + mPendingUpdate.DownloadingFiles.count())) / (float)mInitialFileCount;
			mCurrentProgress = num + (float)data / 1f / (float)mInitialFileCount;
			SoaringInternal.Delegate.OnFileVersionsUpdated(state, error, mCurrentProgress);
		}
	}

	public bool ValidateHash(string filePath, string hash)
	{
		bool result = false;
		if (!File.Exists(filePath))
		{
			return result;
		}
		try
		{
			MBinaryReader mBinaryReader = new MBinaryReader(filePath);
			if (mBinaryReader == null)
			{
				return result;
			}
			if (!mBinaryReader.IsOpen())
			{
				return result;
			}
			byte[] allBytes = mBinaryReader.ReadAllBytes();
			if (CheckAndCalculateMD5Hash(allBytes, hash))
			{
				result = true;
			}
			mBinaryReader.Close();
			mBinaryReader = null;
		}
		catch
		{
			SoaringDebug.Log("ValidateHash: Failed", LogType.Error);
		}
		return result;
	}

	public static string CalculateMD5Hash(byte[] allBytes)
	{
		if (allBytes == null)
		{
			return null;
		}
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(allBytes);
		StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num = array[i];
			if (IntToHexArr[num] == null)
			{
				string text = num.ToString("x2");
				IntToHexArr[num] = text;
			}
			stringBuilder.Append(IntToHexArr[num]);
		}
		return stringBuilder.ToString();
	}

	public static bool CheckAndCalculateMD5Hash(byte[] allBytes, string file_hash)
	{
		if (allBytes == null)
		{
			return false;
		}
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(allBytes);
		int num = array.Length << 1;
		if (file_hash.Length != num)
		{
			SoaringDebug.Log("CheckAndCalculateMD5Hash: FAILED Size: " + file_hash.Length + " : " + num, LogType.Error);
			return false;
		}
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num2 = array[i];
			if (IntToHexArr[num2] == null)
			{
				string text = num2.ToString("x2");
				IntToHexArr[num2] = text;
			}
			if (IntToHexArr[num2][0] != file_hash[num3] || IntToHexArr[num2][1] != file_hash[num3 + 1])
			{
				SoaringDebug.Log("CheckAndCalculateMD5Hash: FAILED Data: " + file_hash + " : " + file_hash[num3] + file_hash[num3 + 1] + " : " + IntToHexArr[num2].ToString(), LogType.Error);
				return false;
			}
			num3 += 2;
		}
		return true;
	}

	private void CreateHexTable()
	{
		IntToHexArr = new string[256];
	}

	private void DestroyHexTable()
	{
		IntToHexArr = null;
	}
}
