using System;
using System.Text;
using UnityEngine;

public class SoaringPlatformAndroid : SoaringPlatform.SoaringPlatformDelegate
{
	private AndroidJavaClass cls_Soaring;

	private string mAndroidID;

	private string mIMEI;

	private string[] mMacAddresses;

	private long mTotalMemory;

	public string IMEI
	{
		get
		{
			if (!string.IsNullOrEmpty(mIMEI))
			{
				return mIMEI;
			}
			try
			{
				mIMEI = cls_Soaring.CallStatic<string>("GetIMEI", new object[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Soaring: " + ex.Message, LogType.Warning);
				mIMEI = string.Empty;
			}
			if (mIMEI == null)
			{
				mIMEI = string.Empty;
			}
			return mIMEI;
		}
	}

	public string AndroidID
	{
		get
		{
			if (!string.IsNullOrEmpty(mAndroidID))
			{
				return mAndroidID;
			}
			try
			{
				mAndroidID = cls_Soaring.CallStatic<string>("GetAndroidID", new object[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Error: " + ex.Message, LogType.Warning);
				mAndroidID = string.Empty;
			}
			if (mAndroidID == null)
			{
				mAndroidID = string.Empty;
			}
			return mAndroidID;
		}
	}

	public string[] MacAddresses
	{
		get
		{
			if (mMacAddresses != null)
			{
				return mMacAddresses;
			}
			try
			{
				string text = cls_Soaring.CallStatic<string>("GetAllMACAddress", new object[0]);
				if (!string.IsNullOrEmpty(text))
				{
					char[] separator = new char[1] { ',' };
					mMacAddresses = text.Split(separator);
					for (int i = 0; i < mMacAddresses.Length; i++)
					{
						string text2 = SoaringVersions.CalculateMD5Hash(Encoding.UTF8.GetBytes(mMacAddresses[i]));
						if (text2.Length > 2)
						{
							text2 = text2.Substring(0, text2.Length - 2);
						}
						mMacAddresses[i] = text2;
					}
				}
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Error: " + ex.Message, LogType.Warning);
				mMacAddresses = null;
			}
			if (mMacAddresses == null)
			{
				mMacAddresses = null;
			}
			return mMacAddresses;
		}
	}

	public long TotalMemory
	{
		get
		{
			if (mTotalMemory != 0L)
			{
				return mTotalMemory;
			}
			try
			{
				mTotalMemory = cls_Soaring.CallStatic<long>("GetTotalMemory", new object[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("TotalMemory: " + ex.Message, LogType.Warning);
				mTotalMemory = -1L;
			}
			return mTotalMemory;
		}
	}

	public override void Init()
	{
		mAndroidID = string.Empty;
		mIMEI = string.Empty;
		try
		{
			cls_Soaring = new AndroidJavaClass("com.fws.soaring.Soaring");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					cls_Soaring.CallStatic<bool>("Init", new object[2]
					{
						androidJavaObject,
						string.Empty
					});
				}
				mIMEI = IMEI;
				mAndroidID = AndroidID;
				mMacAddresses = MacAddresses;
				mTotalMemory = TotalMemory;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Soaring: " + ex.Message, LogType.Error);
		}
	}

	public override string PlatformName()
	{
		return "android";
	}

	public override bool PlatformLoginAvailable()
	{
		return false;
	}

	public override bool PlatformAuthenticated()
	{
		return false;
	}

	public override bool PlatformAuthenticate(SoaringContext context)
	{
		return false;
	}

	public override string PlatformID()
	{
		return null;
	}

	public override string PlatformAlias()
	{
		return null;
	}

	public override string DeviceID()
	{
		string text = AndroidID;
		if (string.IsNullOrEmpty(text))
		{
			text = SystemInfo.deviceUniqueIdentifier;
		}
		return text;
	}

	public override SoaringDictionary GenerateDeviceDictionary()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(PlatformName(), "platform");
		string text = IMEI;
		if (Application.isEditor)
		{
			text = "00000000000000";
		}
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "imei");
		}
		string androidID = AndroidID;
		if (!string.IsNullOrEmpty(androidID))
		{
			soaringDictionary.addValue(androidID, "android_id");
		}
		soaringDictionary.addValue(SystemInfo.deviceUniqueIdentifier, "unique");
		soaringDictionary.addValue(DeviceID(), "deviceId");
		return soaringDictionary;
	}

	public override string PushNotificationsProtocol()
	{
		return "gcm";
	}

	public override bool OpenURL(string url)
	{
		bool result = false;
		if (url == null)
		{
			return result;
		}
		try
		{
			int num = cls_Soaring.CallStatic<int>("OpenURL", new object[1] { url });
			if (num == 1)
			{
				result = true;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("OpenURL: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	public override bool SendEmail(string subject, string body, string email)
	{
		bool result = false;
		if (subject == null || email == null || body == null)
		{
			return result;
		}
		try
		{
			result = cls_Soaring.CallStatic<bool>("SendEmail", new object[3] { subject, body, email });
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SendEmail: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	public void OpenDialog(string title, string body, string button)
	{
		try
		{
			cls_Soaring.CallStatic("OpenDialog", title, body, button);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Soaring: " + ex.Message, LogType.Warning);
		}
	}

	public override long SystemBootTime()
	{
		long result = 0L;
		try
		{
			result = cls_Soaring.CallStatic<long>("DeviceBootTime", new object[0]);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Time: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	public override long SystemTimeSinceBootTime()
	{
		long result = 0L;
		try
		{
			result = cls_Soaring.CallStatic<long>("DeviceTimeSinceBoot", new object[0]);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Time: " + ex.Message, LogType.Warning);
		}
		return result;
	}
}
