public class SoaringGenerateInviteCodeModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveInvitationCode";
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		string invite_code = null;
		if (moduleData.data != null)
		{
			invite_code = moduleData.data.soaringValue("invitationCode");
		}
		SoaringInternal.Delegate.OnRetrieveInvitationCode(moduleData.state, moduleData.error, invite_code);
	}
}
