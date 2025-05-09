using System;
using UnityEngine;

public class SBMIIOSPlatformModule : SoaringPlatformIOS
{
	private string sResetDeviceID = string.Empty;

	private bool UseResetDeviceID;

	public override void Init()
	{
		base.Init();
		UseResetDeviceID = PlayerPrefs.GetInt("UseResetDeviceID", 0) != 0;
		sResetDeviceID = PlayerPrefs.GetString("ResetDeviceID", string.Empty);
	}

	public void ResetDeviceID()
	{
		UseResetDeviceID = true;
		PlayerPrefs.SetInt("UseResetDeviceID", 1);
	}

	public override string DeviceID()
	{
		if (!UseResetDeviceID)
		{
			return base.DeviceID();
		}
		if (string.IsNullOrEmpty(sResetDeviceID))
		{
			sResetDeviceID = "SBMIRGK_" + Environment.MachineName + "_" + UnityEngine.Random.Range(10000, 999999);
			PlayerPrefs.SetString("ResetDeviceID", sResetDeviceID);
		}
		return sResetDeviceID;
	}
}
