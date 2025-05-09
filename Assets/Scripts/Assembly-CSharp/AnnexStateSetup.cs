using System.Collections.Generic;

public static class AnnexStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("placing", Simulated.Building.Placing);
		actions.Add("erecting", Simulated.Building.Erecting);
		actions.Add("inactive", Simulated.Building.Inactive);
		actions.Add("activating", Simulated.Building.Activating);
		actions.Add("active", Simulated.Annex.Active);
		actions.Add("reflecting", Simulated.Building.Reflecting);
		actions.Add("relaying", Simulated.Annex.Relaying);
		actions.Add("shuntedcrafting", Simulated.Annex.ShuntedCrafting);
		actions.Add("shuntedcraftcycling", Simulated.Annex.ShuntedCraftCycling);
		actions.Add("craftcollect", Simulated.Building.CraftingCollect);
		actions.Add("crafted", Simulated.Building.Crafted);
		actions.Add("rushingcraft", Simulated.Building.RushingCraft);
		actions.Add("task_feed", Simulated.Building.TaskFeed);
		actions.Add("task_feed_collecting", Simulated.Building.TaskFeedCollecting);
		foreach (Simulated.StateAction value in actions.Values)
		{
			machine.AddState(value);
		}
		if (!friendMode)
		{
			machine.AddTransition(actions["placing"], Command.TYPE.EMPLOY, actions["erecting"]);
			machine.AddTransition(actions["erecting"], Command.TYPE.COMPLETE, actions["inactive"]);
			machine.AddTransition(actions["inactive"], Command.TYPE.CLICKED, actions["activating"]);
			machine.AddTransition(actions["activating"], Command.TYPE.COMPLETE, actions["active"]);
			machine.AddTransition(actions["relaying"], Command.TYPE.COMPLETE, actions["reflecting"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.ACTIVATE, actions["active"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.CRAFT, actions["shuntedcrafting"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.ADVANCE, actions["shuntedcraftcycling"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.CRAFTED, actions["shuntedcraftcycling"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["active"], Command.TYPE.CLICKED, actions["relaying"]);
			machine.AddTransition(actions["active"], Command.TYPE.CRAFT, actions["shuntedcrafting"]);
			machine.AddTransition(actions["active"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["shuntedcrafting"], Command.TYPE.CRAFTED, actions["shuntedcraftcycling"]);
			machine.AddTransition(actions["shuntedcrafting"], Command.TYPE.CLICKED, actions["relaying"]);
			machine.AddTransition(actions["shuntedcrafting"], Command.TYPE.RUSH, actions["rushingcraft"]);
			machine.AddTransition(actions["shuntedcrafting"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["rushingcraft"], Command.TYPE.CRAFTED, actions["shuntedcraftcycling"]);
			machine.AddTransition(actions["shuntedcraftcycling"], Command.TYPE.COMPLETE, actions["crafted"]);
			machine.AddTransition(actions["shuntedcraftcycling"], Command.TYPE.CRAFTED, actions["shuntedcraftcycling"]);
			machine.AddTransition(actions["shuntedcraftcycling"], Command.TYPE.CLICKED, actions["craftcollect"]);
			machine.AddTransition(actions["shuntedcraftcycling"], Command.TYPE.RUSH, actions["rushingcraft"]);
			machine.AddTransition(actions["craftcollect"], Command.TYPE.COMPLETE, actions["shuntedcrafting"]);
			machine.AddTransition(actions["crafted"], Command.TYPE.CLICKED, actions["active"]);
			machine.AddTransition(actions["crafted"], Command.TYPE.CRAFT, actions["shuntedcraftcycling"]);
			machine.AddTransition(actions["task_feed"], Command.TYPE.CLICKED, actions["task_feed_collecting"]);
			machine.AddTransition(actions["task_feed_collecting"], Command.TYPE.COMPLETE, actions["reflecting"]);
		}
	}
}
