public class DialogMatcher : Matcher
{
	public const string DIALOGSEQUENCE_ID = "sequence_id";

	public DialogMatcher(uint dialogSequenceId)
	{
	}

	public override string DescribeSubject(Game game)
	{
		return null;
	}
}
