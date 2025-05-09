using System.Collections.Generic;

public class ButtonTapCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "button_tap";

	public static ButtonTapCondition FromDict(Dictionary<string, object> dict)
	{
		ButtonTapCondition buttonTapCondition = new ButtonTapCondition();
		buttonTapCondition.Parse(dict, "button_tap", new List<string> { typeof(ButtonTapAction).ToString() }, new List<IMatcher> { ButtonTapMatcher.FromDict(dict) });
		return buttonTapCondition;
	}

	public override string Description(Game game)
	{
		return "button_tap";
	}
}
