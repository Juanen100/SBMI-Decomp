using System.Collections.Generic;

public interface ITrigger
{
	string Type { get; }

	ulong TimeStamp { get; }

	Dictionary<string, object> Data { get; }
}
