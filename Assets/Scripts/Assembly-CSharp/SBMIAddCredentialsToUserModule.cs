public class SBMIAddCredentialsToUserModule : SoaringModule
{
	public const string NAME = "addCredentialsToUser";

	public override string ModuleName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}
}
