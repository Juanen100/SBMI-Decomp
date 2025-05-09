public class SBMIAddCredentialsToUserModule : SoaringModule
{
	public const string NAME = "addCredentialsToUser";

	public override string ModuleName()
	{
		return "addCredentialsToUser";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text = text + ",\n\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(1);
		soaringDictionary2.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary2, 1, context, false);
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}
}
