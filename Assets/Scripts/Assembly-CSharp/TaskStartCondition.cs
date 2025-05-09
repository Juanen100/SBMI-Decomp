using System.Collections.Generic;

public class TaskStartCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "task_start";

	public static TaskStartCondition FromDict(Dictionary<string, object> dict)
	{
		TaskStartCondition taskStartCondition = new TaskStartCondition();
		taskStartCondition.Parse(dict, "task_start", new List<string> { typeof(TaskStartAction).ToString() }, new List<IMatcher> { TaskMatcher.FromDict(dict) });
		return taskStartCondition;
	}

	public override string Description(Game game)
	{
		return "task_start";
	}
}
