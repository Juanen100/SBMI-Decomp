using System.Collections.Generic;

public interface ITriggerable
{
	ITrigger CreateTrigger(Dictionary<string, object> data);
}
