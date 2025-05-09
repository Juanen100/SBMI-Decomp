using System;
using System.Text;
using MTools;

public class SoaringAddressKeeper
{
	public enum AddressKeys
	{
		AppleStoreReview = 0,
		GoogleStoreReview = 1,
		AmazonStoreReview = 2,
		Facebook = 3,
		Twitter = 4,
		Homepage = 5,
		Review = 6
	}

	private string[] mKeyNames = new string[6] { "AK_AppleStoreReview", "AK_GoogleStoreReview", "AK_AmazonStoreReview", "AK_Facebook", "AK_Twitter", "AK_Homepage" };

	private SoaringDictionary mSoaringKeys;

	public SoaringAddressKeeper()
	{
		SoaringInternal.instance.RegisterModule(new SoaringAddressKeeperModule());
		Load();
	}

	private void Load()
	{
		try
		{
			MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringAK", "Soaring", "addr", 3);
			if (fileStream != null && fileStream.IsOpen())
			{
				byte[] array = fileStream.ReadAllBytes();
				if (array != null)
				{
					string text = Encoding.UTF8.GetString(array);
					if (text != null)
					{
						mSoaringKeys = new SoaringDictionary(text);
					}
				}
				fileStream.Close();
				fileStream = null;
			}
		}
		catch
		{
			if (mSoaringKeys != null)
			{
				mSoaringKeys.clear();
			}
		}
		if (mSoaringKeys == null)
		{
			mSoaringKeys = new SoaringDictionary();
		}
		QuickCopySoaring();
	}

	private void QuickCopySoaring()
	{
		mSoaringKeys = mSoaringKeys.makeCopy();
	}

	private void Save()
	{
		try
		{
			string s = mSoaringKeys.ToJsonString();
			string empty = string.Empty;
			string writePath = ResourceUtils.GetWritePath("SoaringAK.addr", empty + "Soaring", 1);
			MBinaryWriter mBinaryWriter = new MBinaryWriter();
			if (!mBinaryWriter.Open(writePath, true, true))
			{
				throw new Exception();
			}
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			int num = bytes.Length;
			for (int i = 0; i < num; i++)
			{
				mBinaryWriter.Write(bytes[i]);
			}
			mBinaryWriter.Close();
			mBinaryWriter = null;
		}
		catch
		{
		}
		QuickCopySoaring();
	}

	public void SetAddressData(SoaringDictionary data)
	{
		if (data != null)
		{
			int num = data.count();
			string[] array = data.allKeys();
			SoaringObjectBase[] array2 = data.allValues();
			for (int i = 0; i < num; i++)
			{
				SoaringValue val = new SoaringValue(array2[i].ToString());
				mSoaringKeys.setValue(val, array[i]);
			}
			Save();
		}
	}

	public void SetSoaringAddressData(SoaringDictionary data)
	{
		SetAddressData(data);
	}

	public string Address(AddressKeys name)
	{
		AddressKeys addressKeys = name;
		if (addressKeys == AddressKeys.Review)
		{
			name = AddressKeys.GoogleStoreReview;
		}
		int num = (int)name;
		if (num >= 6)
		{
			return string.Empty;
		}
		return mKeyNames[num];
	}

	public string Address(string name)
	{
		if (name == null)
		{
			return string.Empty;
		}
		string text = mSoaringKeys.soaringValue(name);
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}
}
