public class DialogMatcher : Matcher
{
	public const string DIALOGSEQUENCE_ID = "sequence_id";

	public DialogMatcher(uint dialogSequenceId)
	{
		AddRequiredProperty("sequence_id", dialogSequenceId.ToString());
	}

	public override string DescribeSubject(Game game)
	{
		return "Close dialog " + GetTarget("sequence_id");
	}
}
