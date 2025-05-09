using System.Collections.Generic;

public static class LandmarkStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("unpurchased", Simulated.Landmark.Unpurchased);
		actions.Add("inactive", Simulated.Landmark.Inactive);
		actions.Add("active", Simulated.Landmark.Active);
		foreach (Simulated.StateAction value in actions.Values)
		{
			machine.AddState(value);
		}
		if (!friendMode)
		{
			machine.AddTransition(actions["unpurchased"], Command.TYPE.PURCHASE, actions["inactive"]);
			machine.AddTransition(actions["inactive"], Command.TYPE.ACTIVATE, actions["active"]);
			machine.AddTransition(actions["active"], Command.TYPE.COMPLETE, actions["inactive"]);
			machine.AddTransition(actions["active"], Command.TYPE.ABORT, actions["inactive"]);
		}
	}
}
