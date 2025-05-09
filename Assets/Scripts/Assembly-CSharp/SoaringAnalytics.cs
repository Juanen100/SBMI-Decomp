using System;
using System.IO;
using MTools;
using UnityEngine;

public class SoaringAnalytics
{
	private class Buffer
	{
		private SoaringArray mData;

		private FileStream mFilestream;

		private StreamWriter mStreamWriter;

		public Buffer()
		{
			mData = new SoaringArray();
			mFilestream = null;
			mStreamWriter = null;
		}

		public SoaringArray GetData()
		{
			return mData;
		}

		public void Open(string filepath)
		{
			mData = new SoaringArray();
			try
			{
				string directoryName = Path.GetDirectoryName(filepath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!File.Exists(filepath))
				{
					mFilestream = File.Create(filepath);
				}
				else
				{
					mFilestream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite);
				}
			}
			catch (Exception ex)
			{
				mFilestream = null;
				mStreamWriter = null;
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Open Failed: " + text + "\n" + ex.StackTrace);
			}
			if (mFilestream == null)
			{
				return;
			}
			try
			{
				if (mFilestream.Length > 0)
				{
					mFilestream.Seek(0L, SeekOrigin.Begin);
					StreamReader streamReader = new StreamReader(mFilestream);
					string text2 = streamReader.ReadToEnd();
					streamReader = null;
					string[] array = text2.Split('|');
					int num = array.Length;
					for (int i = 0; i < num; i++)
					{
						SoaringDictionary soaringDictionary = new SoaringDictionary(array[i]);
						SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("entries");
						int num2 = soaringArray.count();
						for (int j = 0; j < num2; j++)
						{
							mData.addObject(soaringArray.objectAtIndex(j));
						}
					}
					mFilestream.Seek(mFilestream.Length, SeekOrigin.Begin);
				}
				mStreamWriter = new StreamWriter(mFilestream);
			}
			catch (Exception ex2)
			{
				mFilestream = null;
				mStreamWriter = null;
				string text3 = ex2.Message;
				if (text3 == null)
				{
					text3 = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Read Failed: " + text3 + "\n" + ex2.StackTrace);
			}
		}

		public void Append(SoaringArray mAppendData)
		{
			if (mFilestream == null || mStreamWriter == null || mFilestream.Length >= SoaringInternalProperties.AnalyticsBufferSize)
			{
				return;
			}
			int num = mAppendData.count();
			if (num <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				mData.addObject(mAppendData.objectAtIndex(i));
			}
			try
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(mAppendData, "entries");
				string value = ((mFilestream.Length > 0) ? ('|' + soaringDictionary.ToJsonString()) : soaringDictionary.ToJsonString());
				mStreamWriter.Write(value);
				mStreamWriter.Flush();
				mFilestream.Flush();
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Append Failed: " + text + "\n" + ex.StackTrace);
			}
		}

		public void Clear()
		{
			if (mFilestream == null || mStreamWriter == null)
			{
				return;
			}
			mData.clear();
			try
			{
				mStreamWriter = null;
				mFilestream.SetLength(0L);
				mStreamWriter = new StreamWriter(mFilestream);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Clear Failed: " + text + "\n" + ex.StackTrace);
			}
		}

		public void Close()
		{
			if (mFilestream == null || mStreamWriter == null)
			{
				return;
			}
			try
			{
				mFilestream.Close();
				mFilestream = null;
				mStreamWriter = null;
			}
			catch (Exception ex)
			{
				mFilestream = null;
				mStreamWriter = null;
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Close Failed: " + text + "\n" + ex.StackTrace);
			}
		}
	}

	private class SoaringAnalyticsDelegate : SoaringDelegate
	{
		private SoaringAnalytics mAnalytics;

		public SoaringAnalyticsDelegate(SoaringAnalytics analytics)
		{
			mAnalytics = analytics;
		}

		public override void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
		{
			if (mAnalytics != null)
			{
				if (anonymous)
				{
					mAnalytics._OnSaveStat(success, 1, error, context);
				}
				else
				{
					mAnalytics._OnSaveStat(success, 0, error, context);
				}
			}
		}
	}

	private class BufferContainer
	{
		public Buffer mBuffer;

		public Buffer mBufferTemp;

		public bool mWaitingForResponse;

		public float mUpdateTime;

		public float mUpdateInterval = 10f;
	}

	public enum EmbededGUIDType
	{
		None = 0,
		AllEntries = 1,
		AllValues = 2
	}

	private const int kStandardLog = 0;

	private const int kAnonymousLog = 1;

	private static bool _bERROR_LOG = true;

	private static string mDeviceGUID;

	private static ulong mGUIDSequenceID;

	private static SoaringDictionary sMetaData;

	public bool mInitialized;

	private BufferContainer[] mBuffersData = new BufferContainer[2];

	private static uint mRandVal = 2147483647u;

	private static uint mRandSeed = 2147483647u;

	private EmbededGUIDType mEmbededGUIDType;

	public static ulong DeviceSequenceID
	{
		get
		{
			ulong result = mGUIDSequenceID;
			mGUIDSequenceID++;
			return result;
		}
	}

	public static string DeviceGUID
	{
		get
		{
			if (string.IsNullOrEmpty(mDeviceGUID))
			{
				LoadSoaringAnalytics();
				mDeviceGUID = GenerateGUID();
			}
			return mDeviceGUID;
		}
	}

	public static SoaringDictionary StampDeviceMetadata()
	{
		if (sMetaData == null)
		{
			sMetaData = new SoaringDictionary();
			sMetaData.addValue_unsafe(new SoaringValue(SoaringPlatform.PrimaryPlatformName), "platform");
			sMetaData.addValue_unsafe(new SoaringValue(SystemInfo.operatingSystem), "os_version");
			sMetaData.addValue_unsafe(new SoaringValue(SystemInfo.deviceModel), "device_name");
		}
		SoaringDictionary soaringDictionary = sMetaData.makeCopy();
		soaringDictionary.addValue_unsafe(new SoaringValue(GenerateGUID()), "guid");
		soaringDictionary.addValue_unsafe(new SoaringValue(DeviceSequenceID), "device_seq_num");
		soaringDictionary.addValue_unsafe(new SoaringValue(DeviceGUID), "device_seq_id");
		soaringDictionary.addValue_unsafe(new SoaringValue(AnalyticTime()), "event_time");
		return soaringDictionary;
	}

	private static void LoadSoaringAnalytics()
	{
		if (!string.IsNullOrEmpty(mDeviceGUID))
		{
			return;
		}
		MBinaryReader mBinaryReader = null;
		try
		{
			mBinaryReader = ResourceUtils.GetFileStream("SoaringAnalytic", "Soaring", "dat", 1);
			if (mBinaryReader != null && mBinaryReader.IsOpen())
			{
				if (mBinaryReader.ReadInt() == 1)
				{
					mDeviceGUID = mBinaryReader.ReadString();
					mGUIDSequenceID = mBinaryReader.ReadULong();
				}
				mBinaryReader.Close();
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Failed to Load SoaringAnalytics.dat: " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
			try
			{
				if (mBinaryReader != null)
				{
					mBinaryReader.Close();
				}
			}
			catch
			{
			}
		}
		mBinaryReader = null;
		try
		{
			if (string.IsNullOrEmpty(mDeviceGUID))
			{
				mDeviceGUID = GenerateGUID();
				mGUIDSequenceID = 0uL;
				SaveSoaringAnalyticFile();
			}
		}
		catch (Exception ex2)
		{
			SoaringDebug.Log("Failed to Verify SoaringAnalytics.dat: " + ex2.Message + "\n" + ex2.StackTrace, LogType.Error);
		}
	}

	public void Initialize()
	{
		if (mInitialized || mBuffersData == null)
		{
			return;
		}
		string[] array = new string[2] { "Analytics", "Anonymous" };
		Debug.Log("Soaring Initialize Analytics");
		mRandSeed = (uint)AnalyticTime();
		LoadSoaringAnalytics();
		for (int i = 0; i < mBuffersData.Length; i++)
		{
			mBuffersData[i] = new BufferContainer();
			mBuffersData[i].mBuffer = new Buffer();
			mBuffersData[i].mBufferTemp = new Buffer();
			try
			{
				mBuffersData[i].mBuffer.Open(ResourceUtils.GetWritePath("Soaring" + array[i] + "Buffer.json", "Soaring", 1));
				mBuffersData[i].mBufferTemp.Open(ResourceUtils.GetWritePath("Soaring" + array[i] + "BufferTemp.json", "Soaring", 1));
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Initialization Failed: " + text + "\n" + ex.StackTrace);
			}
		}
		mInitialized = true;
	}

	private static void SaveSoaringAnalyticFile()
	{
		if (string.IsNullOrEmpty(mDeviceGUID))
		{
			return;
		}
		MBinaryWriter mBinaryWriter = null;
		try
		{
			string writePath = ResourceUtils.GetWritePath("SoaringAnalytic.dat", "Soaring", 1);
			mBinaryWriter = new MBinaryWriter();
			if (mBinaryWriter.Open(writePath, true, true, "bak"))
			{
				mBinaryWriter.Write(1);
				mBinaryWriter.Write(mDeviceGUID);
				mBinaryWriter.Write(mGUIDSequenceID);
				mBinaryWriter.Close();
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Failed to Save SoaringAnalytics.dat: " + ex.Message + "\n" + ex.StackTrace);
			try
			{
				if (mBinaryWriter != null)
				{
					mBinaryWriter.Close();
				}
			}
			catch
			{
			}
		}
		mBinaryWriter = null;
	}

	public void Shutdown()
	{
		if (mBuffersData != null)
		{
			for (int i = 0; i < mBuffersData.Length; i++)
			{
				if (mBuffersData[i] != null)
				{
					mBuffersData[i].mBuffer.Close();
					mBuffersData[i].mBufferTemp.Close();
				}
			}
		}
		SaveSoaringAnalyticFile();
		mInitialized = false;
	}

	public void LogAnonymousEvent(string key, SoaringObjectBase value)
	{
		if (mInitialized)
		{
			LogEvent(key, value, 1);
		}
	}

	public void LogAnonymousEvents(SoaringArray entries)
	{
		if (mInitialized)
		{
			LogEvents(entries, 1);
		}
	}

	public void LogEvent(string key, SoaringObjectBase value, int logIndex = 0)
	{
		if (mInitialized)
		{
			SoaringArray soaringArray = new SoaringArray(1);
			SoaringDictionary soaringDictionary = new SoaringDictionary(1);
			soaringArray.addObject(soaringDictionary);
			soaringDictionary.addValue(key, "key");
			soaringDictionary.addValue(value, "value");
			LogEvents(soaringArray, logIndex);
		}
	}

	public void LogEvents(SoaringArray entries, int logIndex = 0)
	{
		if (!mInitialized || entries == null)
		{
			return;
		}
		int num = entries.count();
		switch (mEmbededGUIDType)
		{
		case EmbededGUIDType.AllEntries:
		{
			for (int j = 0; j < num; j++)
			{
				SoaringDictionary soaringDictionary3 = (SoaringDictionary)entries.objectAtIndex(j);
				if (soaringDictionary3 != null)
				{
					soaringDictionary3.addValue(GenerateGUID(), "guid");
				}
			}
			break;
		}
		case EmbededGUIDType.AllValues:
		{
			for (int i = 0; i < num; i++)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)entries.objectAtIndex(i);
				if (soaringDictionary != null)
				{
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("value");
					if (soaringDictionary2 != null)
					{
						soaringDictionary2.addValue(GenerateGUID(), "guid");
					}
				}
			}
			break;
		}
		}
		if (!SoaringInternal.IsProductionMode && _bERROR_LOG)
		{
			SoaringDebug.Log("SoaringAnalytics.cs | Logging events: " + entries.ToJsonString());
		}
		if (mBuffersData[logIndex].mWaitingForResponse)
		{
			mBuffersData[logIndex].mBufferTemp.Append(entries);
		}
		else
		{
			mBuffersData[logIndex].mBuffer.Append(entries);
		}
	}

	public void Update(float deltaTime)
	{
		if (!mInitialized || !Soaring.IsInitialized)
		{
			return;
		}
		for (int i = 0; i < mBuffersData.Length; i++)
		{
			if (mBuffersData[i].mWaitingForResponse)
			{
				continue;
			}
			mBuffersData[i].mUpdateTime += deltaTime;
			if (mBuffersData[i].mUpdateTime < mBuffersData[i].mUpdateInterval)
			{
				continue;
			}
			mBuffersData[i].mUpdateTime = 0f;
			mBuffersData[i].mBuffer.Append(mBuffersData[i].mBufferTemp.GetData());
			mBuffersData[i].mBufferTemp.Clear();
			SoaringArray data = mBuffersData[i].mBuffer.GetData();
			if (data.count() > 0)
			{
				mBuffersData[i].mWaitingForResponse = true;
				SoaringContext soaringContext = "SoaringAnalyticsNew";
				soaringContext.Responder = new SoaringAnalyticsDelegate(this);
				if (i == 0)
				{
					SoaringInternal.instance.internal_SaveStat(data, soaringContext);
				}
				else
				{
					SoaringInternal.instance.internal_SaveAnonymousStat(data, soaringContext);
				}
				SaveSoaringAnalyticFile();
			}
		}
	}

	public void _OnSaveStat(bool success, int nLogIndex, SoaringError error, SoaringContext context)
	{
		if (mBuffersData[nLogIndex] != null && mBuffersData[nLogIndex].mWaitingForResponse)
		{
			if (success)
			{
				mBuffersData[nLogIndex].mBuffer.Clear();
			}
			else if (Soaring.IsOnline)
			{
				mBuffersData[nLogIndex].mUpdateTime = mBuffersData[nLogIndex].mUpdateInterval;
			}
			else
			{
				mBuffersData[nLogIndex].mUpdateTime = 0f;
			}
			mBuffersData[nLogIndex].mWaitingForResponse = false;
		}
	}

	public static ulong AnalyticTime()
	{
		return (ulong)SoaringTime.AdjustedServerTime;
	}

	public static string GenerateGUID()
	{
		return AnalyticTime().ToString() + (ushort)Fast_Rand() + (ushort)(Environment.TickCount & 0x7FFFFFFF);
	}

	public static uint Fast_Rand()
	{
		uint num = (1 & mRandSeed) + 1;
		for (uint num2 = 0u; num2 < num; num2++)
		{
			mRandVal += mRandSeed;
		}
		return mRandVal;
	}
}
