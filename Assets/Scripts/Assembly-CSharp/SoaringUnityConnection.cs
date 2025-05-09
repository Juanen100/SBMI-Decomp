using System;
using MTools;
using UnityEngine;

public class SoaringUnityConnection : SoaringConnection
{
	private WWW mConnection;

	public override float Progress
	{
		get
		{
			return mConnection.progress;
		}
	}

	public override string ContentAsText
	{
		get
		{
			return mConnection.text;
		}
	}

	public override byte[] Content
	{
		get
		{
			return mConnection.bytes;
		}
	}

	public override bool HasError
	{
		get
		{
			if (mConnection.error != null)
			{
				mError = mConnection.error;
				mErrorCode = -7;
			}
			return Error != null;
		}
	}

	public override bool IsValid
	{
		get
		{
			if (mProperties == null)
			{
				return false;
			}
			return !string.IsNullOrEmpty(mProperties.URL);
		}
	}

	public override bool Create(SCWebQueue.SCData properties)
	{
		if (properties == null)
		{
			return false;
		}
		base.Create(properties);
		string text = mProperties.URL;
		if (CacheVersion != -1)
		{
			mConnection = WWW.LoadFromCacheOrDownload(text, CacheVersion);
			return true;
		}
		if (properties.GetParams != null)
		{
			SoaringDictionary getParams = properties.GetParams;
			if (getParams.count() != 0)
			{
				text += "?";
				string[] array = getParams.allKeys();
				SoaringObjectBase[] array2 = getParams.allValues();
				int num = getParams.count();
				for (int i = 0; i < num; i++)
				{
					text += array[i];
					text += "=";
					text += array2[i].ToString();
					if (i + 1 < num)
					{
						text += "@";
					}
				}
			}
		}
		WWWForm wWWForm = null;
		if (properties.PostParams != null)
		{
			SoaringDictionary postParams = properties.PostParams;
			if (postParams.count() != 0)
			{
				wWWForm = new WWWForm();
				wWWForm.headers.Add("content-type", "application/x-www-form-urlencoded");
				wWWForm.headers.Add("soaring-sdk", SCWebQueue.ReportedSDK);
				string[] array3 = postParams.allKeys();
				SoaringObjectBase[] array4 = postParams.allValues();
				int num2 = postParams.count();
				bool flag = SoaringDebug.IsLoggingToConsole & SoaringDebug.IsLoggingToFile;
				for (int j = 0; j < num2; j++)
				{
					string text2 = array4[j].ToString();
					wWWForm.AddField(array3[j], text2);
					if (flag)
					{
						SoaringDebug.Log(array3[j] + ":" + text2);
					}
				}
			}
		}
		if (wWWForm != null)
		{
			mConnection = new WWW(text, wWWForm);
		}
		else
		{
			mConnection = new WWW(text);
		}
		return IsValid;
	}

	public override bool SaveData()
	{
		bool result = false;
		if (mProperties == null)
		{
			return result;
		}
		string saveLocation = mProperties.SaveLocation;
		if (string.IsNullOrEmpty(saveLocation))
		{
			return result;
		}
		if (CacheVersion != -1)
		{
			return result;
		}
		try
		{
			MBinaryWriter mBinaryWriter = new MBinaryWriter();
			if (mBinaryWriter.Open(mProperties.SaveLocation, true, true))
			{
				if (mBinaryWriter.IsOpen())
				{
					mBinaryWriter.Write(mConnection.bytes);
					mBinaryWriter.Flush();
					mBinaryWriter.Close();
				}
				result = true;
			}
			mBinaryWriter = null;
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
		return result;
	}

	public override bool IsDone()
	{
		return mConnection.isDone;
	}
}
