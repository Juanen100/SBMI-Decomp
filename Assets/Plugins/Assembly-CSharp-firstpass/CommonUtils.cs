using System.Collections.Generic;

public static class CommonUtils
{
	public enum LevelOfDetail
	{
		None = 0,
		Low = 1,
		Standard = 2,
		High = 3,
		_Total = 4
	}

	public class DeviceLevels
	{
		public string name;

		public int lod;

		public bool isNonParticle;

		public bool isNonScaling;

		public bool isPlusSize;

		public int resolutionScale;
	}

	public const string LOW_RESOURCE_ID = "_lr";

	private static int MemoryLevel;

	private static Dictionary<string, object> TextureOverrides;

	private static Dictionary<string, object> CommonProperties;

	private static DeviceLevels CurrentLevels;

	private static int[] QualityMemoryRanges;

	private static bool isLoaded;

	private static bool firstLODLog;

	public static bool IsNonParticleDevice()
	{
		return false;
	}

	public static bool IsNonScalingDevice()
	{
		return false;
	}

	public static bool IsPlusSize()
	{
		return false;
	}

	public static int ResolutionScale()
	{
		return 0;
	}

	public static string SettingsName()
	{
		return null;
	}

	public static void SetMemoryLevel(int ml)
	{
	}

	public static int GetMemoryLevel()
	{
		return 0;
	}

	public static void Reload()
	{
	}

	public static void Init()
	{
	}

	public static string ReadAllText(string filePath)
	{
		return null;
	}

	private static string GetStreamingAssetsFile(string fileName)
	{
		return null;
	}

	private static bool FileExists(string filePath)
	{
		return false;
	}

	private static bool LoadWWWExist(string filePath)
	{
		return false;
	}

	public static string TextureForDeviceOverride(string textureName)
	{
		return null;
	}

	public static string PropertyForDeviceOverride(string propertyName)
	{
		return null;
	}

	public static LevelOfDetail TextureLod()
	{
		return default(LevelOfDetail);
	}

	public static bool CheckReloadShader()
	{
		return false;
	}
}
