using System;
using System.IO;
using MTools;
using UnityEngine;

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
		: base(IsType.Object)
	{
		mSoaringAdDataReferences = new SoaringArray();
		mActiveAdRequests = new SoaringDictionary();
		string empty = string.Empty;
		mAdFilePath = ResourceUtils.GetFilePath(empty + "Soaring/AdServer", string.Empty, 9, false);
		SoaringInternal.instance.RegisterModule(new SoaringAdServerModule());
		LoadAdReferences();
	}

	public void RequestAd(string adName, bool displayAdOnComplete, SoaringContext context)
	{
		if (string.IsNullOrEmpty(adName))
		{
			adName = "kS_ANY";
		}
		mActiveAdRequests.removeObjectWithKey(adName);
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(Application.platform.ToString(), "platform");
		soaringDictionary.addValue(mAdServer + "advert.json?" + UnityEngine.Random.Range(0, int.MaxValue), "turl");
		SoaringContext soaringContext = context;
		if (soaringContext == null)
		{
			soaringContext = new SoaringContext();
		}
		soaringContext.addValue(displayAdOnComplete, "display_advert");
		soaringContext.addValue(adName, "advert_name");
		if (!SoaringInternal.instance.CallModule("adServer", soaringDictionary, soaringContext))
		{
			HandleAdDownload(null, soaringContext);
		}
	}

	public bool AdAvailable(string adName)
	{
		if (string.IsNullOrEmpty(adName))
		{
			adName = "kS_ANY";
		}
		SoaringContext soaringContext = (SoaringContext)mActiveAdRequests.objectWithKey(adName);
		if (soaringContext == null)
		{
			return false;
		}
		SoaringAdData soaringAdData = (SoaringAdData)soaringContext.objectWithKey("advert_data");
		if (soaringAdData == null)
		{
			return false;
		}
		return soaringAdData.AdTexture != null;
	}

	public bool DisplayAd(string adName)
	{
		if (string.IsNullOrEmpty(adName))
		{
			adName = "kS_ANY";
		}
		if (!AdAvailable(adName))
		{
			return false;
		}
		SoaringContext soaringContext = (SoaringContext)mActiveAdRequests.objectWithKey(adName);
		if (soaringContext == null)
		{
			return false;
		}
		mActiveAdRequests.removeObjectWithKey(adName);
		SoaringAdData soaringAdData = (SoaringAdData)soaringContext.objectWithKey("advert_data");
		if (soaringAdData == null)
		{
			return false;
		}
		return SoaringAdView.CreateAdView(soaringAdData, this, soaringContext);
	}

	public void SetAdServerURL(string url)
	{
		mAdServer = url;
	}

	private void CleanupAds()
	{
		if (mSoaringAdDataReferences == null)
		{
			return;
		}
		bool flag = false;
		long adjustedServerTime = SoaringTime.AdjustedServerTime;
		for (int i = 0; i < mSoaringAdDataReferences.count(); i++)
		{
			SoaringAdData soaringAdData = (SoaringAdData)mSoaringAdDataReferences.objectAtIndex(i);
			if (adjustedServerTime <= soaringAdData.AdExpires || soaringAdData.AdExpires <= 0)
			{
				continue;
			}
			mSoaringAdDataReferences.removeObjectAtIndex(i);
			if (!string.IsNullOrEmpty(soaringAdData.AdID))
			{
				try
				{
					if (File.Exists(mAdFilePath + "/" + soaringAdData.AdID))
					{
						File.Delete(mAdFilePath + "/" + soaringAdData.AdID);
					}
				}
				catch
				{
				}
			}
			flag = true;
			i--;
		}
		if (flag)
		{
			SaveAdReferences();
		}
	}

	private SoaringAdData CheckAdExists(string adID)
	{
		SoaringAdData soaringAdData = null;
		if (mSoaringAdDataReferences == null)
		{
			return null;
		}
		for (int i = 0; i < mSoaringAdDataReferences.count(); i++)
		{
			soaringAdData = (SoaringAdData)mSoaringAdDataReferences.objectAtIndex(i);
			if (soaringAdData.AdID == adID)
			{
				break;
			}
			soaringAdData = null;
		}
		return soaringAdData;
	}

	private void SetAdReference(SoaringAdData data)
	{
		if (data == null || mSoaringAdDataReferences == null)
		{
			return;
		}
		for (int i = 0; i < mSoaringAdDataReferences.count(); i++)
		{
			SoaringAdData soaringAdData = (SoaringAdData)mSoaringAdDataReferences.objectAtIndex(i);
			if (data.AdID == soaringAdData.AdID)
			{
				mSoaringAdDataReferences.setObjectAtIndex(data, i);
				return;
			}
		}
		mSoaringAdDataReferences.addObject(data);
	}

	public void MarkAdAsShown(SoaringAdData data)
	{
		if (data != null)
		{
			data.SetAdShown();
			SaveAdReferences();
		}
	}

	internal void HandleAdRequestReturn(SoaringDictionary returnData, SoaringContext context)
	{
		CleanupAds();
		if (returnData == null)
		{
			HandleAdDownload(null, context);
			return;
		}
		if (mSoaringAdDataReferences == null)
		{
			LoadAdReferences();
			if (mSoaringAdDataReferences == null)
			{
				mSoaringAdDataReferences = new SoaringArray();
			}
		}
		SoaringArray soaringArray = null;
		string primaryPlatformName = SoaringPlatform.PrimaryPlatformName;
		if (!string.IsNullOrEmpty(primaryPlatformName) && returnData.containsKey(primaryPlatformName))
		{
			soaringArray = (SoaringArray)returnData.objectWithKey(primaryPlatformName);
		}
		if (soaringArray == null)
		{
			soaringArray = (SoaringArray)returnData.objectWithKey("Adverts");
		}
		if (soaringArray != null)
		{
			int num = soaringArray.count();
			if (num == 0)
			{
				HandleAdDownload(null, context);
				return;
			}
			SoaringArray soaringArray2 = new SoaringArray();
			for (int i = 0; i < num; i++)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
				string text = soaringDictionary.soaringValue("AdID");
				int num2 = soaringDictionary.soaringValue("AdNumShows");
				int num3 = 0;
				int num4 = 0;
				SoaringAdData soaringAdData = CheckAdExists(text);
				if (soaringAdData != null)
				{
					if (soaringAdData.TimesDisplayed >= num2 && num2 != -1)
					{
						continue;
					}
					num3 = soaringAdData.TimesDisplayed;
					num4 = soaringAdData.TimesClicked;
				}
				long num5 = soaringDictionary.soaringValue("AdStart");
				long num6 = soaringDictionary.soaringValue("AdExpires");
				string text2 = soaringDictionary.soaringValue("AdStartTime");
				string text3 = soaringDictionary.soaringValue("AdEndTime");
				SoaringDictionary localizations = (SoaringDictionary)soaringDictionary.objectWithKey("AdLocalization");
				try
				{
					if (!string.IsNullOrEmpty(text2))
					{
						DateTime time = DateTime.Parse(text2).ToUniversalTime();
						num5 = MTime.TimeSinceEpoch(time);
					}
					if (!string.IsNullOrEmpty(text3))
					{
						DateTime time2 = DateTime.Parse(text3).ToUniversalTime();
						num6 = MTime.TimeSinceEpoch(time2);
					}
				}
				catch (Exception ex)
				{
					SoaringDebug.Log(ex.Message, LogType.Error);
				}
				if (num5 > 0 && num6 > 0)
				{
					long adjustedServerTime = SoaringTime.AdjustedServerTime;
					if (num5 > adjustedServerTime || num6 < adjustedServerTime)
					{
						continue;
					}
				}
				string text4 = soaringDictionary.soaringValue("AdType");
				SoaringAdData.SoaringAdType adType = SoaringAdData.SoaringAdType.Other;
				switch (text4.ToLower())
				{
				case "local":
					adType = SoaringAdData.SoaringAdType.Local;
					break;
				case "market":
					adType = SoaringAdData.SoaringAdType.Market;
					break;
				case "web":
					adType = SoaringAdData.SoaringAdType.Web;
					break;
				}
				string path = soaringDictionary.soaringValue("Path");
				SoaringDictionary userData = (SoaringDictionary)soaringDictionary.objectWithKey("UserData");
				SoaringAdData soaringAdData2 = new SoaringAdData();
				soaringAdData2.SetData(null, text, path, num5, num6, num2, adType, localizations);
				soaringAdData2.SetUserData(userData);
				soaringAdData2.SetCachedData((short)num3, (short)num4);
				soaringArray2.addObject(soaringAdData2);
				SetAdReference(soaringAdData2);
			}
			string text5 = context.soaringValue("advert_name");
			int num7 = 0;
			num = soaringArray2.count();
			if (num == 0)
			{
				HandleAdDownload(null, context);
				return;
			}
			if (num > 1)
			{
				num7 = (int)MTime.ConstantTimeStamp() % num;
			}
			if (num7 >= num)
			{
				HandleAdDownload(null, context);
				return;
			}
			bool flag = false;
			SoaringAdData soaringAdData3 = null;
			if (text5 != null && text5 != "kS_ANY")
			{
				flag = true;
				if (text5.Contains("%"))
				{
					flag = false;
				}
				else
				{
					for (int j = 0; j < soaringArray2.count(); j++)
					{
						SoaringAdData soaringAdData4 = (SoaringAdData)soaringArray2.objectAtIndex(j);
						if (soaringAdData4.AdID == text5)
						{
							soaringAdData3 = soaringAdData4;
							break;
						}
					}
				}
			}
			if (soaringAdData3 == null && !flag)
			{
				soaringAdData3 = (SoaringAdData)soaringArray2.objectAtIndex(num7);
			}
			if (soaringAdData3 == null || string.IsNullOrEmpty(soaringAdData3.AdID))
			{
				HandleAdDownload(null, context);
				return;
			}
			SaveAdReferences();
			if (string.IsNullOrEmpty(text5))
			{
				text5 = "kS_ANY";
			}
			context.addValue(soaringAdData3, "advert_data");
			mActiveAdRequests.addValue(context, text5);
			LanguageCode mSoaringLanguage = SoaringInternal.instance.mSoaringLanguage;
			string text6 = null;
			if (soaringAdData3.AdLocalizations != null)
			{
				SoaringValue soaringValue = (SoaringValue)soaringAdData3.AdLocalizations.objectWithKey(mSoaringLanguage.ToString().ToLower());
				if (soaringValue != null)
				{
					text6 = soaringValue;
				}
			}
			if (string.IsNullOrEmpty(text6))
			{
				text6 = soaringAdData3.AdID;
			}
			if (File.Exists(mAdFilePath + "/" + soaringAdData3.AdID))
			{
				AdCallback(text5, true, mAdFilePath + "/" + soaringAdData3.AdID);
			}
			else
			{
				SoaringInternal.instance.DownloadFileWithSoaring(text5, mAdServer + text6, mAdFilePath + "/" + soaringAdData3.AdID, AdCallback);
			}
		}
		else
		{
			HandleAdDownload(null, context);
		}
	}

	private void AdCallback(string id, bool success, string path)
	{
		SoaringContext soaringContext = (SoaringContext)mActiveAdRequests.objectWithKey(id);
		if (soaringContext == null)
		{
			SoaringDebug.Log("SoaringAdModule: Error: No Context", LogType.Error);
			HandleAdDownload(null, soaringContext);
		}
		SoaringAdData soaringAdData = (SoaringAdData)soaringContext.objectWithKey("advert_data");
		if (success && soaringAdData != null)
		{
			if (path != null)
			{
				if (!File.Exists(path))
				{
					HandleAdDownload(null, soaringContext);
					return;
				}
				byte[] array = null;
				Texture2D texture2D = null;
				try
				{
					MBinaryReader mBinaryReader = new MBinaryReader(path);
					if (!mBinaryReader.IsOpen())
					{
						HandleAdDownload(null, soaringContext);
						return;
					}
					array = mBinaryReader.ReadAllBytes();
					mBinaryReader.Close();
					mBinaryReader = null;
				}
				catch (Exception ex)
				{
					SoaringDebug.Log(ex.Message, LogType.Error);
					soaringAdData = null;
				}
				try
				{
					if (array != null && soaringAdData != null)
					{
						texture2D = new Texture2D(0, 0, TextureFormat.RGB24, false);
						if (texture2D.LoadImage(array))
						{
							soaringAdData.SetData(texture2D, soaringAdData.AdID, soaringAdData.Path, soaringAdData.AdStarts, soaringAdData.AdExpires, soaringAdData.TimesWillBeDisplayed, soaringAdData.AdType, soaringAdData.AdLocalizations);
						}
						else
						{
							soaringAdData = null;
						}
					}
					else
					{
						soaringAdData = null;
					}
				}
				catch (Exception ex2)
				{
					SoaringDebug.Log(ex2.Message, LogType.Error);
					soaringAdData = null;
				}
			}
		}
		else
		{
			soaringAdData = null;
		}
		HandleAdDownload(soaringAdData, soaringContext);
	}

	private void HandleAdDownload(SoaringAdData adData, SoaringContext context)
	{
		if (adData != null && (bool)context.soaringValue("display_advert"))
		{
			DisplayAd(context.soaringValue("advert_name"));
		}
		bool flag = adData != null;
		Soaring.Delegate.OnAdServed(flag, adData, flag ? SoaringAdServerState.Retrieved : SoaringAdServerState.Failed, context);
	}

	private void LoadAdReferences()
	{
		if (mSoaringAdDataReferences == null)
		{
			return;
		}
		mSoaringAdDataReferences.clear();
		MBinaryReader mBinaryReader = new MBinaryReader(mAdFilePath + "/SoaringAD.ad");
		if (mBinaryReader == null || !mBinaryReader.IsOpen())
		{
			return;
		}
		try
		{
			if (mBinaryReader.ReadInt() == 0)
			{
				int num = mBinaryReader.ReadInt();
				for (int i = 0; i < num; i++)
				{
					string addID = mBinaryReader.ReadString();
					long expires = mBinaryReader.ReadLong();
					long starts = mBinaryReader.ReadLong();
					SoaringAdData.SoaringAdType adType = (SoaringAdData.SoaringAdType)mBinaryReader.ReadInt();
					short mAdDisplays = mBinaryReader.ReadShort();
					short shown = mBinaryReader.ReadShort();
					short clicks = mBinaryReader.ReadShort();
					string path = mBinaryReader.ReadString();
					SoaringAdData soaringAdData = new SoaringAdData();
					soaringAdData.SetData(null, addID, path, starts, expires, mAdDisplays, adType, null);
					soaringAdData.SetCachedData(shown, clicks);
					mSoaringAdDataReferences.addObject(soaringAdData);
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringAdServer: " + ex.Message);
		}
		mBinaryReader.Close();
		mBinaryReader = null;
	}

	private void SaveAdReferences()
	{
		if (mSoaringAdDataReferences == null)
		{
			return;
		}
		MBinaryWriter mBinaryWriter = new MBinaryWriter(mAdFilePath + "/SoaringAD.ad");
		if (mBinaryWriter == null || !mBinaryWriter.IsOpen())
		{
			return;
		}
		try
		{
			mBinaryWriter.Write(0);
			int num = mSoaringAdDataReferences.count();
			mBinaryWriter.Write(num);
			for (int i = 0; i < num; i++)
			{
				SoaringAdData soaringAdData = (SoaringAdData)mSoaringAdDataReferences.objectAtIndex(i);
				mBinaryWriter.Write(soaringAdData.AdID);
				mBinaryWriter.Write(soaringAdData.AdExpires);
				mBinaryWriter.Write(soaringAdData.AdStarts);
				mBinaryWriter.Write((int)soaringAdData.AdType);
				mBinaryWriter.Write(soaringAdData.TimesWillBeDisplayed);
				mBinaryWriter.Write(soaringAdData.TimesDisplayed);
				mBinaryWriter.Write(soaringAdData.TimesClicked);
				mBinaryWriter.Write(soaringAdData.Path);
			}
			mBinaryWriter.Flush();
		}
		catch
		{
		}
		mBinaryWriter.Close();
		mBinaryWriter = null;
	}
}
