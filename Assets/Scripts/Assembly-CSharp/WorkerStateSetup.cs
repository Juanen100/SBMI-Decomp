using System.Collections.Generic;

public static class WorkerStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("idle", Simulated.Worker.Idle);
		actions.Add("moving", Simulated.Worker.Moving);
		actions.Add("erecting", Simulated.Worker.Erecting);
		actions.Add("returning", Simulated.Worker.Returning);
		foreach (Simulated.StateAction value in actions.Values)
		{
			machine.AddState(value);
		}
		machine.AddTransition(actions["idle"], Command.TYPE.MOVE, actions["moving"]);
		machine.AddTransition(actions["idle"], Command.TYPE.RETURN, actions["returning"]);
		machine.AddTransition(actions["idle"], Command.TYPE.ERECT, actions["erecting"]);
		machine.AddTransition(actions["moving"], Command.TYPE.ERECT, actions["erecting"]);
		machine.AddTransition(actions["moving"], Command.TYPE.COMPLETE, actions["idle"]);
		machine.AddTransition(actions["moving"], Command.TYPE.RETURN, actions["returning"]);
		machine.AddTransition(actions["returning"], Command.TYPE.COMPLETE, actions["idle"]);
		machine.AddTransition(actions["erecting"], Command.TYPE.ERECT, actions["erecting"]);
		machine.AddTransition(actions["erecting"], Command.TYPE.COMPLETE, actions["idle"]);
		machine.AddTransition(actions["erecting"], Command.TYPE.RETURN, actions["returning"]);
	}
}
