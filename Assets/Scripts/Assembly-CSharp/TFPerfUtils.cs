public class TFPerfUtils
{
	public const string LOW_RESOURCE_ID = "_lr";

	private static bool isNonParticleDevice;

	private static bool isNonScalingDevice;

	public static CommonUtils.LevelOfDetail MemoryLod
	{
		get
		{
			return CommonUtils.TextureLod();
		}
	}

	static TFPerfUtils()
	{
		SetNonParticleDevice();
		SetNonScalingDevice();
	}

	public static bool IsNonParticleDevice()
	{
		return isNonParticleDevice;
	}

	public static bool IsNonScalingDevice()
	{
		return isNonScalingDevice;
	}

	private static void SetNonParticleDevice()
	{
		isNonParticleDevice = false;
	}

	private static void SetNonScalingDevice()
	{
		isNonScalingDevice = false;
	}
}
