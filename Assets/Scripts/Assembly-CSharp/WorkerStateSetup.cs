using System.Collections.Generic;

public static class WorkerStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = null;
		machine = null;
	}
}
