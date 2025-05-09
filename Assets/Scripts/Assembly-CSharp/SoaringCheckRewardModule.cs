public class SoaringCheckRewardModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveVirtualGoodCoupons";
	}

	public override int ModuleChannel()
	{
		return 1;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringArray rewards = null;
		if (moduleData.data != null)
		{
			SoaringObjectBase soaringObjectBase = moduleData.data.objectWithKey("coupons");
			if (soaringObjectBase != null)
			{
				if (soaringObjectBase.Type == IsType.Array)
				{
					rewards = (SoaringArray)soaringObjectBase;
				}
				else
				{
					rewards = new SoaringArray(1);
					rewards.addObject(soaringObjectBase);
				}
				int num = rewards.count();
				SoaringArray soaringArray = new SoaringArray(num);
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)rewards.objectAtIndex(i);
					string text = soaringDictionary.soaringValue("coupon");
					string text2 = soaringDictionary.soaringValue("receipt");
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						SoaringCoupon obj = new SoaringCoupon(text, text2);
						soaringArray.addObject(obj);
					}
				}
				rewards = soaringArray;
			}
		}
		SoaringInternal.Delegate.OnCheckUserRewards(moduleData.state, moduleData.error, rewards);
	}
}
