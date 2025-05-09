using System;
using MTools;
using UnityEngine;

public class SoaringRetrieveProductModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveIapProducts";
	}

	public override int ModuleChannel()
	{
		return 1;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + ModuleName() + "\",\n";
		text = string.Concat(text, "\"authToken\":\"", data.soaringValue("authToken"), "\"\n},");
		text += "\n\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, ModuleChannel(), context, false);
	}

	public static SoaringPurchasable[] LoadCachedProductData()
	{
		string empty = string.Empty;
		SoaringDictionary soaringDictionary = null;
		SoaringInternal.instance.Purchasables.clear();
		MBinaryReader mBinaryReader = null;
		try
		{
			mBinaryReader = ResourceUtils.GetFileStream("Products", empty + "Soaring", "dat", 9);
			if (mBinaryReader != null)
			{
				string json_data = mBinaryReader.ReadString();
				soaringDictionary = new SoaringDictionary(json_data);
				mBinaryReader.Close();
				mBinaryReader = null;
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message + "\n" + ex.StackTrace);
			try
			{
				mBinaryReader.Close();
				mBinaryReader = null;
			}
			catch
			{
			}
		}
		SoaringPurchasable[] array = null;
		if (soaringDictionary == null)
		{
			return array;
		}
		SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("products");
		if (soaringDictionary != null)
		{
			int num = soaringArray.count();
			array = new SoaringPurchasable[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new SoaringPurchasable((SoaringDictionary)soaringArray.objectAtIndex(i));
				SoaringInternal.instance.Purchasables.addValue_unsafe(array[i], array[i].ProductID);
			}
		}
		return array;
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
		SoaringPurchasable[] array = null;
		SoaringInternal.instance.Purchasables.clear();
		if (!data.state || data.error != null || data.data == null)
		{
			data.state = false;
			string empty = string.Empty;
			try
			{
				MBinaryReader fileStream = ResourceUtils.GetFileStream("Products", empty + "Soaring", "dat", 9);
				if (fileStream != null)
				{
					string json_data = fileStream.ReadString();
					data.data = new SoaringDictionary(json_data);
					fileStream.Close();
					fileStream = null;
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message + "\n" + ex.StackTrace);
				data.data = null;
			}
		}
		if (data.data != null)
		{
			SoaringArray soaringArray = (SoaringArray)data.data.objectWithKey("products");
			if (soaringArray != null)
			{
				int num = soaringArray.count();
				array = new SoaringPurchasable[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = new SoaringPurchasable((SoaringDictionary)soaringArray.objectAtIndex(i));
					SoaringInternal.instance.Purchasables.addValue_unsafe(array[i], array[i].ProductID);
				}
			}
			string empty2 = string.Empty;
			try
			{
				string writePath = ResourceUtils.GetWritePath("Products.dat", empty2 + "Soaring", 9);
				MBinaryWriter mBinaryWriter = new MBinaryWriter();
				if (!mBinaryWriter.Open(writePath, true))
				{
					throw new Exception();
				}
				mBinaryWriter.Write(data.data.ToJsonString());
				mBinaryWriter.Close();
				mBinaryWriter = null;
			}
			catch (Exception ex2)
			{
				Debug.Log(ex2.Message + "\n" + ex2.StackTrace);
			}
		}
		Soaring.Delegate.OnRetrieveProducts(data.state, data.error, array, data.context);
	}
}
