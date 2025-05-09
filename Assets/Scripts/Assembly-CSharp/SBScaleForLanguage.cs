using System;
using UnityEngine;

public class SBScaleForLanguage : MonoBehaviour
{
	public enum DeviceType
	{
		Free = 0,
		iPhone = 1,
		iPhoneWide = 2,
		iPad = 3
	}

	[Serializable]
	public class SBScaler
	{
		public LanguageCode language;

		public Vector3 position = new Vector3(0f, 0f, 0f);

		public Vector3 scale = new Vector3(1f, 1f, 1f);

		public DeviceType type;
	}

	public SBScaler[] scales = new SBScaler[0];

	public GameObject scaledObject;

	public void Scale()
	{
		Scale(scaledObject);
	}

	public void Scale(GameObject ob)
	{
		if (ob == null || scales == null)
		{
			return;
		}
		LanguageCode languageCode = Language.CurrentLanguage();
		if (languageCode == LanguageCode.N)
		{
			return;
		}
		DeviceType deviceType = FindDeviceType();
		SBScaler sBScaler = null;
		int num = scales.Length;
		for (int i = 0; i < num; i++)
		{
			if (scales[i].language == languageCode && scales[i].type == deviceType)
			{
				sBScaler = scales[i];
				break;
			}
		}
		if (sBScaler != null)
		{
			ob.transform.localPosition = sBScaler.position;
			ob.transform.localScale = sBScaler.scale;
		}
	}

	private DeviceType FindDeviceType()
	{
		return DeviceType.Free;
	}
}
