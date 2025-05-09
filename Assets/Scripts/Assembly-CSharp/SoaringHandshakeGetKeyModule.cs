using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

internal class SoaringHandshakeGetKeyModule : SoaringModule
{
	public override string ModuleName()
	{
		return "handshake_pt1";
	}

	public override int ModuleChannel()
	{
		return 0;
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		string text = UnityEngine.Random.Range(100000000, int.MaxValue).ToString();
		context.addValue(text, "ra");
		soaringDictionary.addValue(text, "ra");
		string text2 = "{\n" + SCQueueTools.CreateJsonMessage("action", "handshake", data.soaringValue("gameId"), null) + ",\n";
		text2 = text2 + "\"data\" : " + soaringDictionary.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary2.addValue(text2, "data");
		context.addValue(soaringDictionary.ToJsonString(), "request");
		PushCorePostDataToQueue(soaringDictionary2, 0, context, false);
	}

	public byte[] GetRSAData(string certData, byte[] encryptedData)
	{
		byte[] array = null;
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(certData);
			X509Certificate2 x509Certificate = new X509Certificate2(bytes);
			RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;
			return rSACryptoServiceProvider.Encrypt(encryptedData, false);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
			return null;
		}
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		bool flag = false;
		if (moduleData.state)
		{
			string text = moduleData.data.soaringValue("cert");
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = moduleData.data.soaringValue("digest");
				if ("sha256" == text2)
				{
					string text3 = moduleData.data.soaringValue("rb");
					string text4 = moduleData.context.soaringValue("ra");
					SoaringEncryption soaringEncryption = new SoaringEncryption(moduleData.data.soaringValue("cipher"), text2);
					soaringEncryption.SetSID(moduleData.data.soaringValue("sid"));
					moduleData.context.addValue(soaringEncryption, "encryption");
					byte[] bytes = Encoding.ASCII.GetBytes(text4 + "-" + text3);
					System.Random random = new System.Random();
					byte[] array = new byte[32];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = (byte)random.Next();
					}
					byte[] rSAData = GetRSAData(text, array);
					string text5 = Convert.ToBase64String(rSAData);
					HMACSHA256 hMACSHA = new HMACSHA256(array);
					byte[] array2 = hMACSHA.ComputeHash(bytes);
					SoaringDebug.Log(Convert.ToBase64String(array2));
					soaringEncryption.SetEncryptionKey(array2);
					SoaringDictionary soaringDictionary = new SoaringDictionary();
					soaringDictionary.addValue(text5, "pk");
					soaringDictionary.addValue(SoaringEncryption.SID, "sid");
					flag = SoaringInternal.instance.CallModule("handshake_pt2", soaringDictionary, moduleData.context);
				}
			}
		}
		if (!flag)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
			if (!SoaringInternal.instance.IsInitialized())
			{
				SoaringInternal.instance.HandleFinalGameInitialization(false);
			}
			else
			{
				SoaringInternal.instance.HandleStashedCalls();
			}
		}
	}
}
