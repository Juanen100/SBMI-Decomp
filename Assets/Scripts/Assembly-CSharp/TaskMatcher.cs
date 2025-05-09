using System.Collections.Generic;

public class TaskMatcher : Matcher
{
	public const string DEFINITION_ID = "task_id";

	public static TaskMatcher FromDict(Dictionary<string, object> dict)
	{
		TaskMatcher taskMatcher = new TaskMatcher();
		taskMatcher.RegisterProperty("task_id", dict);
		return taskMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + GetTarget("task_id");
		}
		uint nDID = uint.Parse(GetTarget("task_id"));
		return game.taskManager.GetTaskData((int)nDID).m_sName;
	}
}
