using System.Collections.Generic;

public static class DebrisStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("unpurchased", Simulated.Debris.Unpurchased);
		actions.Add("inactive", Simulated.Debris.Inactive);
		actions.Add("clearing", Simulated.Debris.Clearing);
		actions.Add("clearing_more", Simulated.Debris.ClearingMoreInfo);
		actions.Add("priming_rush", Simulated.Debris.PrimingRush);
		actions.Add("confirming_rush", Simulated.Debris.Clearing);
		actions.Add("rushing", Simulated.Debris.RushingClearing);
		actions.Add("deleting", Simulated.Debris.Deleting);
		actions.Add("deleted", Simulated.Debris.Deleted);
		foreach (Simulated.StateAction value in actions.Values)
		{
			machine.AddState(value);
		}
		if (!friendMode)
		{
			machine.AddTransition(actions["unpurchased"], Command.TYPE.PURCHASE, actions["inactive"]);
			machine.AddTransition(actions["inactive"], Command.TYPE.CLEAR, actions["clearing"]);
			machine.AddTransition(actions["clearing"], Command.TYPE.COMPLETE, actions["deleting"]);
			machine.AddTransition(actions["clearing"], Command.TYPE.CLICKED, actions["clearing_more"]);
			machine.AddTransition(actions["clearing_more"], Command.TYPE.RUSH, actions["priming_rush"]);
			machine.AddTransition(actions["clearing_more"], Command.TYPE.COMPLETE, actions["deleting"]);
			machine.AddTransition(actions["clearing_more"], Command.TYPE.CLICKED, actions["clearing"]);
			machine.AddTransition(actions["clearing_more"], Command.TYPE.ABORT, actions["clearing"]);
			machine.AddTransition(actions["priming_rush"], Command.TYPE.COMPLETE, actions["confirming_rush"]);
			machine.AddTransition(actions["confirming_rush"], Command.TYPE.RUSH, actions["rushing"]);
			machine.AddTransition(actions["confirming_rush"], Command.TYPE.ABORT, actions["clearing"]);
			machine.AddTransition(actions["rushing"], Command.TYPE.COMPLETE, actions["deleting"]);
			machine.AddTransition(actions["deleting"], Command.TYPE.CLICKED, actions["deleted"]);
		}
	}
}
