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

		public Vector3 position;

		public Vector3 scale;

		public DeviceType type;
	}

	public SBScaler[] scales;

	public GameObject scaledObject;

	public void Scale()
	{
	}

	public void Scale(GameObject ob)
	{
	}

	private DeviceType FindDeviceType()
	{
		return default(DeviceType);
	}
}
