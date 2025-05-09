public class SBMIFinalizeMigrationModule : SoaringCustomQueryModule
{
	public const string NAME = "finalizeMigration";

	public override string CustomSoaringModuleName()
	{
		return "finalizeMigration";
	}

	public override bool ShouldEncryptCall()
	{
		return SoaringInternalProperties.SecureCommunication;
	}

	public override string QueryActionName()
	{
		return "customQuery2";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(data.soaringValue("gameId"), "gameId");
		callData.removeObjectWithKey("authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", QueryActionName(), null, soaringDictionary) + ",\n";
		string text2 = text;
		text = text2 + "\"data\" : {\n\"queryService\" : \"" + CustomSoaringModuleName() + "\",\n\"queryParameters\" : " + callData.ToJsonString() + "\n}\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PostCallData(soaringDictionary, context);
	}
}
