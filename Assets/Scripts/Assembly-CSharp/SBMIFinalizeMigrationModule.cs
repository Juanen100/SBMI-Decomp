public class SBMIFinalizeMigrationModule : SoaringCustomQueryModule
{
	public const string NAME = "finalizeMigration";

	public override string CustomSoaringModuleName()
	{
		return null;
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override string QueryActionName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}
}
