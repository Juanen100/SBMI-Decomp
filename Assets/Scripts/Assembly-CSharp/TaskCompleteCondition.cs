using System.Collections.Generic;

public class TaskCompleteCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "task_complete";

	public static TaskCompleteCondition FromDict(Dictionary<string, object> dict)
	{
		TaskCompleteCondition taskCompleteCondition = new TaskCompleteCondition();
		taskCompleteCondition.Parse(dict, "task_complete", new List<string> { typeof(TaskCompleteAction).ToString() }, new List<IMatcher> { TaskMatcher.FromDict(dict) });
		return taskCompleteCondition;
	}

	public override string Description(Game game)
	{
		return "task_complete";
	}
}
