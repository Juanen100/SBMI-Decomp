public class SoaringServerTimeModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveServerTime";
	}

	public override int ModuleChannel()
	{
		return 4;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		long num = 0L;
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		if (moduleData.state)
		{
			SoaringValue soaringValue = moduleData.data.soaringValue("timestamp");
			if (soaringValue != null)
			{
				long num2 = soaringValue;
				num = num2;
				SoaringTime.UpdateServerTime(num);
			}
		}
		SoaringInternal.Delegate.OnServerTimeUpdated(moduleData.state, moduleData.error, num, moduleData.context);
	}
}
