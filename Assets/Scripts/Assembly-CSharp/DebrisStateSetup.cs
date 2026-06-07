using System.Collections.Generic;

public static class DebrisStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = null;
		machine = null;
	}
}
