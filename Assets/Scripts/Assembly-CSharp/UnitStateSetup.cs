using System.Collections.Generic;

public static class UnitStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = null;
		machine = null;
	}

	public static void GenerateFriendsStates(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = null;
		machine = null;
	}
}
