using System.Collections.Generic;

public interface IMatcher
{
	ICollection<string> Keys { get; }

	uint MatchAmount(Game game, Dictionary<string, object> data);

	bool IsRequired(string property);

	bool HasRequirements();

	string GetTarget(string key);

	string DescribeSubject(Game game);

	new string ToString();
}
