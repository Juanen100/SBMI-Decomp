using System.Collections.Generic;

public class ButtonTapMatcher : Matcher
{
	public const string BUTTON_ID = "button_id";

	public static ButtonTapMatcher FromDict(Dictionary<string, object> dict)
	{
		ButtonTapMatcher buttonTapMatcher = new ButtonTapMatcher();
		buttonTapMatcher.RegisterProperty("button_id", dict);
		return buttonTapMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		return "button_tap";
	}
}
